using System;

namespace WindowsFormsApp1.CCLink.Models
{
   /// <summary>
   /// CC-Link 位址封裝。支援 LB/LW/LX/LY 起始與長度，並提供字串解析。
   /// </summary>
   public sealed class LinkDeviceAddress
   {
      #region Constructors

      public LinkDeviceAddress(string kind, int start, int length, int bitWidth = 16, string endian = "Big")
      {
         if (string.IsNullOrWhiteSpace(kind))
         {
            throw new ArgumentException("kind");
         }

         if (start < 0)
         {
            throw new ArgumentOutOfRangeException(nameof(start));
         }

         if (length <= 0)
         {
            throw new ArgumentOutOfRangeException(nameof(length));
         }

         Kind = kind;
         Start = start;
         Length = length;
         BitWidth = bitWidth;
         Endian = endian;
      }

      #endregion

      #region Properties

      /// <summary>裝置種類（LB/LW/LX/LY）。</summary>
      public string Kind { get; }

      /// <summary>起始位址（非負整數）。</summary>
      public int Start { get; }

      /// <summary>批量長度（必須 > 0）。</summary>
      public int Length { get; }

      /// <summary>位寬（預設 16）。</summary>
      public int BitWidth { get; }

      /// <summary>位元序設定（Big/Little）。</summary>
      public string Endian { get; }

      #endregion

      #region Public Methods

      /// <summary>
      /// 解析位址字串，如 "LW100"。無效格式會擲出 <see cref="ArgumentException"/>。
      /// </summary>
      public static LinkDeviceAddress Parse(string address, int length)
      {
         if (string.IsNullOrWhiteSpace(address))
         {
            throw new ArgumentException(nameof(address));
         }

         var prefix = address.Substring(0, 2).ToUpperInvariant();
         var numPart = address.Substring(2);
         if (!(prefix == "LB" || prefix == "LW" || prefix == "LX" || prefix == "LY"))
         {
            throw new ArgumentException("Invalid prefix");
         }

         // 嘗試以十六進位解析，若失敗則嘗試十進位
         int start;
         if (!int.TryParse(numPart, System.Globalization.NumberStyles.HexNumber, null, out start))
         {
            if (!int.TryParse(numPart, out start))
            {
               throw new ArgumentException("Invalid number");
            }
         }

         return new LinkDeviceAddress(prefix, start, length);
      }

      /// <summary>
      /// 回傳格式化的地址字串 (e.g., "LW0100")。
      /// </summary>
      public override string ToString()
      {
         return $"{Kind}{Start:X4}";
      }

      #endregion
   }
}
