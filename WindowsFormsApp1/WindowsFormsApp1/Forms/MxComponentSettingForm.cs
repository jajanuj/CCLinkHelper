using System;
using System.Windows.Forms;
using WindowsFormsApp1.CCLink.Forms;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.Forms
{
   public partial class MxComponentSettingForm : SettingsForm
   {
      public MxComponentSettingForm(AppControllerSettings settings) : base(settings)
      {
         InitializeComponent();
      }

      protected override void LoadToUI()
      {
         base.LoadToUI();

         if (Settings is AppControllerSettings appSettings)
         {
            // Mx Parameters
            numLogicalStation.Value = appSettings.LogicalStationNumber;

            // App Parameters
            numHeartbeat.Value = appSettings.HeartbeatIntervalMs;
            numTimeSync.Value = appSettings.TimeSyncIntervalMs;
            txtTrigger.Text = appSettings.TimeSync?.TriggerAddress ?? "LB0301";
            txtData.Text = appSettings.TimeSync?.DataBaseAddress ?? "LW0000";
         }
      }

      protected override void btnSave_Click(object sender, EventArgs e)
      {
         // Call base to handle ScanRanges and Close logic
         // Note: If Base btnSave_Click calls Close(), we need to save first or insert logic before base call.
         // Actually, SettingsForm.btnSave_Click does Close(). So we must update settings BEFORE calling base.
         // But wait, base.btnSave_Click reads from UI controls (dgvRanges).
         // It does not read our controls. So we can update our part of Settings object, then call base.
         
         if (Settings is AppControllerSettings appSettings)
         {
            appSettings.LogicalStationNumber = (int)numLogicalStation.Value;
            appSettings.DriverType = MelsecDriverType.MxComponent;

            appSettings.HeartbeatIntervalMs = (int)numHeartbeat.Value;
            appSettings.TimeSyncIntervalMs = (int)numTimeSync.Value;
            appSettings.TimeSync.TriggerAddress = txtTrigger.Text?.Trim() ?? "LB0301";
            appSettings.TimeSync.DataBaseAddress = txtData.Text?.Trim() ?? "LW0000";
         }

         base.btnSave_Click(sender, e);
      }
   }
}
