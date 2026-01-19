using System;
using WindowsFormsApp1.CCLink.Forms;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.Forms
{
   public partial class MelsecBoardSettingForm : SettingsForm
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
            numTimeSync.Value = appSettings.TimeSyncIntervalMs;
            txtTrigger.Text = appSettings.TimeSync?.TriggerAddress ?? "LB0301";
            txtData.Text = appSettings.TimeSync?.DataBaseAddress ?? "LW0000";
            paramHeartbeatRequestAddress.TextValue = appSettings.Heartbeat?.RequestAddress ?? "LB0100";
            paramHeartbeatResponseAddress.TextValue = appSettings.Heartbeat?.ResponseAddress ?? "LB0500";
            paramHeartbeatInterval.Value = appSettings.Heartbeat?.HeartbeatIntervalMs ?? 300;

            // Tracking Parameters
            txtLoadingRobotAddr.Text = appSettings.Tracking?.LoadingRobotAddress ?? "LW0000";
            txtLoadingStationAddr.Text = appSettings.Tracking?.LoadingStationAddress ?? "LW0010";
            txtUnloadingRobotAddr.Text = appSettings.Tracking?.UnloadingRobotAddress ?? "LW0020";
            txtUnloadingStationAddr.Text = appSettings.Tracking?.UnloadingStationAddress ?? "LW0030";

            paramMaintenanceT1Timeout.Value = appSettings.Maintenance?.MaintenanceT1Timeout ?? 1000;
            paramMaintenanceT2Timeout.Value = appSettings.Maintenance?.MaintenanceT2Timeout ?? 1000;
            nudEqToPlcT1Timeout.Value = appSettings.Maintenance?.MaintenanceEqToPlcT1Timeout ?? 1000;
            nudEqToPlcT2Timeout.Value = appSettings.Maintenance?.MaintenanceEqToPlcT2Timeout ?? 1000;
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
            appSettings.Heartbeat.RequestAddress = paramHeartbeatRequestAddress.TextValue;
            appSettings.Heartbeat.ResponseAddress = paramHeartbeatResponseAddress.TextValue;
            appSettings.Heartbeat.HeartbeatIntervalMs = (int)paramHeartbeatInterval.Value;
            appSettings.TimeSyncIntervalMs = (int)numTimeSync.Value;
            appSettings.TimeSync.TriggerAddress = txtTrigger.Text?.Trim() ?? "LB0301";
            appSettings.TimeSync.DataBaseAddress = txtData.Text?.Trim() ?? "LW0000";

            // Tracking Parameters
            appSettings.Tracking.LoadingRobotAddress = txtLoadingRobotAddr.Text?.Trim() ?? "LW0000";
            appSettings.Tracking.LoadingStationAddress = txtLoadingStationAddr.Text?.Trim() ?? "LW0010";
            appSettings.Tracking.UnloadingRobotAddress = txtUnloadingRobotAddr.Text?.Trim() ?? "LW0020";
            appSettings.Tracking.UnloadingStationAddress = txtUnloadingStationAddr.Text?.Trim() ?? "LW0030";

            appSettings.Maintenance.MaintenanceT1Timeout = (int)paramMaintenanceT1Timeout.Value;
            appSettings.Maintenance.MaintenanceT2Timeout = (int)paramMaintenanceT2Timeout.Value;
            appSettings.Maintenance.MaintenanceEqToPlcT1Timeout = (int)nudEqToPlcT1Timeout.Value;
            appSettings.Maintenance.MaintenanceEqToPlcT2Timeout = (int)nudEqToPlcT2Timeout.Value;
         }
      }

      #endregion
   }
}