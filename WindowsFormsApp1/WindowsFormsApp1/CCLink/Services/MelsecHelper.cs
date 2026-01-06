using System;
using System.Collections.Concurrent;
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
      private readonly ControllerSettings _settings;
      private readonly ControllerStatus _status = new ControllerStatus();
      private readonly SynchronizationContext _syncContext;
      private readonly List<ScanRange> _userScanRanges = new List<ScanRange>();
      private CancellationTokenSource _backgroundCts;

      // 背景監控 (用於自動回應 PLC 請求)
      private BackgroundMonitor _backgroundMonitor;
      private Task _backgroundTask;

      // 狀態
      private bool _disposed;
      private DateTime? _handshakeWatchdog;
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
      private int _mergeGapTolerance = 32;
      private bool _pendingRst;

      // 待確認的寫入期待：當我們發出 mdDevSet/mdDevRst 時設為 true，等待下一次輪詢確認
      private bool _pendingSet;
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

      public MelsecHelper(IMelsecApiAdapter api, ControllerSettings settings, TimeSpan? pollInterval = null,
         Func<Task<bool>> reconnectAsync = null, Func<bool> reconnectSync = null, Action<string> logger = null)
      {
         _api = api ?? throw new ArgumentNullException(nameof(api));
         _settings = settings ?? throw new ArgumentNullException(nameof(settings));
         _pollInterval = pollInterval ?? TimeSpan.FromMilliseconds(200);
         _reconnectAsync = reconnectAsync;
         _reconnectSync = reconnectSync;
         _logger = logger;
         _syncContext = SynchronizationContext.Current;

         // 套用 Settings 設定
         _heartbeatInterval = TimeSpan.FromMilliseconds(_settings.HeartbeatIntervalMs);
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
         while (!ct.IsCancellationRequested && !_disposed && _heartbeatEnabled)
         {
            try
            {
               // 執行心跳
               await RunHeartbeatAsync(ct).ConfigureAwait(false);
               await Task.Delay(_heartbeatInterval, ct).ConfigureAwait(false);
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

                  _status.IsConnected = rc == 0;
                  _status.LastErrorCode = rc;
                  _status.LastUpdated = DateTime.UtcNow;

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

      private Task RunHeartbeatAsync(CancellationToken ct)
      {
         return Task.Run(() =>
         {
            if (_disposed || !_heartbeatEnabled)
            {
               return;
            }

            bool ok = false;
            bool handshakeCompleted = false;
            try
            {
               bool requestOn = false;
               bool responseOn = false;

               // 為了即時性，心跳狀態直接從 API 讀取，而不完全依賴可能延遲 200ms+ 的輪詢快取
               lock (_apiLock)
               {
                  int size = 2;
                  var buf = new short[1];

                  // 讀取 Request Flag
                  int rc1 = _api.ReceiveEx(_resolvedPath, 0, 0, MapDeviceCode(_heartbeatRequestFlagAddr.Kind), _heartbeatRequestFlagAddr.Start, ref size, buf);
                  if (rc1 == 0)
                  {
                     requestOn = buf[0] != 0;
                  }

                  // 讀取 Response Flag
                  size = 2;
                  int rc2 = _api.ReceiveEx(_resolvedPath, 0, 0, MapDeviceCode(_heartbeatResponseFlagAddr.Kind), _heartbeatResponseFlagAddr.Start, ref size,
                     buf);
                  if (rc2 == 0)
                  {
                     responseOn = buf[0] != 0;
                  }

                  // 同時同步更新一下快取，維持一致性
                  if (rc1 == 0)
                  {
                     UpdateCache(_heartbeatRequestFlagAddr.Kind, _heartbeatRequestFlagAddr.Start, (short)(requestOn ? 1 : 0));
                  }

                  if (rc2 == 0)
                  {
                     UpdateCache(_heartbeatResponseFlagAddr.Kind, _heartbeatResponseFlagAddr.Start, (short)(responseOn ? 1 : 0));
                  }
               }

               // 使用 bit mask: request bit << 1 | response bit
               int state = (requestOn ? 2 : 0) | (responseOn ? 1 : 0);

               // 追蹤上一個狀態
               int lastState = (_lastObservedRequest ?? false ? 2 : 0) | (_lastObservedResponse ?? false ? 1 : 0);

               bool stateChanged = !(_lastObservedRequest.HasValue && _lastObservedResponse.HasValue) || state != lastState;

               // 心跳進展邏輯：只要狀態有變化，或是處於 (0,0) 閒置狀態，就更新 Watchdog
               if (stateChanged || state == 0)
               {
                  _handshakeWatchdog = DateTime.UtcNow;
                  ok = true;

                  if (stateChanged)
                  {
                     _logger?.Invoke(
                        $"心跳：狀態 ({(requestOn ? 1 : 0)},{(responseOn ? 1 : 0)}) 上次 ({(_lastObservedRequest ?? false ? 1 : 0)},{(_lastObservedResponse ?? false ? 1 : 0)}) [狀態變更]");
                  }
               }
               else
               {
                  // 狀態未變動且非閒置，判斷是否超時
                  var elapsed = DateTime.UtcNow - (_handshakeWatchdog ?? DateTime.UtcNow);
                  var timeout = TimeSpan.FromMilliseconds(_heartbeatInterval.TotalMilliseconds * _heartbeatFailThreshold);

                  if (elapsed < timeout)
                  {
                     ok = true; // 尚未超時
                  }
                  else
                  {
                     _logger?.Invoke($"心跳：狀態 ({state}) 長時間未跳轉，已超過 {timeout.TotalMilliseconds:F0}ms");
                     ok = false;
                  }
               }

               switch (state)
               {
                  case 0: // (0,0) - 待機狀態或心跳完成
                     if (lastState == 1)
                     {
                        // 從 (0,1) 轉換到 (0,0) - 心跳循環完成！
                        handshakeCompleted = true;
                        if (_pendingRst)
                        {
                           _pendingRst = false;
                           _logger?.Invoke("心跳：✅ 完成一次心跳循環 (0,1) -> (0,0)");
                        }
                        else
                        {
                           _logger?.Invoke("心跳：✅ 心跳循環由監測確認完成 (0,0)");
                        }
                     }

                     _lastObservedRequest = false;
                     _lastObservedResponse = false;
                     break;

                  case 2: // (1,0) - PLC 發起請求
                     if (lastState == 0 || !_lastObservedRequest.HasValue)
                     {
                        _logger?.Invoke("心跳：偵測到 PLC 請求 (0,0) -> (1,0)");

                        try
                        {
                           lock (_apiLock)
                           {
                              int devCode = MapDeviceCode(_heartbeatResponseFlagAddr.Kind);
                              int r = _api.DevSetEx(_resolvedPath, 0, 0, devCode, _heartbeatResponseFlagAddr.Start);
                              _logger?.Invoke($"心跳：執行 DevSetEx {_heartbeatResponseFlagAddr.Kind} 0x{_heartbeatResponseFlagAddr.Start:X} => 1");

                              if (r == 0)
                              {
                                 _pendingSet = true;

                                 // 嘗試同步確認
                                 int sizeConfirm = 2;
                                 var confirmBuf = new short[1];
                                 int rc = _api.ReceiveEx(_resolvedPath, 0, 0, devCode, _heartbeatResponseFlagAddr.Start, ref sizeConfirm, confirmBuf);
                                 if (rc == 0 && confirmBuf[0] != 0)
                                 {
                                    UpdateCache(_heartbeatResponseFlagAddr.Kind, _heartbeatResponseFlagAddr.Start, confirmBuf[0]);
                                    _pendingSet = false;
                                    state = 3; // 直接進入 (1,1)
                                    _logger?.Invoke("心跳：response set 立即確認成功 (1,0) -> (1,1)");
                                 }
                              }
                           }
                        }
                        catch (Exception ex)
                        {
                           TryRaiseException(ex);
                           _logger?.Invoke($"心跳：設定 response 時發生例外: {ex.Message}");
                           ok = false;
                        }

                        _lastObservedRequest = true;
                        _lastObservedResponse = (state & 1) != 0;
                     }

                     break;

                  case 3: // (1,1) - PC 已回應
                     _lastObservedRequest = true;
                     _lastObservedResponse = true;
                     break;

                  case 1: // (0,1) - PLC 已清除請求，等待 PC 清除回應
                     if (lastState == 3)
                     {
                        _logger?.Invoke("心跳：PLC 已清除請求 (1,1) -> (0,1)，準備清除 response");

                        try
                        {
                           lock (_apiLock)
                           {
                              int devCode = MapDeviceCode(_heartbeatResponseFlagAddr.Kind);
                              int r = _api.DevRstEx(_resolvedPath, 0, 0, devCode, _heartbeatResponseFlagAddr.Start);
                              _logger?.Invoke($"心跳：執行 DevRstEx {_heartbeatResponseFlagAddr.Kind} 0x{_heartbeatResponseFlagAddr.Start:X} => 0");

                              if (r == 0)
                              {
                                 _pendingRst = true;

                                 // 嘗試同步確認
                                 int sizeConfirm = 2;
                                 var confirmBuf = new short[1];
                                 int rc = _api.ReceiveEx(_resolvedPath, 0, 0, devCode, _heartbeatResponseFlagAddr.Start, ref sizeConfirm, confirmBuf);
                                 if (rc == 0 && confirmBuf[0] == 0)
                                 {
                                    UpdateCache(_heartbeatResponseFlagAddr.Kind, _heartbeatResponseFlagAddr.Start, 0);
                                    _pendingRst = false;
                                    state = 0; // 直接進入 (0,0)
                                    handshakeCompleted = true;
                                    _logger?.Invoke("心跳：✅ response reset 立即確認，心跳完成 (0,1) -> (0,0)");
                                 }
                              }
                           }
                        }
                        catch (Exception ex)
                        {
                           TryRaiseException(ex);
                           _logger?.Invoke($"心跳：清除 response 時發生例外: {ex.Message}");
                           ok = false;
                        }

                        _lastObservedRequest = false;
                        _lastObservedResponse = (state & 1) != 0;
                     }

                     break;

                  default:
                     break;
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
               if (ConsecutiveHeartbeatFailures > 0)
               {
                  ConsecutiveHeartbeatFailures = 0;
               }

               if (handshakeCompleted)
               {
                  PostEvent(() => HeartbeatSucceeded?.Invoke());
               }
            }
            else
            {
               ConsecutiveHeartbeatFailures++;
               PostEvent(() => HeartbeatFailed?.Invoke(ConsecutiveHeartbeatFailures));

               if (ConsecutiveHeartbeatFailures >= _heartbeatFailThreshold)
               {
                  PostEvent(() => Disconnected?.Invoke());
                  _logger?.Invoke("偵測到心跳中斷（長期未更新）；啟動重新連線循環。");
                  _ = Task.Run(async () =>
                  {
                     await AttemptReconnectLoop().ConfigureAwait(false);
                     if (!_disposed && _heartbeatEnabled)
                     {
                        ConsecutiveHeartbeatFailures = 0;
                        _handshakeWatchdog = DateTime.UtcNow;
                     }
                  });
               }
            }
         }, ct);
      }

      private void UpdateCache(string kind, int address, short value)
      {
         if (_deviceMemory.TryGetValue(kind, out var mem) && address >= 0 && address < mem.Length)
         {
            mem[address] = value;
         }
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

      public void StartHeartbeat(TimeSpan interval, LinkDeviceAddress requestFlagAddr, LinkDeviceAddress responseFlagAddr, int failThreshold = 3)
      {
         StopHeartbeat();

         HeartbeatInterval = interval;
         HeartbeatFailThreshold = Math.Max(1, failThreshold);

         _heartbeatRequestFlagAddr = requestFlagAddr ?? throw new ArgumentNullException(nameof(requestFlagAddr));
         _heartbeatResponseFlagAddr = responseFlagAddr ?? throw new ArgumentNullException(nameof(responseFlagAddr));
         _heartbeatInterval = interval;
         _heartbeatFailThreshold = HeartbeatFailThreshold;

         // 啟動獨立的心跳 Task
         _heartbeatEnabled = true;
         _heartbeatCts = new CancellationTokenSource();
         _heartbeatTask = Task.Run(() => HeartbeatLoopAsync(_heartbeatCts.Token));
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
      public void StartTimeSync(TimeSpan interval, LinkDeviceAddress requestFlagAddr, LinkDeviceAddress requestDataAddr, LinkDeviceAddress responseFlagAddr)
      {
         throw new NotSupportedException("TimeSync is disabled in this build.");
      }

      public void StopTimeSync()
      {
         // noop
      }

      public bool ForceTimeSync(DateTime dt, LinkDeviceAddress a, LinkDeviceAddress b, LinkDeviceAddress c) => false;

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

            // 步驟 3: 將合併後的區間直接加入 Batch 計畫 (不再手動分割)
            foreach (var m in merged)
            {
               newPlan.Add(new BatchRead
               {
                  Kind = kind,
                  Start = m.Start,
                  Words = m.End - m.Start + 1
               });
            }
         }

         _pollingPlan = newPlan;
         _logger?.Invoke($"輪詢編譯完成：總計 {newPlan.Count} 個通訊批次");
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

      /// <summary>
      /// 取得指定 Kind 與位址的 Bit 狀態。
      /// </summary>
      public bool GetBit(string kind, int address)
      {
         if (string.IsNullOrWhiteSpace(kind))
         {
            return false;
         }

         if (_deviceMemory.TryGetValue(kind, out var memory))
         {
            if (address >= 0 && address < memory.Length)
            {
               return memory[address] != 0;
            }
         }

         return false;
      }

      /// <summary>
      /// 取得指定地址物件的 Bit 狀態 (回傳第一個點位)。
      /// </summary>
      public bool GetBit(LinkDeviceAddress addr)
      {
         if (addr == null)
         {
            return false;
         }

         return GetBit(addr.Kind, addr.Start);
      }

      /// <summary>
      /// 取得指定 Kind 與位址的 Word 數值。
      /// </summary>
      public short GetWord(string kind, int address)
      {
         if (string.IsNullOrWhiteSpace(kind))
         {
            return 0;
         }

         if (_deviceMemory.TryGetValue(kind, out var memory))
         {
            if (address >= 0 && address < memory.Length)
            {
               return memory[address];
            }
         }

         return 0;
      }

      #endregion

      #region ICCLinkController Implementation

      public async Task OpenAsync(CancellationToken ct = default)
      {
         await Task.Run(() =>
         {
            lock (_apiLock)
            {
               int path;
               // 使用 Settings 中的 Port 作為 Channel，Mode 設為 1 (與 MelsecControlCard 對齊)
               int rc = _api.Open((short)_settings.Port, 1, out path);
               if (rc != 0)
               {
                  throw MelsecException.FromCode(rc, nameof(_api.Open));
               }

               _resolvedPath = path;
               _status.IsConnected = true;
               _status.Channel = _settings.Port;
               _status.LastUpdated = DateTime.UtcNow;
               _logger?.Invoke($"已開啟 PLC 通訊路徑: {path} (通道: {_settings.Port})");
            }
         }, ct).ConfigureAwait(false);
      }

      public async Task CloseAsync(CancellationToken ct = default)
      {
         StopPollingWorker();
         StopHeartbeat();

         await Task.Run(() =>
         {
            lock (_apiLock)
            {
               if (_resolvedPath >= 0)
               {
                  int rc = _api.Close(_resolvedPath);
                  _logger?.Invoke($"已關閉 PLC 通訊路徑: {_resolvedPath} (RC: {rc})");
                  _resolvedPath = -1;
               }

               _status.IsConnected = false;
               _status.LastUpdated = DateTime.UtcNow;
            }
         }, ct).ConfigureAwait(false);
      }

      public async Task<IReadOnlyList<bool>> ReadBitsAsync(string address, int count, CancellationToken ct = default)
      {
         var parsed = LinkDeviceAddress.Parse(address, count);
         int deviceCode = MapDeviceCode(parsed.Kind);
         int size = count * 2;
         short[] buffer = new short[count];

         await Task.Run(() =>
         {
            lock (_apiLock)
            {
               int rc = _api.ReceiveEx(_resolvedPath, 0, 0, deviceCode, parsed.Start, ref size, buffer);
               if (rc != 0)
               {
                  throw MelsecException.FromCode(rc, nameof(_api.ReceiveEx));
               }
            }
         }, ct).ConfigureAwait(false);

         return buffer.Select(x => x != 0).ToArray();
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
               int rc = _api.SendEx(_resolvedPath, 0, 0, deviceCode, parsed.Start, ref size, src);
               if (rc != 0)
               {
                  throw MelsecException.FromCode(rc, nameof(_api.SendEx));
               }
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
               int rc = _api.ReceiveEx(_resolvedPath, 0, 0, deviceCode, parsed.Start, ref size, buffer);
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
               int rc = _api.SendEx(_resolvedPath, 0, 0, deviceCode, parsed.Start, ref size, src);
               if (rc != 0)
               {
                  throw MelsecException.FromCode(rc, nameof(_api.SendEx));
               }
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