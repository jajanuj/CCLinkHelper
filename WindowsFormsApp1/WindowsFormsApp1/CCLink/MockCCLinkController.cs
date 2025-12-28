using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApp1.CCLink
{
   /// <summary>
   /// 無硬體環境用的模擬控制器。可設定固定延遲與回傳資料模式。
   /// </summary>
   public sealed class MockCCLinkController : ICCLinkController
   {
      #region Fields

      private readonly Random _rand = new Random();
      private readonly ControllerSettings _settings;
      private bool _connected;
      private ControllerStatus _status = new ControllerStatus();

      #endregion

      #region Constructors

      public MockCCLinkController(ControllerSettings settings) => _settings = settings;

      #endregion

      public Task OpenAsync(CancellationToken ct = default)
      {
         _connected = true;
         _status.IsConnected = true;
         _status.LastUpdated = DateTime.UtcNow;
         return Task.CompletedTask;
      }

      public Task CloseAsync(CancellationToken ct = default)
      {
         _connected = false;
         _status.IsConnected = false;
         _status.LastUpdated = DateTime.UtcNow;
         return Task.CompletedTask;
      }

      public async Task<IReadOnlyList<bool>> ReadBitsAsync(string address, int count, CancellationToken ct = default)
      {
         await Task.Delay(_settings.RetryBackoffMs, ct).ConfigureAwait(false);
         return Enumerable.Range(0, count).Select(i => _rand.Next(0, 2) == 1).ToArray();
      }

      public async Task WriteBitsAsync(string address, IEnumerable<bool> values, CancellationToken ct = default)
      {
         await Task.Delay(_settings.RetryBackoffMs, ct).ConfigureAwait(false);
      }

      public async Task<IReadOnlyList<short>> ReadWordsAsync(string address, int count, CancellationToken ct = default)
      {
         await Task.Delay(_settings.RetryBackoffMs, ct).ConfigureAwait(false);
         return Enumerable.Range(0, count).Select(i => (short)_rand.Next(short.MinValue, short.MaxValue)).ToArray();
      }

      public async Task WriteWordsAsync(string address, IEnumerable<short> values, CancellationToken ct = default)
      {
         await Task.Delay(_settings.RetryBackoffMs, ct).ConfigureAwait(false);
      }

      public Task<ControllerStatus> GetStatusAsync(CancellationToken ct = default)
      {
         _status.LastUpdated = DateTime.UtcNow;
         _status.LastErrorCode = 0;
         return Task.FromResult(_status);
      }
   }
}
