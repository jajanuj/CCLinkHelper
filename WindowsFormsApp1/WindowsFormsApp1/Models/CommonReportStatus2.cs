using System;

namespace WindowsFormsApp1.Models
{
   /// <summary>
   /// 定期上報共通資料 - Status2（詳細狀態資料）
   /// PLC 地址：LW114B-LW1189 (13個 UINT16 + 50個 UINT16 配方名稱)
   /// </summary>
   public sealed class CommonReportStatus2 : IEquatable<CommonReportStatus2>
   {
      /// <summary>
      /// 警示燈狀態（紅）(LW114B)
      /// 0=熄燈, 1=常亮, 2=閃爍, 3=旋轉
      /// </summary>
      public ushort RedLightStatus { get; set; }

      /// <summary>
      /// 警示燈狀態（黃）(LW114C)
      /// 0=熄燈, 1=常亮, 2=閃爍, 3=旋轉
      /// </summary>
      public ushort YellowLightStatus { get; set; }

      /// <summary>
      /// 警示燈狀態（綠）(LW114D)
      /// 0=熄燈, 1=常亮, 2=閃爍, 3=旋轉
      /// </summary>
      public ushort GreenLightStatus { get; set; }

      /// <summary>
      /// 上游等待狀態 (LW114E)
      /// </summary>
      public ushort UpstreamWaitingStatus { get; set; }

      /// <summary>
      /// 下游等待狀態 (LW114F)
      /// </summary>
      public ushort DownstreamWaitingStatus { get; set; }

      /// <summary>
      /// 排出節拍 (LW1150)
      /// </summary>
      public ushort DischargeRate { get; set; }

      /// <summary>
      /// 停止時間 (LW1151)
      /// </summary>
      public ushort StopTime { get; set; }

      /// <summary>
      /// 處理實績計數器 (LW1152-LW1153, UINT32)
      /// 低位在 LW1152，高位在 LW1153
      /// </summary>
      public uint ProcessingCounter { get; set; }

      /// <summary>
      /// 滯留板數 (LW1154)
      /// </summary>
      public ushort RetainedBoardCount { get; set; }

      /// <summary>
      /// 目前配方 No. (LW1155)
      /// </summary>
      public ushort CurrentRecipeNo { get; set; }

      /// <summary>
      /// 設定板厚狀態 (LW1156)
      /// </summary>
      public ushort BoardThicknessStatus { get; set; }

      /// <summary>
      /// ULD 判別旗標 (LW1157)
      /// 固定填入 0
      /// </summary>
      public ushort UldFlag { get; set; }

      /// <summary>
      /// 目前配方名稱 (LW1158-LW1189, ASCII 100 字元)
      /// 不足補 0x00
      /// </summary>
      public string CurrentRecipeName { get; set; } = string.Empty;

        public CommonReportStatus2() { }

        public CommonReportStatus2(ushort red, ushort yellow, ushort green, ushort upWait, ushort downWait,
                                   ushort rate, ushort stopTime, uint processing, ushort retained,
                                   ushort recipeNo, ushort thickness, ushort uld, string recipeName)
        {
            RedLightStatus = red;
            YellowLightStatus = yellow;
            GreenLightStatus = green;
            UpstreamWaitingStatus = upWait;
            DownstreamWaitingStatus = downWait;
            DischargeRate = rate;
            StopTime = stopTime;
            ProcessingCounter = processing;
            RetainedBoardCount = retained;
            CurrentRecipeNo = recipeNo;
            BoardThicknessStatus = thickness;
            UldFlag = uld;
            CurrentRecipeName = recipeName;
        }

      #region Equality Members

      public bool Equals(CommonReportStatus2 other)
      {
         if (ReferenceEquals(null, other)) return false;
         if (ReferenceEquals(this, other)) return true;
         return RedLightStatus == other.RedLightStatus 
                && YellowLightStatus == other.YellowLightStatus 
                && GreenLightStatus == other.GreenLightStatus 
                && UpstreamWaitingStatus == other.UpstreamWaitingStatus 
                && DownstreamWaitingStatus == other.DownstreamWaitingStatus 
                && DischargeRate == other.DischargeRate 
                && StopTime == other.StopTime 
                && ProcessingCounter == other.ProcessingCounter 
                && RetainedBoardCount == other.RetainedBoardCount 
                && CurrentRecipeNo == other.CurrentRecipeNo 
                && BoardThicknessStatus == other.BoardThicknessStatus 
                && UldFlag == other.UldFlag 
                && string.Equals(CurrentRecipeName, other.CurrentRecipeName, StringComparison.Ordinal);
      }

      public override bool Equals(object obj)
      {
         return ReferenceEquals(this, obj) || obj is CommonReportStatus2 other && Equals(other);
      }

      public override int GetHashCode()
      {
         unchecked
         {
            var hashCode = RedLightStatus.GetHashCode();
            hashCode = (hashCode * 397) ^ YellowLightStatus.GetHashCode();
            hashCode = (hashCode * 397) ^ GreenLightStatus.GetHashCode();
            hashCode = (hashCode * 397) ^ UpstreamWaitingStatus.GetHashCode();
            hashCode = (hashCode * 397) ^ DownstreamWaitingStatus.GetHashCode();
            hashCode = (hashCode * 397) ^ DischargeRate.GetHashCode();
            hashCode = (hashCode * 397) ^ StopTime.GetHashCode();
            hashCode = (hashCode * 397) ^ ProcessingCounter.GetHashCode();
            hashCode = (hashCode * 397) ^ RetainedBoardCount.GetHashCode();
            hashCode = (hashCode * 397) ^ CurrentRecipeNo.GetHashCode();
            hashCode = (hashCode * 397) ^ BoardThicknessStatus.GetHashCode();
            hashCode = (hashCode * 397) ^ UldFlag.GetHashCode();
            hashCode = (hashCode * 397) ^ (CurrentRecipeName?.GetHashCode() ?? 0);
            return hashCode;
         }
      }

      public static bool operator ==(CommonReportStatus2 left, CommonReportStatus2 right)
      {
         return Equals(left, right);
      }

      public static bool operator !=(CommonReportStatus2 left, CommonReportStatus2 right)
      {
         return !Equals(left, right);
      }

      #endregion

      public override string ToString()
      {
         return $"Status2[Lights:R{RedLightStatus}/Y{YellowLightStatus}/G{GreenLightStatus}, Recipe:{CurrentRecipeNo}({CurrentRecipeName}), Counter:{ProcessingCounter}]";
      }
   }
}
