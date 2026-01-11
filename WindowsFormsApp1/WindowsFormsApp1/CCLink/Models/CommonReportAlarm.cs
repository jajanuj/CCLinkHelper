using System;
using System.Linq;

namespace WindowsFormsApp1.CCLink.Models
{
   /// <summary>
   /// 定期上報共通資料 - Alarm（警報資料）
   /// PLC 地址：LW113A-LW1145 (12個 UINT16)
   /// </summary>
   public sealed class CommonReportAlarm : IEquatable<CommonReportAlarm>
   {
      /// <summary>
      /// 錯誤代碼 01-12 (LW113A-LW1145)
      /// </summary>
      public ushort[] ErrorCodes { get; set; } = new ushort[12];

      #region Equality Members

      public bool Equals(CommonReportAlarm other)
      {
         if (ReferenceEquals(null, other)) return false;
         if (ReferenceEquals(this, other)) return true;
         
         // 比對陣列內容
         if (ErrorCodes == null && other.ErrorCodes == null) return true;
         if (ErrorCodes == null || other.ErrorCodes == null) return false;
         if (ErrorCodes.Length != other.ErrorCodes.Length) return false;
         
         return ErrorCodes.SequenceEqual(other.ErrorCodes);
      }

      public override bool Equals(object obj)
      {
         return ReferenceEquals(this, obj) || obj is CommonReportAlarm other && Equals(other);
      }

      public override int GetHashCode()
      {
         if (ErrorCodes == null) return 0;
         
         unchecked
         {
            int hashCode = 17;
            foreach (var code in ErrorCodes)
            {
               hashCode = (hashCode * 397) ^ code.GetHashCode();
            }
            return hashCode;
         }
      }

      public static bool operator ==(CommonReportAlarm left, CommonReportAlarm right)
      {
         return Equals(left, right);
      }

      public static bool operator !=(CommonReportAlarm left, CommonReportAlarm right)
      {
         return !Equals(left, right);
      }

      #endregion

      public override string ToString()
      {
         if (ErrorCodes == null || ErrorCodes.Length == 0)
            return "Alarm[No Errors]";
         
         var activeCodes = ErrorCodes.Where(c => c != 0).ToArray();
         if (activeCodes.Length == 0)
            return "Alarm[No Active Errors]";
         
         return $"Alarm[{string.Join(", ", activeCodes)}]";
      }
   }
}
