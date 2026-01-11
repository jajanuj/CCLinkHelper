using System;

namespace WindowsFormsApp1.Models
{
   /// <summary>
   /// 定期上報共通資料 - Status1（機台狀態資料）
   /// PLC 地址：LW1146-LW114A (5個 UINT16)
   /// </summary>
   public sealed class CommonReportStatus1 : IEquatable<CommonReportStatus1>
   {
      #region Constructors

      public CommonReportStatus1()
      {
      }

      public CommonReportStatus1(ushort alarm, ushort machine, ushort action, ushort waiting, ushort control)
      {
         AlarmStatus = alarm;
         MachineStatus = machine;
         ActionStatus = action;
         WaitingStatus = waiting;
         ControlStatus = control;
      }

      #endregion

      #region Properties

      /// <summary>
      /// 警報狀態 (LW1146)
      /// 1=重大警報, 2=輕警報, 3=預報, 4=無警報
      /// </summary>
      public ushort AlarmStatus { get; set; }

      /// <summary>
      /// 機台狀態 (LW1147)
      /// 1=初始化中, 2=準備中, 3=準備完成, 4=生產中, 5=停機, 6=停止中
      /// </summary>
      public ushort MachineStatus { get; set; }

      /// <summary>
      /// 機台動作狀態 (LW1148)
      /// 1=原點復歸中, 2=基板搬送中, 99=其他
      /// </summary>
      public ushort ActionStatus { get; set; }

      /// <summary>
      /// 基板等待狀態 (LW1149)
      /// 1=無等待, 2=下游等待中, 3=上游等待中, 4=上下游等待中, 5=其他（特殊模式）, 99=其他
      /// </summary>
      public ushort WaitingStatus { get; set; }

      /// <summary>
      /// 設備控制狀態 (LW114A)
      /// 1=自動運轉, 2=手動運轉, 3=其他（條件設定/假循環）, 4=其他（準備/調整）, 5=其他（品種切換）, 99=不屬於上述已定義
      /// </summary>
      public ushort ControlStatus { get; set; }

      #endregion

      #region Public Methods

      public override string ToString()
      {
         return $"Status1[Alarm:{AlarmStatus}, Machine:{MachineStatus}, Action:{ActionStatus}, Waiting:{WaitingStatus}, Control:{ControlStatus}]";
      }

      #endregion

      #region Equality Members

      public bool Equals(CommonReportStatus1 other)
      {
         if (ReferenceEquals(null, other))
         {
            return false;
         }

         if (ReferenceEquals(this, other))
         {
            return true;
         }

         return AlarmStatus == other.AlarmStatus
                && MachineStatus == other.MachineStatus
                && ActionStatus == other.ActionStatus
                && WaitingStatus == other.WaitingStatus
                && ControlStatus == other.ControlStatus;
      }

      public override bool Equals(object obj)
      {
         return ReferenceEquals(this, obj) || obj is CommonReportStatus1 other && Equals(other);
      }

      public override int GetHashCode()
      {
         unchecked
         {
            var hashCode = AlarmStatus.GetHashCode();
            hashCode = (hashCode * 397) ^ MachineStatus.GetHashCode();
            hashCode = (hashCode * 397) ^ ActionStatus.GetHashCode();
            hashCode = (hashCode * 397) ^ WaitingStatus.GetHashCode();
            hashCode = (hashCode * 397) ^ ControlStatus.GetHashCode();
            return hashCode;
         }
      }

      public static bool operator ==(CommonReportStatus1 left, CommonReportStatus1 right)
      {
         return Equals(left, right);
      }

      public static bool operator !=(CommonReportStatus1 left, CommonReportStatus1 right)
      {
         return !Equals(left, right);
      }

      #endregion
   }
}