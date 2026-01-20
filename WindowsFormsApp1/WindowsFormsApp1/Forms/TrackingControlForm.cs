using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Models;
using WindowsFormsApp1.Services;

namespace WindowsFormsApp1.Forms
{
   public partial class TrackingControlForm : Form
   {
      #region Fields

      private readonly CancellationTokenSource _cts = new CancellationTokenSource();
      private readonly AppPlcService _service;
      private readonly PlcSimulator _simulator;

      #endregion

      #region Constructors

      public TrackingControlForm(AppPlcService service, PlcSimulator simulator = null)
      {
         _service = service ?? throw new ArgumentNullException(nameof(service));
         _simulator = simulator;
         InitializeComponent();
         InitializeStationComboBox();
         InitializeReasonCodeComboBox();

         // 訂閱維護資料接收事件
         _service.MaintenanceDataReceived += OnMaintenanceDataReceived;

         if (_simulator == null)
         {
            grpLcsSimulator.Enabled = false;
            grpLcsSimulator.Text += " (Not Available - Simulator Mode Only)";
         }
      }

      #endregion

      #region Protected Methods

      protected override void OnFormClosing(FormClosingEventArgs e)
      {
         _service.MaintenanceDataReceived -= OnMaintenanceDataReceived;
         _cts?.Cancel();
         base.OnFormClosing(e);
      }

      #endregion

      #region Private Methods

      private void InitializeStationComboBox()
      {
         cmbStation.DataSource = Enum.GetValues(typeof(TrackingStation));
         cmbStation.SelectedIndex = 1; // 預設選擇第一個有效站點
      }

      private void InitializeReasonCodeComboBox()
      {
         cboReasonCode.SelectedIndex = 0; // 預設選擇「0: 廢棄」
      }

      private async void btnDeviceSendMaint_Click(object sender, EventArgs e)
      {
         try
         {
            btnDeviceSendMaint.Enabled = false;

            if (_simulator != null)
            {
               _simulator.Stop();
               _simulator.StartTrackingMaintenanceListenMode();
            }

            // 1. Prepare Data from UI
            var data = new TrackingData
            {
               BoardId = new ushort[]
               {
                  (ushort)nudBoardId1.Value,
                  (ushort)nudBoardId2.Value,
                  (ushort)nudBoardId3.Value
               },
               LayerCount = (ushort)nudLayerCount.Value,
               LotNoChar = string.IsNullOrEmpty(txtLotChar.Text) ? (ushort)0 : (ushort)txtLotChar.Text[0],
               LotNoNum = (uint)nudLotNum.Value,
               JudgeFlag1 = (ushort)nudJudge1.Value,
               JudgeFlag2 = (ushort)nudJudge2.Value,
               JudgeFlag3 = (ushort)nudJudge3.Value
            };

            int pos = (int)nudDeviceMaintPos.Value;

            // 2. Send Request via Service
            Log($"[Device] 發送維護請求 (Pos: {pos})...");
            bool result = await _service.SendTrackingMaintenanceRequestAsync(data, pos, _cts.Token);

            if (result)
            {
               Log("[Device] 維護請求成功 (Response OK)");
               MessageBox.Show("維護請求成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
               Log("[Device] 維護請求失敗或逾時");
               MessageBox.Show("維護請求失敗或逾時", "失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
         }
         catch (Exception ex)
         {
            Log($"[Device] Error: {ex.Message}");
            MessageBox.Show($"Error: {ex.Message}");
         }
         finally
         {
            btnDeviceSendMaint.Enabled = true;
         }
      }

      private void cmbStation_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (cmbStation.SelectedItem is TrackingStation station && station != TrackingStation.Unknown)
         {
            try
            {
               txtAddress.Text = GetStationAddress(station);
               _ = RefreshDataAsync();
            }
            catch (Exception ex)
            {
               Log($"位址錯誤: {ex.Message}");
            }
         }
      }

      private string GetStationAddress(TrackingStation station)
      {
         // Access through public property or use reflection
         var settingsField = _service.GetType().GetField("_settings",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
         if (settingsField != null)
         {
            var settings = settingsField.GetValue(_service) as AppControllerSettings;
            return settings?.Tracking?.GetAddress(station) ?? "未設定";
         }

         return "未設定";
      }

      private async void btnRefresh_Click(object sender, EventArgs e)
      {
         await RefreshDataAsync();
      }

      private async Task RefreshDataAsync()
      {
         if (!(cmbStation.SelectedItem is TrackingStation station) || station == TrackingStation.Unknown)
         {
            return;
         }

         try
         {
            btnRefresh.Enabled = false;
            var data = await _service.GetMoveOutDataAsync(station, _cts.Token);

            // 更新顯示
            txtBoardId.Text = data.TrackingData.FormatBoardId();
            txtLayerCount.Text = data.TrackingData.LayerCount.ToString();
            txtLotNo.Text = data.TrackingData.FormatLotNo();
            txtJudgeFlags.Text = $"F1:{data.TrackingData.JudgeFlag1}, F2:{data.TrackingData.JudgeFlag2}, F3:{data.TrackingData.JudgeFlag3}";

            // 同時更新測試輸入欄位，方便使用者修改後寫回
            nudBoardId1.Value = data.TrackingData.BoardId[0];
            nudBoardId2.Value = data.TrackingData.BoardId[1];
            nudBoardId3.Value = data.TrackingData.BoardId[2];
            nudLayerCount.Value = data.TrackingData.LayerCount;
            txtLotChar.Text = data.TrackingData.LotNoChar > 0 ? ((char)data.TrackingData.LotNoChar).ToString() : "";
            nudLotNum.Value = data.TrackingData.LotNoNum;
            nudJudge1.Value = data.TrackingData.JudgeFlag1;
            nudJudge2.Value = data.TrackingData.JudgeFlag2;
            nudJudge3.Value = data.TrackingData.JudgeFlag3;
            cboReasonCode.SelectedIndex = Math.Min((int)data.ReasonCode, 2); // 確保在範圍內

            Log($"已刷新站點 {station} 的資料");
         }
         catch (Exception ex)
         {
            Log($"讀取失敗: {ex.Message}");
            MessageBox.Show($"讀取失敗: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
         finally
         {
            btnRefresh.Enabled = true;
         }
      }

      private async void btnClear_Click(object sender, EventArgs e)
      {
         if (!(cmbStation.SelectedItem is TrackingStation station) || station == TrackingStation.Unknown)
         {
            return;
         }

         var result = MessageBox.Show(
            $"確定要清除站點 [{station}] 的排出資料（包含追蹤資料與排出理由）嗎？",
            "確認清除",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

         if (result != DialogResult.Yes)
         {
            return;
         }

         try
         {
            btnClear.Enabled = false;

            var data = new TrackingData
            {
               BoardId = new ushort[]
               {
                  (ushort)nudBoardId1.Value,
                  (ushort)nudBoardId2.Value,
                  (ushort)nudBoardId3.Value
               },
               LayerCount = (ushort)nudLayerCount.Value,
               LotNoChar = string.IsNullOrEmpty(txtLotChar.Text) ? (ushort)0 : (ushort)txtLotChar.Text[0],
               LotNoNum = (uint)nudLotNum.Value,
               JudgeFlag1 = (ushort)nudJudge1.Value,
               JudgeFlag2 = (ushort)nudJudge2.Value,
               JudgeFlag3 = (ushort)nudJudge3.Value
            };

            await _service.ClearMoveOutDataAsync(station, data, _cts.Token);
            Log($"已清除站點 {station} 的排出資料");
            await RefreshDataAsync();
         }
         catch (Exception ex)
         {
            Log($"清除失敗: {ex.Message}");
            MessageBox.Show($"清除失敗: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
         finally
         {
            btnClear.Enabled = true;
         }
      }

      private async void btnWriteTest_Click(object sender, EventArgs e)
      {
         if (!(cmbStation.SelectedItem is TrackingStation station) || station == TrackingStation.Unknown)
         {
            return;
         }

         try
         {
            btnWriteTest.Enabled = false;

            // 從輸入欄位建立 MoveOutData
            var moveOutData = new MoveOutData
            {
               TrackingData = new TrackingData
               {
                  BoardId = new ushort[]
                  {
                     (ushort)nudBoardId1.Value,
                     (ushort)nudBoardId2.Value,
                     (ushort)nudBoardId3.Value
                  },
                  LayerCount = (ushort)nudLayerCount.Value,
                  LotNoChar = string.IsNullOrEmpty(txtLotChar.Text) ? (ushort)0 : (ushort)txtLotChar.Text[0],
                  LotNoNum = (uint)nudLotNum.Value,
                  JudgeFlag1 = (ushort)nudJudge1.Value,
                  JudgeFlag2 = (ushort)nudJudge2.Value,
                  JudgeFlag3 = (ushort)nudJudge3.Value
               },
               ReasonCode = (ushort)cboReasonCode.SelectedIndex
            };

            await _service.WriteMoveOutDataAsync(station, moveOutData, _cts.Token);
            Log($"已寫入測試資料至站點 {station} (排出理由: {cboReasonCode.SelectedItem})");
            await RefreshDataAsync();
         }
         catch (Exception ex)
         {
            Log($"寫入失敗: {ex.Message}");
            MessageBox.Show($"寫入失敗: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
         finally
         {
            btnWriteTest.Enabled = true;
         }
      }

      private async void btnQuickLoadingRobot_Click(object sender, EventArgs e)
      {
         await QuickWriteAsync(TrackingStation.LoadingRobot, 4, 4, 'A', 1234, 114, 114, 114);
      }

      private async void btnQuickLoading_Click(object sender, EventArgs e)
      {
         await QuickWriteAsync(TrackingStation.Loading, 3, 3, 'A', 1234, 113, 113, 113);
      }

      private async void btnQuickUnloading_Click(object sender, EventArgs e)
      {
         await QuickWriteAsync(TrackingStation.Unloading, 2, 2, 'A', 1234, 112, 112, 112);
      }

      private async void btnQuickUnloadingRobot_Click(object sender, EventArgs e)
      {
         await QuickWriteAsync(TrackingStation.UnloadingRobot, 1, 1, 'A', 1234, 111, 111, 111);
      }

      private async void btnSimDataMaint_Click(object sender, EventArgs e)
      {
         if (_simulator == null)
         {
            MessageBox.Show("Simulator not available.");
            return;
         }

         try
         {
            btnSimDataMaint.Enabled = false;

            // Construct TrackingData from UI inputs
            var data = new TrackingData
            {
               BoardId = new ushort[]
               {
                  (ushort)nudBoardId1.Value,
                  (ushort)nudBoardId2.Value,
                  (ushort)nudBoardId3.Value
               },
               LayerCount = (ushort)nudLayerCount.Value,
               LotNoChar = string.IsNullOrEmpty(txtLotChar.Text) ? (ushort)0 : (ushort)txtLotChar.Text[0],
               LotNoNum = (uint)nudLotNum.Value,
               JudgeFlag1 = (ushort)nudJudge1.Value,
               JudgeFlag2 = (ushort)nudJudge2.Value,
               JudgeFlag3 = (ushort)nudJudge3.Value
            };

            int pos = (int)nudSimPos.Value;

            Log($"[Simulator] Sending Maintenance Request for Pos {pos}...");
            bool output = await _simulator.TriggerTrackingMaintenanceRequest(data, pos);

            if (output)
            {
               Log($"[Simulator] Maintenance Request OK.");
               MessageBox.Show("Maintenance Request OK", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
               Log($"[Simulator] Maintenance Request Failed/NG.");
               MessageBox.Show("Maintenance Request Failed or NG", "Result", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
         }
         catch (Exception ex)
         {
            Log($"[Simulator] Request Error: {ex.Message}");
            MessageBox.Show($"Request Error: {ex.Message}");
         }
         finally
         {
            btnSimDataMaint.Enabled = true;
         }
      }

      /// <summary>
      /// 當接收到維護資料時的事件處理器
      /// </summary>
      private void OnMaintenanceDataReceived(TrackingData data, int position)
      {
         // 在 UI 執行緒上更新
         if (InvokeRequired)
         {
            Invoke(new Action(() => OnMaintenanceDataReceived(data, position)));
            return;
         }

         Log($"[Maintenance] 接收到位置 {position} 的維護資料");

         // 更新 UI 顯示接收到的資料
         nudBoardId1.Value = data.BoardId[0];
         nudBoardId2.Value = data.BoardId[1];
         nudBoardId3.Value = data.BoardId[2];
         nudLayerCount.Value = data.LayerCount;
         txtLotChar.Text = data.LotNoChar > 0 ? ((char)data.LotNoChar).ToString() : "";
         nudLotNum.Value = data.LotNoNum;
         nudJudge1.Value = data.JudgeFlag1;
         nudJudge2.Value = data.JudgeFlag2;
         nudJudge3.Value = data.JudgeFlag3;
         nudSimPos.Value = position;

         Log($"[Received] BoardId: {data.BoardId[0]}-{data.BoardId[1]}-{data.BoardId[2]}, " +
             $"Layer: {data.LayerCount}, Lot: {(char)data.LotNoChar}{data.LotNoNum}");

         MessageBox.Show($"接收到維護資料 (Position {position})", "Maintenance Data Received",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
      }

      private async Task QuickWriteAsync(TrackingStation station, ushort boardId, ushort layerCount,
         char lotChar, uint lotNum, ushort judge1, ushort judge2, ushort judge3)
      {
         try
         {
            var data = new TrackingData
            {
               BoardId = new ushort[] { boardId, boardId, boardId },
               LayerCount = layerCount,
               LotNoChar = (ushort)lotChar,
               LotNoNum = lotNum,
               JudgeFlag1 = judge1,
               JudgeFlag2 = judge2,
               JudgeFlag3 = judge3
            };

            await _service.WriteTrackingDataAsync(station, data, _cts.Token);
            Log($"快速寫入: 站點 {station} - Board:{boardId:D3}, Layer:{layerCount:D2}, Lot:{lotChar}{lotNum}");

            // 如果當前選擇的站點就是寫入的站點，自動刷新
            if (cmbStation.SelectedItem is TrackingStation currentStation && currentStation == station)
            {
               await RefreshDataAsync();
            }
         }
         catch (Exception ex)
         {
            Log($"快速寫入失敗: {ex.Message}");
            MessageBox.Show($"快速寫入失敗: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void Log(string message)
      {
         var text = $"{DateTime.Now:HH:mm:ss} | {message}";
         rtbLog.AppendText(text + Environment.NewLine);
         rtbLog.ScrollToCaret();
      }

      #endregion
   }
}