using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WindowsFormsApp1.CCLink.Interfaces;
using WindowsFormsApp1.CCLink.Models;

namespace WindowsFormsApp1.CCLink.Services
{
   public sealed class MelsecHelper : ICCLinkController, IDisposable
   {
      #region Constant

      private const short OpenMode = -1;

      private struct BatchRead
      {
         public string Kind;
         public int Start;
         public int Words;
      }

      #endregion

      #region Fields

      // Connection & API
      private readonly IMelsecApiAdapter _api;

      // Synchronization
      private readonly object _apiLock = new object();

      // Device Cache: Key = Kind (e.g. "LB"), Value = short[]
      private readonly Dictionary<string, short[]> _deviceMemory = new Dictionary<string, short[]>(StringComparer.OrdinalIgnoreCase);

      private readonly Action<string> _logger;
      private readonly object _planLock = new object();
      private readonly ControllerSettings _settings;

      // State
      private readonly ControllerStatus _status = new ControllerStatus();
      private readonly SynchronizationContext _syncContext;
      private readonly List<ScanRange> _userScanRanges = new List<ScanRange>();
      private bool _disposed;
      private List<BatchRead> _pollingPlan = new List<BatchRead>();

      // Polling & Cache
      private TimeSpan _pollInterval;
      private int _resolvedPath = -1;
      private readonly int _mergeGapTolerance = 4; // 合併相鄰掃描範圍的容差

      // Worker
      private CancellationTokenSource _workerCts;
      private Task _workerTask;

      #endregion

      #region Private Properties

      private int NetworkNo => _settings.NetworkNo;
      private int StationNo => _settings.StationNo;

      #endregion

      #region Constructors

      public MelsecHelper(IMelsecApiAdapter api, ControllerSettings settings, TimeSpan? pollInterval = null, Action<string> logger = null)
      {
         _api = api ?? throw new ArgumentNullException(nameof(api));
         _settings = settings ?? throw new ArgumentNullException(nameof(settings));
         _pollInterval = pollInterval ?? TimeSpan.FromMilliseconds(200);
         _logger = logger;
         _syncContext = SynchronizationContext.Current;
      }

      #endregion

      #region Properties

      public TimeSpan PollInterval
      {
         get => _pollInterval;
         set
         {
            if (value <= TimeSpan.Zero)
            {
               throw new ArgumentOutOfRangeException(nameof(value));
            }

            _pollInterval = value;
         }
      }

      #endregion

      #region Public Methods

      public void SetScanRanges(IEnumerable<ScanRange> ranges)
      {
         if (ranges == null)
         {
            throw new ArgumentNullException(nameof(ranges));
         }

         lock (_planLock)
         {
            _userScanRanges.Clear();
            _userScanRanges.AddRange(ranges);
         }

         UpdatePollingPlan();
         EnsurePollingWorkerStarted();
      }

      public bool GetBit(string kind, int address)
      {
         if (string.IsNullOrWhiteSpace(kind))
         {
            return false;
         }

         lock (_apiLock)
         {
            if (_deviceMemory.ContainsKey(kind))
            {
               var memory = _deviceMemory[kind];
               if (address >= 0 && address < memory.Length)
               {
                  return memory[address] != 0;
               }
            }
         }

         return false;
      }

      public short GetWord(string kind, int address)
      {
         if (string.IsNullOrWhiteSpace(kind))
         {
            return 0;
         }

         lock (_apiLock)
         {
            if (_deviceMemory.ContainsKey(kind))
            {
               var memory = _deviceMemory[kind];
               if (address >= 0 && address < memory.Length)
               {
                  return memory[address];
               }
            }
         }

         return 0;
      }

      #endregion

      #region Private Methods

      private void UpdateCache(string kind, int start, short[] data)
      {
         if (_deviceMemory.ContainsKey(kind))
         {
            var memory = _deviceMemory[kind];
            if (start + data.Length <= memory.Length)
            {
               Array.Copy(data, 0, memory, start, data.Length);
            }
            else
            {
               // Expand if needed
               int newSize = Math.Max(16384, start + data.Length);
               var newMem = new short[newSize];
               Array.Copy(memory, newMem, memory.Length);
               Array.Copy(data, 0, newMem, start, data.Length);
               _deviceMemory[kind] = newMem;
            }
         }
      }

      private void EnsurePollingWorkerStarted()
      {
         if (_workerTask == null)
         {
            _workerCts = new CancellationTokenSource();
            _workerTask = Task.Run(() => PollingWorkerLoopAsync(_workerCts.Token));
         }
      }

      private void StopPollingWorker()
      {
         if (_workerCts != null)
         {
            _workerCts.Cancel();
            try
            {
               _workerTask?.Wait(1000);
            }
            catch
            {
            }

            _workerCts.Dispose();
            _workerCts = null;
            _workerTask = null;
         }
      }

      private async Task PollingWorkerLoopAsync(CancellationToken ct)
      {
         DateTime nextPoll = DateTime.UtcNow + _pollInterval;

         while (!ct.IsCancellationRequested && !_disposed)
         {
            try
            {
               bool hasPlan;
               lock (_planLock)
               {
                  hasPlan = _pollingPlan.Count > 0;
               }

               if (!hasPlan)
               {
                  await Task.Delay(1000, ct).ConfigureAwait(false);
                  nextPoll = DateTime.UtcNow + _pollInterval;
                  continue;
               }

               TimeSpan delay = nextPoll - DateTime.UtcNow;
               if (delay > TimeSpan.Zero)
               {
                  await Task.Delay(delay, ct).ConfigureAwait(false);
               }

               PollAddresses();

               nextPoll += _pollInterval;
               if (nextPoll < DateTime.UtcNow)
               {
                  nextPoll = DateTime.UtcNow + _pollInterval;
               }
            }
            catch (OperationCanceledException)
            {
               break;
            }
            catch (Exception ex)
            {
               _logger?.Invoke($"Polling Error: {ex.Message}");
               await Task.Delay(_pollInterval, ct);
            }
         }
      }

      private void PollAddresses()
      {
         List<BatchRead> batches;
         lock (_planLock)
         {
            batches = _pollingPlan.ToList();
         }

         lock (_apiLock)
         {
            if (!_status.IsConnected)
            {
               return;
            }

            foreach (var batch in batches)
            {
               if (IsBitDevice(batch.Kind))
               {
                  PollBitDeviceBatch(batch);
               }
               else
               {
                  PollWordDeviceBatch(batch);
               }
            }
         }
      }

      private void PollBitDeviceBatch(BatchRead batch)
      {
         int path = _resolvedPath;
         int devCode = MapDeviceCode(batch.Kind);
         int alignedStart = batch.Start / 8 * 8;
         int alignedEnd = (batch.Start + batch.Words - 1) / 8 * 8 + 7;
         int alignedWords = (alignedEnd - alignedStart + 1 + 7) / 8;
         int size = alignedWords;
         var buffer = new short[(size + 1) / 2];

         int rc = _api.ReceiveEx(path, NetworkNo, StationNo, devCode, alignedStart, ref size, buffer);
         UpdateConnectionStatus(rc);

         if (rc == 0)
         {
            var dest = new short[alignedWords];
            for (int i = 0; i < alignedWords; i++)
            {
               int bufferIndex = i / 2;
               bool isLower = i % 2 == 0;
               if (bufferIndex < buffer.Length)
               {
                  short raw = buffer[bufferIndex];
                  dest[i] = (short)(isLower ? raw & 0xFF : (raw >> 8) & 0xFF);
               }
            }

            UpdateBitCache(batch, alignedStart, dest);
         }
      }

      private void PollWordDeviceBatch(BatchRead batch)
      {
         int size = batch.Words * 2;
         var dest = new short[batch.Words];
         int rc = _api.ReceiveEx(_resolvedPath, NetworkNo, StationNo, MapDeviceCode(batch.Kind), batch.Start, ref size, dest);
         UpdateConnectionStatus(rc);

         if (rc == 0 && _deviceMemory.ContainsKey(batch.Kind))
         {
            var memory = _deviceMemory[batch.Kind];
            Array.Copy(dest, 0, memory, batch.Start, batch.Words);
         }
      }

      private void UpdateBitCache(BatchRead batch, int alignedStart, short[] dest)
      {
         if (!_deviceMemory.TryGetValue(batch.Kind, out var memory))
         {
            return;
         }

         for (int i = 0; i < batch.Words; i++)
         {
            int addr = batch.Start + i;
            int alignedIndex = (addr - alignedStart) / 8;
            int bitPos = (addr - alignedStart) % 8;
            if (alignedIndex < dest.Length && addr < memory.Length)
            {
               memory[addr] = (short)((dest[alignedIndex] >> bitPos) & 1);
            }
         }
      }

      private void UpdateConnectionStatus(int rc)
      {
         _status.IsConnected = rc == 0;
         _status.LastErrorCode = rc;
      }

      private void UpdatePollingPlanInternal()
      {
         lock (_planLock)
         {
            UpdatePollingPlan();
         }
      }

      private void UpdatePollingPlan()
      {
         var newPlan = new List<BatchRead>();
         List<ScanRange> ranges;
         lock (_planLock)
         {
            ranges = _userScanRanges.ToList();
         }

         // 過濾掉不支援的裝置類型（MelsecHelper 只支援 CC-Link 裝置）
         var supportedRanges = ranges.Where(r =>
         {
            string kind = r.Kind?.ToUpperInvariant() ?? "";
            return kind == "LB" || kind == "LW" || kind == "LX" || kind == "LY";
         }).ToList();

         var groups = supportedRanges.GroupBy(r => r.Kind, StringComparer.OrdinalIgnoreCase);

         lock (_apiLock)
         {
            foreach (var g in groups)
            {
               string kind = g.Key.ToUpperInvariant();
               int maxEnd = g.Max(r => r.End);

               if (!_deviceMemory.ContainsKey(kind))
               {
                  _deviceMemory[kind] = new short[Math.Max(16384, maxEnd + 1)];
               }
               else if (_deviceMemory[kind].Length <= maxEnd)
               {
                  var old = _deviceMemory[kind];
                  var next = new short[Math.Max(16384, maxEnd + 1)];
                  Array.Copy(old, next, old.Length);
                  _deviceMemory[kind] = next;
               }

               var sorted = g.OrderBy(r => r.Start).ToList();
               if (sorted.Count == 0)
               {
                  continue;
               }

               int currentStart = sorted[0].Start;
               int currentEnd = sorted[0].End;

               for (int i = 1; i < sorted.Count; i++)
               {
                  if (sorted[i].Start <= currentEnd + 1 + _mergeGapTolerance)
                  {
                     currentEnd = Math.Max(currentEnd, sorted[i].End);
                  }
                  else
                  {
                     newPlan.Add(new BatchRead { Kind = kind, Start = currentStart, Words = currentEnd - currentStart + 1 });
                     currentStart = sorted[i].Start;
                     currentEnd = sorted[i].End;
                  }
               }

               newPlan.Add(new BatchRead { Kind = kind, Start = currentStart, Words = currentEnd - currentStart + 1 });
            }
         }

         lock (_planLock)
         {
            _pollingPlan = newPlan;
         }
      }

      private bool GetBitFromAddressString(string address)
      {
         var p = LinkDeviceAddress.Parse(address, 1);
         return GetBit(p.Kind, p.Start);
      }

      private bool IsBitDevice(string kind)
      {
         string k = kind.ToUpperInvariant();
         return k == "LB" || k == "LX" || k == "LY";
      }

      private void PostEvent(Action action)
      {
         if (action == null)
         {
            return;
         }

         if (_syncContext != null)
         {
            _syncContext.Post(_ => action(), null);
         }
         else
         {
            action();
         }
      }

      #endregion

      public bool GetBit(string address)
      {
         return GetBitFromAddressString(address);
      }

      public short GetWord(string address)
      {
         var parsed = LinkDeviceAddress.Parse(address, 1);
         return GetWord(parsed.Kind, parsed.Start);
      }

      public void Dispose()
      {
         _disposed = true;
         StopPollingWorker();
         if (_resolvedPath >= 0)
         {
            _api.Close(_resolvedPath);
         }
      }

      internal int MapDeviceCode(string kind)
      {
         switch (kind.ToUpperInvariant())
         {
            case "LB": return CCLinkConstants.DEV_LB;
            case "LW": return CCLinkConstants.DEV_LW;
            case "LX": return CCLinkConstants.DEV_LX;
            case "LY": return CCLinkConstants.DEV_LY;
            default: throw new ArgumentException($"Unknown device: {kind}");
         }
      }

      #region Events

      public event Action Connected;
      public event Action Disconnected;
      public event Action<Exception> ExceptionOccurred;

      #endregion

      #region ICCLinkController

      public async Task OpenAsync(CancellationToken ct = default)
      {
         await Task.Run(() =>
         {
            _logger?.Invoke("[OpenAsync] Starting connection...");
            StopPollingWorker();

            lock (_apiLock)
            {
               int path;
               int rc = _api.Open((short)_settings.Channel, OpenMode, out path);
               if (rc != 0)
               {
                  throw MelsecException.FromCode(rc, nameof(_api.Open));
               }

               _resolvedPath = path;
               _deviceMemory.Clear();

               _status.IsConnected = true;
               _status.Channel = _settings.Channel;
               _status.LastUpdated = DateTime.UtcNow;

               UpdatePollingPlanInternal();
            }

            EnsurePollingWorkerStarted();
            PostEvent(() => Connected?.Invoke());
            _logger?.Invoke($"[OpenAsync] Connection opened (Path={_resolvedPath})");
         }, ct).ConfigureAwait(false);
      }

      public async Task CloseAsync(CancellationToken ct = default)
      {
         _logger?.Invoke("[CloseAsync] Closing connection...");
         StopPollingWorker();
         await Task.Delay(100, ct).ConfigureAwait(false);

         await Task.Run(() =>
         {
            lock (_apiLock)
            {
               if (_resolvedPath >= 0)
               {
                  _api.Close(_resolvedPath);
                  _resolvedPath = -1;
               }

               _deviceMemory.Clear();
               _status.IsConnected = false;
               _status.LastUpdated = DateTime.UtcNow;
            }

            PostEvent(() => Disconnected?.Invoke());
         }, ct).ConfigureAwait(false);
      }

      public async Task<bool> ReadBitsAsync(string address, CancellationToken ct = default)
      {
         var parsed = LinkDeviceAddress.Parse(address, 1);
         int deviceCode = MapDeviceCode(parsed.Kind);
         int alignedStart = parsed.Start / 8 * 8;
         int bitPosition = parsed.Start - alignedStart;
         int size = 1;
         short[] buffer = new short[1];

         await Task.Run(() =>
         {
            lock (_apiLock)
            {
               int rc = _api.ReceiveEx(_resolvedPath, NetworkNo, StationNo, deviceCode, alignedStart, ref size, buffer);
               if (rc != 0)
               {
                  throw MelsecException.FromCode(rc, nameof(_api.ReceiveEx));
               }
            }
         }, ct).ConfigureAwait(false);

         return ((buffer[0] >> bitPosition) & 1) != 0;
      }

      public async Task WriteBitsAsync(string address, IEnumerable<bool> values, CancellationToken ct = default)
      {
         var vals = values?.ToArray() ?? Array.Empty<bool>();
         var parsed = LinkDeviceAddress.Parse(address, vals.Length);
         int deviceCode = MapDeviceCode(parsed.Kind);
         int size = vals.Length * 2;
         short[] src = vals.Select(v => (short)(v ? 1 : 0)).ToArray();

         await Task.Run(() =>
         {
            lock (_apiLock)
            {
               int rc = _api.SendEx(_resolvedPath, NetworkNo, StationNo, deviceCode, parsed.Start, ref size, src);
               if (rc != 0)
               {
                  throw MelsecException.FromCode(rc, nameof(_api.SendEx));
               }

               // Update Cache
               UpdateCache(parsed.Kind, parsed.Start, src);
            }
         }, ct).ConfigureAwait(false);
      }

      public async Task<IReadOnlyList<short>> ReadWordsAsync(string address, int count, CancellationToken ct = default)
      {
         var parsed = LinkDeviceAddress.Parse(address, count);
         int deviceCode = MapDeviceCode(parsed.Kind);
         int size = count * 2;
         short[] buffer = new short[count];

         await Task.Run(() =>
         {
            lock (_apiLock)
            {
               int rc = _api.ReceiveEx(_resolvedPath, NetworkNo, StationNo, deviceCode, parsed.Start, ref size, buffer);
               if (rc != 0)
               {
                  throw MelsecException.FromCode(rc, nameof(_api.ReceiveEx));
               }
            }
         }, ct).ConfigureAwait(false);

         return buffer;
      }

      public async Task WriteWordsAsync(string address, IEnumerable<short> values, CancellationToken ct = default)
      {
         var src = values?.ToArray() ?? Array.Empty<short>();
         var parsed = LinkDeviceAddress.Parse(address, src.Length);
         int deviceCode = MapDeviceCode(parsed.Kind);
         int size = src.Length * 2;

         await Task.Run(() =>
         {
            lock (_apiLock)
            {
               int rc = _api.SendEx(_resolvedPath, NetworkNo, StationNo, deviceCode, parsed.Start, ref size, src);
               if (rc != 0)
               {
                  throw MelsecException.FromCode(rc, nameof(_api.SendEx));
               }

               // Update Cache
               UpdateCache(parsed.Kind, parsed.Start, src);
            }
         }, ct).ConfigureAwait(false);
      }

      public Task<ControllerStatus> GetStatusAsync(CancellationToken ct = default)
      {
         lock (_apiLock)
         {
            _status.IsConnected = _resolvedPath >= 0;
            _status.LastUpdated = DateTime.UtcNow;
            return Task.FromResult(_status);
         }
      }

      #endregion
   }
}