using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using WindowsFormsApp1.CCLink.Models;
using WindowsFormsApp1.CCLink.Services;

namespace WindowsFormsApp1
{
   public partial class ScanMonitorForm : Form
   {
      #region Fields

      private MelsecHelper _helper;
      private readonly Timer _updateTimer;
      private readonly List<MonitorItem> _monitorItems = new List<MonitorItem>();
      private bool _isHexFormat = false;

      #endregion

      #region Constructors

      public ScanMonitorForm(MelsecHelper helper)
      {
         InitializeComponent();

         _helper = helper ?? throw new ArgumentNullException(nameof(helper));
         _updateTimer = new Timer { Interval = (int)nudUpdateInterval.Value };
         _updateTimer.Tick += UpdateTimer_Tick;

         InitializeMonitorItems();
         LoadDataToGrid();
      }

      #endregion

      #region Public Methods

      /// <summary>
      /// 更新 Helper 實例（當重新連線產生新的 Helper 時使用）。
      /// </summary>
      public void SetHelper(MelsecHelper helper)
      {
         _helper = helper ?? throw new ArgumentNullException(nameof(helper));
         
         // 更新標籤顯示新的 HelperID
         int helperHashCode = RuntimeHelpers.GetHashCode(_helper);
         // 保留原本的文字前綴，只更新 ID 部分，或直接重置
         lblLastUpdate.Text = $"Helper Updated (ID: {helperHashCode})";
      }

      /// <summary>
      /// 啟動監控（供外部呼叫）。
      /// </summary>
      public void StartMonitoring()
      {
         if (InvokeRequired)
         {
            BeginInvoke((Action)StartMonitoring);
            return;
         }

         // 直接呼叫 StartUpdate 按鈕的事件處理邏輯
         btnStartUpdate_Click(this, EventArgs.Empty);
      }

      #endregion

      #region Private Methods

      /// <summary>
      /// 從 MelsecHelper 的掃描範圍初始化監控項目。
      /// </summary>
      private void InitializeMonitorItems()
      {
         _monitorItems.Clear();

         // 從 helper 的設定中取得掃描範圍
         var settings = _helper.GetType()
            .GetField("_settings", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.GetValue(_helper) as ControllerSettings;

         if (settings?.ScanRanges == null || settings.ScanRanges.Count == 0)
         {
            MessageBox.Show("No scan ranges configured. Please configure scan ranges in settings first.",
               "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
         }

         foreach (var range in settings.ScanRanges)
         {
            string kind = range.Kind?.ToUpper() ?? "";
            bool isBit = kind == "LB" || kind == "LX" || kind == "LY";
            bool isWord = kind == "LW";

            if (!isBit && !isWord)
               continue;

            for (int addr = range.Start; addr <= range.End; addr++)
            {
               _monitorItems.Add(new MonitorItem
               {
                  Kind = kind,
                  Index = addr,
                  Type = isBit ? "Bit" : "Word",
                  IsBit = isBit
               });
            }
         }

         // 排序: 先按 Kind (LB, LW, LX, LY), 再按 Index
         _monitorItems.Sort((a, b) =>
         {
            int kindCompare = string.Compare(a.Kind, b.Kind, StringComparison.Ordinal);
            return kindCompare != 0 ? kindCompare : a.Index.CompareTo(b.Index);
         });
      }

      /// <summary>
      /// 將監控項目載入到 DataGridView。
      /// </summary>
      private void LoadDataToGrid()
      {
         dgvMonitor.SuspendLayout();
         try
         {
            dgvMonitor.Rows.Clear();

            foreach (var item in _monitorItems)
            {
               int rowIndex = dgvMonitor.Rows.Add();
               var row = dgvMonitor.Rows[rowIndex];
               row.Cells[colAddress.Index].Value = FormatAddress(item.Kind, item.Index);
               row.Cells[colType.Index].Value = item.Type;
               row.Cells[colCurrentValue.Index].Value = "-";
               row.Cells[colNewValue.Index].Value = "";
               row.Tag = item;
            }
         }
         finally
         {
            dgvMonitor.ResumeLayout();
         }
      }

      /// <summary>
      /// 格式化位址顯示 (10進制或16進制)。
      /// </summary>
      private string FormatAddress(string kind, int index)
      {
         if (_isHexFormat)
         {
            return $"{kind} 0x{index:X}";
         }
         else
         {
            return $"{kind} {index:D4}";
         }
      }

      /// <summary>
      /// 切換顯示格式並重新載入位址欄位。
      /// </summary>
      private void ToggleDisplayFormat()
      {
         _isHexFormat = !_isHexFormat;
         chkHexFormat.Checked = _isHexFormat;

         // 只更新位址欄位,不重新載入整個 grid
         foreach (DataGridViewRow row in dgvMonitor.Rows)
         {
            if (row.Tag is MonitorItem item)
            {
               row.Cells[colAddress.Index].Value = FormatAddress(item.Kind, item.Index);
            }
         }
      }

      /// <summary>
      /// 更新所有監控項目的當前值。
      /// </summary>
      private void UpdateValues()
      {
         try
         {
            // [Diagnostic] 記錄 helper 實例的 HashCode
            int helperHashCode = RuntimeHelpers.GetHashCode(_helper);
            
            foreach (DataGridViewRow row in dgvMonitor.Rows)
            {
               if (row.Tag is MonitorItem item)
               {
                  object currentValue = null;

                  if (item.IsBit)
                  {
                     // [Diagnostic] 第一次呼叫時記錄 helper 實例
                     if (row.Index == 0)
                     {
                        lblLastUpdate.Text = $"HelperID: {helperHashCode}";
                     }
                     
                     bool bitValue = _helper.GetBit(item.Kind, item.Index);
                     currentValue = bitValue.ToString();
                  }
                  else
                  {
                     short wordValue = _helper.GetWord(item.Kind, item.Index);
                     currentValue = wordValue.ToString();
                  }

                  row.Cells[colCurrentValue.Index].Value = currentValue;
               }
            }

            lblLastUpdate.Text = $"Last Update: {DateTime.Now:HH:mm:ss.fff} (HelperID: {helperHashCode})";
         }
         catch (Exception ex)
         {
            lblLastUpdate.Text = $"Update Error: {ex.Message}";
         }
      }

      /// <summary>
      /// 寫入選取的值到 PLC。
      /// </summary>
      private async void WriteSelectedValue()
      {
         if (dgvMonitor.SelectedRows.Count == 0)
         {
            MessageBox.Show("Please select a row to write.", "Information",
               MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
         }

         try
         {
            var row = dgvMonitor.SelectedRows[0];
            if (row.Tag is MonitorItem item)
            {
               string newValueStr = row.Cells[colNewValue.Index].Value?.ToString();
               if (string.IsNullOrWhiteSpace(newValueStr))
               {
                  MessageBox.Show("Please enter a new value to write.", "Information",
                     MessageBoxButtons.OK, MessageBoxIcon.Information);
                  return;
               }

               if (item.IsBit)
               {
                  if (!bool.TryParse(newValueStr, out bool bitValue))
                  {
                     MessageBox.Show("Invalid bit value. Please enter 'True' or 'False'.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                     return;
                  }

                  await _helper.WriteBitsAsync(item.GetAddress(), new[] { bitValue });
                  MessageBox.Show($"Successfully wrote {bitValue} to {item.GetAddress()}", "Success",
                     MessageBoxButtons.OK, MessageBoxIcon.Information);
               }
               else
               {
                  if (!short.TryParse(newValueStr, out short wordValue))
                  {
                     MessageBox.Show("Invalid word value. Please enter a valid number (-32768 to 32767).", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                     return;
                  }

                  await _helper.WriteWordsAsync(item.GetAddress(), new[] { wordValue });
                  MessageBox.Show($"Successfully wrote {wordValue} to {item.GetAddress()}", "Success",
                     MessageBoxButtons.OK, MessageBoxIcon.Information);
               }

               // 清除已寫入的新值欄位
               row.Cells[colNewValue.Index].Value = "";
               // 立即更新一次以顯示寫入結果
               UpdateValues();
            }
         }
         catch (Exception ex)
         {
            MessageBox.Show($"Write failed: {ex.Message}", "Error",
               MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      /// <summary>
      /// 寫入所有已修改的值到 PLC。
      /// </summary>
      private async void WriteAllValues()
      {
         try
         {
            int writeCount = 0;
            var errors = new List<string>();

            foreach (DataGridViewRow row in dgvMonitor.Rows)
            {
               if (row.Tag is MonitorItem item)
               {
                  string newValueStr = row.Cells[colNewValue.Index].Value?.ToString();
                  if (string.IsNullOrWhiteSpace(newValueStr))
                     continue;

                  try
                  {
                     if (item.IsBit)
                     {
                        if (bool.TryParse(newValueStr, out bool bitValue))
                        {
                           await _helper.WriteBitsAsync(item.GetAddress(), new[] { bitValue });
                           row.Cells[colNewValue.Index].Value = "";
                           writeCount++;
                        }
                        else
                        {
                           errors.Add($"{item.GetAddress()}: Invalid bit value");
                        }
                     }
                     else
                     {
                        if (short.TryParse(newValueStr, out short wordValue))
                        {
                           await _helper.WriteWordsAsync(item.GetAddress(), new[] { wordValue });
                           row.Cells[colNewValue.Index].Value = "";
                           writeCount++;
                        }
                        else
                        {
                           errors.Add($"{item.GetAddress()}: Invalid word value");
                        }
                     }
                  }
                  catch (Exception ex)
                     {
                        errors.Add($"{item.GetAddress()}: {ex.Message}");
                  }
               }
            }

            // 立即更新一次以顯示寫入結果
            UpdateValues();

            string message = $"Successfully wrote {writeCount} value(s).";
            if (errors.Count > 0)
            {
               message += $"\n\nErrors ({errors.Count}):\n" + string.Join("\n", errors);
            }

            MessageBox.Show(message, writeCount > 0 ? "Write Complete" : "Write Failed",
               MessageBoxButtons.OK, writeCount > 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
         }
         catch (Exception ex)
         {
            MessageBox.Show($"Write all failed: {ex.Message}", "Error",
               MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      #endregion

      #region Event Handlers

      private void btnStartUpdate_Click(object sender, EventArgs e)
      {
         // [Diagnostic] 在啟動前記錄狀態
         var helperType = _helper.GetType();
         var memoryField = helperType.GetField("_deviceMemory", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
         
         if (memoryField != null)
         {
            var memory = memoryField.GetValue(_helper);
            int count = 0;
            
            if (memory != null)
            {
               var countProperty = memory.GetType().GetProperty("Count");
               if (countProperty != null)
               {
                  count = (int)countProperty.GetValue(memory);
               }
            }
            
            lblLastUpdate.Text = $"[Start] MemoryCount={count}, HelperID={RuntimeHelpers.GetHashCode(_helper)}";
         }
         
         _updateTimer.Interval = (int)nudUpdateInterval.Value;
         _updateTimer.Start();
         btnStartUpdate.Enabled = false;
         btnStopUpdate.Enabled = true;
         nudUpdateInterval.Enabled = false;
         UpdateValues(); // 立即更新一次
      }

      private void btnStopUpdate_Click(object sender, EventArgs e)
      {
         _updateTimer.Stop();
         btnStartUpdate.Enabled = true;
         btnStopUpdate.Enabled = false;
         nudUpdateInterval.Enabled = true;
         lblLastUpdate.Text = "Update stopped";
      }

      private void btnRefresh_Click(object sender, EventArgs e)
      {
         UpdateValues();
      }

      private void btnWriteSelected_Click(object sender, EventArgs e)
      {
         WriteSelectedValue();
      }

      private void btnWriteAll_Click(object sender, EventArgs e)
      {
         WriteAllValues();
      }

      private void chkHexFormat_CheckedChanged(object sender, EventArgs e)
      {
         ToggleDisplayFormat();
      }

      private void UpdateTimer_Tick(object sender, EventArgs e)
      {
         UpdateValues();
      }

      protected override void OnClosing(CancelEventArgs e)
      {
         // [Fix] 確保關閉視窗時停止定時器，避免在 Close/Open 過程中繼續讀取
         if (_updateTimer != null)
         {
            _updateTimer.Stop();
            _updateTimer.Tick -= UpdateTimer_Tick;
            _updateTimer.Dispose();
         }

         base.OnClosing(e);
      }

      #endregion

      #region Nested Classes

      /// <summary>
      /// 監控項目資料類別。
      /// </summary>
      private class MonitorItem
      {
         public string Kind { get; set; }
         public int Index { get; set; }
         public string Type { get; set; }
         public bool IsBit { get; set; }

         public string GetAddress()
         {
            return $"{Kind}{Index:D4}";
         }
      }

      #endregion
   }
}
