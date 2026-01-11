using System;
using WindowsFormsApp1.CCLink.Forms;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.Forms
{
   public partial class AppSettingsForm : SettingsForm
   {
      #region Constructors

      public AppSettingsForm(AppControllerSettings settings) : base(settings)
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
            appSettings.HeartbeatIntervalMs = (int)numHeartbeat.Value;
            appSettings.TimeSyncIntervalMs = (int)numTimeSync.Value;
            appSettings.TimeSync.TriggerAddress = txtTrigger.Text?.Trim() ?? "LB0301";
            appSettings.TimeSync.DataBaseAddress = txtData.Text?.Trim() ?? "LW0000";
         }
      }

      #endregion
   }
}