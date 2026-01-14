using System;
using System.Text;

namespace WindowsFormsApp1.Models
{
   /// <summary>
   /// 關聯數據回報模型
   /// </summary>
   public class LinkReportData
   {
      #region Properties

      /// <summary>
      /// 基板條碼 ID (追蹤資料)
      /// </summary>
      public string BoardId { get; set; }

      /// <summary>
      /// 基板開始時間
      /// </summary>
      public DateTime StartTime { get; set; }

      /// <summary>
      /// 基板結束時間
      /// </summary>
      public DateTime EndTime { get; set; }

      /// <summary>
      /// 生產配方編號
      /// </summary>
      public ushort RecipeNo { get; set; }

      #endregion

      #region Public Methods

      /// <summary>
      /// 將數據轉換為 PLC 寫入所需的原始 short 陣列格式 (總計 17 words)
      /// </summary>
      /// <returns>回傳 17 words 的 short 陣列</returns>
      public short[] ToRawData()
      {
         // 總計 17 words:
         // 0-9: 追蹤資料 (10 words)
         // 10: 開始時間 年月
         // 11: 開始時間 日時
         // 12: 開始時間 分秒
         // 13: 結束時間 年月
         // 14: 結束時間 日時
         // 15: 結束時間 分秒
         // 16: 配方編號 (Recipe No)

         short[] data = new short[17];

         // 1. 基板條碼 ID (10 words = 20 bytes)
         byte[] idBytes = new byte[20];
         if (!string.IsNullOrEmpty(BoardId))
         {
            byte[] source = Encoding.ASCII.GetBytes(BoardId);
            Array.Copy(source, idBytes, Math.Min(source.Length, 20));
         }

         for (int i = 0; i < 10; i++)
         {
            // 將 byte 陣列以 Little Endian 方式打包進 short 陣列
            data[i] = (short)(idBytes[i * 2] | (idBytes[i * 2 + 1] << 8));
         }

         // 2. 基板開始時間
         data[10] = EncodeDateTimeWord(StartTime.Year, StartTime.Month);
         data[11] = EncodeDateTimeWord(StartTime.Day, StartTime.Hour);
         data[12] = EncodeDateTimeWord(StartTime.Minute, StartTime.Second);

         // 3. 基板結束時間
         data[13] = EncodeDateTimeWord(EndTime.Year, EndTime.Month);
         data[14] = EncodeDateTimeWord(EndTime.Day, EndTime.Hour);
         data[15] = EncodeDateTimeWord(EndTime.Minute, EndTime.Second);

         // 4. 配方編號
         data[16] = (short)RecipeNo;

         return data;
      }

      #endregion

      #region Private Methods

      /// <summary>
      /// 將兩個時間單位編碼為一個 16 位元的十進制 Word (格式：yyMM, ddHH, mmss)
      /// </summary>
      /// <param name="high">高位數值 (如：年、日、分)</param>
      /// <param name="low">低位數值 (如：月、時、秒)</param>
      /// <returns>編碼後的 short 值</returns>
      private short EncodeDateTimeWord(int high, int low)
      {
         // 採用十進制編碼：(高位 % 100) * 100 + 低位
         // 例如：2025年1月 -> (25 * 100) + 1 = 2501
         
         int h = high % 100;
         return (short)(h * 100 + low);
      }

      #endregion
   }
}