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
        private readonly AppPlcService _service;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public TrackingControlForm(AppPlcService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            InitializeComponent();
            InitializeStationComboBox();
        }

        private void InitializeStationComboBox()
        {
            cmbStation.DataSource = Enum.GetValues(typeof(TrackingStation));
            cmbStation.SelectedIndex = 1; // 預設選擇第一個有效站點
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
                var data = await _service.GetTrackingDataAsync(station, _cts.Token);
                
                // 更新顯示
                txtBoardId.Text = data.FormatBoardId();
                txtLayerCount.Text = data.LayerCount.ToString();
                txtLotNo.Text = data.FormatLotNo();
                txtJudgeFlags.Text = $"F1:{data.JudgeFlag1}, F2:{data.JudgeFlag2}, F3:{data.JudgeFlag3}";

                // 同時更新測試輸入欄位，方便使用者修改後寫回
                nudBoardId1.Value = data.BoardId[0];
                nudBoardId2.Value = data.BoardId[1];
                nudBoardId3.Value = data.BoardId[2];
                nudLayerCount.Value = data.LayerCount;
                txtLotChar.Text = data.LotNoChar > 0 ? ((char)data.LotNoChar).ToString() : "";
                nudLotNum.Value = data.LotNoNum;
                nudJudge1.Value = data.JudgeFlag1;
                nudJudge2.Value = data.JudgeFlag2;
                nudJudge3.Value = data.JudgeFlag3;

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
                $"確定要清除站點 [{station}] 的追蹤資料嗎？",
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
                await _service.ClearTrackingDataAsync(station, _cts.Token);
                Log($"已清除站點 {station} 的資料");
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

                // 從輸入欄位建立 TrackingData
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

                await _service.WriteTrackingDataAsync(station, data, _cts.Token);
                Log($"已寫入測試資料至站點 {station}");
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

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _cts.Cancel();
            base.OnFormClosing(e);
        }
    }
}
