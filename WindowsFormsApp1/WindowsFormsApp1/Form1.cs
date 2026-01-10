using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.CCLink.Adapters;
using WindowsFormsApp1.CCLink.Controllers;
using WindowsFormsApp1.CCLink.Interfaces;
using WindowsFormsApp1.CCLink.Models;
using WindowsFormsApp1.CCLink.Services;

namespace WindowsFormsApp1
{
   public partial class Form1 : Form
   {
      #region Fields

      private readonly CancellationTokenSource _cts = new CancellationTokenSource();
      private ICCLinkController _controller;
      private MelsecHelper _helper;
      private IMelsecApiAdapter _adapter;
      private int _path = -1;
      private ControllerSettings _settings;

      // PLC 模擬器
      private PlcSimulator _simulator;

      // 掃描監控視窗 (單例)
      private ScanMonitorForm _scanMonitorForm;

      // 連接狀態標記
      private bool _isOpened = false;

      // 事件綁定標記
      private bool _eventsbound = false;

      #endregion

      #region Constructors

      public Form1()
      {
         InitializeComponent();

         // 1. 加載或設定連線參數 (內建自動檢查檔名與開啟 UI 邏輯)
         _settings = new ControllerSettings("Settings");

         // 注意: adapter 和 helper 將在 btnOpen_Click 時根據選擇的模式初始化

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

      #endregion

      #region Protected Methods

      protected override void OnClosing(CancelEventArgs e)
      {
         _cts.Cancel();
         try
         {
            _simulator?.Stop();
            _simulator?.Dispose();
         }
         catch
         {
         }

         _helper?.Dispose();
         base.OnClosing(e);
      }

      #endregion

      #region Private Methods

      private void BindingHelperEvent()
      {
         // 防止重複綁定
         if (_eventsbound || _helper == null)
         {
            return;
         }

         // 監聽事件更新狀態燈
         _helper.Connected += () => UpdateStatus(true);
         _helper.Reconnected += () => UpdateStatus(true);
         _helper.Disconnected += () => UpdateStatus(false);

         // 註冊心跳事件以便診斷
         _helper.ConsecutiveHeartbeatFailures += count => Log($"[Helper] 心跳失敗 | Heartbeat failed (Count: {count})");
         _helper.HeartbeatFailed += () => Log("[Helper] 心跳中斷 | Heartbeat failed");
         _helper.HeartbeatSucceeded += () => Log("[Helper] 心跳成功 | Heartbeat success");

         _eventsbound = true;
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

            // 根據選擇的模式建立對應的 adapter
            if (rbMockMode.Checked)
            {
               Log("使用模擬模式 | Using Mock mode");
               _adapter = new MockMelsecApiAdapter();
            }
            else if (rbRealMode.Checked)
            {
               Log("使用實際連接模式 | Using Real PLC mode");
               _adapter = new MelsecApiAdapter();
            }
            else
            {
               throw new Exception("請選擇連接模式 | Please select connection mode");
            }

            // 建立 helper
            _helper = new MelsecHelper(_adapter, _settings, logger: s => Log($"[Helper] {s}"));
            _controller = _helper;



            // 綁定事件
            BindingHelperEvent();

            if (_settings.ScanRanges != null && _settings.ScanRanges.Count > 0)
            {
               _helper.SetScanRanges(_settings.ScanRanges);
               Log($"已從設定檔載入掃描區域 | Scan ranges loaded from config (Count: {_settings.ScanRanges.Count})");
            }

            // 開啟 helper 連接（helper.OpenAsync 會呼叫 adapter.Open 並設定 _resolvedPath）
            await _helper.OpenAsync(_cts.Token).ConfigureAwait(false);

            _isOpened = true;
            
            BeginInvoke((Action)(() =>
            {
               // 互鎖: 開啟後禁用 Open 按鈕和 RadioButton，啟用 Close 按鈕
               grpConnectionMode.Enabled = false;
               btnClose.Enabled = true;
               
               if (_scanMonitorForm != null && !_scanMonitorForm.IsDisposed)
               {
                  // 更新 Monitor 視窗的 Helper 參照，確保使用新的連線實例
                  _scanMonitorForm.SetHelper(_helper);
                  
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
                  var nudIntervalField = formType.GetField("nudUpdateInterval", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                  
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

            // 關閉 helper 連接
            if (_helper != null)
            {
               await _helper.CloseAsync(_cts.Token).ConfigureAwait(false);
            }

            _isOpened = false;
            _eventsbound = false; // 重置事件綁定標記

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
            BeginInvoke((Action)(() =>
            {
               UseWaitCursor = false;
            }));
         }
      }

      private async void btnRead_Click(object sender, EventArgs e)
      {
         try
         {
            UseWaitCursor = true;
            btnRead.Enabled = false;
            var values = await _helper.ReadWordsAsync("LW100", 10, _cts.Token).ConfigureAwait(false);
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
         _helper.StartTimeSync(
            TimeSpan.FromMilliseconds(_settings.TimeSyncIntervalMs),
            _settings.TimeSync.TriggerAddress,
            _settings.TimeSync.DataBaseAddress);

         btnStartTimeSync.Enabled = false;
         btnStopTimeSync.Enabled = true;
         Log("啟動系統時間同步監控 | System time sync monitoring started");
      }

      private void btnStopTimeSync_Click(object sender, EventArgs e)
      {
         _helper.StopTimeSync();
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

            // 使用 _controller.WriteBitsAsync 以確保 MelsecHelper 的快取能立即同步
            await _controller.WriteBitsAsync(_settings.TimeSync.TriggerAddress, new[] { true }).ConfigureAwait(true);

            await Task.Delay(1000).ConfigureAwait(true);

            // OFF
            await _controller.WriteBitsAsync(_settings.TimeSync.TriggerAddress, new[] { false }).ConfigureAwait(true);

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

            await _helper.WriteWordsAsync(_settings.TimeSync.DataBaseAddress, values, _cts.Token).ConfigureAwait(true);

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

      private async void btnStartHeartbeat_Click(object sender, EventArgs e)
      {
         try
         {
            // 確認 helper 已初始化
            if (_helper == null)
            {
               MessageBox.Show("請先點擊 Open 按鈕建立連接 | Please click Open button first", "Error");
               return;
            }

            btnStartHeartbeat.Enabled = false;
            btnStopSimulator.Enabled = true;
            btnStopHeartbeat.Enabled = true;
            // 使用表單欄位中的 helper
            // 開啟 helper（若還沒開）並用表單的 CancellationToken
            await _helper.OpenAsync(_cts.Token).ConfigureAwait(false);

            var reqAddr = new LinkDeviceAddress("LB", CCLinkConstants.DefaultRequestFlagAddress, 1);
            var respAddr = new LinkDeviceAddress("LB", CCLinkConstants.DefaultResponseFlagAddress, 1);

            // 啟動心跳
            _helper.StartHeartbeat(_settings.HeartbeatIntervalMs > 0 ? TimeSpan.FromMilliseconds(_settings.HeartbeatIntervalMs) : TimeSpan.FromSeconds(0.3),
               reqAddr, respAddr);

            // 只有在使用 Mock 模式時才建立模擬器
            if (_adapter is MockMelsecApiAdapter mockAdapter)
            {
               // 建立並保留模擬器實例（使用表單上的 mock 與 path）
               _simulator?.Stop();
               _simulator?.Dispose();
               _simulator = new PlcSimulator(mockAdapter, _path >= 0 ? _path : 1, reqAddr, respAddr, s => Log($"[PLC] {s}"));

               // 使用 StartPulse 實現心跳週期
               // period = 2 秒（完整週期），pulseMs = 1000 毫秒（ON 時間）
               // 實際行為：每個週期內，ON 維持 1 秒，然後 OFF 維持 1 秒（period - pulseMs）
               // 時序：ON (1s) -> OFF (1s) -> [下一週期] ON (1s) -> OFF (1s) ...
               _simulator.StartPulse(TimeSpan.FromSeconds(2), 1000);

               Log("測試環境已啟動 | Test environment started (Helper + PLC Simulator)");
               Log("模擬器設定詳情 | Simulator settings details (Period: 2s, ON: 1s, OFF: 1s)");
            }
            else
            {
               Log("心跳已啟動 (實際 PLC 模式) | Heartbeat started (Real PLC mode)");
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
            if (_helper != null && _settings.ScanRanges != null)
            {
               _helper.SetScanRanges(_settings.ScanRanges);
               Log($"設定已更新 | Settings updated (Reloaded: {_settings.ScanRanges.Count} Scan Ranges)");
            }
         }
      }

      private void btnStopHeartbeat_Click(object sender, EventArgs e)
      {
         _helper?.StopHeartbeat();
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
            _scanMonitorForm = new ScanMonitorForm(_helper);
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

      #endregion
   }
}