using System;
using WindowsFormsApp1.CCLink.Forms;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.Forms
{
   public partial class MelsecBoardSettingForm : WindowsFormsApp1.CCLink.Forms.SettingsForm
   {
      #region Constructors

      public MelsecBoardSettingForm(AppControllerSettings settings) : base(settings)
      {
         InitializeComponent();
      }

      #endregion

      #region Protected Methods

      protected override void LoadToUI()
      {
         base.LoadToUI();

         if (Settings is AppControllerSettings appSettings)
         {
            // Board Parameters
            numPort.Value = appSettings.Channel;
            numNetwork.Value = appSettings.NetworkNo;
            numStation.Value = appSettings.StationNo;
            numTimeout.Value = appSettings.TimeoutMs;
            numRetryCount.Value = appSettings.RetryCount;
            numRetryBackoff.Value = appSettings.RetryBackoffMs;
            cmbEndian.SelectedItem = appSettings.Endian ?? "Big";
            chkIsx64.Checked = appSettings.Isx64;

            // App Parameters
            numHeartbeat.Value = appSettings.HeartbeatIntervalMs;
            numTimeSync.Value = appSettings.TimeSyncIntervalMs;
            txtTrigger.Text = appSettings.TimeSync?.TriggerAddress ?? "LB0301";
            txtData.Text = appSettings.TimeSync?.DataBaseAddress ?? "LW0000";
         }
      }

      protected override void btnSave_Click(object sender, EventArgs e)
      {
         base.btnSave_Click(sender, e);

         if (Settings is AppControllerSettings appSettings)
         {
            // Board Parameters
            appSettings.Channel = (int)numPort.Value;
            appSettings.NetworkNo = (int)numNetwork.Value;
            appSettings.StationNo = (int)numStation.Value;
            appSettings.TimeoutMs = (int)numTimeout.Value;
            appSettings.RetryCount = (int)numRetryCount.Value;
            appSettings.RetryBackoffMs = (int)numRetryBackoff.Value;
            appSettings.Endian = cmbEndian.SelectedItem?.ToString() ?? "Big";
            appSettings.Isx64 = chkIsx64.Checked;
            appSettings.DriverType = MelsecDriverType.MelsecBoard; // Ensure driver type is set

            // App Parameters
            appSettings.HeartbeatIntervalMs = (int)numHeartbeat.Value;
            appSettings.TimeSyncIntervalMs = (int)numTimeSync.Value;
            appSettings.TimeSync.TriggerAddress = txtTrigger.Text?.Trim() ?? "LB0301";
            appSettings.TimeSync.DataBaseAddress = txtData.Text?.Trim() ?? "LW0000";
         }
      }

      #endregion
   }
}