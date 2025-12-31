using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WindowsFormsApp1.CCLink.Interfaces;
using WindowsFormsApp1.CCLink.Models;
using WindowsFormsApp1.CCLink.Drivers;

namespace WindowsFormsApp1.CCLink.Services
{
   public sealed class MelsecHelper : IDisposable
   {
      #region Fields

      // 核心相依
      private readonly IMelsecApiAdapter _api;

      // 同步 / 工作緒
      private readonly object _apiLock = new object();

      // 設備記憶體快取 (Key: Kind, Value: short[] array representing the device memory)
      private readonly ConcurrentDictionary<string, short[]> _deviceMemory = new ConcurrentDictionary<string, short[]>(StringComparer.OrdinalIgnoreCase);

      private readonly Action<string> _logger;
      private readonly object _planLock = new object();

      // _pollLock 已不再需要，cache 與 registered 採用 thread-safe collection
      // private readonly object _pollLock = new object();
      private readonly Func<Task<bool>> _reconnectAsync;

      private readonly Func<bool> _reconnectSync;
      private readonly SynchronizationContext _syncContext;
      private readonly List<ScanRange> _userScanRanges = new List<ScanRange>();
      private CancellationTokenSource _backgroundCts;

      // 背景監控 (用於自動回應 PLC 請求)
      private BackgroundMonitor _backgroundMonitor;
      private Task _backgroundTask;

      // 狀態
      private bool _disposed;
      private CancellationTokenSource _heartbeatCts;

      // 心跳設定 / 狀態
      private bool _heartbeatEnabled;
      private int _heartbeatFailThreshold = 3;
      private TimeSpan _heartbeatInterval = TimeSpan.FromSeconds(5);
      private LinkDeviceAddress _heartbeatRequestFlagAddr;
      private LinkDeviceAddress _heartbeatResponseFlagAddr;

      // 心跳專用的 Task 和 CancellationTokenSource
      private Task _heartbeatTask;

      // 心跳觀察快取，避免在無變化時重複宣告成功
      private bool? _lastObservedRequest;
      private bool? _lastObservedResponse;
      private int _maxWordsPerRead = 900;
      private int _mergeGapTolerance = 32;
      private List<BatchRead> _pollingPlan = new List<BatchRead>();

      // 輪詢設定
      private TimeSpan _pollInterval;

      // 已解析的路徑（Start 時呼叫一次 getPath）
      private int _resolvedPath = -1;

      // 工作緒控制
      private CancellationTokenSource _workerCts;
      private Task _workerTask;

      #endregion

      #region Constructors

      public MelsecHelper(IMelsecApiAdapter api, TimeSpan? pollInterval = null, Func<Task<bool>> reconnectAsync = null, Func<bool> reconnectSync = null,
         Action<string> logger = null)
      {
         _api = api ?? throw new ArgumentNullException(nameof(api));
         _pollInterval = pollInterval ?? TimeSpan.FromMilliseconds(200);
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
      public int MaxWordsPerRead
      {
         get => _maxWordsPerRead;
         set => _maxWordsPerRead = Math.Max(1, value);
      }

      public int MergeGapTolerance
      {
         get => _mergeGapTolerance;
         set => _mergeGapTolerance = Math.Max(0, value);
      }

      public TimeSpan PollInterval
      {
         get => _pollInterval;
         set
         {
            if (value <= TimeSpan.Zero)
            {
               throw new ArgumentOutOfRangeException(nameof(value));
            }

            var oldValue = _pollInterval;
            _pollInterval = value;
            _logger?.Invoke($"輪詢間隔已從 {oldValue.TotalMilliseconds:F0}ms 變更為 {value.TotalMilliseconds:F0}ms");
         }
      }

      #endregion

      #region Private Methods

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

      #endregion

      #endregion

      public void Dispose()
      {
         if (_disposed)
         {
            return;
         }

         _disposed = true;

         // 停止心跳
         StopHeartbeat();

         // 停止輪詢工作
         try
         {
            StopPollingWorker();
         }
         catch (Exception ex)
         {
            _logger?.Invoke($"Dispose 停止輪詢工作時發生例外: {ex.Message}");
         }
      }

      #region Internal Models

      public sealed class ScanRange
      {
         #region Properties

         public string Kind { get; set; }
         public int Start { get; set; }
         public int End { get; set; }
         public int Length => End - Start + 1;

         #endregion
      }

      private sealed class BatchRead
      {
         #region Properties

         public string Kind { get; set; }
         public int Start { get; set; }
         public int Words { get; set; }

         #endregion
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
         // 將心跳所需的位元位址加入掃描區間
         lock (_planLock)
         {
            _userScanRanges.Add(new ScanRange
            {
               Kind = _heartbeatRequestFlagAddr.Kind, Start = _heartbeatRequestFlagAddr.Start,
               End = _heartbeatRequestFlagAddr.Start + _heartbeatRequestFlagAddr.Length - 1
            });
            _userScanRanges.Add(new ScanRange
            {
               Kind = _heartbeatResponseFlagAddr.Kind, Start = _heartbeatResponseFlagAddr.Start,
               End = _heartbeatResponseFlagAddr.Start + _heartbeatResponseFlagAddr.Length - 1
            });
            UpdatePollingPlan();
         }

         // 啟動獨立的心跳 Task
         _heartbeatEnabled = true;
         _heartbeatCts = new CancellationTokenSource();
         _heartbeatTask = Task.Run(() => HeartbeatLoopAsync(_heartbeatCts.Token));

         // start background monitor if path already resolved
         try
         {
            var path = _resolvedPath;
            if (path >= 0)
            {
               _backgroundMonitor = new BackgroundMonitor(new MelsecPlcDriver(_api, path));
               _backgroundCts = new CancellationTokenSource();
               _backgroundTask = Task.Run(() => _backgroundMonitor.RunLoopAsync());
            }
         }
         catch (Exception ex)
         {
            _logger?.Invoke($"背景監控啟動失敗: {ex.Message}");
         }
      }

      private void AssignResolvedPath(int p)
      {
         _resolvedPath = p;
      }

      public void StopHeartbeat()
      {
         _heartbeatEnabled = false;
         ConsecutiveHeartbeatFailures = 0;

         // 停止心跳 Task
         try
         {
            if (_heartbeatCts != null)
            {
               _heartbeatCts.Cancel();
               try
               {
                  _heartbeatTask?.Wait(500);
               }
               catch (Exception ex)
               {
                  _logger?.Invoke($"停止心跳 Task 時等待發生例外: {ex.Message}");
               }

               _heartbeatCts.Dispose();
               _heartbeatCts = null;
               _heartbeatTask = null;
            }
         }
         catch (Exception ex)
         {
            _logger?.Invoke($"停止心跳時發生例外: {ex.Message}");
         }

         // heartbeat addresses are part of scan ranges, but since we don't have a specific RemoveScanRange per address yet, 
         // we'll leave it for now or user can call SetScanRanges to clear.

         // stop background monitor if running
         try
         {
            if (_backgroundMonitor != null)
            {
               _backgroundMonitor.Stop();
               try
               {
                  _backgroundTask?.Wait(200);
               }
               catch
               {
               }

               try
               {
                  _backgroundCts?.Cancel();
               }
               catch
               {
               }

               _backgroundTask = null;
               _backgroundCts = null;
               _backgroundMonitor = null;
            }
         }
         catch (Exception ex)
         {
            _logger?.Invoke($"停止背景監控時發生例外: {ex.Message}");
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
      /// 設定要掃描的區塊範圍。這會替換先前的所有掃描設定。
      /// </summary>
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
            UpdatePollingPlan();
         }

         // 確保輪詢工作已啟動
         EnsurePollingWorkerStarted();
      }

      private void UpdatePollingPlan()
      {
         var newPlan = new List<BatchRead>();

         // 按 Kind 分組
         var groups = _userScanRanges.GroupBy(r => r.Kind, StringComparer.OrdinalIgnoreCase);

         foreach (var g in groups)
         {
            string kind = g.Key;
            // 確保該 Kind 的快取已初始化 (假設最大 65536)
            if (!_deviceMemory.ContainsKey(kind))
            {
               int size = kind.ToUpperInvariant() == "LW" ? 65536 : 4096;
               _deviceMemory.TryAdd(kind, new short[size]);
            }

            // 步驟 1 & 2: 排序、合併重疊與相近區間 (Gap Tolerance)
            var sorted = g.OrderBy(r => r.Start).ToList();
            if (sorted.Count == 0)
            {
               continue;
            }

            var merged = new List<(int Start, int End)>();
            int currentStart = sorted[0].Start;
            int currentEnd = sorted[0].End;

            for (int i = 1; i < sorted.Count; i++)
            {
               var next = sorted[i];
               // 若兩區間重疊、相鄰，或間隙在容許範圍內則合併
               if (next.Start <= currentEnd + 1 + _mergeGapTolerance)
               {
                  currentEnd = Math.Max(currentEnd, next.End);
               }
               else
               {
                  merged.Add((currentStart, currentEnd));
                  currentStart = next.Start;
                  currentEnd = next.End;
               }
            }

            merged.Add((currentStart, currentEnd));

            // 步驟 3: 依 MaxWordsPerRead 將合併後的區間切割為 Batch
            foreach (var m in merged)
            {
               int totalLen = m.End - m.Start + 1;
               int offset = 0;
               while (offset < totalLen)
               {
                  int words = Math.Min(_maxWordsPerRead, totalLen - offset);
                  newPlan.Add(new BatchRead
                  {
                     Kind = kind,
                     Start = m.Start + offset,
                     Words = words
                  });
                  offset += words;
               }
            }
         }

         _pollingPlan = newPlan;
         _logger?.Invoke($"輪詢編譯完成：總計 {newPlan.Count} 個通訊批次");
      }

      /// <summary>
      /// 註冊一個地址至中央輪詢工作 (已棄用，請改用 SetScanRanges)。
      /// </summary>
      [Obsolete("請改用 SetScanRanges 提供明確的掃描區間。")]
      public void RegisterPollingAddress(LinkDeviceAddress addr)
      {
         throw new NotSupportedException("RegisterPollingAddress 已棄用，請改用 SetScanRanges。");
      }

      /// <summary>
      /// 取消註冊一個先前註冊的輪詢地址 (已棄用)。
      /// </summary>
      [Obsolete("請改用 SetScanRanges。")]
      public void UnregisterPollingAddress(LinkDeviceAddress addr)
      {
         // No-op or throw
      }

      /// <summary>
      /// 取得最新快取值。
      /// </summary>
      public short[] GetLatest(LinkDeviceAddress addr)
      {
         if (addr == null)
         {
            return null;
         }

         if (_deviceMemory.TryGetValue(addr.Kind, out var memory))
         {
            // 讀取位元裝置與字組裝置的邏輯略有不同 (若 LinkDeviceAddress 已處理好 offset 則直接使用)
            // 這裡假設 addr.Start 與 addr.Length 是基於該裝置 Kind 的索引
            if (addr.Start + addr.Length <= memory.Length)
            {
               var copy = new short[addr.Length];
               Array.Copy(memory, addr.Start, copy, 0, addr.Length);
               return copy;
            }
         }

         return null;
      }

      #endregion

      #region Worker and operations

      /// <summary>
      /// 確保輪詢工作正在運行。輪詢是基礎功能，只要有任何註冊的地址就應該運行。
      /// </summary>
      private void EnsurePollingWorkerStarted()
      {
         if (_workerTask != null)
         {
            return;
         }

         _workerCts = new CancellationTokenSource();
         _workerTask = Task.Run(() => PollingWorkerLoopAsync(_workerCts.Token));
      }

      /// <summary>
      /// 停止輪詢工作。
      /// </summary>
      private void StopPollingWorker()
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
                  _logger?.Invoke($"停止輪詢工作緒時取消時發生例外: {ex.Message}");
               }

               try
               {
                  _workerTask?.Wait(500);
               }
               catch (Exception ex)
               {
                  _logger?.Invoke($"停止輪詢工作緒時等待時發生例外: {ex.Message}");
               }
            }
         }
         finally
         {
            _workerTask = null;
            _workerCts = null;
         }
      }

      /// <summary>
      /// 輪詢工作迴圈：只負責定期輪詢已註冊的地址。
      /// 使用固定間隔模式，補償執行時間以維持穩定的輪詢頻率。
      /// </summary>
      private async Task PollingWorkerLoopAsync(CancellationToken ct)
      {
         DateTime nextPoll = DateTime.UtcNow + _pollInterval;

         while (!ct.IsCancellationRequested && !_disposed)
         {
            try
            {
               DateTime loopStart = DateTime.UtcNow;

               // 檢查是否有註冊的地址，如果沒有則延長等待時間
               bool hasPlan;
               lock (_planLock)
               {
                  hasPlan = _pollingPlan.Count > 0;
               }

               if (!hasPlan)
               {
                  // 沒有地址時，使用較長的等待時間以節省資源
                  await Task.Delay(TimeSpan.FromSeconds(1), ct).ConfigureAwait(false);
                  nextPoll = DateTime.UtcNow + _pollInterval;
                  continue;
               }

               // 計算延遲時間
               TimeSpan delay = nextPoll - loopStart;

               if (delay > TimeSpan.Zero)
               {
                  await Task.Delay(delay, ct).ConfigureAwait(false);
               }
               else if (delay < -TimeSpan.FromMilliseconds(100))
               {
                  // 如果延遲超過 100ms，記錄警告（表示執行時間過長）
                  _logger?.Invoke($"輪詢執行時間過長，延遲 {Math.Abs(delay.TotalMilliseconds):F0}ms");
               }

               // 執行輪詢
               DateTime pollStart = DateTime.UtcNow;
               try
               {
                  PollAddresses();
               }
               catch (Exception ex)
               {
                  TryRaiseException(ex);
                  _logger?.Invoke($"輪詢時發生例外: {ex.Message}");
               }

               // 計算實際執行時間
               TimeSpan pollDuration = DateTime.UtcNow - pollStart;

               // 使用固定間隔模式：基於預期時間而非實際時間
               // 這樣可以補償執行時間，維持穩定的輪詢頻率
               nextPoll += _pollInterval;

               // 如果執行時間超過間隔，立即進行下一次輪詢
               if (nextPoll < DateTime.UtcNow)
               {
                  nextPoll = DateTime.UtcNow + _pollInterval;
                  _logger?.Invoke($"輪詢執行時間 ({pollDuration.TotalMilliseconds:F0}ms) 超過輪詢間隔 ({_pollInterval.TotalMilliseconds:F0}ms)");
               }
            }
            catch (OperationCanceledException)
            {
               break;
            }
            catch (Exception ex)
            {
               TryRaiseException(ex);
               _logger?.Invoke($"輪詢工作迴圈例外: {ex.Message}");

               // 異常後使用輪詢間隔作為延遲，而非固定 500ms
               try
               {
                  await Task.Delay(_pollInterval, ct).ConfigureAwait(false);
               }
               catch (Exception ex2)
               {
                  _logger?.Invoke($"在輪詢工作迴圈延遲時發生例外: {ex2.Message}");
               }

               // 重置下次輪詢時間
               nextPoll = DateTime.UtcNow + _pollInterval;
            }
         }
      }

      /// <summary>
      /// 心跳迴圈：獨立處理心跳邏輯，依賴輪詢工作的 cache。
      /// </summary>
      private async Task HeartbeatLoopAsync(CancellationToken ct)
      {
         DateTime nextHeartbeat = DateTime.UtcNow + _heartbeatInterval;

         while (!ct.IsCancellationRequested && !_disposed && _heartbeatEnabled)
         {
            try
            {
               DateTime now = DateTime.UtcNow;
               TimeSpan delay = nextHeartbeat - now;

               if (delay > TimeSpan.Zero)
               {
                  await Task.Delay(delay, ct).ConfigureAwait(false);
                  continue;
               }

               // 執行心跳
               await RunHeartbeatAsync(ct).ConfigureAwait(false);
               nextHeartbeat = DateTime.UtcNow + _heartbeatInterval;
            }
            catch (OperationCanceledException)
            {
               break;
            }
            catch (Exception ex)
            {
               TryRaiseException(ex);
               _logger?.Invoke($"心跳迴圈例外: {ex.Message}");
               try
               {
                  await Task.Delay(500, ct).ConfigureAwait(false);
               }
               catch (Exception ex2)
               {
                  _logger?.Invoke($"在心跳迴圈延遲時發生例外: {ex2.Message}");
               }
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

         if (batches.Count == 0)
         {
            return;
         }

         lock (_apiLock)
         {
            foreach (var batch in batches)
            {
               try
               {
                  int path = _resolvedPath;
                  int devCode = MapDeviceCode(batch.Kind);
                  int sizeInBytes = batch.Words * 2;
                  var dest = new short[batch.Words];

                  int rc = _api.ReceiveEx(path, 0, 0, devCode, batch.Start, ref sizeInBytes, dest);

                  if (rc == 0)
                  {
                     if (_deviceMemory.TryGetValue(batch.Kind, out var memory))
                     {
                        // 更新全域快取
                        Array.Copy(dest, 0, memory, batch.Start, batch.Words);

                        // 觸發事件 (可選：這裡可以比對舊值決定是否觸發，但現在改為區間後，
                        // ValuesUpdated 事件可能需要調整為針對 ScanRange 觸發，或保持相容)
                        // 基於相容性，這裡可以廣播該批次的更新，或由使用者自行調用 GetLatest 檢查
                        // ValuesUpdated?.Invoke(new LinkDeviceAddress(batch.Kind, batch.Start, batch.Words), dest);
                     }
                  }
                  else
                  {
                     _logger?.Invoke($"讀取失敗: {batch.Kind}{batch.Start} (Words:{batch.Words}), RC={rc}");
                  }
               }
               catch (Exception ex)
               {
                  TryRaiseException(ex);
                  _logger?.Invoke($"輪詢讀取計畫批次發生例外: {ex.Message}");
               }
            }
         }
      }

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
                        int r = _api.DevSetEx(_resolvedPath, 0, 0, devCode, _heartbeatResponseFlagAddr.Start);
                        _logger?.Invoke($"心跳：執行 DevSetEx {_heartbeatResponseFlagAddr.Kind}{_heartbeatResponseFlagAddr.Start} => {r}");
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
                        int r = _api.DevRstEx(_resolvedPath, 0, 0, devCode, _heartbeatResponseFlagAddr.Start);
                        _logger?.Invoke($"心跳：執行 DevRstEx {_heartbeatResponseFlagAddr.Kind}{_heartbeatResponseFlagAddr.Start} => {r}");
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
                  // 心跳失敗時觸發斷線事件，但不停止輪詢工作
                  PostEvent(() => Disconnected?.Invoke());
                  _logger?.Invoke("偵測到心跳中斷；啟動重新連線循環。");

                  // 在背景執行重新連線循環，成功後重新啟動心跳
                  _ = Task.Run(async () =>
                  {
                     await AttemptReconnectLoop().ConfigureAwait(false);
                     // 如果重新連線成功且未被釋放，重新啟動心跳
                     if (!_disposed && _heartbeatEnabled)
                     {
                        ConsecutiveHeartbeatFailures = 0;
                        // 心跳迴圈會自動繼續（如果還在運行）
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