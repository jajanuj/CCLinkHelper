using WindowsFormsApp1.CCLink.Models;

namespace WindowsFormsApp1.Models
{
   /// <summary>
   /// 應用程式特定的控制器設定，包含對時與心跳。
   /// </summary>
   public class AppControllerSettings : ControllerSettings
   {
      #region Constructors

      public AppControllerSettings()
      {
      }

      public AppControllerSettings(string configName)
      {
         var loaded = CCLink.Services.SettingsPersistence.Load<AppControllerSettings>(configName);
         if (loaded != null)
         {
            Channel = loaded.Channel;
            NetworkNo = loaded.NetworkNo;
            StationNo = loaded.StationNo;
            TimeoutMs = loaded.TimeoutMs;
            RetryCount = loaded.RetryCount;
            RetryBackoffMs = loaded.RetryBackoffMs;
            Endian = loaded.Endian;
            Isx64 = loaded.Isx64;
            ScanRanges = loaded.ScanRanges;

            HeartbeatIntervalMs = loaded.HeartbeatIntervalMs;
            TimeSyncIntervalMs = loaded.TimeSyncIntervalMs;
            TimeSync = loaded.TimeSync ?? new TimeSyncSettings();
         }
         else
         {
            ShowDialog(configName);
         }
      }

      #endregion

      #region Properties

      /// <summary>心跳間隔毫秒（狀態監測）。</summary>
      public int HeartbeatIntervalMs { get; set; } = 300;

      /// <summary>對時監控間隔毫秒。</summary>
      public int TimeSyncIntervalMs { get; set; } = 1000;

      /// <summary>對時位址設定。</summary>
      public TimeSyncSettings TimeSync { get; set; } = new TimeSyncSettings();

      #endregion

      #region Public Methods

      public override System.Windows.Forms.DialogResult ShowDialog(string configName)
      {
         using (var form = new Forms.AppSettingsForm(this))
         {
            var result = form.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
               CCLink.Services.SettingsPersistence.Save(this, configName);
            }

            return result;
         }
      }

      #endregion
   }
}