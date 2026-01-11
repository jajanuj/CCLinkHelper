using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using WindowsFormsApp1.CCLink.Forms;
using WindowsFormsApp1.CCLink.Services;

namespace WindowsFormsApp1.CCLink.Models
{
   /// <summary>
   /// 控制器連線與行為設定。
   /// </summary>
   public class ControllerSettings
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
         var loaded = SettingsPersistence.Load<ControllerSettings>(configName);
         if (loaded != null)
         {
            // 複製屬性

            Channel = loaded.Channel;
            NetworkNo = loaded.NetworkNo;
            StationNo = loaded.StationNo;
            TimeoutMs = loaded.TimeoutMs;
            RetryCount = loaded.RetryCount;
            RetryBackoffMs = loaded.RetryBackoffMs;

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

      /// <summary>通訊通道。</summary>
      public int Channel { get; set; }

      /// <summary>Network 編號。</summary>
      public int NetworkNo { get; set; }

      /// <summary>Station 編號。</summary>
      public int StationNo { get; set; }

      /// <summary>逾時毫秒數（mdSetTimeout）。</summary>
      public int TimeoutMs { get; set; } = 3000;

      /// <summary>重試次數（暫時性錯誤）。</summary>
      public int RetryCount { get; set; } = 3;

      /// <summary>重試退避毫秒。</summary>
      public int RetryBackoffMs { get; set; } = 50;

      /// <summary>位元序設定（Big/Little）。</summary>
      public string Endian { get; set; } = "Big";

      /// <summary>是否為 64 位元環境。</summary>
      public bool Isx64 { get; set; } = true;

      /// <summary>要掃描的區域列表。</summary>
      public List<ScanRange> ScanRanges { get; set; } = new List<ScanRange>();

      #endregion

      #region Public Methods

      /// <summary>
      /// 顯示設定視窗。
      /// </summary>
      /// <param name="configName">設定檔名稱（不含副檔名）。</param>
      /// <returns>回傳 DialogResult。</returns>
      public virtual DialogResult ShowDialog(string configName)
      {
         using (var form = new SettingsForm(this))
         {
            var result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
               SettingsPersistence.Save(this, configName);
            }

            return result;
         }
      }

      #endregion
   }
}