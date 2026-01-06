namespace WindowsFormsApp1.CCLink.Models
{
   /// <summary>
   /// 控制器連線與行為設定。
   /// 從 app.config 或 JSON 注入，供控制卡初始化與逾時/重試策略使用。
   /// </summary>
   public sealed class ControllerSettings
   {
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
      public int HeartbeatIntervalMs { get; set; } = 1000;

      /// <summary>位元序設定（Big/Little）。</summary>
      public string Endian { get; set; } = "Big";

      /// <summary>是否為 64 位元環境。</summary>
      public bool Isx64 { get; set; } = true;

      /// <summary>要掃描的區域列表。</summary>
      public System.Collections.Generic.List<ScanRange> ScanRanges { get; set; } = new System.Collections.Generic.List<ScanRange>();

      #endregion
   }
}
