using System;

namespace WindowsFormsApp1.CCLink.Models
{
   /// <summary>
   /// 對時功能設定。
   /// </summary>
   [Serializable]
   public class TimeSyncSettings
   {
      /// <summary>
      /// 觸發對時的位址 (例如: LB0301)。
      /// </summary>
      public string TriggerAddress { get; set; } = "LB0301";

      /// <summary>
      /// 時間資料起點位址 (例如: LW0000)。
      /// 預期連續讀取 7 個字組。
      /// </summary>
      public string DataBaseAddress { get; set; } = "LW0000";
   }
}
