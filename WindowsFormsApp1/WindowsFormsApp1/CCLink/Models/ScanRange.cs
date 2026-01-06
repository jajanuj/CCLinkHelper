namespace WindowsFormsApp1.CCLink.Models
{
   /// <summary>
   /// 定義要掃描的設備區域（對應 MelsecHelper 內部的掃描規劃）。
   /// </summary>
   public sealed class ScanRange
   {
      #region Properties

      /// <summary>設備種類 (e.g., LB, LW, LX, LY)。</summary>
      public string Kind { get; set; }

      /// <summary>起始位址。</summary>
      public int Start { get; set; }

      /// <summary>結束位址。</summary>
      public int End { get; set; }

      /// <summary>計算出的長度。</summary>
      public int Length => End - Start + 1;

      #endregion
   }
}
