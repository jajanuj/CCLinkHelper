using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.CCLink.Forms;
using WindowsFormsApp1.CCLink.Models;
using WindowsFormsApp1.CCLink.Services;
using WindowsFormsApp1.Forms;
using WindowsFormsApp1.Models;
using WindowsFormsApp1.Services;
using Timer = System.Threading.Timer;

namespace WindowsFormsApp1
{
   public partial class Form1 : Form
   {
      #region Fields

      private readonly CancellationTokenSource _cts = new CancellationTokenSource();
      private ushort _actionStatus = 2; // 預設：基板搬送中

      // 定期上報 Status1 設定值
      private ushort _alarmStatus = 4; // 預設：無警報
      private AppPlcService _appPlcService;
      private ushort _boardThicknessStatus;
      private ushort _controlStatus = 1; // 預設：自動運轉
      private string _currentRecipeName = "DEFAULT_RECIPE";
      private ushort _currentRecipeNo = 1;
      private ushort _dischargeRate = 85; // 預設：85%
      private ushort _downstreamWaitingStatus = 1;

      // 定期上報 Alarm 設定值（12個錯誤代碼）
      private ushort[] _errorCodes = new ushort[12];

      // 事件綁定標記
      private bool _eventsBound;
      private ushort _greenLightStatus = 3; // 預設：閃爍

      // 連接狀態標記
      private bool _isOpened;
      private ushort _machineStatus = 4; // 預設：生產中
      private int _path = -1;
      private uint _processingCounter;

      // 定期上報 Status2 設定值
      private ushort _redLightStatus;
      private ushort _retainedBoardCount;

      // 掃描監控視窗 (單例)
      private ScanMonitorForm _scanMonitorForm;
      private AppControllerSettings _settings;

      // PLC 模擬器
      private PlcSimulator _simulator;
      private ushort _stopTime;
      private Timer _updateTimer;
      private ushort _upstreamWaitingStatus = 1;
      private ushort _waitingStatus = 1; // 預設：無等待
      private ushort _yellowLightStatus;

      #endregion

      #region Constructors

      public Form1()
      {
         InitializeComponent();

         // 1. 加載或設定連線參數 (內建自動檢查檔名與開啟 UI 邏輯)
         _settings = new AppControllerSettings("Settings");

         // Initialize Driver Type ComboBox
         cmbDriverType.DataSource = Enum.GetValues(typeof(MelsecDriverType));
         cmbDriverType.SelectedItem = _settings.DriverType;

         // 設定 lstLog 右鍵選單
         SetupLogContextMenu();

         // 初始狀態為未連接
         UpdateStatus(false);
      }

      #endregion

      #region Public Methods

      public void UpdateStatus(bool connected)
      {
         if (InvokeRequired)
         {
            BeginInvoke((Action)(() => UpdateStatus(connected)));
            return;
         }

         // now running on UI thread and handle exists (InvokeRequired false implies handle exists)
         lblStatus.Text = connected ? "Connected" : "Disconnected";
         lblStatus.BackColor = connected ? System.Drawing.Color.LightGreen : System.Drawing.Color.LightCoral;
      }

      public void StartCommonReporting()
      {
         // 每 100ms 更新一次（頻率可調整）
         _updateTimer = new Timer(_ =>
         {
            // 更新 Status1（機台狀態）
            _appPlcService.UpdateStatus1(
               alarmStatus: GetCurrentAlarmStatus(),     // 1-4
               machineStatus: GetCurrentMachineStatus(), // 1-6
               actionStatus: GetCurrentActionStatus(),   // 1,2,99
               waitingStatus: GetCurrentWaitingStatus(), // 1-5,99
               controlStatus: GetCurrentControlStatus()  // 1-5,99
            );

            // 更新 Status2（詳細狀態）
            _appPlcService.UpdateStatus2(
               redLight: GetRedLightStatus(),       // 0-3
               yellowLight: GetYellowLightStatus(), // 0-3
               greenLight: GetGreenLightStatus(),   // 0-3
               upstreamWait: GetUpstreamWaiting(),
               downstreamWait: GetDownstreamWaiting(),
               dischargeRate: GetDischargeRate(),
               stopTime: GetStopTime(),
               processingCounter: GetProcessingCounter(), // UINT32
               retainedBoard: GetRetainedBoardCount(),
               currentRecipeNo: GetCurrentRecipeNo(),
               boardThickness: GetBoardThickness(),
               uldFlag: 0,                        // 固定填入 0
               recipeName: GetCurrentRecipeName() // 最多100字元
            );

            // 更新 Alarm（警報資料）
            ushort[] errorCodes = GetCurrentErrorCodes(); // 12個錯誤代碼
            _appPlcService.UpdateAlarm(errorCodes);
         }, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(100));
      }

      public void StartLinkDataReporting()
      {
      }

      #endregion

      #region Protected Methods

      protected override void OnClosing(CancelEventArgs e)
      {
         // 1. 取消後台任務
         _cts.Cancel();

         // 2. 停止並釋放定期更新 Timer
         try
         {
            if (_updateTimer != null)
            {
               _updateTimer.Change(Timeout.Infinite, Timeout.Infinite);
               _updateTimer.Dispose();
               _updateTimer = null;
            }
         }
         catch (Exception ex)
         {
            System.Diagnostics.Debug.WriteLine($"Error disposing timer: {ex.Message}");
         }

         // 3. 關閉 Monitor 視窗 (若存在)
         try
         {
            if (_scanMonitorForm != null && !_scanMonitorForm.IsDisposed)
            {
               _scanMonitorForm.Close();
               _scanMonitorForm.Dispose();
               _scanMonitorForm = null;
            }
         }
         catch (Exception ex)
         {
            System.Diagnostics.Debug.WriteLine($"Error closing ScanMonitor: {ex.Message}");
         }

         // 4. 停止模擬器 (若存在)
         try
         {
            _simulator?.Stop();
            _simulator?.Dispose();
         }
         catch (Exception ex)
         {
            System.Diagnostics.Debug.WriteLine($"Error stopping Simulator: {ex.Message}");
         }

         // 5. 釋放 Service
         try
         {
            _appPlcService?.Dispose();
         }
         catch (Exception ex)
         {
            System.Diagnostics.Debug.WriteLine($"Error disposing AppPlcService: {ex.Message}");
         }

         // 6. 釋放 Controller
         try
         {
            (_appPlcService?.Controller as IDisposable)?.Dispose();
         }
         catch (Exception ex)
         {
            System.Diagnostics.Debug.WriteLine($"Error disposing Controller: {ex.Message}");
         }

         base.OnClosing(e);
      }

      #endregion

      #region Private Methods

      private ushort GetCurrentControlStatus() => _controlStatus;
      private ushort GetCurrentWaitingStatus() => _waitingStatus;
      private ushort GetCurrentActionStatus() => _actionStatus;
      private ushort GetCurrentMachineStatus() => _machineStatus;
      private ushort GetCurrentAlarmStatus() => _alarmStatus;

      // Status2 資料生成方法
      private ushort GetRedLightStatus() => _redLightStatus;
      private ushort GetYellowLightStatus() => _yellowLightStatus;
      private ushort GetGreenLightStatus() => _greenLightStatus;
      private ushort GetUpstreamWaiting() => _upstreamWaitingStatus;
      private ushort GetDownstreamWaiting() => _downstreamWaitingStatus;
      private ushort GetDischargeRate() => _dischargeRate;
      private ushort GetStopTime() => _stopTime;
      private uint GetProcessingCounter() => _processingCounter;
      private ushort GetRetainedBoardCount() => _retainedBoardCount;
      private ushort GetCurrentRecipeNo() => _currentRecipeNo;
      private ushort GetBoardThickness() => _boardThicknessStatus;
      private string GetCurrentRecipeName() => _currentRecipeName;

      // Alarm 資料生成方法
      private ushort[] GetCurrentErrorCodes() => _errorCodes;

      private void BindingHelperEvent()
      {
         // 防止重複綁定
         if (_eventsBound || _appPlcService == null)
         {
            return;
         }

         // 監聽事件更新狀態燈 (需轉型或依賴 Service 通知)
         // MelsecHelper 有 Connected 事件，但 ICCLinkController 沒有
         // AppPlcService 也不代理 Connected?
         // 暫時解決方案：檢查 Controller 類型並綁定，或依賴介面的擴充 (若有)
         // 之前我們在 MelsecHelper 加入了 interface events? No, Interface doesn't have them yet.
         // 但 MelsecHelper implementation has them.
         // MxComponentAdapter implementation doesn't have them visible (Wait, I added GetStatusAsync but no events).
         // 為了讓 UI 更新，我們可以使用 Service 的狀態? Service 沒有暴露 Connection Status Changed.
         // Quick Fix: Cast controller to known types with events OR rely on Poll timer to check status?
         // Poll Timer (GetStatusAsync) is cleaner but slower update.
         // Helper event is instant.

         if (_appPlcService.Controller is MelsecHelper helper)
         {
            helper.Connected += () => UpdateStatus(true);
            helper.Disconnected += () => UpdateStatus(false);
         }
         // MxComponentAdapter could have events if we add them, but interface doesn't enforce.
         // We can use a timer to check GetStatusAsync if needed, or assume Connected after OpenAsync returns successfully.
         // After OpenAsync success above, we call UpdateStatus(true) conceptually?
         // btnOpen handles success, but Disconnect might happen async.

         // Fallback: If no event, we rely on manual UpdateStatus calls in Open/Close.
         // btnOpen_Click sets _isOpened = true and valid.
         // If unexpected disconnect happens, we might miss it without events.

         // 註冊心跳事件以便診斷
         _appPlcService.ConsecutiveHeartbeatFailures += count => Log($"[Helper] 心跳失敗 | Heartbeat failed (Count: {count})");
         _appPlcService.HeartbeatFailed += () => Log("[Helper] 心跳中斷 | Heartbeat failed");
         _appPlcService.HeartbeatSucceeded += () => Log("[Helper] 心跳成功 | Heartbeat success");

         _eventsBound = true;
      }

      private void SetupLogContextMenu()
      {
         var contextMenu = new ContextMenuStrip();
         var copyItem = new ToolStripMenuItem("複製文字");
         copyItem.Click += (s, e) =>
         {
            if (lstLog.Items.Count > 0)
            {
               var allText = string.Join(Environment.NewLine, System.Linq.Enumerable.Cast<object>(lstLog.Items));
               Clipboard.SetText(allText);
               MessageBox.Show("已將所有 Log 複製到剪貼簿。");
            }
         };
         contextMenu.Items.Add(copyItem);
         lstLog.ContextMenuStrip = contextMenu;
      }

      private void Log(string message)
      {
         var text = $"{DateTime.Now:HH:mm:ss.fff} | {message}";
         if (InvokeRequired)
         {
            if (IsHandleCreated)
            {
               BeginInvoke((Action)(() => Log(message)));
            }
         }
         else
         {
            lstLog.Items.Insert(0, text);
         }
      }

      private void cmbDriverType_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (cmbDriverType.SelectedItem is MelsecDriverType type)
         {
            _settings.DriverType = type;
            Log($"已切換驅動模式至: {type} | Driver mode switched to: {type}");
         }
      }

      /// <summary>開啟通訊。</summary>
      private async void btnOpen_Click(object sender, EventArgs e)
      {
         try
         {
            if (_isOpened)
            {
               Log("連接已開啟,無需重複操作 | Connection already opened");
               return;
            }

            btnOpen.Enabled = false;

            // Log current mode
            Log($"準備連接... 模式: {_settings.DriverType} | Connecting... Mode: {_settings.DriverType}");

            // 建立 Service (Service 會根據 Settings 建立 Controller)
            _appPlcService = new AppPlcService(_settings, logger: s => Log($"[Service] {s}"));

            // 綁定事件
            BindingHelperEvent();

            // Set Scan Ranges (Platform agnostic)
            if (_settings.ScanRanges != null && _settings.ScanRanges.Count > 0)
            {
               _appPlcService.Controller.SetScanRanges(_settings.ScanRanges);
               Log($"已從設定檔載入掃描區域 | Scan ranges loaded from config (Count: {_settings.ScanRanges.Count})");
            }

            // 開啟 Helper 連接
            await _appPlcService.Controller.OpenAsync(_cts.Token).ConfigureAwait(false);

            _isOpened = true;

            BeginInvoke((Action)(() =>
            {
               // 互鎖: 開啟後禁用 Open 按鈕和 RadioButton，啟用 Close 按鈕
               grpConnectionMode.Enabled = false;
               btnClose.Enabled = true;

               if (_scanMonitorForm != null && !_scanMonitorForm.IsDisposed)
               {
                  // 更新 Monitor 視窗的 Controller 參照
                  _scanMonitorForm.SetController(_appPlcService.Controller);

                  // 自動啟動監控
                  _scanMonitorForm.StartMonitoring();
               }
            }));
         }
         catch (Exception ex)
         {
            BeginInvoke((Action)(() => MessageBox.Show(ex.Message, "Open Failed")));
         }
         finally
         {
            BeginInvoke((Action)(() =>
            {
               UseWaitCursor = false;
               // 只有在開啟失敗時才重新啟用按鈕
               if (!_isOpened)
               {
                  btnOpen.Enabled = true;
               }
            }));
         }
      }

      /// <summary>關閉通訊。</summary>
      private async void btnClose_Click(object sender, EventArgs e)
      {
         try
         {
            if (!_isOpened)
            {
               Log("連接未開啟 | Connection not opened");
               return;
            }

            btnClose.Enabled = false;

            // 先在 UI 執行緒上停止 Timer 並重置按鈕狀態
            if (_scanMonitorForm != null && !_scanMonitorForm.IsDisposed)
            {
               try
               {
                  var formType = _scanMonitorForm.GetType();

                  // 停止 Timer
                  var timerField = formType.GetField("_updateTimer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                  if (timerField != null)
                  {
                     var timer = timerField.GetValue(_scanMonitorForm) as System.Windows.Forms.Timer;
                     if (timer != null && timer.Enabled)
                     {
                        timer.Stop();
                        Log("[Close] 已暫停 ScanMonitor Timer | ScanMonitor timer stopped");
                     }
                  }

                  // 重置按鈕狀態：啟用 Start 按鈕，禁用 Stop 按鈕
                  var btnStartField = formType.GetField("btnStartUpdate", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                  var btnStopField = formType.GetField("btnStopUpdate", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                  var nudIntervalField = formType.GetField("nudUpdateInterval",
                     System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                  if (btnStartField != null && btnStopField != null)
                  {
                     var btnStart = btnStartField.GetValue(_scanMonitorForm) as Button;
                     var btnStop = btnStopField.GetValue(_scanMonitorForm) as Button;
                     var nudInterval = nudIntervalField?.GetValue(_scanMonitorForm) as NumericUpDown;

                     if (btnStart != null && btnStop != null)
                     {
                        btnStart.Enabled = true;
                        btnStop.Enabled = false;

                        if (nudInterval != null)
                        {
                           nudInterval.Enabled = true;
                        }

                        Log("[Close] 已重置 ScanMonitor 按鈕狀態 | ScanMonitor button states reset");
                     }
                  }
               }
               catch (Exception ex)
               {
                  Log($"[Close] 重置 ScanMonitor 狀態時發生錯誤 | Error resetting ScanMonitor state (Error: {ex.Message})");
               }
            }

            // 關閉 service
            _appPlcService?.Dispose();

            // 關閉 Controller 連接
            if (_appPlcService?.Controller != null)
            {
               await _appPlcService.Controller.CloseAsync(_cts.Token).ConfigureAwait(false);
            }

            _isOpened = false;
            _eventsBound = false; // 重置事件綁定標記

            BeginInvoke((Action)(() =>
            {
               // 互鎖: 關閉後啟用 Open 按鈕和 RadioButton，禁用 Close 按鈕
               grpConnectionMode.Enabled = true;
               btnOpen.Enabled = true;
               btnClose.Enabled = false;
            }));
         }
         catch (Exception ex)
         {
            BeginInvoke((Action)(() => MessageBox.Show(ex.Message, "Close Failed")));
         }
         finally
         {
            BeginInvoke((Action)(() => { UseWaitCursor = false; }));
         }
      }

      private async void btnRead_Click(object sender, EventArgs e)
      {
         try
         {
            UseWaitCursor = true;
            btnRead.Enabled = false;
            var values = await _appPlcService.Controller.ReadWordsAsync("LW100", 10, _cts.Token).ConfigureAwait(false);
            BeginInvoke((Action)(() => Log("讀取成功 | Read success (Values: " + string.Join(", ", values) + ")")));
         }
         catch (Exception ex)
         {
            BeginInvoke((Action)(() => Log("讀取失敗 | Read failed (Error: " + ex.Message + ")")));
         }
         finally
         {
            BeginInvoke((Action)(() =>
            {
               UseWaitCursor = false;
               btnRead.Enabled = true;
            }));
         }
      }

      private void btnStartTimeSync_Click(object sender, EventArgs e)
      {
         _appPlcService.StartTimeSync(
            TimeSpan.FromMilliseconds(_settings.TimeSyncIntervalMs),
            _settings.TimeSync.TriggerAddress,
            _settings.TimeSync.DataBaseAddress);

         btnStartTimeSync.Enabled = false;
         btnStopTimeSync.Enabled = true;
         Log("啟動系統時間同步監控 | System time sync monitoring started");
      }

      private void btnStopTimeSync_Click(object sender, EventArgs e)
      {
         _appPlcService.StopTimeSync();
         btnStartTimeSync.Enabled = true;
         btnStopTimeSync.Enabled = false;
         Log("停止系統時間同步監控 | System time sync monitoring stopped");
      }

      private async void btnForceTimeSync_Click(object sender, EventArgs e)
      {
         try
         {
            btnForceTimeSync.Enabled = false;
            Log($"手動觸發對時脈衝 | Manually triggering time sync pulse (Addr: {_settings.TimeSync.TriggerAddress})");

            // 使用 Controller.WriteBitsAsync
            await _appPlcService.Controller.WriteBitsAsync(_settings.TimeSync.TriggerAddress, new[] { true }).ConfigureAwait(true);

            await Task.Delay(1000).ConfigureAwait(true);

            // OFF
            await _appPlcService.Controller.WriteBitsAsync(_settings.TimeSync.TriggerAddress, new[] { false }).ConfigureAwait(true);

            Log("對時脈衝完成 | Time sync pulse completed (1s)");
         }
         catch (Exception ex)
         {
            Log($"強制對時發生例外 | Exception during manual time sync trigger (Error: {ex.Message})");
         }
         finally
         {
            btnForceTimeSync.Enabled = true;
         }
      }

      private void btnSyncFromPc_Click(object sender, EventArgs e)
      {
         DateTime now = DateTime.Now;
         dtpDate.Value = now;
         dtpTime.Value = now;
         Log("已同步電腦當前時間至 UI | PC time synced to UI");
      }

      private async void btnSetTimeToPlc_Click(object sender, EventArgs e)
      {
         try
         {
            btnSetTimeToPlc.Enabled = false;
            DateTime dt = dtpDate.Value.Date + dtpTime.Value.TimeOfDay;

            // 準備寫入資料 (LW0000..LW0006)
            // 0: 年, 1: 月, 2: 日, 3: 時, 4: 分, 5: 秒, 6: 週 (0-6)
            short[] values = new short[7];
            values[0] = (short)dt.Year;
            values[1] = (short)dt.Month;
            values[2] = (short)dt.Day;
            values[3] = (short)dt.Hour;
            values[4] = (short)dt.Minute;
            values[5] = (short)dt.Second;
            values[6] = (short)dt.DayOfWeek; // DateTime.DayOfWeek is 0=Sunday..6=Saturday, matches requirements

            Log($"手動設定時間至 PLC | Manually setting time to PLC (Addr: {_settings.TimeSync.DataBaseAddress}, Time: {dt:yyyy-MM-dd HH:mm:ss})");

            await _appPlcService.Controller.WriteWordsAsync(_settings.TimeSync.DataBaseAddress, values, _cts.Token).ConfigureAwait(true);

            Log("手動設定時間成功 | Manual time set successful");
         }
         catch (Exception ex)
         {
            Log($"手動設定時間發生例外 | Exception during manual time set (Error: {ex.Message})");
         }
         finally
         {
            btnSetTimeToPlc.Enabled = true;
         }
      }

      // -------------------
      // PLC 模擬器相關
      // -------------------

      private void btnStartHeartbeat_Click(object sender, EventArgs e)
      {
         try
         {
            // 確認 helper 已初始化
            if (_appPlcService?.Controller == null)
            {
               MessageBox.Show("請先點擊 Open 按鈕建立連接 | Please click Open button first", "Error");
               return;
            }

            btnStartHeartbeat.Enabled = false;
            btnStopSimulator.Enabled = true;
            btnStopHeartbeat.Enabled = true;
            // 使用表單欄位中的 helper

            // 開啟 Controller（若還沒開）
            // This should already be open if btnOpen_Click was successful.
            // await _appPlcService.Controller.OpenAsync(_cts.Token).ConfigureAwait(false);

            // 這裡的邏輯有點混亂，因為 Simulator 應該是獨立的。
            // 如果 Controller 是 Mock，則我們可能需要啟動 Simulator 來產生 Pulse。
            // 但我們已經移除了直接存取 _helper。
            // _appPlcService.Controller 就是當前的 Controller。

            var reqAddr = new LinkDeviceAddress("LB", CCLinkConstants.DefaultRequestFlagAddress, 1);
            var respAddr = new LinkDeviceAddress("LB", CCLinkConstants.DefaultResponseFlagAddress, 1);

            // 啟動心跳
            _appPlcService.StartHeartbeat(
               _settings.HeartbeatIntervalMs > 0 ? TimeSpan.FromMilliseconds(_settings.HeartbeatIntervalMs) : TimeSpan.FromSeconds(0.3),
               reqAddr.ToString(), respAddr.ToString());

            // 模擬器邏輯: 只有在 DriverType 為 Simulator 時才啟動 PlcSimulator?
            // 此處 _adapter 已經被移除，我們無法檢查 _adapter is MockMelsecApiAdapter。
            // 而是檢查 _settings.DriverType 或 Controller 類型。

            if (_settings.DriverType == MelsecDriverType.Simulator)
            {
               // In Simulator mode, we might want to start Pulse Generator
               // For now, Simulator logic is pending refactor.
               if (_appPlcService.Controller is MelsecHelper helper) // Try cast
               {
                  Log("警告: 模擬器脈衝產生器暫時無法啟動 (架構調整中) | Warning: Simulator pulse generator unavailable");
               }
            }
         }
         catch (Exception ex)
         {
            btnPlcSettings.Enabled = true;
            Log("環境初始化失敗 | Environment initialization failed (Error: " + ex.Message + ")");
         }
      }

      private void btnStopSimulator_Click(object sender, EventArgs e)
      {
         btnStartHeartbeat.Enabled = true;
         btnStopSimulator.Enabled = false;
         _simulator?.Stop();
         Log("模擬器已停止 | Simulator stopped");
      }

      private void btnPlcSettings_Click(object sender, EventArgs e)
      {
         if (_settings.ShowDialog("Settings") == DialogResult.OK)
         {
            if (_appPlcService != null && _settings.ScanRanges != null)
            {
               _appPlcService.Controller.SetScanRanges(_settings.ScanRanges);
               Log($"設定已更新 | Settings updated (Reloaded: {_settings.ScanRanges.Count} Scan Ranges)");
            }
         }
      }

      private void btnStopHeartbeat_Click(object sender, EventArgs e)
      {
         _appPlcService?.StopHeartbeat();
         _simulator?.Stop();

         btnStartHeartbeat.Enabled = true;
         btnStopSimulator.Enabled = false;
         btnStopHeartbeat.Enabled = false;

         Log("心跳與模擬器已停止 | Heartbeat and Simulator stopped");
      }

      private void btnScanMonitor_Click(object sender, EventArgs e)
      {
         // 單例模式: 只允許開啟一個監控視窗
         if (_scanMonitorForm == null || _scanMonitorForm.IsDisposed)
         {
            if (_appPlcService?.Controller == null)
            {
               MessageBox.Show("請先連接 PLC | Please connect PLC first");
               return;
            }

            _scanMonitorForm = new ScanMonitorForm(_appPlcService.Controller);
            _scanMonitorForm.FormClosed += (s, args) => _scanMonitorForm = null;
            _scanMonitorForm.Show();
            Log("已開啟掃描監控視窗 | Scan monitor window opened");
         }
         else
         {
            _scanMonitorForm.Activate();
            _scanMonitorForm.BringToFront();
            Log("掃描監控視窗已存在,切換至前景 | Scan monitor window already exists, bringing to front");
         }
      }

      private void btnSetCommonReporting_Click(object sender, EventArgs e)
      {
         // Create status objects from current fields
         var s1 = new CommonReportStatus1(_alarmStatus, _machineStatus, _actionStatus, _waitingStatus, _controlStatus);
         var s2 = new CommonReportStatus2(
            _redLightStatus, _yellowLightStatus, _greenLightStatus,
            _upstreamWaitingStatus, _downstreamWaitingStatus,
            _dischargeRate, _stopTime, _processingCounter,
            _retainedBoardCount, _currentRecipeNo, _boardThicknessStatus,
            0, // UldFlag (Always 0 as wasn't exposed)
            _currentRecipeName);
         var alarm = new CommonReportAlarm(_errorCodes);

         // Open Consolidated Form
         using (var form = new CommonReportSettingsForm(s1, s2, alarm))
         {
            if (form.ShowDialog(this) != DialogResult.OK)
            {
               return;
            }

            // 1. Update Status 1
            var newS1 = form.Status1Result;
            if (_alarmStatus != newS1.AlarmStatus)
            {
               Log($"[Status1] Alarm Changed: {_alarmStatus} -> {newS1.AlarmStatus}");
            }

            if (_machineStatus != newS1.MachineStatus)
            {
               Log($"[Status1] MachineStatus Changed: {_machineStatus} -> {newS1.MachineStatus}");
            }

            if (_actionStatus != newS1.ActionStatus)
            {
               Log($"[Status1] ActionStatus Changed: {_actionStatus} -> {newS1.ActionStatus}");
            }

            if (_waitingStatus != newS1.WaitingStatus)
            {
               Log($"[Status1] WaitingStatus Changed: {_waitingStatus} -> {newS1.WaitingStatus}");
            }

            if (_controlStatus != newS1.ControlStatus)
            {
               Log($"[Status1] ControlStatus Changed: {_controlStatus} -> {newS1.ControlStatus}");
            }

            _alarmStatus = newS1.AlarmStatus;
            _machineStatus = newS1.MachineStatus;
            _actionStatus = newS1.ActionStatus;
            _waitingStatus = newS1.WaitingStatus;
            _controlStatus = newS1.ControlStatus;

            // 2. Update Status 2
            var newS2 = form.Status2Result;
            if (_redLightStatus != newS2.RedLightStatus)
            {
               Log($"[Status2] RedLight Changed: {_redLightStatus} -> {newS2.RedLightStatus}");
            }

            if (_yellowLightStatus != newS2.YellowLightStatus)
            {
               Log($"[Status2] YellowLight Changed: {_yellowLightStatus} -> {newS2.YellowLightStatus}");
            }

            if (_greenLightStatus != newS2.GreenLightStatus)
            {
               Log($"[Status2] GreenLight Changed: {_greenLightStatus} -> {newS2.GreenLightStatus}");
            }

            if (_upstreamWaitingStatus != newS2.UpstreamWaitingStatus)
            {
               Log($"[Status2] UpstreamWaiting Changed: {_upstreamWaitingStatus} -> {newS2.UpstreamWaitingStatus}");
            }

            if (_downstreamWaitingStatus != newS2.DownstreamWaitingStatus)
            {
               Log($"[Status2] DownstreamWaiting Changed: {_downstreamWaitingStatus} -> {newS2.DownstreamWaitingStatus}");
            }

            if (_dischargeRate != newS2.DischargeRate)
            {
               Log($"[Status2] DischargeRate Changed: {_dischargeRate} -> {newS2.DischargeRate}");
            }

            if (_stopTime != newS2.StopTime)
            {
               Log($"[Status2] StopTime Changed: {_stopTime} -> {newS2.StopTime}");
            }

            if (_processingCounter != newS2.ProcessingCounter)
            {
               Log($"[Status2] ProcessingCounter Changed: {_processingCounter} -> {newS2.ProcessingCounter}");
            }

            if (_retainedBoardCount != newS2.RetainedBoardCount)
            {
               Log($"[Status2] RetainedBoardCount Changed: {_retainedBoardCount} -> {newS2.RetainedBoardCount}");
            }

            if (_currentRecipeNo != newS2.CurrentRecipeNo)
            {
               Log($"[Status2] CurrentRecipeNo Changed: {_currentRecipeNo} -> {newS2.CurrentRecipeNo}");
            }

            if (_boardThicknessStatus != newS2.BoardThicknessStatus)
            {
               Log($"[Status2] BoardThickness Changed: {_boardThicknessStatus} -> {newS2.BoardThicknessStatus}");
            }

            if (_currentRecipeName != newS2.CurrentRecipeName)
            {
               Log($"[Status2] RecipeName Changed: {_currentRecipeName} -> {newS2.CurrentRecipeName}");
            }

            _redLightStatus = newS2.RedLightStatus;
            _yellowLightStatus = newS2.YellowLightStatus;
            _greenLightStatus = newS2.GreenLightStatus;
            _upstreamWaitingStatus = newS2.UpstreamWaitingStatus;
            _downstreamWaitingStatus = newS2.DownstreamWaitingStatus;
            _dischargeRate = newS2.DischargeRate;
            _stopTime = newS2.StopTime;
            _processingCounter = newS2.ProcessingCounter;
            _retainedBoardCount = newS2.RetainedBoardCount;
            _currentRecipeNo = newS2.CurrentRecipeNo;
            _boardThicknessStatus = newS2.BoardThicknessStatus;
            _currentRecipeName = newS2.CurrentRecipeName;

            // 3. Update Alarm
            var newAlarm = form.AlarmResult;
            for (int i = 0; i < 12 && i < newAlarm.ErrorCodes.Length; i++)
            {
               if (_errorCodes[i] != newAlarm.ErrorCodes[i])
               {
                  Log($"[Alarm] Error Code {i + 1} Changed: {_errorCodes[i]} -> {newAlarm.ErrorCodes[i]}");
                  _errorCodes[i] = newAlarm.ErrorCodes[i];
               }
            }

            Log("Common Reporting Settings Updated.");

            if (_updateTimer == null)
            {
               StartCommonReporting();
            }
         }
      }

      private void btnStartCommonReporting_Click(object sender, EventArgs e)
      {
         StartCommonReporting();
      }

      private void btnTrackingControl_Click(object sender, EventArgs e)
      {
         if (_appPlcService == null)
         {
            MessageBox.Show("請先連接 PLC | Please connect PLC first", "錯誤");
            return;
         }

         using (var form = new Forms.TrackingControlForm(_appPlcService))
         {
            form.ShowDialog(this);
         }
      }

      #endregion
   }
}