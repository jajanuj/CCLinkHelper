using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WindowsFormsApp1.CCLink.Models;

namespace WindowsFormsApp1.CCLink.Forms
{
   public partial class SettingsForm : Form
   {
      #region Constructors

      public SettingsForm() : this(null)
      {
      }

      public SettingsForm(ControllerSettings existing)
      {
         InitializeComponent();
         Settings = existing ?? new ControllerSettings();
      }

      #endregion

      #region Properties

      public ControllerSettings Settings { get; private set; }

      #endregion

      #region Protected Methods

      protected override void OnLoad(EventArgs e)
      {
         base.OnLoad(e);
         if (!DesignMode)
         {
            LoadToUI();
         }
      }

      protected virtual void LoadToUI()
      {


         dgvRanges.Rows.Clear();
         if (Settings.ScanRanges != null)
         {
            foreach (var r in Settings.ScanRanges)
            {
               dgvRanges.Rows.Add(r.Kind, r.Start.ToString("X4"), r.End.ToString("X4"));
            }
         }
      }

      protected virtual void btnSave_Click(object sender, EventArgs e)
      {


         var ranges = new List<ScanRange>();
         foreach (DataGridViewRow row in dgvRanges.Rows)
         {
            if (row.IsNewRow)
            {
               continue;
            }

            string kind = row.Cells[0].Value?.ToString();
            string startHex = row.Cells[1].Value?.ToString().Trim();
            string endHex = row.Cells[2].Value?.ToString().Trim();

            if (!string.IsNullOrEmpty(kind) && !string.IsNullOrEmpty(startHex) && !string.IsNullOrEmpty(endHex))
            {
               try
               {
                  int start = Convert.ToInt32(startHex, 16);
                  int end = Convert.ToInt32(endHex, 16);

                  // 防呆檢查
                  if (start < 0 || end < 0 || start > 0x3FFF || end > 0x3FFF)
                  {
                     MessageBox.Show($"{kind} 位址超出範圍 (0x0000 - 0x3FFF)。");
                     return;
                  }

                  if (start > end)
                  {
                     MessageBox.Show($"{kind} 起始位址不可大於結束位址。");
                     return;
                  }

                  ranges.Add(new ScanRange
                  {
                     Kind = kind.ToUpper(),
                     Start = start,
                     End = end
                  });

                  // 格式化回 UI (統一用大寫)
                  row.Cells[1].Value = start.ToString("X4");
                  row.Cells[2].Value = end.ToString("X4");
               }
               catch
               {
                  MessageBox.Show("請輸入正確的 16 進位位址。");
                  return;
               }
            }
         }

         Settings.ScanRanges = ranges;
         DialogResult = DialogResult.OK;
         Close();
      }

      #endregion
   }
}