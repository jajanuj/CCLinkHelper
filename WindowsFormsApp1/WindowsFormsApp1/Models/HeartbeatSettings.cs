using System;

namespace WindowsFormsApp1.Models
{
   /// <summary>
   /// 對時功能設定。
   /// </summary>
   [Serializable]
   public class HeartbeatSettings
   {
      #region Properties

      /// <summary>
      /// 心跳請求位址。
      /// </summary>
      public string RequestAddress { get; set; } = "LB0100";

      /// <summary>
      /// 心跳回應位址。
      /// </summary>
      public string ResponseAddress { get; set; } = "LB0500";

      /// <summary>心跳間隔毫秒（狀態監測）。</summary>
      public int HeartbeatIntervalMs { get; set; } = 300;

      #endregion
   }
}