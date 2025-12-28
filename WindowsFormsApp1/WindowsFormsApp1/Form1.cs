using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.CCLink;

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
         short r = _mockAdapter.mdOpen(0, 0, out _path);
         _helper = new MelsecHelper(_mockAdapter, reconnectAsync: async () =>
         {
            // simple reconnect simulation: attempt to open and return true
            await Task.Delay(200).ConfigureAwait(false);
            short rr = _mockAdapter.mdOpen(0, 0, out _path);
            return rr == 0;
         }, logger: s => Log(s));

         // 監聽事件更新狀態燈
         _helper.Reconnected += () => UpdateStatus(true);
         _helper.Disconnected += () => UpdateStatus(false);

         // 初始顯示為已連線（mock 已 open）
         UpdateStatus(true);

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

      private void btnStartHeartbeat_Click(object sender, EventArgs e)
      {
         // start heartbeat using LB0300 (request flag) and LB0100 (response flag)
         var reqFlag = new LinkDeviceAddress("LB", CCLinkConstants.DefaultRequestFlagAddress, 1);
         var respFlag = new LinkDeviceAddress("LB", CCLinkConstants.DefaultResponseFlagAddress, 1);

         _helper.HeartbeatFailed += count => Log($"Heartbeat failed count={count}");
         _helper.HeartbeatSucceeded += () => Log("Heartbeat success");
         _helper.Disconnected += () => Log("Disconnected");
         _helper.Reconnected += () => Log("Reconnected");

         // 呼叫新的 StartHeartbeat: 不包含 RequestData
         _helper.StartHeartbeat(TimeSpan.FromSeconds(5), reqFlag, respFlag, () => _path, failThreshold: 3);

         Log("Heartbeat started");
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

         _helper.StartTimeSync(TimeSpan.FromMinutes(5), reqFlag, reqData, respFlag, () => _path);
         Log("TimeSync started");
      }

      private void btnForceTimeSync_Click(object sender, EventArgs e)
      {
         var reqFlag = new LinkDeviceAddress("LB", CCLinkConstants.DefaultRequestFlagAddress, 1);
         var reqData = new LinkDeviceAddress("LW", CCLinkConstants.DefaultRequestDataAddress, CCLinkConstants.DefaultRequestDataLength);
         LinkDeviceAddress respFlag = null;
         bool ok = _helper.ForceTimeSync(DateTime.Now, reqFlag, reqData, respFlag, () => _path);
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

      #endregion
   }
}
