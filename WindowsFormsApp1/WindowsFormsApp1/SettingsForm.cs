using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WindowsFormsApp1.CCLink.Models;

namespace WindowsFormsApp1
{
   public partial class SettingsForm : Form
   {
      public ControllerSettings Settings { get; private set; }

      public SettingsForm(ControllerSettings existing = null)
      {
         InitializeComponent();
         Settings = existing ?? new ControllerSettings();
         LoadToUI();
      }

      private void LoadToUI()
      {
         txtIp.Text = Settings.Ip ?? "127.0.0.1";
         numStation.Value = Settings.Station;
         numHeartbeat.Value = Settings.HeartbeatIntervalMs;

         dgvRanges.Rows.Clear();
         if (Settings.ScanRanges != null)
         {
            foreach (var r in Settings.ScanRanges)
            {
               dgvRanges.Rows.Add(r.Kind, r.Start.ToString("X"), r.End.ToString("X"));
            }
         }
      }

      private void btnSave_Click(object sender, EventArgs e)
      {
         Settings.Ip = txtIp.Text;
         Settings.Station = (int)numStation.Value;
         Settings.HeartbeatIntervalMs = (int)numHeartbeat.Value;

         var ranges = new List<ScanRange>();
         foreach (DataGridViewRow row in dgvRanges.Rows)
         {
            if (row.IsNewRow) continue;

            string kind = row.Cells[0].Value?.ToString();
            string startHex = row.Cells[1].Value?.ToString();
            string endHex = row.Cells[2].Value?.ToString();

            if (!string.IsNullOrEmpty(kind) && !string.IsNullOrEmpty(startHex) && !string.IsNullOrEmpty(endHex))
            {
               try
               {
                  ranges.Add(new ScanRange
                  {
                     Kind = kind.ToUpper(),
                     Start = Convert.ToInt32(startHex, 16),
                     End = Convert.ToInt32(endHex, 16)
                  });
               }
               catch
               {
                  MessageBox.Show("請輸入正確的 16 進位位址。");
                  return;
               }
            }
         }

         Settings.ScanRanges = ranges;
         this.DialogResult = DialogResult.OK;
         this.Close();
      }
   }
}
