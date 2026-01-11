using System.Collections.Generic;
using System.Windows.Forms;
using WindowsFormsApp1.CCLink.Forms;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.CCLink.Models
{
   /// <summary>
   /// 控制器連線與行為設定。
   /// </summary>
   public sealed class ControllerSettings
   {
      #region Constructors

      /// <summary>
      /// 預設建構子。
      /// </summary>
      public ControllerSettings()
      {
      }

      /// <summary>
      /// 從檔案載入設定，若檔案不存在則顯示設定視窗。
      /// </summary>
      /// <param name="configName">設定檔名稱（不含副檔名）。</param>
      public ControllerSettings(string configName)
      {
         var loaded = Services.SettingsPersistence.Load(configName);
         if (loaded != null)
         {
            // 複製屬性
            Ip = loaded.Ip;
            Port = loaded.Port;
            Network = loaded.Network;
            Station = loaded.Station;
            TimeoutMs = loaded.TimeoutMs;
            RetryCount = loaded.RetryCount;
            RetryBackoffMs = loaded.RetryBackoffMs;
            HeartbeatIntervalMs = loaded.HeartbeatIntervalMs;
            TimeSyncIntervalMs = loaded.TimeSyncIntervalMs;
            TimeSync = loaded.TimeSync ?? new TimeSyncSettings();
            Endian = loaded.Endian;
            Isx64 = loaded.Isx64;
            ScanRanges = loaded.ScanRanges;
         }
         else
         {
            // 彈出視窗讓使用者填寫
            ShowDialog(configName);
         }
      }

      #endregion

      #region Properties

      /// <summary>PLC/控制卡的 IP 位址。</summary>
      public string Ip { get; set; }

      /// <summary>通訊埠或通道編號。</summary>
      public int Port { get; set; }

      /// <summary>Network 編號（依現場設定）。</summary>
      public int Network { get; set; }

      /// <summary>Station 編號（依現場設定）。</summary>
      public int Station { get; set; }

      /// <summary>逾時毫秒數（mdSetTimeout）。</summary>
      public int TimeoutMs { get; set; } = 3000;

      /// <summary>重試次數（暫時性錯誤）。</summary>
      public int RetryCount { get; set; } = 3;

      /// <summary>重試退避毫秒。</summary>
      public int RetryBackoffMs { get; set; } = 50;

      /// <summary>心跳間隔毫秒（狀態監測）。</summary>
      public int HeartbeatIntervalMs { get; set; } = 300;

      /// <summary>位元序設定（Big/Little）。</summary>
      public string Endian { get; set; } = "Big";

      /// <summary>是否為 64 位元環境。</summary>
      public bool Isx64 { get; set; } = true;

      /// <summary>對時監控間隔毫秒。</summary>
      public int TimeSyncIntervalMs { get; set; } = 1000;

      /// <summary>對時位址設定。</summary>
      public TimeSyncSettings TimeSync { get; set; } = new TimeSyncSettings();

      /// <summary>要掃描的區域列表。</summary>
      public List<ScanRange> ScanRanges { get; set; } = new List<ScanRange>();

      #endregion

      #region Public Methods

      /// <summary>
      /// 顯示設定視窗。
      /// </summary>
      /// <param name="configName">設定檔名稱（不含副檔名）。</param>
      /// <returns>回傳 DialogResult。</returns>
      public DialogResult ShowDialog(string configName)
      {
         using (var form = new SettingsForm(this))
         {
            var result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
               Services.SettingsPersistence.Save(this, configName);
            }

            return result;
         }
      }

      #endregion
   }
}