using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WindowsFormsApp1.CCLink.Models;

namespace WindowsFormsApp1.CCLink.Interfaces
{
   /// <summary>
   /// CC-Link 控制器介面（非同步）。
   /// 提供開啟/關閉通訊路徑與批量讀寫 Link Bit/Word 的標準方法，
   /// UI 僅呼叫此介面，不直接接觸底層 MELSEC API。
   /// </summary>
   public interface ICCLinkController
   {
      #region Public Methods

      /// <summary>
      /// 開啟通訊路徑（非同步）。傳回例外時請參考 <see cref="MelsecException"/>。
      /// </summary>
      Task OpenAsync(CancellationToken ct = default);

      /// <summary>
      /// 關閉通訊路徑並釋放資源（非同步）。
      /// </summary>
      Task CloseAsync(CancellationToken ct = default);

      /// <summary>
      /// 讀取位元（LB/LX/LY）。
      /// </summary>
      Task<bool> ReadBitsAsync(string address, CancellationToken ct = default);

      /// <summary>
      /// 寫入位元（LB/LY）。
      /// </summary>
      Task WriteBitsAsync(string address, IEnumerable<bool> values, CancellationToken ct = default);

      /// <summary>
      /// 讀取字（LW）。
      /// </summary>
      Task<IReadOnlyList<short>> ReadWordsAsync(string address, int count, CancellationToken ct = default);

      /// <summary>
      /// 寫入字（LW/LY）。
      /// </summary>
      Task WriteWordsAsync(string address, IEnumerable<short> values, CancellationToken ct = default);

      /// <summary>
      /// 取得目前控制卡狀態與最近錯誤碼。
      /// </summary>
      Task<ControllerStatus> GetStatusAsync(CancellationToken ct = default);

      /// <summary>
      /// 從快取讀取位元狀態。
      /// </summary>
      bool GetBit(string address);

      /// <summary>
      /// 從快取讀取字組數值。
      /// </summary>
      short GetWord(string address);

      /// <summary>
      /// 設定自動掃描範圍。
      /// </summary>
      void SetScanRanges(IEnumerable<ScanRange> ranges);

      #endregion
   }
}
