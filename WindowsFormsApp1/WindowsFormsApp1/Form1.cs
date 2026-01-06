using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.CCLink.Interfaces;
using WindowsFormsApp1.CCLink.Models;
using WindowsFormsApp1.CCLink.Adapters;
using WindowsFormsApp1.CCLink.Services;
using WindowsFormsApp1.CCLink.Controllers;

namespace WindowsFormsApp1
{
   public partial class Form1 : Form
   {
      #region Fields

      private readonly CancellationTokenSource _cts = new CancellationTokenSource();
      private readonly ControllerSettings _settings = new ControllerSettings { Isx64 = true, TimeoutMs = 3000 };
      private ICCLinkController _controller;
      private MelsecHelper _helper;
      private MockMelsecApiAdapter _mockAdapter;
      private int _path;

      // PLC 模擬器
      private PlcSimulator _simulator;

      // 動態建立的模擬器按鈕（避免修改 Designer）
      private Button btnStartSim;
      private Button btnStopSim;

      #endregion

      #region Constructors

      public Form1()
      {
         InitializeComponent();
         _controller = new MockCCLinkController(_settings);

         // prepare mock adapter and open path immediately for demo
         _mockAdapter = new MockMelsecApiAdapter();
         short r = _mockAdapter.Open(0, 0, out _path);
         _helper = new MelsecHelper(_mockAdapter, _settings, reconnectAsync: async () =>
         {
            // simple reconnect simulation: attempt to open and return true
            await Task.Delay(200).ConfigureAwait(false);
            short rr = _mockAdapter.Open(0, 0, out _path);
            return rr == 0;
         }, logger: s => Log($"[Helper] {s}"));

         // 監聽事件更新狀態燈
         _helper.Reconnected += () => UpdateStatus(true);
         _helper.Disconnected += () => UpdateStatus(false);

         // 初始顯示為已連線（mock 已 open）
         UpdateStatus(true);

         // 設定 lstLog 右鍵選單
         SetupLogContextMenu();

         // 動態建立模擬器相關按鈕（放在 UI 上，不需改 Designer）
         CreateSimulatorButtons();
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
         // 時間戳 + 訊息，最新顯示在最上方
         var text = $"{DateTime.Now:HH:mm:ss.fff} | {message}";
         BeginInvoke((Action)(() => { lstLog.Items.Insert(0, text); }));
      }

      /// <summary>開啟通訊。</summary>
      private async void btnOpen_Click(object sender, EventArgs e)
      {
         try
         {
            UseWaitCursor = true;
            btnOpen.Enabled = false;
            await _controller.OpenAsync(_cts.Token).ConfigureAwait(false);
            BeginInvoke((Action)(() => MessageBox.Show("Open OK")));
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
               btnOpen.Enabled = true;
            }));
         }
      }

      private async void btnRead_Click(object sender, EventArgs e)
      {
         try
         {
            UseWaitCursor = true;
            btnRead.Enabled = false;
            var values = await _controller.ReadWordsAsync("LW100", 10, _cts.Token).ConfigureAwait(false);
            BeginInvoke((Action)(() => lstLog.Items.Add("Read: " + string.Join(", ", values))));
         }
         catch (Exception ex)
         {
            BeginInvoke((Action)(() => lstLog.Items.Add("Read Failed: " + ex.Message)));
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

      private void btnStopHeartbeat_Click(object sender, EventArgs e)
      {
         _helper?.StopHeartbeat();
         Log("Heartbeat stopped");
      }

      private void btnStartTimeSync_Click(object sender, EventArgs e)
      {
         var reqFlag = new LinkDeviceAddress("LB", CCLinkConstants.DefaultRequestFlagAddress, 1);
         var reqData = new LinkDeviceAddress("LW", CCLinkConstants.DefaultRequestDataAddress, CCLinkConstants.DefaultRequestDataLength);
         LinkDeviceAddress respFlag = null; // assume PLC will clear request flag itself

         //_helper.TimeSyncSucceeded += () => Log("TimeSync success");
         //_helper.TimeSyncFailed += () => Log("TimeSync failed");

         _helper.StartTimeSync(TimeSpan.FromMinutes(5), reqFlag, reqData, respFlag);
         Log("TimeSync started");
      }

      private void btnForceTimeSync_Click(object sender, EventArgs e)
      {
         var reqFlag = new LinkDeviceAddress("LB", CCLinkConstants.DefaultRequestFlagAddress, 1);
         var reqData = new LinkDeviceAddress("LW", CCLinkConstants.DefaultRequestDataAddress, CCLinkConstants.DefaultRequestDataLength);
         LinkDeviceAddress respFlag = null;
         bool ok = _helper.ForceTimeSync(DateTime.Now, reqFlag, reqData, respFlag);
         Log($"ForceTimeSync returned {ok}");
      }

      // -------------------
      // PLC 模擬器相關
      // -------------------

      private void CreateSimulatorButtons()
      {
         // 建立啟動按鈕
         btnStartSim = new Button
         {
            Name = "btnStartSim",
            Text = "啟動 PLC 模擬",
            Width = 120,
            Height = 28,
            Left = 12,
            Top = lstLog.Bottom + 8
         };
         btnStartSim.Click += btnStartSim_Click;
         flowLayoutPanel1.Controls.Add(btnStartSim);

         // 建立停止按鈕
         btnStopSim = new Button
         {
            Name = "btnStopSim",
            Text = "停止 PLC 模擬",
            Width = 120,
            Height = 28,
            Left = btnStartSim.Right + 8,
            Top = btnStartSim.Top,
            Enabled = false
         };
         btnStopSim.Click += btnStopSim_Click;
         flowLayoutPanel1.Controls.Add(btnStopSim);
      }

      private void btnStartSim_Click(object sender, EventArgs e)
      {
         try
         {
            // 使用預設 LB 位址（同 heartbeat 範例）
            var reqFlag = new LinkDeviceAddress("LB", CCLinkConstants.DefaultRequestFlagAddress, 1);
            var respFlag = new LinkDeviceAddress("LB", CCLinkConstants.DefaultResponseFlagAddress, 1);

            // 若已有模擬器先清除
            try
            {
               _simulator?.Stop();
               _simulator?.Dispose();
            }
            catch
            {
            }

            _simulator = new PlcSimulator(_mockAdapter, _path, reqFlag, respFlag, s => Log(s));
            _simulator.RequestChanged += on => Log($"Simulator Request={(on ? 1 : 0)}");
            _simulator.ResponseChanged += on => Log($"Simulator Response={(on ? 1 : 0)}");

            // 範例：每秒脈衝，保持 200ms
            _simulator.StartPulse(TimeSpan.FromSeconds(1), 200);

            Log("PLC 模擬器已啟動（pulse 1s, 200ms）");
            btnStartSim.Enabled = false;
            btnStopSim.Enabled = true;
         }
         catch (Exception ex)
         {
            Log("啟動模擬器失敗: " + ex.Message);
         }
      }

      private void btnStopSim_Click(object sender, EventArgs e)
      {
         try
         {
            _simulator?.Stop();
            _simulator?.Dispose();
            _simulator = null;
            Log("PLC 模擬器已停止");
         }
         catch (Exception ex)
         {
            Log("停止模擬器失敗: " + ex.Message);
         }
         finally
         {
            btnStartSim.Enabled = true;
            btnStopSim.Enabled = false;
         }
      }

      private async void btnStartHeartbeat_Click(object sender, EventArgs e)
      {
         try
         {
            // 使用表單欄位中的 mock & helper（在 ctor 已建立）
            // 開啟 helper（若還沒開）並用表單的 CancellationToken
            await _helper.OpenAsync(_cts.Token).ConfigureAwait(false);

            var reqAddr = new LinkDeviceAddress("LB", CCLinkConstants.DefaultRequestFlagAddress, 1);
            var respAddr = new LinkDeviceAddress("LB", CCLinkConstants.DefaultResponseFlagAddress, 1);

            // 確保輪詢計畫包含 request 與 response 位址（否則 PollAddresses 不會更新這些快取）
            int start = Math.Min(reqAddr.Start, respAddr.Start);
            int end = Math.Max(reqAddr.Start, respAddr.Start);
            _helper.SetScanRanges(new[]
            {
               new MelsecHelper.ScanRange { Kind = "LB", Start = start, End = end }
            });

            // 註冊心跳事件以便診斷
            _helper.HeartbeatFailed += count => Log($"[Helper] Heartbeat failed count={count}");
            _helper.HeartbeatSucceeded += () => Log("[Helper] Heartbeat success");

            // 啟動心跳（間隔改為 1 秒，與模擬器週期同步）
            _helper.StartHeartbeat(TimeSpan.FromSeconds(0.5), reqAddr, respAddr);

            // 建立並保留模擬器實例（使用表單上的 mock 與 path）
            _simulator?.Stop();
            _simulator?.Dispose();
            _simulator = new PlcSimulator(_mockAdapter, _path >= 0 ? _path : 1, reqAddr, respAddr, s => Log($"[PLC] {s}"));

            // 使用 StartPulse 實現心跳週期
            // period = 2 秒（完整週期），pulseMs = 1000 毫秒（ON 時間）
            // 實際行為：每個週期內，ON 維持 1 秒，然後 OFF 維持 1 秒（period - pulseMs）
            // 時序：ON (1s) -> OFF (1s) -> [下一週期] ON (1s) -> OFF (1s) ...
            _simulator.StartPulse(TimeSpan.FromSeconds(2), 1000);

            Log("測試環境已啟動（helper + plc simulator）");
            Log("模擬器設定：週期 2 秒，ON 1 秒 / OFF 1 秒");
         }
         catch (Exception ex)
         {
            Log("button1_Click 初始化失敗: " + ex.Message);
         }
      }

      private void btnStopSimulator_Click(object sender, EventArgs e)
      {
         _simulator?.Stop();
         Log("模擬器已停止");
      }

      #endregion
   }
}