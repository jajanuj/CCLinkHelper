using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApp1.CCLink
{
   public sealed class MelsecControlCard : ICCLinkController, IDisposable
   {
      #region Fields

      private readonly IMelsecApiAdapter _api;
      private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
      private readonly ControllerSettings _settings;
      private int _pathHandle = 0;
      private ControllerStatus _status = new ControllerStatus();

      #endregion

      #region Constructors

      public MelsecControlCard(ControllerSettings settings)
      {
         _settings = settings ?? throw new ArgumentNullException(nameof(settings));
         _api = new MelsecApiAdapter(settings.Isx64);
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
            short chan = (short)_settings.Port; // 依現場設定對應通道
            short mode = 1;                     // 依手冊定義（示例：1=開啟）
            int path;
            var rc = _api.mdOpen(chan, mode, out path);
            if (rc != 0)
            {
               throw MelsecException.FromCode(rc, nameof(_api.mdOpen));
            }

            _pathHandle = path;
            if (_settings.TimeoutMs > 0)
            {
               rc = _api.mdSetTimeout(_pathHandle, _settings.TimeoutMs);
               if (rc != 0)
               {
                  throw MelsecException.FromCode(rc, nameof(_api.mdSetTimeout));
               }
            }

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
               var rc = _api.mdClose(_pathHandle);
               if (rc != 0)
               {
                  throw MelsecException.FromCode(rc, nameof(_api.mdClose));
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

      public async Task<IReadOnlyList<bool>> ReadBitsAsync(string address, int count, CancellationToken ct = default)
      {
         var parsed = LinkDeviceAddress.Parse(address, count);
         await _lock.WaitAsync(ct).ConfigureAwait(false);
         try
         {
            short[] buffer = new short[count];
            int deviceCode = MapDevice(parsed.Kind);
            var rc = _api.mdDevRead(_pathHandle, deviceCode, parsed.Start, count, buffer);
            if (rc != 0)
            {
               throw MelsecException.FromCode(rc, nameof(_api.mdDevRead));
            }

            var bits = buffer.Select(x => x != 0).ToArray();
            return bits;
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
            var rc = _api.mdDevWrite(_pathHandle, deviceCode, parsed.Start, parsed.Length, src);
            if (rc != 0)
            {
               throw MelsecException.FromCode(rc, nameof(_api.mdDevWrite));
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
            var rc = _api.mdDevRead(_pathHandle, deviceCode, parsed.Start, count, buffer);
            if (rc != 0)
            {
               throw MelsecException.FromCode(rc, nameof(_api.mdDevRead));
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
            var rc = _api.mdDevWrite(_pathHandle, deviceCode, parsed.Start, parsed.Length, src);
            if (rc != 0)
            {
               throw MelsecException.FromCode(rc, nameof(_api.mdDevWrite));
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
            int statusCode;
            var rc = _api.mdGetStatus(_pathHandle, out statusCode);
            if (rc != 0)
            {
               throw MelsecException.FromCode(rc, nameof(_api.mdGetStatus));
            }

            _status.LastErrorCode = statusCode;
            _status.LastUpdated = DateTime.UtcNow;
            return _status;
         }
         finally
         {
            _lock.Release();
         }
      }

      public void Dispose()
      {
         if (_pathHandle != 0)
         {
            try
            {
               _api.mdClose(_pathHandle);
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
