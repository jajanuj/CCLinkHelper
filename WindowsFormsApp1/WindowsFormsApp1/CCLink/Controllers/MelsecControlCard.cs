using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WindowsFormsApp1.CCLink.Adapters;
using WindowsFormsApp1.CCLink.Interfaces;
using WindowsFormsApp1.CCLink.Models;

namespace WindowsFormsApp1.CCLink.Controllers
{
   public sealed class MelsecControlCard : ICCLinkController, IDisposable
   {
      #region Fields

      private readonly IMelsecApiAdapter _api;
      private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
      private readonly ControllerSettings _settings;
      private int _pathHandle;
      private ControllerStatus _status = new ControllerStatus();

      #endregion

      #region Constructors

      public MelsecControlCard(ControllerSettings settings)
      {
         _settings = settings ?? throw new ArgumentNullException(nameof(settings));
         _api = new MelsecApiAdapter();
      }

      #endregion

      #region Delegates, Events

      public event EventHandler<ControllerStatusChangedEventArgs> StatusChanged;

      #endregion

      #region Private Methods

      private static int MapDevice(string kind)
      {
         switch (kind)
         {
            case "LB": return 23;
            case "LW": return 24;
            case "LX": return 21; // 依手冊確認
            case "LY": return 22; // 依手冊確認
            default: throw new ArgumentException("Unknown device kind: " + kind);
         }
      }

      #endregion

      public async Task OpenAsync(CancellationToken ct = default)
      {
         await _lock.WaitAsync(ct).ConfigureAwait(false);
         try
         {
            short chan = (short)_settings.Channel; // 依現場設定對應通道
            short mode = 1;                        // 依手冊定義（示例：1=開啟）
            int path;
            var rc = _api.Open(chan, mode, out path);
            if (rc != 0)
            {
               throw MelsecException.FromCode(rc, nameof(_api.Open));
            }

            _pathHandle = path;

            _status.IsConnected = true;
            _status.LastUpdated = DateTime.UtcNow;
            StatusChanged?.Invoke(this, new ControllerStatusChangedEventArgs(_status));
         }
         finally
         {
            _lock.Release();
         }
      }

      public async Task CloseAsync(CancellationToken ct = default)
      {
         await _lock.WaitAsync(ct).ConfigureAwait(false);
         try
         {
            if (_pathHandle != 0)
            {
               var rc = _api.Close(_pathHandle);
               if (rc != 0)
               {
                  throw MelsecException.FromCode(rc, nameof(_api.Close));
               }

               _pathHandle = 0;
            }

            _status.IsConnected = false;
            _status.LastUpdated = DateTime.UtcNow;
            StatusChanged?.Invoke(this, new ControllerStatusChangedEventArgs(_status));
         }
         finally
         {
            _lock.Release();
         }
      }

      public async Task<bool> ReadBitsAsync(string address, CancellationToken ct = default)
      {
         // 解析位址，例如 LB0001
         var parsed = LinkDeviceAddress.Parse(address, 1);
         await _lock.WaitAsync(ct).ConfigureAwait(false);
         try
         {
            int deviceCode = MapDevice(parsed.Kind);

            // 計算對齊到 8 的倍數的起始位址（向下取整）
            // 例如：LB0001 -> LB0000, LB0009 -> LB0008
            int alignedStart = parsed.Start / 8 * 8;

            // 計算目標位元在 8 位元組中的位置 (0-7)
            int bitPosition = parsed.Start - alignedStart;

            // 讀取位元軟元件，size 固定為 1
            int size = 1;
            short[] buffer = new short[1];

            var rc = _api.ReceiveEx(_pathHandle, 0, 0, deviceCode, alignedStart, ref size, buffer);
            if (rc != 0)
            {
               throw MelsecException.FromCode(rc, nameof(_api.ReceiveEx));
            }

            // 從 buffer[0] 中提取特定位元
            // buffer[0] 包含 8 個位元 (bit 0-7)
            // 例如：buffer[0] = 2 (二進制: 0000 0010) 表示 bit 1 = true
            int bitValue = (buffer[0] >> bitPosition) & 1;
            return bitValue != 0;
         }
         finally
         {
            _lock.Release();
         }
      }

      public async Task WriteBitsAsync(string address, IEnumerable<bool> values, CancellationToken ct = default)
      {
         var vals = values?.ToArray() ?? Array.Empty<bool>();
         var parsed = LinkDeviceAddress.Parse(address, vals.Length);
         await _lock.WaitAsync(ct).ConfigureAwait(false);
         try
         {
            short[] src = vals.Select(v => (short)(v ? 1 : 0)).ToArray();
            int deviceCode = MapDevice(parsed.Kind);
            int size = src.Length * 2;
            var rc = _api.SendEx(_pathHandle, 0, 0, deviceCode, parsed.Start, ref size, src);
            if (rc != 0)
            {
               throw MelsecException.FromCode(rc, nameof(_api.SendEx));
            }
         }
         finally
         {
            _lock.Release();
         }
      }

      public async Task<IReadOnlyList<short>> ReadWordsAsync(string address, int count, CancellationToken ct = default)
      {
         var parsed = LinkDeviceAddress.Parse(address, count);
         await _lock.WaitAsync(ct).ConfigureAwait(false);
         try
         {
            short[] buffer = new short[count];
            int deviceCode = MapDevice(parsed.Kind);
            int size = count * 2;
            var rc = _api.ReceiveEx(_pathHandle, 0, 0, deviceCode, parsed.Start, ref size, buffer);
            if (rc != 0)
            {
               throw MelsecException.FromCode(rc, nameof(_api.ReceiveEx));
            }

            return buffer;
         }
         finally
         {
            _lock.Release();
         }
      }

      public async Task WriteWordsAsync(string address, IEnumerable<short> values, CancellationToken ct = default)
      {
         var src = values?.ToArray() ?? Array.Empty<short>();
         var parsed = LinkDeviceAddress.Parse(address, src.Length);
         await _lock.WaitAsync(ct).ConfigureAwait(false);
         try
         {
            int deviceCode = MapDevice(parsed.Kind);
            int size = src.Length * 2;
            var rc = _api.SendEx(_pathHandle, 0, 0, deviceCode, parsed.Start, ref size, src);
            if (rc != 0)
            {
               throw MelsecException.FromCode(rc, nameof(_api.SendEx));
            }
         }
         finally
         {
            _lock.Release();
         }
      }

      public async Task<ControllerStatus> GetStatusAsync(CancellationToken ct = default)
      {
         await _lock.WaitAsync(ct).ConfigureAwait(false);
         try
         {
            _status.LastErrorCode = 0;
            _status.LastUpdated = DateTime.UtcNow;
            return await Task.FromResult(_status).ConfigureAwait(false);
         }
         finally
         {
            _lock.Release();
         }
      }

      public bool GetBit(string address) => false;
      public short GetWord(string address) => 0;

      public void SetScanRanges(IEnumerable<ScanRange> ranges)
      {
         // MelsecControlCard 目前不支援自動掃描與快取，此方法為介面實作空方法
      }

      public void Dispose()
      {
         if (_pathHandle != 0)
         {
            try
            {
               _api.Close(_pathHandle);
            }
            catch
            {
               /* swallow during dispose */
            }

            _pathHandle = 0;
         }

         _lock.Dispose();
      }
   }
}