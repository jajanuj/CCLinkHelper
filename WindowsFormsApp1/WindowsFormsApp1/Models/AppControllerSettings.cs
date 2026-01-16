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
            Heartbeat = loaded.Heartbeat ?? new HeartbeatSettings();
            TimeSyncIntervalMs = loaded.TimeSyncIntervalMs;
            TimeSync = loaded.TimeSync ?? new TimeSyncSettings();
            Tracking = loaded.Tracking ?? new TrackingSettings();
         }
         else
         {
            ShowDialog(configName);
         }
      }

      #endregion

      #region Properties

      /// <summary>
      /// 驅動程式類型
      /// </summary>
      public MelsecDriverType DriverType { get; set; } = MelsecDriverType.MelsecBoard;

      /// <summary>
      /// Mx Component 的邏輯站號 (Logical Station Number)
      /// </summary>
      public int LogicalStationNumber { get; set; } = 1;

      /// <summary>對時監控間隔毫秒。</summary>
      public int TimeSyncIntervalMs { get; set; } = 1000;

      public HeartbeatSettings Heartbeat { get; set; } = new HeartbeatSettings();

      /// <summary>對時位址設定。</summary>
      public TimeSyncSettings TimeSync { get; set; } = new TimeSyncSettings();

      /// <summary>追蹤資料位址設定。</summary>
      public TrackingSettings Tracking { get; set; } = new TrackingSettings();

      public MaintenanceSettings Maintenance { get; set; } = new MaintenanceSettings();

      #endregion

      #region Public Methods

      public override System.Windows.Forms.DialogResult ShowDialog(string configName)
      {
         // 根據驅動類型顯示對應的設定表單
         System.Windows.Forms.Form form;

         if (DriverType == MelsecDriverType.MxComponent)
         {
            form = new Forms.MxComponentSettingForm(this);
         }
         else
         {
            // Default to Board for Board and Simulator (Simulator shares Board settings usually, or separate?)
            // Actually Simulator uses MelsecBoardSettingForm for IP/Port mocking essentially.
            form = new Forms.MelsecBoardSettingForm(this);
         }

         using (form)
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

   public enum MelsecDriverType
   {
      MelsecBoard,
      MxComponent,
      Simulator
   }
}