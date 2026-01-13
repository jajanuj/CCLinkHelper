using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using WindowsFormsApp1.CCLink.Interfaces;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.Services
{
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
      private TimeSpan _heartbeatInterval;
      private string _heartbeatRequestAddr;
      private string _heartbeatResponseAddr;
      private Task _heartbeatTask;
      private CommonReportAlarm _lastCommonReportAlarm;
      private CommonReportStatus1 _lastCommonReportStatus1;
      private CommonReportStatus2 _lastCommonReportStatus2;
      private bool? _lastObservedRequest;
      private bool? _lastObservedResponse;
      private bool _lastTimeSyncTrigger;

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

      #endregion

      #region Properties

      public ICCLinkController Controller => _controller;

      public int ConsecutiveHeartbeatFailuresCount { get; private set; }

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

      #endregion

      #region Common Report Methods

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

      private async Task WriteToPlcAsync(string address, short[] values, Action onSuccess, string logName)
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

      #region Heartbeat Methods

      public void StartHeartbeat(TimeSpan interval, string reqAddr, string respAddr, int failThreshold = 3)
      {
         StopHeartbeat();

         _heartbeatInterval = interval;
         _heartbeatRequestAddr = reqAddr;
         _heartbeatResponseAddr = respAddr;
         _heartbeatFailThreshold = Math.Max(1, failThreshold);
         ConsecutiveHeartbeatFailuresCount = 0;

         _heartbeatCts = new CancellationTokenSource();
         _heartbeatTask = Task.Run(() => HeartbeatLoopAsync(_heartbeatCts.Token));
         _logger?.Invoke($"[AppService] Heartbeat started (Interval: {interval.TotalMilliseconds}ms)");
      }

      public void StopHeartbeat()
      {
         if (_heartbeatCts != null)
         {
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
         bool ok = true;
         bool handshakeCompleted = false;

         // Use Cached GetBit from Controller
         bool requestOn = _controller.GetBit(_heartbeatRequestAddr);
         bool responseOn = _controller.GetBit(_heartbeatResponseAddr);

         int state = (requestOn ? 2 : 0) | (responseOn ? 1 : 0);
         int lastState = (_lastObservedRequest ?? false ? 2 : 0) | (_lastObservedResponse ?? false ? 1 : 0);
         bool stateChanged = !(_lastObservedRequest.HasValue && _lastObservedResponse.HasValue) || state != lastState;

         if (stateChanged || state == 0)
         {
            _handshakeWatchdog = DateTime.UtcNow;
            if (stateChanged)
            {
               // _logger?.Invoke($"[Heartbeat] State Changed: {lastState} -> {state}");
            }
         }
         else
         {
            var elapsed = DateTime.UtcNow - (_handshakeWatchdog ?? DateTime.UtcNow);
            if (elapsed.TotalMilliseconds > _heartbeatInterval.TotalMilliseconds * _heartbeatFailThreshold)
            {
               _logger?.Invoke($"[Heartbeat] Timeout (State: {state})");
               ok = false;
            }
         }

         switch (state)
         {
            case 0: // (0,0) Idle
               if (lastState == 1)
               {
                  handshakeCompleted = true;
               }

               _lastObservedRequest = false;
               _lastObservedResponse = false;
               break;

            case 2: // (1,0) PLC Request
               if (lastState == 0 || !_lastObservedRequest.HasValue)
               {
                  _logger?.Invoke("[Heartbeat] PLC Request Detected (1,0)");
                  try
                  {
                     await _controller.WriteBitsAsync(_heartbeatResponseAddr, new[] { true }, ct);
                     _lastObservedRequest = true;
                     _lastObservedResponse = false;
                  }
                  catch (Exception ex)
                  {
                     _logger?.Invoke($"[Heartbeat] Set Response Failed: {ex.Message}");
                     ok = false;
                  }
               }

               break;

            case 3: // (1,1) PC Responded
               _lastObservedRequest = true;
               _lastObservedResponse = true;
               break;

            case 1: // (0,1) PLC Cleared Request
               if (lastState == 3)
               {
                  _logger?.Invoke("[Heartbeat] PLC Cleared Request, Clearing Response (0,1)");
                  try
                  {
                     await _controller.WriteBitsAsync(_heartbeatResponseAddr, new[] { false }, ct);
                     _lastObservedRequest = false;
                     _lastObservedResponse = true;
                  }
                  catch (Exception ex)
                  {
                     _logger?.Invoke($"[Heartbeat] Clear Response Failed: {ex.Message}");
                     ok = false;
                  }
               }

               break;
         }

         if (ok)
         {
            ConsecutiveHeartbeatFailuresCount = 0;
            if (handshakeCompleted)
            {
               PostEvent(() => HeartbeatSucceeded?.Invoke());
            }
         }
         else
         {
            ConsecutiveHeartbeatFailuresCount++;
            PostEvent(() => ConsecutiveHeartbeatFailures?.Invoke(ConsecutiveHeartbeatFailuresCount));
            if (ConsecutiveHeartbeatFailuresCount >= _heartbeatFailThreshold)
            {
               _logger?.Invoke($"[Heartbeat] 連續失敗 {_heartbeatFailThreshold} 次，已停止心跳監控");
               PostEvent(() => HeartbeatFailed?.Invoke());

               // 方案 C：停止心跳循環，讓上層決定如何處理
               _heartbeatCts?.Cancel();
               return;
            }
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
               _logger?.Invoke($"[TimeSync] Failed to Update System Time (Error: {Marshal.GetLastWin32Error()})");
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

      #region Tracking Methods

      /// <summary>
      /// 讀取指定站點的追蹤資料
      /// </summary>
      public async Task<TrackingData> GetTrackingDataAsync(TrackingStation station, CancellationToken ct = default)
      {
         string address = _settings.Tracking.GetAddress(station);
         if (string.IsNullOrWhiteSpace(address))
         {
            throw new InvalidOperationException($"站點 {station} 的位址未設定");
         }

         var rawData = await _controller.ReadWordsAsync(address, 10, ct);
         return new TrackingData(rawData.ToArray());
      }

      /// <summary>
      /// 清除指定站點的追蹤資料 (寫入 0)
      /// </summary>
      public async Task ClearTrackingDataAsync(TrackingStation station, CancellationToken ct = default)
      {
         string address = _settings.Tracking.GetAddress(station);
         if (string.IsNullOrWhiteSpace(address))
         {
            throw new InvalidOperationException($"站點 {station} 的位址未設定");
         }

         short[] zeroData = new short[10];
         await _controller.WriteWordsAsync(address, zeroData, ct);
         _logger?.Invoke($"[Tracking] 已清除站點 {station} 的追蹤資料 (位址: {address})");
      }

      /// <summary>
      /// 寫入追蹤資料至指定站點 (測試用)
      /// </summary>
      public async Task WriteTrackingDataAsync(TrackingStation station, TrackingData data, CancellationToken ct = default)
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
         _logger?.Invoke($"[Tracking] 已寫入追蹤資料至站點 {station} (位址: {address})");
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
      }

      #endregion
   }
}