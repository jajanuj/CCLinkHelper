using System;

namespace WindowsFormsApp1.CCLink.Models
{
   /// <summary>
   /// 控制卡目前狀態與診斷資訊。
   /// </summary>
   public sealed class ControllerStatus
   {
      #region Properties

      /// <summary>是否已連線/通訊路徑就緒。</summary>
      public bool IsConnected { get; set; }

      /// <summary>卡型號。</summary>
      public string Model { get; set; }

      /// <summary>驅動程式版本。</summary>
      public string DriverVersion { get; set; }

      /// <summary>使用中的通訊通道。</summary>
      public int Channel { get; set; }

      /// <summary>最近錯誤碼或狀態碼。</summary>
      public int LastErrorCode { get; set; }

      /// <summary>平均延遲（毫秒）。</summary>
      public double AvgLatencyMs { get; set; }

      /// <summary>最大延遲（毫秒）。</summary>
      public double MaxLatencyMs { get; set; }

      /// <summary>最後更新時間（UTC）。</summary>
      public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

      #endregion
   }

   /// <summary>
   /// 狀態變更事件引數。
   /// </summary>
   public sealed class ControllerStatusChangedEventArgs : EventArgs
   {
      #region Constructors

      public ControllerStatusChangedEventArgs(ControllerStatus status) => Status = status;

      #endregion

      #region Properties

      /// <summary>最新狀態。</summary>
      public ControllerStatus Status { get; }

      #endregion
   }
}
