using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Models
{
   public class LinkReportData
   {
      public string BoardId { get; set; }
      public DateTime StartTime { get; set; }
      public DateTime EndTime { get; set; }
      public ushort RecipeNo { get; set; }

      public short[] ToRawData()
      {
         // Total 17 words
         // 0-9: Tracking Data (10 words)
         // 10: Start Year/Month
         // 11: Start Day/Hour
         // 12: Start Min/Sec
         // 13: End Year/Month
         // 14: End Day/Hour
         // 15: End Min/Sec
         // 16: Recipe No

         short[] data = new short[17];

         // 1. Board ID (10 words = 20 bytes)
         byte[] idBytes = new byte[20];
         if (!string.IsNullOrEmpty(BoardId))
         {
            byte[] source = Encoding.ASCII.GetBytes(BoardId);
            Array.Copy(source, idBytes, Math.Min(source.Length, 20));
         }

         for (int i = 0; i < 10; i++)
         {
            // Little Endian packing for short array from byte array
            data[i] = (short)(idBytes[i * 2] | (idBytes[i * 2 + 1] << 8));
         }

         // 2. Start Time
         data[10] = EncodeDateTimeWord(StartTime.Year, StartTime.Month);
         data[11] = EncodeDateTimeWord(StartTime.Day, StartTime.Hour);
         data[12] = EncodeDateTimeWord(StartTime.Minute, StartTime.Second);

         // 3. End Time
         data[13] = EncodeDateTimeWord(EndTime.Year, EndTime.Month);
         data[14] = EncodeDateTimeWord(EndTime.Day, EndTime.Hour);
         data[15] = EncodeDateTimeWord(EndTime.Minute, EndTime.Second);

         // 4. Recipe No
         data[16] = (short)RecipeNo;

         return data;
      }

      private short EncodeDateTimeWord(int high, int low)
      {
         // Assuming decimal encoding in hex: year 2025 -> 25.
         // Format: High Byte = First Value, Low Byte = Second Value?
         // OR BCD?
         // "yyMM" usually means 2501 (decimal) -> 0x09C5 in hex value.
         // Let's assume Decimal Value encoded as Word: (yy * 100) + MM
         
         int y = high % 100; // 2025 -> 25
         return (short)((y * 100) + low);
      }
   }
}