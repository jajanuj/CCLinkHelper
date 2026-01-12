using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WindowsFormsApp1.CCLink.Interfaces;
using WindowsFormsApp1.CCLink.Models;

namespace WindowsFormsApp1.CCLink.Controllers
{
   /// <summary>
   /// 無硬體環境用的模擬控制器。可設定固定延遲與回傳資料模式。
   /// </summary>
   public sealed class MockCCLinkController : ICCLinkController
   {
      #region Fields

      private readonly ConcurrentDictionary<string, bool[]> _bitMemory = new ConcurrentDictionary<string, bool[]>(StringComparer.OrdinalIgnoreCase);

      private readonly Random _rand = new Random();
      private readonly ControllerSettings _settings;
      private readonly ConcurrentDictionary<string, short[]> _wordMemory = new ConcurrentDictionary<string, short[]>(StringComparer.OrdinalIgnoreCase);
      private ControllerStatus _status = new ControllerStatus();

      #endregion

      #region Constructors

      public MockCCLinkController(ControllerSettings settings) => _settings = settings;

      #endregion

      public Task OpenAsync(CancellationToken ct = default)
      {
         _status.IsConnected = true;
         _status.LastUpdated = DateTime.UtcNow;
         return Task.CompletedTask;
      }

      public Task CloseAsync(CancellationToken ct = default)
      {
         _status.IsConnected = false;
         _status.LastUpdated = DateTime.UtcNow;
         return Task.CompletedTask;
      }

      public async Task<bool> ReadBitsAsync(string address, CancellationToken ct = default)
      {
         await Task.Delay(_settings.RetryBackoffMs, ct).ConfigureAwait(false);
         if (_bitMemory.TryGetValue(address, out var mem))
         {
            return mem.FirstOrDefault();
         }

         return _rand.Next(0, 2) == 1;
      }

      public async Task WriteBitsAsync(string address, IEnumerable<bool> values, CancellationToken ct = default)
      {
         await Task.Delay(_settings.RetryBackoffMs, ct).ConfigureAwait(false);
         _bitMemory[address] = values.ToArray();
      }

      public async Task<IReadOnlyList<short>> ReadWordsAsync(string address, int count, CancellationToken ct = default)
      {
         await Task.Delay(_settings.RetryBackoffMs, ct).ConfigureAwait(false);
         if (_wordMemory.TryGetValue(address, out var mem))
         {
            return mem.Take(count).ToArray();
         }

         return Enumerable.Range(0, count).Select(i => (short)_rand.Next(short.MinValue, short.MaxValue)).ToArray();
      }

      public async Task WriteWordsAsync(string address, IEnumerable<short> values, CancellationToken ct = default)
      {
         await Task.Delay(_settings.RetryBackoffMs, ct).ConfigureAwait(false);
         _wordMemory[address] = values.ToArray();
      }

      public Task<ControllerStatus> GetStatusAsync(CancellationToken ct = default)
      {
         _status.LastUpdated = DateTime.UtcNow;
         _status.LastErrorCode = 0;
         return Task.FromResult(_status);
      }

      public bool GetBit(string address)
      {
         if (_bitMemory.TryGetValue(address, out var mem))
         {
            return mem.FirstOrDefault();
         }

         return false;
      }

      public short GetWord(string address)
      {
         if (_wordMemory.TryGetValue(address, out var mem))
         {
            return mem.FirstOrDefault();
         }

         return 0;
      }

      public void SetScanRanges(IEnumerable<ScanRange> ranges)
      {
         // Mock Controller 可以選擇忽略掃描範圍，因為它主要依賴 Read/Write 觸發記憶體更新
         // 或可在此處初始化記憶體
      }
   }
}