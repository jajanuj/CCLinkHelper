using System;
using System.Linq;

namespace WindowsFormsApp1.Models
{
   /// <summary>
   /// 定期上報共通資料 - Alarm（警報資料）
   /// PLC 地址：LW113A-LW1145 (12個 UINT16)
   /// </summary>
   public sealed class CommonReportAlarm : IEquatable<CommonReportAlarm>
   {
      #region Constructors

      public CommonReportAlarm()
      {
      }

      public CommonReportAlarm(ushort[] codes)
      {
         if (codes != null && codes.Length == 12)
         {
            ErrorCodes = codes;
         }
         else if (codes != null)
         {
            Array.Copy(codes, ErrorCodes, Math.Min(codes.Length, 12));
         }
      }

      #endregion

      #region Properties

      /// <summary>
      /// 錯誤代碼 01-12 (LW113A-LW1145)
      /// </summary>
      public ushort[] ErrorCodes { get; set; } = new ushort[12];

      #endregion

      #region Public Methods

      public override string ToString()
      {
         if (ErrorCodes == null || ErrorCodes.Length == 0)
         {
            return "Alarm[No Errors]";
         }

         var activeCodes = ErrorCodes.Where(c => c != 0).ToArray();
         if (activeCodes.Length == 0)
         {
            return "Alarm[No Active Errors]";
         }

         return $"Alarm[{string.Join(", ", activeCodes)}]";
      }

      /// <summary>
      /// 新增警報碼到下一個空位置
      /// 會自動找到第一個空位（值為0或空字串對應的0x0000），然後依序填入新的警報碼
      /// </summary>
      /// <param name="newAlarmCodes">要新增的警報碼陣列</param>
      /// <returns>成功新增的警報碼數量（如果空間不足，則只會新增部分）</returns>
      public int AddAlarmCodes(params ushort[] newAlarmCodes)
      {
         if (newAlarmCodes == null || newAlarmCodes.Length == 0)
         {
            return 0;
         }

         int addedCount = 0;

         // 找到第一個空位置
         int currentIndex = 0;
         while (currentIndex < ErrorCodes.Length && ErrorCodes[currentIndex] != 0)
         {
            currentIndex++;
         }

         // 依序填入新的警報碼
         foreach (var alarmCode in newAlarmCodes)
         {
            if (currentIndex >= ErrorCodes.Length)
            {
               // 已經超過12個警報的上限
               break;
            }

            ErrorCodes[currentIndex] = alarmCode;
            currentIndex++;
            addedCount++;
         }

         return addedCount;
      }

      /// <summary>
      /// 清除所有警報，將LW113A開始的12個位址全部重置為0
      /// </summary>
      public void ClearAllAlarms()
      {
         for (int i = 0; i < ErrorCodes.Length; i++)
         {
            ErrorCodes[i] = 0;
         }
      }

      #endregion

      #region Equality Members

      public bool Equals(CommonReportAlarm other)
      {
         if (ReferenceEquals(null, other))
         {
            return false;
         }

         if (ReferenceEquals(this, other))
         {
            return true;
         }

         // 比對陣列內容
         if (ErrorCodes == null && other.ErrorCodes == null)
         {
            return true;
         }

         if (ErrorCodes == null || other.ErrorCodes == null)
         {
            return false;
         }

         if (ErrorCodes.Length != other.ErrorCodes.Length)
         {
            return false;
         }

         return ErrorCodes.SequenceEqual(other.ErrorCodes);
      }

      public override bool Equals(object obj)
      {
         return ReferenceEquals(this, obj) || obj is CommonReportAlarm other && Equals(other);
      }

      public override int GetHashCode()
      {
         if (ErrorCodes == null)
         {
            return 0;
         }

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
   }
}