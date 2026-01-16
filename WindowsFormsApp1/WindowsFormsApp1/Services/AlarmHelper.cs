using System;
using System.Linq;
using System.Threading.Tasks;
using WindowsFormsApp1.CCLink.Interfaces;

namespace WindowsFormsApp1.Services
{
   /// <summary>
   /// PLC 警報碼管理輔助類
   /// 提供新增、清除和移除警報碼的靜態方法
   /// </summary>
   public static class AlarmHelper
   {
      /// <summary>
      /// 預設的警報碼數量上限
      /// </summary>
      private const int MaxAlarmCount = 12;

      /// <summary>
      /// 預設的警報起始位址
      /// </summary>
      private const string DefaultAlarmAddress = "LW113A";

      /// <summary>
      /// 新增警報碼到 PLC
      /// 會自動排除重複的警報碼，並在空間不足時返回被忽略的警報碼
      /// </summary>
      /// <param name="controller">PLC 控制器介面</param>
      /// <param name="newAlarmCodes">要新增的警報碼陣列</param>
      /// <param name="baseAddress">警報起始位址，預設為 LW113A</param>
      /// <returns>
      /// 元組：
      /// - AddedCount: 成功新增的警報碼數量
      /// - IgnoredCodes: 因空間不足而被忽略的警報碼陣列
      /// </returns>
      /// <exception cref="ArgumentNullException">controller 或 newAlarmCodes 為 null</exception>
      /// <exception cref="InvalidOperationException">PLC 讀寫失敗</exception>
      public static async Task<(int AddedCount, ushort[] IgnoredCodes)> AddAlarmCodesAsync(
         ICCLinkController controller,
         ushort[] newAlarmCodes,
         string baseAddress = DefaultAlarmAddress)
      {
         if (controller == null)
            throw new ArgumentNullException(nameof(controller));
         
         if (newAlarmCodes == null)
            throw new ArgumentNullException(nameof(newAlarmCodes));

         if (newAlarmCodes.Length == 0)
            return (0, Array.Empty<ushort>());

         try
         {
            // 1. 讀取目前的警報碼
            var currentAlarms = await ReadAlarmCodesAsync(controller, baseAddress);

            // 2. 找出目前已存在的警報碼（非 0）
            var existingCodes = currentAlarms.Where(code => code != 0).ToHashSet();

            // 3. 排除重複的警報碼
            var uniqueNewCodes = newAlarmCodes.Where(code => code != 0 && !existingCodes.Contains(code)).ToArray();

            if (uniqueNewCodes.Length == 0)
            {
               // 所有新警報都是重複的
               return (0, Array.Empty<ushort>());
            }

            // 4. 找到第一個空位置
            int emptySlotIndex = Array.IndexOf(currentAlarms, (ushort)0);
            if (emptySlotIndex == -1)
            {
               // 沒有空位，全部忽略
               return (0, uniqueNewCodes);
            }

            // 5. 計算可以新增的數量
            int availableSlots = MaxAlarmCount - emptySlotIndex;
            int addedCount = Math.Min(availableSlots, uniqueNewCodes.Length);
            
            // 6. 填入新的警報碼
            for (int i = 0; i < addedCount; i++)
            {
               currentAlarms[emptySlotIndex + i] = uniqueNewCodes[i];
            }

            // 7. 寫回 PLC
            await WriteAlarmCodesAsync(controller, currentAlarms, baseAddress);

            // 8. 返回結果
            var ignoredCodes = uniqueNewCodes.Skip(addedCount).ToArray();
            return (addedCount, ignoredCodes);
         }
         catch (Exception ex)
         {
            throw new InvalidOperationException($"新增警報碼失敗: {ex.Message}", ex);
         }
      }

      /// <summary>
      /// 清除所有警報碼
      /// 將 PLC 中的所有警報位址重置為 0
      /// </summary>
      /// <param name="controller">PLC 控制器介面</param>
      /// <param name="baseAddress">警報起始位址，預設為 LW113A</param>
      /// <exception cref="ArgumentNullException">controller 為 null</exception>
      /// <exception cref="InvalidOperationException">PLC 寫入失敗</exception>
      public static async Task ClearAllAlarmsAsync(
         ICCLinkController controller,
         string baseAddress = DefaultAlarmAddress)
      {
         if (controller == null)
            throw new ArgumentNullException(nameof(controller));

         try
         {
            var emptyAlarms = new ushort[MaxAlarmCount];
            await WriteAlarmCodesAsync(controller, emptyAlarms, baseAddress);
         }
         catch (Exception ex)
         {
            throw new InvalidOperationException($"清除警報碼失敗: {ex.Message}", ex);
         }
      }

      /// <summary>
      /// 移除指定的警報碼
      /// 會從警報列表中移除指定的警報碼，並將後續的警報碼向前移動以填補空位
      /// </summary>
      /// <param name="controller">PLC 控制器介面</param>
      /// <param name="alarmCode">要移除的警報碼</param>
      /// <param name="baseAddress">警報起始位址，預設為 LW113A</param>
      /// <returns>true 表示成功移除，false 表示找不到該警報碼</returns>
      /// <exception cref="ArgumentNullException">controller 為 null</exception>
      /// <exception cref="InvalidOperationException">PLC 讀寫失敗</exception>
      public static async Task<bool> RemoveAlarmCodeAsync(
         ICCLinkController controller,
         ushort alarmCode,
         string baseAddress = DefaultAlarmAddress)
      {
         if (controller == null)
            throw new ArgumentNullException(nameof(controller));

         if (alarmCode == 0)
            return false; // 0 不是有效的警報碼

         try
         {
            // 1. 讀取目前的警報碼
            var currentAlarms = await ReadAlarmCodesAsync(controller, baseAddress);

            // 2. 找到目標警報碼的位置
            int targetIndex = Array.IndexOf(currentAlarms, alarmCode);
            if (targetIndex == -1)
            {
               // 找不到該警報碼
               return false;
            }

            // 3. 將後續的警報碼向前移動（壓縮）
            for (int i = targetIndex; i < MaxAlarmCount - 1; i++)
            {
               currentAlarms[i] = currentAlarms[i + 1];
            }
            currentAlarms[MaxAlarmCount - 1] = 0; // 最後一個位置設為 0

            // 4. 寫回 PLC
            await WriteAlarmCodesAsync(controller, currentAlarms, baseAddress);

            return true;
         }
         catch (Exception ex)
         {
            throw new InvalidOperationException($"移除警報碼失敗: {ex.Message}", ex);
         }
      }

      #region Private Helper Methods

      /// <summary>
      /// 從 PLC 讀取警報碼
      /// </summary>
      private static async Task<ushort[]> ReadAlarmCodesAsync(ICCLinkController controller, string baseAddress)
      {
         var words = await controller.ReadWordsAsync(baseAddress, MaxAlarmCount);
         return words.Select(w => (ushort)w).ToArray();
      }

      /// <summary>
      /// 將警報碼寫入 PLC
      /// </summary>
      private static async Task WriteAlarmCodesAsync(ICCLinkController controller, ushort[] alarmCodes, string baseAddress)
      {
         if (alarmCodes.Length != MaxAlarmCount)
            throw new ArgumentException($"警報碼陣列長度必須為 {MaxAlarmCount}");

         var words = alarmCodes.Select(code => (short)code).ToArray();
         await controller.WriteWordsAsync(baseAddress, words);
      }

      #endregion
   }
}
