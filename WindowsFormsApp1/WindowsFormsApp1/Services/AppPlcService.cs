using GRT.SDK.Framework.Timer;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using WindowsFormsApp1.CCLink.Interfaces;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.Services
{
   public enum ControlStatus
   {
      AutoRun = 1,
      ManualRun = 2,
      DryRun = 3,
      Prepare = 4,
      Other = 99,
   }

   public enum AlarmStatus
   {
      Critical = 1,
      Low = 2,
      PreAlarm = 3,
      NoAlarm = 4,
   }

   public enum MachineStatus
   {
      Init,
      Preparing,
      Ready,
      Running,
      Stopped,
   }

   public enum ActionStatus
   {
      Homing,
      Moving,
      Other,
   }

   public class AppPlcService : IDisposable
   {
      #region Constant

      // Common Report Addresses (Project Specific)
      private const string ADDR_REPORT_STATUS1 = "LW1146";
      private const string ADDR_REPORT_STATUS2 = "LW114B";
      private const string ADDR_REPORT_ALARM = "LW113A";

      #endregion

      #region Fields

      // Common Report Cache
      private readonly object _commonReportLock = new object();

      private readonly ICCLinkController _controller;
      private readonly Action<string> _logger;

      private readonly AppControllerSettings _settings;
      private readonly SynchronizationContext _syncContext;

      // Dispose Flag
      private bool _disposed;
      private DateTime? _handshakeWatchdog;

      // Heartbeat Fields
      private CancellationTokenSource _heartbeatCts;
      private int _heartbeatFailThreshold = 3;
      private uint _heartbeatGlobalTimeout = 3000;
      private TimeSpan _heartbeatInterval;
      private string _heartbeatRequestAddr;
      private string _heartbeatResponseAddr;

      // Heartbeat State Machine Fields
      private int _heartbeatStep = 0;
      private uint _heartbeatT1Timeout = 5000;
      private CTimer _heartbeatT1Timer = new CTimer();
      private Task _heartbeatTask;
      private CTimer _heartbeatTimeoutTimer = new CTimer();

      // Common Report Cache
      private CommonReportAlarm _lastCommonReportAlarm;
      private CommonReportStatus1 _lastCommonReportStatus1;
      private CommonReportStatus2 _lastCommonReportStatus2;

      private bool _lastTimeSyncTrigger;
      private CancellationTokenSource _linkReportCts;
      private int _linkReportStep;
      private Task _linkReportTask;

      // TimeSync Fields
      private CancellationTokenSource _timeSyncCts;
      private Task _timeSyncTask;

      #endregion

      #region Constructors

      public AppPlcService(AppControllerSettings settings, Action<string> logger = null)
      {
         _settings = settings ?? throw new ArgumentNullException(nameof(settings));
         _logger = logger;
         _syncContext = SynchronizationContext.Current;

         // Factory logic for Controller
         if (settings.DriverType == MelsecDriverType.Simulator)
         {
            var mockAdapter = new CCLink.Adapters.MockMelsecApiAdapter();
            _controller = new CCLink.Services.MelsecHelper(mockAdapter, settings);
         }
         else if (settings.DriverType == MelsecDriverType.MxComponent)
         {
            _controller = new CCLink.Adapters.MxComponentAdapter(settings.LogicalStationNumber);
         }
         else
         {
            // Default MelsecBoard
            var adapter = new CCLink.Adapters.MelsecApiAdapter();
            // Note: Channel is set via method calls or not needed for default constructor?
            // MelsecApiAdapter usually takes channel/station in Open method or config.
            // Checking MelsecApiAdapter.cs... It has parameterless constructor. And Open(channel, ...)
            // So here we instantiate it without args.
            // Use MelsecHelper as the controller implementation
            var helper = new CCLink.Services.MelsecHelper(adapter, settings);
            _controller = helper;
         }
      }

      /// <summary>
      /// 構造函數重載：接受外部 Adapter（用於 Simulator 模式共享 MockAdapter）
      /// </summary>
      public AppPlcService(AppControllerSettings settings, IMelsecApiAdapter customAdapter, Action<string> logger = null)
      {
         _settings = settings ?? throw new ArgumentNullException(nameof(settings));
         _logger = logger;
         _syncContext = SynchronizationContext.Current;

         if (customAdapter == null)
         {
            throw new ArgumentNullException(nameof(customAdapter));
         }

         // 使用外部提供的 Adapter
         _controller = new CCLink.Services.MelsecHelper(customAdapter, settings, logger: logger);
      }

      #endregion

      #region Properties

      public bool OnlineMode { get; set; } = false;

      public ICCLinkController Controller => _controller;

      public int ConsecutiveHeartbeatFailuresCount { get; private set; }

      #endregion

      #region Public Methods

      public async Task SetControlStatus(ControlStatus status)
      {
         await _controller.WriteWordsAsync("LW114A", [(short)status]);
      }

      public async Task SetAlarmStatus(AlarmStatus status)
      {
         await _controller.WriteWordsAsync("LW1146", [(short)status]);
      }

      public async Task SetMachineStatus(MachineStatus status)
      {
         ushort statusValue = status switch
         {
            MachineStatus.Init => 1,
            MachineStatus.Preparing => 2,
            MachineStatus.Ready => 3,
            MachineStatus.Running => 4,
            MachineStatus.Stopped => 5,
            _ => 0,
         };
         await _controller.WriteWordsAsync("LW1147", [(short)statusValue]);
      }

      public async Task SetActionStatus(ActionStatus status)
      {
         ushort statusValue = status switch
         {
            ActionStatus.Homing => 1,
            ActionStatus.Moving => 2,
            ActionStatus.Other => 99,
            _ => 0,
         };
         await _controller.WriteWordsAsync("LW1148", [(short)statusValue]);
      }

      #endregion

      #region Events

      // Common Report Events
      public event Action<CommonReportStatus1> CommonReportStatus1Updated;
      public event Action<CommonReportStatus2> CommonReportStatus2Updated;
      public event Action<CommonReportAlarm> CommonReportAlarmUpdated;

      // Heartbeat Events
      public event Action HeartbeatFailed;
      public event Action<int> ConsecutiveHeartbeatFailures;
      public event Action HeartbeatSucceeded;

      /// <summary>
      /// 當接收到維護資料時觸發（Device 端接收 LCS 維護請求）
      /// 參數：(TrackingData data, int position)
      /// </summary>
      public event Action<TrackingData, int> MaintenanceDataReceived;

      #endregion

      #region Common Report Methods

      public void UpdatRouteData(string ovenStatus)
      {
         lock (_commonReportLock)
         {
            try
            {
               short[] values = new short[1];
               if (int.TryParse(ovenStatus, System.Globalization.NumberStyles.Number, null, out int hexVal))
               {
                  values[0] = (short)hexVal;
               }

               _controller.WriteWordsAsync("LW119E", values);
            }
            catch (Exception ex)
            {
               _logger?.Invoke($"update oven Exception: {ex.Message}");
            }
         }
      }

      public void UpdateStatus1(ushort alarmStatus, ushort machineStatus, ushort actionStatus, ushort waitingStatus, ushort controlStatus)
      {
         lock (_commonReportLock)
         {
            var newData = new CommonReportStatus1(alarmStatus, machineStatus, actionStatus, waitingStatus, controlStatus);

            if (newData.Equals(_lastCommonReportStatus1))
            {
               return;
            }

            try
            {
               short[] values = new short[5];
               values[0] = (short)alarmStatus;
               values[1] = (short)machineStatus;
               values[2] = (short)actionStatus;
               values[3] = (short)waitingStatus;
               values[4] = (short)controlStatus;

               // Fire and forget write task, but log errors
               _ = WriteToPlcAsync(ADDR_REPORT_STATUS1, values, () =>
               {
                  _lastCommonReportStatus1 = newData;
                  CommonReportStatus1Updated?.Invoke(newData);
               }, "Status1");
            }
            catch (Exception ex)
            {
               _logger?.Invoke($"update Status1 Exception: {ex.Message}");
            }
         }
      }

      public void UpdateStatus2(
         ushort redLight, ushort yellowLight, ushort greenLight,
         ushort upstreamWait, ushort downstreamWait,
         ushort dischargeRate, ushort stopTime,
         uint processingCounter,
         ushort retainedBoard, ushort currentRecipeNo,
         ushort boardThickness, ushort uldFlag,
         string recipeName)
      {
         lock (_commonReportLock)
         {
            var newData = new CommonReportStatus2(
               redLight, yellowLight, greenLight,
               upstreamWait, downstreamWait,
               dischargeRate, stopTime,
               processingCounter,
               retainedBoard, currentRecipeNo,
               boardThickness, uldFlag,
               recipeName);

            if (newData.Equals(_lastCommonReportStatus2))
            {
               return;
            }

            try
            {
               short[] values = new short[63];
               values[0] = (short)redLight;
               values[1] = (short)yellowLight;
               values[2] = (short)greenLight;
               values[3] = (short)upstreamWait;
               values[4] = (short)downstreamWait;
               values[5] = (short)dischargeRate;
               values[6] = (short)stopTime;
               values[7] = (short)(processingCounter & 0xFFFF);
               values[8] = (short)((processingCounter >> 16) & 0xFFFF);
               values[9] = (short)retainedBoard;
               values[10] = (short)currentRecipeNo;
               values[11] = (short)boardThickness;
               values[12] = (short)uldFlag;

               byte[] nameBytes = new byte[100];
               if (!string.IsNullOrEmpty(recipeName))
               {
                  byte[] source = System.Text.Encoding.ASCII.GetBytes(recipeName);
                  Array.Copy(source, nameBytes, Math.Min(source.Length, 99));
               }

               for (int i = 0; i < 50; i++)
               {
                  values[13 + i] = (short)((nameBytes[i * 2 + 1] << 8) | nameBytes[i * 2]);
               }

               _ = WriteToPlcAsync(ADDR_REPORT_STATUS2, values, () =>
               {
                  _lastCommonReportStatus2 = newData;
                  CommonReportStatus2Updated?.Invoke(newData);
               }, "Status2");
            }
            catch (Exception ex)
            {
               _logger?.Invoke($"update Status2 Exception: {ex.Message}");
            }
         }
      }

      public void UpdateAlarm(ushort[] errorCodes)
      {
         if (errorCodes == null || errorCodes.Length != 12)
         {
            return;
         }

         lock (_commonReportLock)
         {
            var newData = new CommonReportAlarm((ushort[])errorCodes.Clone());

            if (newData.Equals(_lastCommonReportAlarm))
            {
               return;
            }

            try
            {
               short[] values = new short[12];
               for (int i = 0; i < 12; i++)
               {
                  values[i] = (short)errorCodes[i];
               }

               _ = WriteToPlcAsync(ADDR_REPORT_ALARM, values, () =>
               {
                  _lastCommonReportAlarm = newData;
                  CommonReportAlarmUpdated?.Invoke(newData);
               }, "Alarm");
            }
            catch (Exception ex)
            {
               _logger?.Invoke($"update Alarm Exception: {ex.Message}");
            }
         }
      }

      public async Task WriteToPlcAsync(string address, short[] values, Action onSuccess, string logName)
      {
         try
         {
            await _controller.WriteWordsAsync(address, values);
            onSuccess?.Invoke();
            //_logger?.Invoke($"[AppPlcService] {logName} updated.");
         }
         catch (Exception ex)
         {
            _logger?.Invoke($"[AppPlcService] Failed to write {logName}: {ex.Message}");
         }
      }

      #endregion

      #region Link Report Methods

      // 地址常數 (PascalCase)
      private const string AddrLinkReportRequestFlag = "LB0307";  // EQ -> LCS
      private const string AddrLinkReportResponseFlag = "LB0108"; // LCS -> EQ
      private const string AddrLinkReportDataStart = "LW139E";

      // Link Report Fields
      private CTimer _linkReportTimer = new CTimer();
      private uint _linkReportT1Timeout = 5000;
      private uint _linkReportT2Timeout = 5000;
      private LinkReportData _pendingLinkReport;
      private TaskCompletionSource<bool> _linkReportTcs;

      // Link Report Event
      public event Action<LinkReportData, bool, string> LinkReportCompleted;

      /// <summary>
      /// 發送關聯數據回報至 LCS
      /// </summary>
      public async Task<bool> SendLinkReportAsync(LinkReportData data)
      {
         if (data == null)
         {
            throw new ArgumentNullException(nameof(data));
         }

         // 檢查是否已有待處理的報告
         if (_pendingLinkReport != null || _linkReportStep != 0)
         {
            _logger?.Invoke("[LinkReport] 警告：前一筆回報尚未完成，無法發送新報告。");
            return false;
         }

         _linkReportTcs = new TaskCompletionSource<bool>();
         _pendingLinkReport = data;

         return await _linkReportTcs.Task;
      }

      public void StartLinkDataReporting(TimeSpan interval, uint t1Timeout = 5000, uint t2Timeout = 5000)
      {
         _linkReportT1Timeout = t1Timeout;
         _linkReportT2Timeout = t2Timeout;

         _linkReportCts = new CancellationTokenSource();
         _linkReportTask = Task.Run(() => LinkReportFlowAsync(_linkReportCts.Token, interval));
         _logger?.Invoke($"[AppService] LinkReport started (Interval: {interval.TotalMilliseconds}ms, T1: {t1Timeout}ms, T2: {t2Timeout}ms)");
      }

      public void StopLinkDataReporting()
      {
         if (_linkReportCts != null)
         {
            _linkReportCts.Cancel();
            try
            {
               _linkReportTask?.Wait(500);
            }
            catch
            {
            }

            _linkReportCts.Dispose();
            _linkReportCts = null;
         }
      }

      private async Task LinkReportFlowAsync(CancellationToken ct, TimeSpan interval)
      {
         while (!ct.IsCancellationRequested && !_disposed)
         {
            try
            {
               await RunLinkReportFlow(ct);
               await Task.Delay(interval, ct);
            }
            catch (OperationCanceledException)
            {
               break;
            }
            catch (Exception ex)
            {
               _logger?.Invoke($"[LinkReport] Loop Exception: {ex.Message}");
               await Task.Delay(1000, ct);
            }
         }
      }

      private async Task RunLinkReportFlow(CancellationToken ct)
      {
         bool requestOn = _controller.GetBit(AddrLinkReportRequestFlag);
         bool responseOn = _controller.GetBit(AddrLinkReportResponseFlag);

         switch (_linkReportStep)
         {
            case 0: // Idle - 等待回報請求
            {
               if (_pendingLinkReport != null && !requestOn && !responseOn)
               {
                  try
                  {
                     _logger?.Invoke($"[LinkReport] 開始回報 - BoardID: {_pendingLinkReport.BoardId}");

                     // 1. 寫入數據
                     var rawData = _pendingLinkReport.ToRawData();
                     await _controller.WriteWordsAsync(AddrLinkReportDataStart, rawData, ct);

                     // 2. 設置 Request Flag ON
                     await _controller.WriteBitsAsync(AddrLinkReportRequestFlag, new[] { true }, ct);
                     _logger?.Invoke($"[LinkReport] Request Flag ({AddrLinkReportRequestFlag}) ON");

                     _linkReportTimer.Reset();
                     _linkReportStep = 10;
                  }
                  catch (Exception ex)
                  {
                     _logger?.Invoke($"[LinkReport] 寫入數據失敗: {ex.Message}");
                     CompleteLinkReport(false, $"寫入數據失敗: {ex.Message}");
                  }
               }

               break;
            }

            case 10: // 等待 LCS Response ON
            {
               if (responseOn)
               {
                  _logger?.Invoke($"[LinkReport] Response Flag ({AddrLinkReportResponseFlag}) ON");

                  // 3. 清除 Request Flag
                  await _controller.WriteBitsAsync(AddrLinkReportRequestFlag, new[] { false }, ct);
                  _logger?.Invoke($"[LinkReport] Request Flag ({AddrLinkReportRequestFlag}) OFF");

                  _linkReportTimer.Reset();
                  _linkReportStep = 20;
               }
               else if (_linkReportTimer.On(_linkReportT1Timeout))
               {
                  _logger?.Invoke("[LinkReport] T1 逾時 - LCS 未回應");
                  await _controller.WriteBitsAsync(AddrLinkReportRequestFlag, new[] { false }, ct);
                  CompleteLinkReport(false, "T1 逾時");
               }

               break;
            }

            case 20: // 等待 LCS Response OFF
            {
               if (!responseOn)
               {
                  _logger?.Invoke($"[LinkReport] Response Flag ({AddrLinkReportResponseFlag}) OFF - 完成");
                  CompleteLinkReport(true, "成功");
               }
               else if (_linkReportTimer.On(_linkReportT2Timeout))
               {
                  _logger?.Invoke("[LinkReport] T2 逾時 - LCS 未清除 Response");
                  CompleteLinkReport(false, "T2 逾時");
               }

               break;
            }
         }
      }

      private void CompleteLinkReport(bool success, string message)
      {
         var data = _pendingLinkReport;
         _pendingLinkReport = null;
         _linkReportStep = 0;

         // 通知等待者
         _linkReportTcs?.TrySetResult(success);

         // 觸發事件
         PostEvent(() => LinkReportCompleted?.Invoke(data, success, message));

         _logger?.Invoke($"[LinkReport] 回報完成 - {(success ? "成功" : "失敗")}: {message}");
      }

      #endregion

      #region Heartbeat Methods

      public void StartHeartbeat(int failThreshold = 3)
      {
         StopHeartbeat();

         _heartbeatInterval = _settings.Heartbeat.HeartbeatIntervalMs > 0
            ? TimeSpan.FromMilliseconds(_settings.Heartbeat.HeartbeatIntervalMs)
            : TimeSpan.FromSeconds(0.3);
         _heartbeatRequestAddr = _settings.Heartbeat.RequestAddress;
         _heartbeatResponseAddr = _settings.Heartbeat.ResponseAddress;
         _heartbeatFailThreshold = Math.Max(1, failThreshold);
         ConsecutiveHeartbeatFailuresCount = 0;

         _heartbeatCts = new CancellationTokenSource();
         _heartbeatTask = Task.Run(() => HeartbeatLoopAsync(_heartbeatCts.Token));
         _logger?.Invoke($"[AppService] Heartbeat started (Interval: {_heartbeatInterval.TotalMilliseconds}ms)");
      }

      public async void StopHeartbeat()
      {
         if (_heartbeatCts != null)
         {
            await _controller.WriteBitsAsync(_heartbeatResponseAddr, [false]);
            _heartbeatCts.Cancel();
            try
            {
               _heartbeatTask?.Wait(500);
            }
            catch
            {
            }

            _heartbeatCts.Dispose();
            _heartbeatCts = null;
         }

         ConsecutiveHeartbeatFailuresCount = 0;
      }

      private async Task HeartbeatLoopAsync(CancellationToken ct)
      {
         while (!ct.IsCancellationRequested && !_disposed)
         {
            try
            {
               await RunHeartbeatAsync(ct);
               await Task.Delay(_heartbeatInterval, ct);
            }
            catch (OperationCanceledException)
            {
               break;
            }
            catch (Exception ex)
            {
               _logger?.Invoke($"[Heartbeat] Loop Exception: {ex.Message}");
               await Task.Delay(1000, ct);
            }
         }
      }

      private async Task RunHeartbeatAsync(CancellationToken ct)
      {
         // 檢查 OnlineMode 狀態
         if (!OnlineMode)
         {
            // 離線模式：重置狀態機
            if (_heartbeatStep != 0)
            {
               _logger?.Invoke("[Heartbeat] 離開連線模式，重置狀態機 | Exit Online Mode, resetting state machine");
               _heartbeatStep = 0;
            }

            return;
         }

         // 讀取當前信號狀態
         bool requestOn = _controller.GetBit(_heartbeatRequestAddr);
         bool responseOn = _controller.GetBit(_heartbeatResponseAddr);

         switch (_heartbeatStep)
         {
            case 0: // Idle - 等待 PLC Request
            {
               if (requestOn && !responseOn)
               {
                  _logger?.Invoke("[Heartbeat] PLC Request detected (1,0)");

                  try
                  {
                     // 寫入 Response ON
                     await _controller.WriteBitsAsync(_heartbeatResponseAddr, new[] { true }, ct);

                     _heartbeatT1Timer.Reset();
                     _heartbeatTimeoutTimer.Reset();
                     _heartbeatStep = 2; // → Responding
                  }
                  catch (Exception ex)
                  {
                     _logger?.Invoke($"[Heartbeat] Set Response Failed: {ex.Message}");
                     HandleHeartbeatFailure();
                  }
               }

               break;
            }

            case 2: // Responding - 等待 PLC 清除 Request
            {
               if (!requestOn && responseOn)
               {
                  _logger?.Invoke("[Heartbeat] PLC cleared Request (0,1)");

                  try
                  {
                     // PLC 已清除 Request，清除 Response
                     await _controller.WriteBitsAsync(_heartbeatResponseAddr, new[] { false }, ct);

                     _logger?.Invoke("[Heartbeat] Handshake completed");
                     _heartbeatStep = 0; // → Idle

                     // 觸發成功事件
                     ConsecutiveHeartbeatFailuresCount = 0;
                     PostEvent(() => HeartbeatSucceeded?.Invoke());
                  }
                  catch (Exception ex)
                  {
                     _logger?.Invoke($"[Heartbeat] Clear Response Failed: {ex.Message}");
                     HandleHeartbeatFailure();
                     _heartbeatStep = 0; // → Idle
                  }
               }
               else if (!requestOn && !responseOn)
               {
                  // 異常狀態：兩個都變成 OFF 了
                  _logger?.Invoke("[Heartbeat] 異常狀態：Request 和 Response 都是 OFF");
                  _heartbeatStep = 0; // → Idle
               }
               else if (_heartbeatT1Timer.On(_heartbeatT1Timeout))
               {
                  // T1 逾時：PLC 未在時限內清除 Request
                  _logger?.Invoke($"[Heartbeat] T1 Timeout - PLC 未在 {_heartbeatT1Timeout}ms 內清除 Request");

                  try
                  {
                     await AlarmHelper.AddAlarmCodeAsync(this, "0xC000");
                     await _controller.WriteBitsAsync(_heartbeatResponseAddr, [false], ct);
                  }
                  catch (Exception ex)
                  {
                     _logger?.Invoke($"[Heartbeat] T1 Timeout處理失敗: {ex.Message}");
                  }

                  HandleHeartbeatFailure();
                  _heartbeatStep = 0; // → Idle
               }
               //else if (_heartbeatTimeoutTimer.On(_heartbeatGlobalTimeout))
               //{
               //   // 全局逾時
               //   _logger?.Invoke($"[Heartbeat] Global Timeout - 超過 {_heartbeatGlobalTimeout}ms");
               //   HandleHeartbeatFailure();
               //   _heartbeatStep = 0; // → Idle
               //}

               break;
            }
         }
      }

      /// <summary>
      /// 處理心跳失敗
      /// </summary>
      private void HandleHeartbeatFailure()
      {
         ConsecutiveHeartbeatFailuresCount++;
         PostEvent(() => ConsecutiveHeartbeatFailures?.Invoke(ConsecutiveHeartbeatFailuresCount));

         if (ConsecutiveHeartbeatFailuresCount >= _heartbeatFailThreshold)
         {
            _logger?.Invoke($"[Heartbeat] 連續失敗 {_heartbeatFailThreshold} 次，已停止心跳監控");
            PostEvent(() => HeartbeatFailed?.Invoke());
            _heartbeatCts?.Cancel();
         }
      }

      #endregion

      #region TimeSync Methods

      public void StartTimeSync(TimeSpan interval, string triggerAddr, string dataAddr)
      {
         StopTimeSync();
         _timeSyncCts = new CancellationTokenSource();
         _logger?.Invoke($"[AppService] TimeSync started (Trigger: {triggerAddr}, Data: {dataAddr})");
         _timeSyncTask = Task.Run(() => TimeSyncLoopAsync(interval, triggerAddr, dataAddr, _timeSyncCts.Token));
      }

      public void StopTimeSync()
      {
         if (_timeSyncCts != null)
         {
            _timeSyncCts.Cancel();
            _timeSyncCts.Dispose();
            _timeSyncCts = null;
         }
      }

      private async Task TimeSyncLoopAsync(TimeSpan interval, string triggerAddr, string dataAddr, CancellationToken ct)
      {
         while (!ct.IsCancellationRequested && !_disposed)
         {
            try
            {
               bool requestOn = _controller.GetBit(triggerAddr);
               if (requestOn && !_lastTimeSyncTrigger)
               {
                  _logger?.Invoke("[TimeSync] Rising edge detected, syncing...");
                  await SyncTimeAsync(dataAddr, ct);
               }

               _lastTimeSyncTrigger = requestOn;
            }
            catch (Exception ex)
            {
               _logger?.Invoke($"[TimeSync] Exception: {ex.Message}");
            }

            await Task.Delay(interval, ct);
         }
      }

      private async Task SyncTimeAsync(string dataAddr, CancellationToken ct)
      {
         try
         {
            var values = await _controller.ReadWordsAsync(dataAddr, 7, ct);
            if (values.Count < 7)
            {
               return;
            }

            var st = new SystemTime
            {
               Year = (ushort)values[0],
               Month = (ushort)values[1],
               Day = (ushort)values[2],
               Hour = (ushort)values[3],
               Minute = (ushort)values[4],
               Second = (ushort)values[5],
               DayOfWeek = (ushort)values[6]
            };

            if (SetLocalTime(ref st))
            {
               _logger?.Invoke($"[TimeSync] System Time Updated: {st.Year}/{st.Month}/{st.Day} {st.Hour}:{st.Minute}:{st.Second}");
            }
            else
            {
               int errorCode = Marshal.GetLastWin32Error();
               _logger?.Invoke($"[TimeSync] Failed to Update System Time (Error: {errorCode})");
               await AlarmHelper.AddAlarmCodeAsync(this, "C001");
            }
         }
         catch (Exception ex)
         {
            _logger?.Invoke($"[TimeSync] Sync Exception: {ex.Message}");
         }
      }

      [StructLayout(LayoutKind.Sequential)]
      private struct SystemTime
      {
         public ushort Year;
         public ushort Month;
         public ushort DayOfWeek;
         public ushort Day;
         public ushort Hour;
         public ushort Minute;
         public ushort Second;
         public ushort Milliseconds;
      }

      [DllImport("kernel32.dll", SetLastError = true)]
      private static extern bool SetLocalTime(ref SystemTime lpSystemTime);

      #endregion

      #region Tracking Data Maintenance Methods

      // Tracking Maintenance PLC To Device
      private readonly string AddrMaintenanceRequestFlag = "LB0106";
      private readonly string AddrMaintenanceRequestData = "LW17D1"; // Length 10
      private readonly string AddrMaintenanceRequestPos = "LW17DB";

      private readonly string AddrMaintenanceResponseOk = "LB0304";
      private readonly string AddrMaintenanceResponseNg = "LB0305";
      private readonly string AddrTrackingDataBase = "LW184A"; // Base address for 540 words (54 positions?)

      // Tracking Maintenance Device To PLC
      private readonly string AddrMaintenanceSenderRequestFlag = "LB0306";
      private readonly string AddrMaintenanceSenderResponseFlag = "LB0107";
      private readonly string AddrMaintenanceSenderData = "LW05BE";
      private readonly string AddrMaintenanceSenderPos = "LW05C8";
      private const int TrackingDataSize = 10;
      private const int MaxPositions = 540; // 540 words / 10 words per pos = 54

      private CancellationTokenSource _maintenanceMonitorCts;
      private Task _maintenanceMonitorTask;
      private CTimer _maintenanceTimer = new CTimer();
      private int _maintenanceStep;
      private uint _maintenanceT1Timeout = 5000;
      private uint _maintenanceT2Timeout = 5000;

      // 接收到的維護資料（用於觸發事件）
      private TrackingData _receivedMaintenanceData;
      private int _receivedMaintenancePos;

      // Tracking Maintenance Request (Sender Side) Handshake Fields
      private int _maintenanceRequestStep;
      private TrackingData _pendingMaintenanceData;
      private int _pendingMaintenancePos;
      private TaskCompletionSource<bool> _maintenanceRequestTcs;
      private CTimer _maintenanceRequestTimer = new CTimer();
      private uint _maintenanceRequestT1Timeout = 5000;
      private uint _maintenanceRequestT2Timeout = 5000;

      public void StartTrackingDataMaintenanceMonitor(TimeSpan interval)
      {
         StopTrackingDataMaintenanceMonitor();
         _maintenanceT1Timeout = (ushort)_settings.Maintenance.PlcToDeviceT1Timeout;
         _maintenanceT2Timeout = (ushort)_settings.Maintenance.PlcToDeviceT2Timeout;
         _maintenanceRequestT1Timeout = (ushort)_settings.Maintenance.PlcToDeviceT1Timeout;
         _maintenanceRequestT2Timeout = (ushort)_settings.Maintenance.PlcToDeviceT2Timeout;

         _maintenanceStep = 0;
         _maintenanceRequestStep = 0;

         _maintenanceMonitorCts = new CancellationTokenSource();

         // 啟動接收端監控 (LCS -> Device)
         _maintenanceMonitorTask = Task.Run(() => TrackingDataMaintenanceLoopAsync(interval, _maintenanceMonitorCts.Token));

         // 啟動傳送端握手 (Device -> LCS)
         Task.Run(() => MaintenanceRequestLoopAsync(interval, _maintenanceMonitorCts.Token));

         _logger?.Invoke($"[AppService] Tracking Data Maintenance started (T1: {_maintenanceT1Timeout}ms, T2: {_maintenanceT2Timeout}ms)");
      }

      public void StopTrackingDataMaintenanceMonitor()
      {
         if (_maintenanceMonitorCts != null)
         {
            _maintenanceMonitorCts.Cancel();
            try
            {
               // Wait for receiver monitor task if it exists (not strictly required if we don't await)
               // _maintenanceMonitorTask?.Wait(100); 
            }
            catch
            {
            }

            _maintenanceMonitorCts = null;
         }

         // 清除待處理的發送請求
         if (_maintenanceRequestTcs != null && !_maintenanceRequestTcs.Task.IsCompleted)
         {
            _maintenanceRequestTcs.TrySetCanceled();
         }

         CompleteMaintenanceRequest(false);

         _logger?.Invoke("[AppService] Tracking Data Maintenance stopped");
      }

      private async Task TrackingDataMaintenanceLoopAsync(TimeSpan interval, CancellationToken ct)
      {
         while (!ct.IsCancellationRequested && !_disposed)
         {
            try
            {
               await RunMaintenanceFlow(ct);
               await Task.Delay(interval, ct);
            }
            catch (OperationCanceledException)
            {
               break;
            }
            catch (Exception ex)
            {
               _logger?.Invoke($"[Maintenance] Loop Exception: {ex.Message}");
               await Task.Delay(1000, ct);
            }
         }
      }

      private async Task TrackingDataMaintenanceLoopAsync_Old(TimeSpan interval, CancellationToken ct)
      {
         while (!ct.IsCancellationRequested && !_disposed)
         {
            try
            {
               // 1. Monitor Request Flag (LB 0x0106)
               bool requestOn = _controller.GetBit(AddrMaintenanceRequestFlag);

               if (requestOn)
               {
                  _logger?.Invoke("[Maintenance] Request ON detected, processing...");

                  // 2. Read Request Data (Tracking Data 10 words + Position 1 word)
                  var trackWords = await _controller.ReadWordsAsync(AddrMaintenanceRequestData, TrackingDataSize, ct);
                  var posWords = await _controller.ReadWordsAsync(AddrMaintenanceRequestPos, 1, ct);

                  if (trackWords.Count == TrackingDataSize && posWords.Count == 1)
                  {
                     int pos = (ushort)posWords[0];

                     // Validate Position
                     if (pos >= 0 && pos < MaxPositions) // Assuming 54 positions based on 540 words
                     {
                        // 3. Update Device Memory (Write to LW 184A + Offset)
                        int baseAddr = Convert.ToInt32(AddrTrackingDataBase.Substring(2), 16);
                        int targetAddr = baseAddr + pos * TrackingDataSize;
                        string targetAddrStr = $"LW{targetAddr:X4}";

                        await _controller.WriteWordsAsync(targetAddrStr, trackWords.ToArray(), ct);
                        _logger?.Invoke($"[Maintenance] Updated Position {pos} at {targetAddrStr}");

                        // 4. Response OK (LB 0x0304)
                        await _controller.WriteBitsAsync(AddrMaintenanceResponseOk, new[] { true }, ct);
                        _logger?.Invoke("[Maintenance] Response OK Set");
                     }
                     else
                     {
                        _logger?.Invoke($"[Maintenance] Invalid Position: {pos}");
                        // Response NG
                        await _controller.WriteBitsAsync(AddrMaintenanceResponseNg, new[] { true }, ct);
                     }
                  }
                  else
                  {
                     _logger?.Invoke("[Maintenance] Read Data Failed");
                     // Response NG
                     await _controller.WriteBitsAsync(AddrMaintenanceResponseNg, new[] { true }, ct);
                  }

                  // 5. Wait for Request OFF
                  while (!ct.IsCancellationRequested)
                  {
                     bool stillOn = _controller.GetBit(AddrMaintenanceRequestFlag);
                     if (!stillOn)
                     {
                        break;
                     }

                     await Task.Delay(100, ct);
                  }

                  // 6. Clear Response Flags
                  await _controller.WriteBitsAsync(AddrMaintenanceResponseOk, new[] { false }, ct);
                  await _controller.WriteBitsAsync(AddrMaintenanceResponseNg, new[] { false }, ct);
                  _logger?.Invoke("[Maintenance] Request Cycle Completed, Responses Cleared");
               }
            }
            catch (Exception ex)
            {
               _logger?.Invoke($"[Maintenance] Loop Exception: {ex.Message}");
               await Task.Delay(1000, ct);
            }

            await Task.Delay(interval, ct);
         }
      }

      /// <summary>
      /// 發送追蹤資料維護請求 (LCS -> DEVICE)
      /// </summary>
      /// <param name="ct">CancellationToken</param>
      /// <returns>True: 成功 (Response ON), False: 失敗或逾時</returns>
      private async Task RunMaintenanceFlow(CancellationToken ct)
      {
         bool requestOn = _controller.GetBit(_settings.Maintenance.AddrPlcToDeviceRequestFlag);

         switch (_maintenanceStep)
         {
            case 0: // Idle - Wait for Request ON
               if (requestOn)
               {
                  _logger?.Invoke("[Maintenance] Request ON detected, processing...");

                  // 2. Read Request Data (Tracking Data 10 words + Position 1 word)
                  var trackWords = await _controller.ReadWordsAsync(_settings.Maintenance.AddrPlcToDeviceRequestTrackingData, TrackingDataSize, ct);
                  var posWords = await _controller.ReadWordsAsync(_settings.Maintenance.AddrPlcToDeviceRequestPosData, 1, ct);

                  if (trackWords.Count == TrackingDataSize && posWords.Count == 1)
                  {
                     int pos = (ushort)posWords[0];

                     // Validate Position
                     if (pos is > 0 and < MaxPositions)
                     {
                        // 將 trackWords 轉換為 TrackingData 並保存
                        _receivedMaintenanceData = TrackingData.FromWords(trackWords.Select(w => (ushort)w).ToArray());
                        _receivedMaintenancePos = pos;

                        // 3. Update Device Memory (Write to LW Base + Offset)
                        int baseAddr = Convert.ToInt32(_settings.Maintenance.AddrPositionDataBase.Substring(2), 16);
                        int targetAddr = baseAddr + (pos - 1) * TrackingDataSize;
                        string targetAddrStr = $"LW{targetAddr:X4}";

                        await _controller.WriteWordsAsync(targetAddrStr, trackWords.ToArray(), ct);
                        _logger?.Invoke($"[Maintenance] Updated Position {pos} at {targetAddrStr}");

                        // 4. Response OK
                        await _controller.WriteBitsAsync(_settings.Maintenance.AddrPlcToDeviceResponseOk, [true], ct);
                        _logger?.Invoke("[Maintenance] Response OK Set");
                        bool requestOn1 = _controller.GetBit(_settings.Maintenance.AddrPlcToDeviceResponseOk);
                     }
                     else
                     {
                        _logger?.Invoke($"[Maintenance] Invalid Position: {pos}");
                        // Response NG
                        await _controller.WriteBitsAsync(_settings.Maintenance.AddrPlcToDeviceResponseNg, new[] { true }, ct);
                        await AlarmHelper.AddAlarmCodeAsync(this, "C00B");
                     }
                  }
                  else
                  {
                     _logger?.Invoke("[Maintenance] Read Data Failed");
                     // Response NG
                     await _controller.WriteBitsAsync(_settings.Maintenance.AddrPlcToDeviceResponseNg, new[] { true }, ct);
                     await AlarmHelper.AddAlarmCodeAsync(this, "C00B");
                  }

                  _maintenanceTimer.Reset();
                  _maintenanceStep = 10;
               }

               break;

            case 10: // Wait for Request OFF
               if (!requestOn)
               {
                  _logger?.Invoke("[Maintenance] Request OFF detected, Clearing Responses");

                  // 6. Clear Response Flags
                  await _controller.WriteBitsAsync(_settings.Maintenance.AddrPlcToDeviceResponseOk, new[] { false }, ct);
                  await _controller.WriteBitsAsync(_settings.Maintenance.AddrPlcToDeviceResponseNg, new[] { false }, ct);

                  // 握手完成：觸發事件通知 UI
                  if (_receivedMaintenanceData != null)
                  {
                     var data = _receivedMaintenanceData;
                     var pos = _receivedMaintenancePos;

                     PostEvent(() => MaintenanceDataReceived?.Invoke(data, pos));
                     _logger?.Invoke($"[Maintenance] Data received notification sent for Position {pos}");
                  }

                  _receivedMaintenanceData = null;
                  _maintenanceStep = 0;
               }
               else if (_maintenanceTimer.On(_maintenanceT1Timeout))
               {
                  _logger?.Invoke($"[Maintenance] T1 Timeout: Request did not turn OFF within {_maintenanceT1Timeout}ms");
                  _maintenanceTimer.Reset();
                  await AlarmHelper.AddAlarmCodeAsync(this, "C00B");
               }

               break;
         }
      }

      /// <summary>
      /// 處理傳送端的維護 handshake (Device -> LCS)
      /// </summary>
      private async Task RunMaintenanceRequestFlow(CancellationToken ct)
      {
         bool responseOn = _controller.GetBit(AddrMaintenanceSenderResponseFlag);

         switch (_maintenanceRequestStep)
         {
            case 0: // Idle - 等待待處理請求
               if (_pendingMaintenanceData != null)
               {
                  try
                  {
                     await _controller.WriteBitsAsync(AddrMaintenanceResponseOk, new[] { false }, ct);
                     await _controller.WriteBitsAsync(AddrMaintenanceResponseNg, new[] { false }, ct);
                     _logger?.Invoke($"[MaintenanceSync] 開始發送維護請求... Pos:{_pendingMaintenancePos}");

                     // 1. 寫入資料
                     short[] trackWords = _pendingMaintenanceData.ToRawData(); // 10 words

                     int pos = _pendingMaintenancePos;

                     // Validate Position
                     if (pos is > 0 and < MaxPositions)
                     {
                        // 將 trackWords 轉換為 TrackingData 並保存
                        _receivedMaintenanceData = TrackingData.FromWords(trackWords.Select(w => (ushort)w).ToArray());
                        _receivedMaintenancePos = pos;

                        // 3. Update Device Memory (Write to LW Base + Offset)
                        int baseAddr = Convert.ToInt32(AddrTrackingDataBase.Substring(2), 16);
                        int targetAddr = baseAddr + (pos - 1) * TrackingDataSize;
                        string targetAddrStr = $"LW{targetAddr:X4}";

                        await _controller.WriteWordsAsync(targetAddrStr, trackWords.ToArray(), ct);
                        _logger?.Invoke($"[Maintenance] Updated Position {pos} at {targetAddrStr}");

                        // 4. Response OK (LB 0x0304)
                        await _controller.WriteBitsAsync(AddrMaintenanceResponseOk, new[] { true }, ct);
                        _logger?.Invoke("[Maintenance] Response OK Set");
                     }
                     else
                     {
                        _logger?.Invoke($"[Maintenance] Invalid Position: {pos}");
                        // Response NG
                        await _controller.WriteBitsAsync(AddrMaintenanceResponseNg, new[] { true }, ct);
                        await AlarmHelper.AddAlarmCodeAsync(this, "C00B");
                     }

                     //await _controller.WriteWordsAsync(AddrMaintenanceSenderData, trackWords, ct);
                     //await _controller.WriteWordsAsync(AddrMaintenanceSenderPos, new short[] { (short)_pendingMaintenancePos }, ct);
                     await _controller.WriteWordsAsync(AddrMaintenanceRequestData, trackWords, ct);
                     await _controller.WriteWordsAsync(AddrMaintenanceRequestPos, new short[] { (short)_pendingMaintenancePos }, ct);

                     // 2. 設定 Request Flag ON
                     await _controller.WriteBitsAsync(AddrMaintenanceSenderRequestFlag, new[] { true }, ct);
                     _logger?.Invoke($"[MaintenanceSync] Set Request Flag ({AddrMaintenanceSenderRequestFlag}) ON");

                     _maintenanceRequestTimer.Reset();
                     _maintenanceRequestStep = 10;
                  }
                  catch (Exception ex)
                  {
                     _logger?.Invoke($"[MaintenanceSync] 寫入資料失敗: {ex.Message}");
                     CompleteMaintenanceRequest(false);
                  }
               }

               break;

            case 10: // Waiting for Response ON (T1)
               if (responseOn)
               {
                  _logger?.Invoke($"[MaintenanceSync] Response Flag ({AddrMaintenanceSenderResponseFlag}) ON Detected");

                  // 3. 清除 Request Flag OFF
                  await _controller.WriteBitsAsync(AddrMaintenanceSenderRequestFlag, new[] { false }, ct);
                  _logger?.Invoke($"[MaintenanceSync] Set Request Flag ({AddrMaintenanceSenderRequestFlag}) OFF");

                  _maintenanceRequestTimer.Reset();
                  _maintenanceRequestStep = 20;
               }
               else if (_maintenanceRequestTimer.On(_maintenanceRequestT1Timeout))
               {
                  _logger?.Invoke($"[MaintenanceSync] T1 Timeout: 未能在 {_maintenanceRequestT1Timeout}ms 內收到 Response ON");

                  // 異常處理：清除 Request Flag
                  await _controller.WriteBitsAsync(AddrMaintenanceSenderRequestFlag, new[] { false }, ct);
                  await AlarmHelper.AddAlarmCodeAsync(this, "C00C");
                  CompleteMaintenanceRequest(false);
               }

               break;

            case 20: // Waiting for Response OFF (T2)
               if (!responseOn)
               {
                  // Response OK (LB 0x0304)
                  await _controller.WriteBitsAsync(AddrMaintenanceResponseOk, new[] { true }, ct);
                  _logger?.Invoke($"[MaintenanceSync] Response Flag ({AddrMaintenanceSenderResponseFlag}) OFF Confirmed - 完成");
                  CompleteMaintenanceRequest(true);
               }
               else if (_maintenanceRequestTimer.On(_maintenanceRequestT2Timeout))
               {
                  _logger?.Invoke($"[MaintenanceSync] T2 Alarm: LCS 未能在 {_maintenanceRequestT2Timeout}ms 內清除 Response");
                  // T2 逾時通常發出警報，但通訊本身可視為成功（若定義為 Request 被接受）
                  // 依指示：送信端發出警報。
                  // Response NG (LB 0x0305)
                  await _controller.WriteBitsAsync(AddrMaintenanceResponseNg, new[] { true }, ct);
                  CompleteMaintenanceRequest(true); // 完成流程，但已記錄警報
               }

               break;
         }
      }

      private void CompleteMaintenanceRequest(bool success)
      {
         _pendingMaintenanceData = null;
         _maintenanceRequestStep = 0;
         _maintenanceRequestTcs?.TrySetResult(success);
      }

      private async Task MaintenanceRequestLoopAsync(TimeSpan interval, CancellationToken ct)
      {
         while (!ct.IsCancellationRequested && !_disposed)
         {
            try
            {
               await RunMaintenanceRequestFlow(ct);
               await Task.Delay(interval, ct);
            }
            catch (OperationCanceledException ex)
            {
               _logger?.Invoke($"[MaintenanceSync] OperationCanceledException Exception: {ex.Message}");
               break;
            }
            catch (Exception ex)
            {
               _logger?.Invoke($"[MaintenanceSync] Loop Exception: {ex.Message}");
               await Task.Delay(1000, ct);
            }
         }
      }

      public async Task<bool> SendTrackingMaintenanceRequestAsync(TrackingData data, int positionNo, CancellationToken ct = default)
      {
         if (data == null)
         {
            throw new ArgumentNullException(nameof(data));
         }

         // 檢查是否已有待處理的請求
         if (_pendingMaintenanceData != null || _maintenanceRequestStep != 0)
         {
            _logger?.Invoke("[MaintenanceSync] 警告：前一筆維護請求尚未完成，無法發送新請求。");
            return false;
         }

         _maintenanceRequestTcs = new TaskCompletionSource<bool>();
         _pendingMaintenanceData = data;
         _pendingMaintenancePos = positionNo;

         // 等待狀態機完成處理
         return await _maintenanceRequestTcs.Task;
      }

      #endregion

      #region Tracking Methods

      /// <summary>
      /// 讀取指定站點的排出資料（追蹤資料 + 排出理由）
      /// </summary>
      public async Task<MoveOutData> GetMoveOutDataAsync(TrackingStation station, CancellationToken ct = default)
      {
         string address = _settings.Tracking.GetAddress(station);
         if (string.IsNullOrWhiteSpace(address))
         {
            throw new InvalidOperationException($"站點 {station} 的位址未設定");
         }

         var rawData = await _controller.ReadWordsAsync(address, 11, ct);
         return MoveOutData.FromRawData(rawData.ToArray());
      }

      /// <summary>
      /// 讀取指定站點的追蹤資料（僅 10 words，向下相容）
      /// </summary>
      public async Task<TrackingData> GetTrackingDataAsync(TrackingStation station, CancellationToken ct = default)
      {
         var moveOutData = await GetMoveOutDataAsync(station, ct);
         return moveOutData.TrackingData;
      }

      /// <summary>
      /// 清除指定站點的排出資料（寫入 0，包含 11 words）
      /// </summary>
      public async Task ClearMoveOutDataAsync(TrackingStation station, TrackingData trackingData, CancellationToken ct = default)
      {
         string address = _settings.Tracking.GetAddress(station);
         if (string.IsNullOrWhiteSpace(address))
         {
            throw new InvalidOperationException($"站點 {station} 的位址未設定");
         }

         await _controller.WriteWordsAsync("LW17B2", trackingData.ToRawData(), ct);

         short[] zeroData = new short[11];
         await _controller.WriteWordsAsync(address, zeroData, ct);
         _logger?.Invoke($"[Tracking] 已清除站點 {station} 的排出資料 (位址: {address})");
      }

      /// <summary>
      /// 清除指定站點的追蹤資料（寫入 0，僅 10 words，向下相容）
      /// </summary>
      public async Task ClearTrackingDataAsync(TrackingStation station, TrackingData trackingData, CancellationToken ct = default)
      {
         await ClearMoveOutDataAsync(station, trackingData, ct);
      }

      /// <summary>
      /// 寫入排出資料至指定站點（追蹤資料 + 排出理由，11 words）
      /// </summary>
      public async Task WriteMoveOutDataAsync(TrackingStation station, MoveOutData data, CancellationToken ct = default)
      {
         if (data == null)
         {
            throw new ArgumentNullException(nameof(data));
         }

         string address = _settings.Tracking.GetAddress(station);
         if (string.IsNullOrWhiteSpace(address))
         {
            throw new InvalidOperationException($"站點 {station} 的位址未設定");
         }

         short[] rawData = data.ToRawData();
         await _controller.WriteWordsAsync(address, rawData, ct);
         _logger?.Invoke($"[Tracking] 已寫入排出資料至站點 {station} (位址: {address}, 理由碼: {data.ReasonCode})");
      }

      /// <summary>
      /// 寫入追蹤資料至指定站點（僅 10 words，向下相容）
      /// </summary>
      public async Task WriteTrackingDataAsync(TrackingStation station, TrackingData data, CancellationToken ct = default)
      {
         var moveOutData = new MoveOutData
         {
            TrackingData = data,
            ReasonCode = 0
         };
         await WriteMoveOutDataAsync(station, moveOutData, ct);
      }

      #endregion

      #region Helpers

      private void PostEvent(Action action)
      {
         if (action == null)
         {
            return;
         }

         if (_syncContext != null)
         {
            _syncContext.Post(_ => action(), null);
         }
         else
         {
            action();
         }
      }

      public void Dispose()
      {
         if (_disposed)
         {
            return;
         }

         _disposed = true;
         StopHeartbeat();
         StopTimeSync();
         StopLinkDataReporting();
      }

      #endregion
   }
}