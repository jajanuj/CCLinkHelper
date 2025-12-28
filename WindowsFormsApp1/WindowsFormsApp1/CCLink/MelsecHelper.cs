using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApp1.CCLink
{
   public sealed class MelsecHelper : IDisposable
   {
      #region Fields

      // 核心相依
      private readonly IMelsecApiAdapter _api;

      // 同步 / 工作緒
      private readonly object _apiLock = new object();
      private readonly Dictionary<string, short[]> _cache = new Dictionary<string, short[]>();

      private readonly Action<string> _logger;
      private readonly object _pollLock = new object();
      private readonly Func<Task<bool>> _reconnectAsync;
      private readonly Func<bool> _reconnectSync;
      private readonly Dictionary<string, LinkDeviceAddress> _registered = new Dictionary<string, LinkDeviceAddress>();
      private readonly SynchronizationContext _syncContext;

      // 狀態
      private bool _disposed;

      // 心跳設定 / 狀態
      private bool _heartbeatEnabled;
      private int _heartbeatFailThreshold = 3;
      private TimeSpan _heartbeatInterval = TimeSpan.FromSeconds(5);
      private LinkDeviceAddress _heartbeatRequestFlagAddr;
      private LinkDeviceAddress _heartbeatResponseFlagAddr;

      // 輪詢註冊 / 快取
      private TimeSpan _pollInterval = TimeSpan.FromMilliseconds(200);

      // 已解析的路徑（Start 時呼叫一次 getPath）
      private int _resolvedPath = -1;

      // 時間同步功能已移除（目前未使用）
      private CancellationTokenSource _workerCts;
      private Task _workerTask;

      #endregion

      #region Constructors

      public MelsecHelper(IMelsecApiAdapter api, Func<Task<bool>> reconnectAsync = null, Func<bool> reconnectSync = null, Action<string> logger = null)
      {
         _api = api ?? throw new ArgumentNullException(nameof(api));
         _reconnectAsync = reconnectAsync;
         _reconnectSync = reconnectSync;
         _logger = logger;
         _syncContext = SynchronizationContext.Current;
      }

      #endregion

      #region Properties

      public TimeSpan HeartbeatInterval
      {
         get => _heartbeatInterval;
         private set => _heartbeatInterval = value;
      }

      public int HeartbeatFailThreshold
      {
         get => _heartbeatFailThreshold;
         private set => _heartbeatFailThreshold = value;
      }

      public int ReconnectBackoffBaseMs { get; set; } = 2000;
      public int ReconnectBackoffMultiplier { get; set; } = 2;
      public int ReconnectMaxAttempts { get; set; } = 8;
      public int ConsecutiveHeartbeatFailures { get; private set; }
      public bool IsHeartbeatRunning => _heartbeatEnabled && !_disposed;

      /// <summary>
      /// 全域的輪詢間隔，由中央輪詢工作使用。
      /// </summary>
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

      public void Dispose()
      {
         if (_disposed)
         {
            return;
         }

         _disposed = true;
         try
         {
            _workerCts?.Cancel();
            try
            {
               _workerTask?.Wait(500);
            }
            catch (Exception ex)
            {
               _logger?.Invoke($"Dispose 等待工作緒時發生例外: {ex.Message}");
            }
         }
         catch (Exception ex)
         {
            _logger?.Invoke($"Dispose 取消工作緒時發生例外: {ex.Message}");
         }

         _heartbeatEnabled = false;
      }

      #region Utilities

      private static int MapDeviceCode(string kind)
      {
         if (string.IsNullOrWhiteSpace(kind))
         {
            throw new ArgumentException(nameof(kind));
         }

         switch (kind.ToUpperInvariant())
         {
            case "LB": return CCLinkConstants.DEV_LB;
            case "LW": return CCLinkConstants.DEV_LW;
            case "LX": return CCLinkConstants.DEV_LX;
            case "LY": return CCLinkConstants.DEV_LY;
            default: throw new ArgumentException($"Unsupported device kind: {kind}");
         }
      }

      private static string KeyFor(LinkDeviceAddress a) => $"{a.Kind}:{a.Start}:{a.Length}";

      private static bool ArraysEqual(short[] a, short[] b)
      {
         if (ReferenceEquals(a, b))
         {
            return true;
         }

         if (a == null || b == null)
         {
            return false;
         }

         if (a.Length != b.Length)
         {
            return false;
         }

         for (int i = 0; i < a.Length; i++)
         {
            if (a[i] != b[i])
            {
               return false;
            }
         }

         return true;
      }

      #endregion

      #region Events

      public event Action Disconnected;
      public event Action<Exception> ExceptionOccurred;
      public event Action<int> HeartbeatFailed; // 連續失敗計數
      public event Action HeartbeatSucceeded;
      public event Action<int> ReconnectAttemptFailed;
      public event Action Reconnected;

      /// <summary>
      /// 當中央輪詢工作讀到已註冊的地址資料時觸發，會傳回該地址及其資料陣列（short[]）。
      /// </summary>
      public event Action<LinkDeviceAddress, short[]> ValuesUpdated;

      #endregion

      #region Public API (Start/Stop)

      public void StartHeartbeat(TimeSpan interval, LinkDeviceAddress requestFlagAddr, LinkDeviceAddress responseFlagAddr, Func<int> getPath,
         int failThreshold = 3)
      {
         if (requestFlagAddr == null)
         {
            throw new ArgumentNullException(nameof(requestFlagAddr));
         }

         if (responseFlagAddr == null)
         {
            throw new ArgumentNullException(nameof(responseFlagAddr));
         }

         if (getPath == null)
         {
            throw new ArgumentNullException(nameof(getPath));
         }

         HeartbeatInterval = interval;
         HeartbeatFailThreshold = Math.Max(1, failThreshold);

         // resolve path once per your requirement
         AssignResolvedPath(getPath());

         _heartbeatRequestFlagAddr = requestFlagAddr;
         _heartbeatResponseFlagAddr = responseFlagAddr;
         _heartbeatInterval = interval;
         _heartbeatFailThreshold = HeartbeatFailThreshold;

         // register addresses to be polled by central polling worker
         RegisterPollingAddress(_heartbeatRequestFlagAddr);
         RegisterPollingAddress(_heartbeatResponseFlagAddr);

         _heartbeatEnabled = true;
         EnsureWorkerStarted();
      }

      private void AssignResolvedPath(int p)
      {
         _resolvedPath = p;
      }

      public void StopHeartbeat()
      {
         _heartbeatEnabled = false;
         ConsecutiveHeartbeatFailures = 0;

         // unregister heartbeat addresses from polling (optional)
         try
         {
            if (_heartbeatRequestFlagAddr != null)
            {
               UnregisterPollingAddress(_heartbeatRequestFlagAddr);
            }

            if (_heartbeatResponseFlagAddr != null)
            {
               UnregisterPollingAddress(_heartbeatResponseFlagAddr);
            }
         }
         catch (Exception ex)
         {
            _logger?.Invoke($"停止心跳時取消註冊發生例外: {ex.Message}");
         }
      }

      // TimeSync removed for now - keep API but mark unsupported
      public void StartTimeSync(TimeSpan interval, LinkDeviceAddress requestFlagAddr, LinkDeviceAddress requestDataAddr, LinkDeviceAddress responseFlagAddr,
         Func<int> getPath)
      {
         throw new NotSupportedException("TimeSync is disabled in this build.");
      }

      public void StopTimeSync()
      {
         // noop
      }

      public bool ForceTimeSync(DateTime dt, LinkDeviceAddress a, LinkDeviceAddress b, LinkDeviceAddress c, Func<int> gp) => false;

      /// <summary>
      /// 註冊一個地址至中央輪詢工作，該地址將被定期讀取並快取其最新值。
      /// </summary>
      public void RegisterPollingAddress(LinkDeviceAddress addr)
      {
         if (addr == null)
         {
            throw new ArgumentNullException(nameof(addr));
         }

         var k = KeyFor(addr);
         lock (_pollLock)
         {
            if (!_registered.ContainsKey(k))
            {
               _registered[k] = addr;
            }
         }
      }

      /// <summary>
      /// 取消註冊一個先前註冊的輪詢地址。
      /// </summary>
      public void UnregisterPollingAddress(LinkDeviceAddress addr)
      {
         if (addr == null)
         {
            return;
         }

         var k = KeyFor(addr);
         lock (_pollLock)
         {
            _registered.Remove(k);
            _cache.Remove(k);
         }
      }

      /// <summary>
      /// 取得已註冊地址的最新快取值。回傳陣列為複本以避免外部改變內部快取；若無資料則回傳 null。
      /// </summary>
      public short[] GetLatest(LinkDeviceAddress addr)
      {
         if (addr == null)
         {
            return null;
         }

         var k = KeyFor(addr);
         lock (_pollLock)
         {
            if (_cache.TryGetValue(k, out var v))
            {
               var copy = new short[v.Length];
               Array.Copy(v, copy, v.Length);
               return copy;
            }
         }

         return null;
      }

      #endregion

      #region Worker and operations

      private void EnsureWorkerStarted()
      {
         if (_workerTask != null)
         {
            return;
         }

         _workerCts = new CancellationTokenSource();
         _workerTask = Task.Run(() => WorkerLoopAsync(_workerCts.Token));
      }

      private void StopWorkerInternal()
      {
         try
         {
            var c = Interlocked.Exchange(ref _workerCts, null);
            if (c != null)
            {
               try
               {
                  c.Cancel();
               }
               catch (Exception ex)
               {
                  _logger?.Invoke($"停止工作緒時取消時發生例外: {ex.Message}");
               }

               try
               {
                  _workerTask?.Wait(500);
               }
               catch (Exception ex)
               {
                  _logger?.Invoke($"停止工作緒時等待時發生例外: {ex.Message}");
               }
            }
         }
         finally
         {
            _workerTask = null;
            _workerCts = null;
         }
      }

      private async Task WorkerLoopAsync(CancellationToken ct)
      {
         // 排程下一次心跳執行時間
         DateTime nextHeartbeat = DateTime.UtcNow + _heartbeatInterval;
         // 排程下一次輪詢執行時間
         DateTime nextPoll = DateTime.UtcNow + _pollInterval;

         while (!ct.IsCancellationRequested && !_disposed)
         {
            try
            {
               DateTime now = DateTime.UtcNow;
               DateTime next = DateTime.MaxValue;
               if (_heartbeatEnabled)
               {
                  next = Min(next, nextHeartbeat);
               }

               // 輪詢也列入考量
               next = Min(next, nextPoll);

               TimeSpan delay = next == DateTime.MaxValue ? TimeSpan.FromMilliseconds(200) : next - now;
               if (delay > TimeSpan.Zero)
               {
                  await Task.Delay(delay, ct).ConfigureAwait(false);
                  continue;
               }

               // 到時間執行輪詢
               if (DateTime.UtcNow >= nextPoll)
               {
                  try
                  {
                     PollAddresses();
                  }
                  catch (Exception ex)
                  {
                     TryRaiseException(ex);
                     _logger?.Invoke($"輪詢時發生例外: {ex.Message}");
                  }

                  nextPoll = DateTime.UtcNow + _pollInterval;
               }

               // 優先處理心跳
               if (_heartbeatEnabled && DateTime.UtcNow >= nextHeartbeat)
               {
                  await RunHeartbeatAsync(ct).ConfigureAwait(false);
                  nextHeartbeat = DateTime.UtcNow + _heartbeatInterval;
               }
            }
            catch (OperationCanceledException)
            {
               break;
            }
            catch (Exception ex)
            {
               TryRaiseException(ex);
               _logger?.Invoke($"工作迴圈例外: {ex.Message}");
               try
               {
                  await Task.Delay(500, ct).ConfigureAwait(false);
               }
               catch (Exception ex2)
               {
                  _logger?.Invoke($"在工作迴圈延遲時發生例外: {ex2.Message}");
               }
            }
         }
      }

      private static DateTime Min(DateTime a, DateTime b) => a < b ? a : b;

      private void PollAddresses()
      {
         // 複製註冊清單以在鎖外操作
         List<LinkDeviceAddress> regs;
         lock (_pollLock)
         {
            regs = _registered.Values.ToList();
         }

         if (regs.Count == 0)
         {
            return;
         }

         // 依 Kind 分組並合併連續區段
         var groups = regs.GroupBy(r => r.Kind, StringComparer.OrdinalIgnoreCase);

         lock (_apiLock)
         {
            foreach (var g in groups)
            {
               var list = g.OrderBy(r => r.Start).ToList();
               int idx = 0;
               while (idx < list.Count)
               {
                  var current = list[idx];
                  int mergedStart = current.Start;
                  int mergedEnd = current.Start + current.Length - 1;
                  int j = idx + 1;
                  while (j < list.Count)
                  {
                     var next = list[j];
                     // 若重疊或相鄰則合併
                     if (next.Start <= mergedEnd + 1)
                     {
                        mergedEnd = Math.Max(mergedEnd, next.Start + next.Length - 1);
                        j++;
                     }
                     else
                     {
                        break;
                     }
                  }

                  int mergedLength = mergedEnd - mergedStart + 1;
                  var dest = new short[mergedLength];
                  try
                  {
                     int path = _resolvedPath;
                     int devCode = MapDeviceCode(g.Key);
                     short rc = _api.mdDevRead(path, devCode, mergedStart, mergedLength, dest);

                     // 計算此合併區間中受影響的已註冊地址
                     var affected = list.Where(r => r.Start <= mergedEnd && r.Start + r.Length - 1 >= mergedStart).ToList();

                     // 檢查是否有任何片段與快取不同
                     bool anyChanged = false;
                     foreach (var a in affected)
                     {
                        int off = a.Start - mergedStart;
                        var slice = new short[a.Length];
                        Array.Copy(dest, off, slice, 0, a.Length);
                        var k = KeyFor(a);
                        if (!_cache.TryGetValue(k, out var prev) || !ArraysEqual(prev, slice))
                        {
                           anyChanged = true;
                           break;
                        }
                     }

                     if (anyChanged)
                     {
                        // 只有當讀到的片段與快取不同時才記錄與更新快取
                        _logger?.Invoke($"輪詢: mdDevRead {g.Key}{mergedStart}..{mergedEnd} => {rc}");

                        // 分配片段到快取並對變更的片段觸發事件
                        lock (_pollLock)
                        {
                           foreach (var a in affected)
                           {
                              int off = a.Start - mergedStart;
                              var slice = new short[a.Length];
                              Array.Copy(dest, off, slice, 0, a.Length);
                              var k = KeyFor(a);

                              if (_cache.TryGetValue(k, out var prev) && ArraysEqual(prev, slice))
                              {
                                 // 未變更，跳過
                              }
                              else
                              {
                                 var stored = (short[])slice.Clone();
                                 _cache[k] = stored;
                                 try
                                 {
                                    ValuesUpdated?.Invoke(a, (short[])stored.Clone());
                                 }
                                 catch (Exception ex)
                                 {
                                    _logger?.Invoke($"ValuesUpdated 事件處理程序發生例外: {ex.Message}");
                                 }
                              }
                           }
                        }
                     }
                     else
                     {
                        // 欄位未變更，則不記錄日誌
                     }
                  }
                  catch (Exception ex)
                  {
                     TryRaiseException(ex);
                     _logger?.Invoke($"輪詢讀取發生例外: {ex.Message}");
                  }

                  idx = j;
               }
            }
         }
      }

      // 心跳觀察快取，避免在無變化時重複宣告成功
      private bool? _lastObservedRequest;

      private bool? _lastObservedResponse;

      // 待確認的寫入期待：當我們發出 mdDevSet/mdDevRst 時設為 true，等待下一次輪詢確認
      private bool _pendingSet;
      private bool _pendingRst;

      private Task RunHeartbeatAsync(CancellationToken ct)
      {
         return Task.Run(() =>
         {
            if (_disposed || !_heartbeatEnabled)
            {
               return;
            }

            bool ok = false;
            try
            {
               // read from cache
               short[] reqVals = GetLatest(_heartbeatRequestFlagAddr);
               short[] respVals = GetLatest(_heartbeatResponseFlagAddr);

               int reqVal = reqVals != null && reqVals.Length >= 1 ? reqVals[0] : (short)0;
               int respVal = respVals != null && respVals.Length >= 1 ? respVals[0] : (short)0;

               bool requestOn = reqVal != 0;
               bool responseOn = respVal != 0;

               // 如果之前有發出寫入命令，先檢查是否已被輪詢確認
               if (_pendingSet && responseOn)
               {
                  ok = true;
                  _pendingSet = false;
                  _lastObservedRequest = requestOn;
                  _lastObservedResponse = responseOn;
               }
               else if (_pendingRst && !responseOn)
               {
                  ok = true;
                  _pendingRst = false;
                  _lastObservedRequest = requestOn;
                  _lastObservedResponse = responseOn;
               }

               // 中性狀態（request 跟 response 都為 0）: 若沒有待確認的寫入，視為無須處理
               if (!requestOn && !responseOn)
               {
                  // 若沒有待確認寫入，更新最後觀察狀態並離開
                  if (!_pendingSet && !_pendingRst)
                  {
                     _lastObservedRequest = requestOn;
                     _lastObservedResponse = responseOn;
                     return;
                  }
               }

               // determine if the observed pair changed since last run
               bool changed = !_lastObservedRequest.HasValue || !_lastObservedResponse.HasValue || _lastObservedRequest.Value != requestOn ||
                              _lastObservedResponse.Value != responseOn;

               // capture previous request/response for transition detection
               bool prevRequest = _lastObservedRequest.HasValue ? _lastObservedRequest.Value : requestOn;
               bool prevResponse = _lastObservedResponse.HasValue ? _lastObservedResponse.Value : responseOn;

               // If PLC requested (requestOn) and we haven't set response, set it.
               // If PLC cleared request and response still set, reset it.
               if (requestOn && !responseOn)
               {
                  try
                  {
                     lock (_apiLock)
                     {
                        int devCode = MapDeviceCode(_heartbeatResponseFlagAddr.Kind);
                        short r = _api.mdDevSet(_resolvedPath, 0, devCode, _heartbeatResponseFlagAddr.Start);
                        _logger?.Invoke($"心跳：執行 mdDevSet {_heartbeatResponseFlagAddr.Kind}{_heartbeatResponseFlagAddr.Start} => {r}");
                        if (r == 0)
                        {
                           // mark expectation and wait for poll to confirm
                           _pendingSet = true;
                        }
                     }
                  }
                  catch (Exception ex)
                  {
                     TryRaiseException(ex);
                     _logger?.Invoke($"心跳設定 response 時發生例外: {ex.Message}");
                     ok = false;
                  }
               }
               else if (!requestOn && responseOn && prevRequest && !requestOn)
               {
                  try
                  {
                     lock (_apiLock)
                     {
                        int devCode = MapDeviceCode(_heartbeatResponseFlagAddr.Kind);
                        short r = _api.mdDevRst(_resolvedPath, 0, devCode, _heartbeatResponseFlagAddr.Start);
                        _logger?.Invoke($"心跳：執行 mdDevRst {_heartbeatResponseFlagAddr.Kind}{_heartbeatResponseFlagAddr.Start} => {r}");
                        if (r == 0)
                        {
                           // mark expectation and wait for poll to confirm
                           _pendingRst = true;
                        }
                     }
                  }
                  catch (Exception ex)
                  {
                     TryRaiseException(ex);
                     _logger?.Invoke($"心跳清除 response 時發生例外: {ex.Message}");
                     ok = false;
                  }
               }
               else
               {
                  // both on case: only report success if state changed (someone requested just now)
                  if (requestOn && responseOn)
                  {
                     if (changed)
                     {
                        ok = true;
                        _lastObservedRequest = requestOn;
                        _lastObservedResponse = responseOn;
                     }
                     else
                     {
                        // unchanged both-on: no-op
                        return;
                     }
                  }
               }
            }
            catch (Exception ex)
            {
               TryRaiseException(ex);
               _logger?.Invoke($"心跳處理例外: {ex.Message}");
               ok = false;
            }

            if (ok)
            {
               ConsecutiveHeartbeatFailures = 0;
               PostEvent(() => HeartbeatSucceeded?.Invoke());
            }
            else
            {
               ConsecutiveHeartbeatFailures++;
               PostEvent(() => HeartbeatFailed?.Invoke(ConsecutiveHeartbeatFailures));
               _logger?.Invoke($"心跳失敗（連續 {ConsecutiveHeartbeatFailures} 次）");
               if (ConsecutiveHeartbeatFailures >= _heartbeatFailThreshold)
               {
                  // stop worker and attempt reconnect loop
                  PostEvent(() => Disconnected?.Invoke());
                  _logger?.Invoke("偵測到心跳中斷；停止工作並啟動重新連線循環。");
                  StopWorkerInternal();
                  // run reconnect loop on background thread and when succeeded restart worker/heartbeat
                  _ = Task.Run(async () =>
                  {
                     await AttemptReconnectLoop().ConfigureAwait(false);
                     // if not disposed, restart heartbeat and worker
                     if (!_disposed)
                     {
                        ConsecutiveHeartbeatFailures = 0;
                        _heartbeatEnabled = true;
                        EnsureWorkerStarted();
                     }
                  });
               }
            }
         }, ct);
      }

      private async Task AttemptReconnectLoop()
      {
         int attempt = 0;
         while (ReconnectMaxAttempts < 0 || attempt < ReconnectMaxAttempts)
         {
            attempt++;
            bool ok = false;
            try
            {
               if (_reconnectAsync != null)
               {
                  ok = await _reconnectAsync().ConfigureAwait(false);
               }
               else if (_reconnectSync != null)
               {
                  ok = _reconnectSync();
               }
               else
               {
                  _logger?.Invoke("未提供重新連線委派。");
                  break;
               }
            }
            catch (Exception ex)
            {
               TryRaiseException(ex);
               _logger?.Invoke($"重新連線嘗試時發生例外: {ex.Message}");
               ok = false;
            }

            if (ok)
            {
               _logger?.Invoke($"重新連線在 {attempt} 次嘗試後成功");
               PostEvent(() => Reconnected?.Invoke());
               return;
            }
            else
            {
               PostEvent(() => ReconnectAttemptFailed?.Invoke(attempt));
               _logger?.Invoke($"重新連線嘗試 {attempt} 失敗。");
            }

            try
            {
               int wait = (int)(ReconnectBackoffBaseMs * Math.Pow(ReconnectBackoffMultiplier, attempt - 1));
               await Task.Delay(wait).ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
               break;
            }
         }

         _logger?.Invoke("重新連線循環結束但未成功。");
      }

      private void PostEvent(Action action)
      {
         if (action == null)
         {
            return;
         }

         if (_syncContext != null)
         {
            try
            {
               _syncContext.Post(_ => action(), null);
            }
            catch (Exception ex)
            {
               _logger?.Invoke($"在 PostEvent 使用 SynchronizationContext.Post 時發生例外: {ex.Message}");
               try
               {
                  action();
               }
               catch (Exception ex2)
               {
                  _logger?.Invoke($"在 PostEvent 中備援執行 action 時發生例外: {ex2.Message}");
               }
            }
         }
         else
         {
            try
            {
               action();
            }
            catch (Exception ex)
            {
               _logger?.Invoke($"在 PostEvent 直接執行 action 時發生例外: {ex.Message}");
            }
         }
      }

      private void TryRaiseException(Exception ex)
      {
         try
         {
            ExceptionOccurred?.Invoke(ex);
         }
         catch (Exception ex2)
         {
            _logger?.Invoke($"ExceptionOccurred 事件處理程序引發例外: {ex2.Message}");
         }
      }

      #endregion
   }
}