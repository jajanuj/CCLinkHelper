using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;
using WindowsFormsApp1.CCLink.Models;
using WindowsFormsApp1.CCLink.Interfaces;
using WindowsFormsApp1.CCLink.Models;


namespace WindowsFormsApp1.CCLink.Forms
{
   public partial class ScanMonitorForm : Form
   {
      #region Fields

      private ICCLinkController _controller;
      private readonly Timer _updateTimer;
      private readonly List<MonitorItem> _monitorItems = new List<MonitorItem>();
      private bool _isHexFormat = false;

      // Win32 API for performance optimization
      [DllImport("user32.dll")]
      private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
      private const int WM_SETREDRAW = 0x000B;

      #endregion

      #region Constructors

      public ScanMonitorForm(ICCLinkController controller)
      {
         InitializeComponent();

         _controller = controller ?? throw new ArgumentNullException(nameof(controller));
         _updateTimer = new Timer { Interval = (int)nudUpdateInterval.Value };
         _updateTimer.Tick += UpdateTimer_Tick;

         // Enable double buffering for DataGridView to reduce flickering
         typeof(DataGridView).InvokeMember("DoubleBuffered",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
            null, dgvMonitor, new object[] { true });
         typeof(DataGridView).InvokeMember("DoubleBuffered",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
            null, dgvFavorites, new object[] { true });

         InitializeMonitorItems();
         LoadFavorites();
         LoadDataToGrid();
         
         // Register events
         dgvMonitor.CellContentClick += DgvMonitor_CellContentClick;
      }

      #endregion

      #region Public Methods

      /// <summary>
      /// 更新 Controller 實例（當重新連線產生新的 Controller 時使用）。
      /// </summary>
      public void SetController(ICCLinkController controller)
      {
         _controller = controller ?? throw new ArgumentNullException(nameof(controller));
         
         // 更新標籤顯示新的 ID
         int hashCode = RuntimeHelpers.GetHashCode(_controller);
         lblLastUpdate.Text = $"Controller Updated (ID: {hashCode})";
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

         // 嘗試從 Controller 中取得設定 (若支援)
         // 注意：ICCLinkController 未必暴露 ScanRanges，我們需要依賴實作細節或外部傳入
         // 目前架構下 ScanRanges 存於 Settings，但 monitor 需要知道哪些被掃描
         // 此處我們使用 Reflection 嘗試讀取 _settings from Controller (if possible) or just show all if we pass settings in
         
         // Better approach: ScanMonitorForm creation logic in Form1 passes ranges or we access them via Form1?
         // Quick fix: Reflection on _controller to find _settings or _userScanRanges
         
         var field = _controller.GetType()
            .GetField("_settings", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
         
         var settings = field?.GetValue(_controller) as ControllerSettings;
         
         // Check for AppControllerSettings if above failed (MxComponentAdapter/AppPlcService might not match exactly)
         if (settings == null)
         {
            // Try explicit cast to MelsecHelper or MxComponentAdapter to get ranges?
            // Actually ScanRanges are set via SetScanRanges internally.
            // Let's rely on finding "_settings" or pass ranges to Constructor.
            
            // Fallback: If controller is MxComponentAdapter, it has _scanRanges
            var scanField = _controller.GetType().GetField("_scanRanges", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
             if (scanField != null)
             {
                 var ranges = scanField.GetValue(_controller) as List<ScanRange>;
                 if (ranges != null)
                 {
                     // Convert to settings-like object for loop below
                     settings = new ControllerSettings { ScanRanges = ranges };
                 }
             }
         }

         if (settings?.ScanRanges == null || settings.ScanRanges.Count == 0)
         {
            MessageBox.Show("No scan ranges configured. Please configure scan ranges in settings first.",
               "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
         }

         foreach (var range in settings.ScanRanges)
         {
            string kind = range.Kind?.ToUpper() ?? "";
            
            // Bit 類型裝置：M, X, Y, L, F, B, LB, LX, LY, SB
            bool isBit = kind == "M" || kind == "X" || kind == "Y" || 
                         kind == "L" || kind == "F" || kind == "B" ||
                         kind == "LB" || kind == "LX" || kind == "LY" || kind == "SB";
            
            // Word 類型裝置：D, W, R, LW, SW
            bool isWord = kind == "D" || kind == "W" || kind == "R" || 
                          kind == "LW" || kind == "SW";

            if (!isBit && !isWord)
               continue;  // 只有不支援的裝置類型才會被跳過

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

          // 排序: 先按 Kind (字母順序), 再按 Index (數字順序)
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

             // Batch add rows for better performance
             var rows = new List<DataGridViewRow>(_monitorItems.Count);
             foreach (var item in _monitorItems)
             {
                var row = new DataGridViewRow();
                row.CreateCells(dgvMonitor);
                row.Cells[colSelect.Index].Value = item.IsFavorite;
                row.Cells[colAddress.Index].Value = FormatAddress(item.Kind, item.Index);
                row.Cells[colType.Index].Value = item.Type;
                row.Cells[colCurrentValue.Index].Value = "-";
                row.Cells[colNewValue.Index].Value = "";
                row.Tag = item;
                rows.Add(row);
             }
             
             // Add all rows at once instead of one by one
             if (rows.Count > 0)
             {
                dgvMonitor.Rows.AddRange(rows.ToArray());
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

         // 更新兩個 grid 的顯示
         foreach (DataGridViewRow row in dgvMonitor.Rows)
         {
            if (row.Tag is MonitorItem item)
            {
               row.Cells[colAddress.Index].Value = FormatAddress(item.Kind, item.Index);
            }
         }
         
         foreach (DataGridViewRow row in dgvFavorites.Rows)
         {
            if (row.Tag is MonitorItem item)
            {
               row.Cells[colFavAddress.Index].Value = FormatAddress(item.Kind, item.Index);
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
             // 決定要更新哪個 Grid
             DataGridView targetGrid;
             int colValIdx;
             
             if (tabControlMain.SelectedTab == tabFavorites)
             {
                targetGrid = dgvFavorites;
                colValIdx = colFavCurrentValue.Index;
             }
             else
             {
                targetGrid = dgvMonitor;
                colValIdx = colCurrentValue.Index;
             }

             // [Diagnostic] 記錄 controller 實例的 HashCode
             int helperHashCode = RuntimeHelpers.GetHashCode(_controller);
             
             // Suspend drawing to improve performance
             SendMessage(targetGrid.Handle, WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);
             
             // Check if controller is MelsecHelper (only supports LB/LW/LX/LY)
             bool isMelsecHelper = _controller.GetType().Name == "MelsecHelper";
             
             try
             {
                 foreach (DataGridViewRow row in targetGrid.Rows)
                 {
                    if (row.Tag is MonitorItem item)
                    {
                       object currentValue = null;
  
                       // Pre-check: Skip unsupported devices for MelsecHelper to avoid exceptions
                       if (isMelsecHelper)
                       {
                          string kind = item.Kind.ToUpper();
                          if (kind != "LB" && kind != "LW" && kind != "LX" && kind != "LY")
                          {
                             // MelsecHelper doesn't support this device type
                             currentValue = "N/A";
                             row.Cells[colValIdx].Value = currentValue;
                             continue;  // Skip reading, avoid throwing exception
                          }
                       }
  
                       try
                       {
                          if (item.IsBit)
                          {
                             if (row.Index == 0)
                             {
                                lblLastUpdate.Text = $"HelperID: {helperHashCode}";
                             }
                             
                             // 直接使用 MonitorItem.GetAddress() 方法取得正確格式的位址
                             bool bitValue = _controller.GetBit(item.GetAddress());
                             currentValue = bitValue.ToString();
                          }
                          else
                          {
                             // 直接使用 MonitorItem.GetAddress() 方法
                             short wordValue = _controller.GetWord(item.GetAddress());
                             currentValue = wordValue.ToString();
                          }
                       }
                       catch (ArgumentException)
                       {
                          // 裝置類型不支援（不應該到這裡，因為上面已經預先檢查）
                          currentValue = "N/A";
                       }
                       catch (MelsecException)
                       {
                          // PLC 通訊錯誤
                          currentValue = "ERROR";
                       }
  
                       row.Cells[colValIdx].Value = currentValue;
                    }
                 }
             }
             finally
             {
                 // Resume drawing and refresh
                 SendMessage(targetGrid.Handle, WM_SETREDRAW, new IntPtr(1), IntPtr.Zero);
                 targetGrid.Refresh();
             }
 
             lblLastUpdate.Text = $"Last Update: {DateTime.Now:HH:mm:ss.fff} (HelperID: {helperHashCode})";
          }
          catch (MelsecException mex)
          {
             lblLastUpdate.Text = $"Melsec Error: {mex.Message}";
             System.Diagnostics.Debug.WriteLine($"[ScanMonitor] Melsec 錯誤: {mex}");
          }
          catch (System.Runtime.InteropServices.COMException comEx)
          {
             lblLastUpdate.Text = $"COM Error: 0x{comEx.HResult:X8}";
             System.Diagnostics.Debug.WriteLine($"[ScanMonitor] COM 元件錯誤: {comEx}");
          }
          catch (ArgumentException argEx)
          {
             lblLastUpdate.Text = $"Address Error: {argEx.ParamName} - {argEx.Message}";
             System.Diagnostics.Debug.WriteLine($"[ScanMonitor] 位址格式錯誤: {argEx}");
          }
          catch (Exception ex)
          {
             lblLastUpdate.Text = $"Update Error: {ex.GetType().Name} - {ex.Message}";
             System.Diagnostics.Debug.WriteLine($"[ScanMonitor] 未知錯誤: {ex}");
          }
      }

      /// <summary>
      /// 寫入選取的值到 PLC。
      /// </summary>
      private async void WriteSelectedValue()
      {
         var activeGrid = tabControlMain.SelectedTab == tabFavorites ? dgvFavorites : dgvMonitor;
         var newValColIdx = tabControlMain.SelectedTab == tabFavorites ? colFavNewValue.Index : colNewValue.Index;

         if (activeGrid.SelectedRows.Count == 0)
         {
            MessageBox.Show("Please select a row to write.", "Information",
               MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
         }

         try
         {
            var row = activeGrid.SelectedRows[0];
            if (row.Tag is MonitorItem item)
            {
               string newValueStr = row.Cells[newValColIdx].Value?.ToString();
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

                  await _controller.WriteBitsAsync(item.GetAddress(), new[] { bitValue });
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

                  await _controller.WriteWordsAsync(item.GetAddress(), new[] { wordValue });
                  MessageBox.Show($"Successfully wrote {wordValue} to {item.GetAddress()}", "Success",
                     MessageBoxButtons.OK, MessageBoxIcon.Information);
               }


                // 清除已寫入的新值欄位
                row.Cells[newValColIdx].Value = "";
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
            var activeGrid = tabControlMain.SelectedTab == tabFavorites ? dgvFavorites : dgvMonitor;
            var newValColIdx = tabControlMain.SelectedTab == tabFavorites ? colFavNewValue.Index : colNewValue.Index;

            int writeCount = 0;
            var errors = new List<string>();
 
            foreach (DataGridViewRow row in activeGrid.Rows)
            {
               if (row.Tag is MonitorItem item)
               {
                  string newValueStr = row.Cells[newValColIdx].Value?.ToString();
                  if (string.IsNullOrWhiteSpace(newValueStr))
                     continue;

                  try
                  {
                     if (item.IsBit)
                     {
                        if (bool.TryParse(newValueStr, out bool bitValue))
                        {
                           await _controller.WriteBitsAsync(item.GetAddress(), new[] { bitValue });
                           row.Cells[newValColIdx].Value = "";
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
                           await _controller.WriteWordsAsync(item.GetAddress(), new[] { wordValue });
                           row.Cells[newValColIdx].Value = "";
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
         var type = _controller.GetType();
         var memoryField = type.GetField("_deviceMemory", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
         
         if (memoryField != null)
         {
            var memory = memoryField.GetValue(_controller);
            int count = 0;
            
            if (memory != null)
            {
               var countProperty = memory.GetType().GetProperty("Count");
               if (countProperty != null)
               {
                  count = (int)countProperty.GetValue(memory);
               }
            }
            
             lblLastUpdate.Text = $"[Start] MemoryCount={count}, HelperID={RuntimeHelpers.GetHashCode(_controller)}";
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
         SaveFavorites();

         // [Fix] 確保關閉視窗時停止定時器，避免在 Close/Open 過程中繼續讀取
         if (_updateTimer != null)
         {
            _updateTimer.Stop();
            _updateTimer.Tick -= UpdateTimer_Tick;
            _updateTimer.Dispose();
         }

         base.OnClosing(e);
      }

      private void tabControlMain_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (tabControlMain.SelectedTab == tabFavorites)
         {
             RefreshFavoritesGrid();
         }
         UpdateValues();
      }

      private void DgvMonitor_CellContentClick(object sender, DataGridViewCellEventArgs e)
      {
         if (e.RowIndex >= 0 && e.ColumnIndex == colSelect.Index)
         {
            dgvMonitor.CommitEdit(DataGridViewDataErrorContexts.Commit);
            
            var row = dgvMonitor.Rows[e.RowIndex];
            if (row.Tag is MonitorItem item)
            {
               // CheckBox value might be null initially
               var isChecked = (bool)(row.Cells[colSelect.Index].Value ?? false);
               item.IsFavorite = isChecked;
            }
         }
      }

      private void RefreshFavoritesGrid()
      {
         dgvFavorites.SuspendLayout();
         try
         {
            dgvFavorites.Rows.Clear();
            // 已排序 _monitorItems (Kind -> Index)
            var favItems = _monitorItems.Where(x => x.IsFavorite).ToList();
            
            foreach (var item in favItems)
            {
               int rowIndex = dgvFavorites.Rows.Add();
               var row = dgvFavorites.Rows[rowIndex];
               row.Cells[colFavAddress.Index].Value = FormatAddress(item.Kind, item.Index);
               row.Cells[colFavType.Index].Value = item.Type;
               row.Cells[colFavCurrentValue.Index].Value = "-";
               row.Cells[colFavNewValue.Index].Value = "";
               row.Tag = item;
            }
         }
         finally
         {
            dgvFavorites.ResumeLayout();
         }
      }

      private void LoadFavorites()
      {
         try
         {
            string path = Path.Combine(Application.StartupPath, "monitor_favorites.txt");
            if (File.Exists(path))
            {
               var addresses = File.ReadAllLines(path);
               if (addresses != null)
               {
                  var set = new HashSet<string>(addresses, StringComparer.OrdinalIgnoreCase);
                  foreach (var item in _monitorItems)
                  {
                     if (set.Contains(item.GetAddress()))
                     {
                        item.IsFavorite = true;
                     }
                  }
               }
            }
         }
         catch (Exception ex)
         {
            // Fail silently or log
            Console.WriteLine($"Failed to load favorites: {ex.Message}");
         }
      }

      private void SaveFavorites()
      {
         try
         {
            var addresses = _monitorItems.Where(x => x.IsFavorite)
                                         .Select(x => x.GetAddress())
                                         .ToList();
            string path = Path.Combine(Application.StartupPath, "monitor_favorites.txt");
            File.WriteAllLines(path, addresses);
         }
         catch (Exception ex)
         {
            Console.WriteLine($"Failed to save favorites: {ex.Message}");
         }
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
         public bool IsFavorite { get; set; }

         public string GetAddress()
         {
            // For Write methods, we prefer the unified hex format logic if applicable
            // But MxComponentAdapter.GetBit uses ParseAddress which handles digit part.
            // My MonitorItem.Index is int.
            // If Kind is Hex type, format as Hex.
            string type = Kind.ToUpper();
            bool isHex = (type == "X" || type == "Y" || type == "B" || type == "W" || type == "LB" || type == "LW" || type == "SB" || type == "SW");
            if (isHex) return $"{Kind}{Index:X4}";
            return $"{Kind}{Index}";
         }
      }

      #endregion
      
      /// <summary>
      /// [已過時] 格式化位址字串。請使用 MonitorItem.GetAddress() 方法。
      /// </summary>
      [Obsolete("請使用 MonitorItem.GetAddress() 方法")]
      private string FormatAddressForGet(string kind, int index)
      {
          // Reuse MonitorItem.GetAddress logic basically
          string type = kind.ToUpper();
          bool isHex = (type == "X" || type == "Y" || type == "B" || type == "W" || type == "LB" || type == "LW" || type == "SB" || type == "SW");
          if (isHex) return $"{kind}{index:X}"; // No padding or padding ok
          return $"{kind}{index}";
      }
   }
}
