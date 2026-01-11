using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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

      [StructLayout(LayoutKind.Sequential)]
      private struct SystemTime
      {
         public ushort Year;
         public ushort Month;
         public ushort DayOfWeek;
         public ushort Day;
         public ushort Hour;
         public ushort Minute;
         public ushort Second;
         public ushort Milliseconds;
      }

      #endregion

      #region Fields

      // 核心相依
      private readonly IMelsecApiAdapter _api;

      // 同步 / 工作緒
      private readonly object _apiLock = new object();
      private readonly object _commonReportLock = new object();

      // 設備記憶體快取 (Key: Kind, Value: short[] array representing the device memory)
      // [Fix] 改用 Dictionary，完全由 _apiLock 保護，避免 ConcurrentDictionary 的替換競爭
      private readonly Dictionary<string, short[]> _deviceMemory = new Dictionary<string, short[]>(StringComparer.OrdinalIgnoreCase);

      // 追蹤上次的 ArrayID，用於偵測陣列更換 (Key: "Kind:Address")
      private readonly Dictionary<string, int> _lastArrayIds = new Dictionary<string, int>();

      // 追蹤 PollBitDeviceBatch 上次的 buffer 內容 (Key: "Kind:Start")
      private readonly Dictionary<string, string> _lastPollBuffers = new Dictionary<string, string>();

      // 追蹤 UpdateBitCache 上次的 ArrayID (Key: "Kind")
      private readonly Dictionary<string, int> _lastUpdateArrayIds = new Dictionary<string, int>();

      private readonly Action<string> _logger;
      private readonly object _planLock = new object();

      private readonly Func<Task<bool>> _reconnectAsync;

      private readonly Func<bool> _reconnectSync;
      private readonly ControllerSettings _settings;
      private readonly ControllerStatus _status = new ControllerStatus();
      private readonly SynchronizationContext _syncContext;
      private readonly List<ScanRange> _userScanRanges = new List<ScanRange>();

      // 狀態
      private bool _disposed;
      private DateTime? _handshakeWatchdog;
      private CancellationTokenSource _heartbeatCts;

      // 心跳設定 / 狀態
      private int _heartbeatFailThreshold = 3;
      private TimeSpan _heartbeatInterval;
      private LinkDeviceAddress _heartbeatRequestFlagAddr;
      private LinkDeviceAddress _heartbeatResponseFlagAddr;

      // 心跳專用的 Task 和 CancellationTokenSource
      private Task _heartbeatTask;
      private CommonReportAlarm _lastCommonReportAlarm;

      // 定期上報共通資料 - 快取上一次的資料，用於變化檢測
      private CommonReportStatus1 _lastCommonReportStatus1;
      private CommonReportStatus2 _lastCommonReportStatus2;

      // 心跳觀察快取，避免在無變化時重複宣告成功
      private bool? _lastObservedRequest;
      private bool? _lastObservedResponse;
      private bool _lastTimeSyncTrigger;
      private int _mergeGapTolerance = 300;
      private List<BatchRead> _pollingPlan = new List<BatchRead>();

      // 輪詢設定
      private TimeSpan _pollInterval;

      // 已解析的路徑（Start 時呼叫一次 getPath）
      private int _resolvedPath = -1;
      private CancellationTokenSource _timeSyncCts;
      private Task _timeSyncTask;

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
      public int ConsecutiveHeartbeatFailuresCount { get; private set; }
      public bool IsHeartbeatRunning => _heartbeatCts != null && !_heartbeatCts.IsCancellationRequested && !_disposed;

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
            _logger?.Invoke($"輪詢間隔已變更 | Polling interval changed (From: {oldValue.TotalMilliseconds:F0}ms, To: {value.TotalMilliseconds:F0}ms)");
         }
      }

      #endregion

      #region Private Methods

      [DllImport("kernel32.dll", SetLastError = true)]
      private static extern bool SetLocalTime(ref SystemTime lpSystemTime);

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
         var ct = _workerCts.Token; // 先保存 Token，避免競爭條件
         _workerTask = Task.Run(() => PollingWorkerLoopAsync(ct));
      }

      /// <summary>
      /// 停止輪詢工作。
      /// </summary>
      private void StopPollingWorker()
      {
         _logger?.Invoke("[StopPollingWorker] 開始停止輪詢 | Starting to stop polling...");

         try
         {
            var c = Interlocked.Exchange(ref _workerCts, null);
            if (c != null)
            {
               try
               {
                  c.Cancel();
                  _logger?.Invoke("[StopPollingWorker] 已發送取消信號 | Cancel signal sent");
               }
               catch (Exception ex)
               {
                  _logger?.Invoke($"停止輪詢工作緒取消例外 | Exception while canceling polling worker (Error: {ex.Message})");
               }

               try
               {
                  // 增加等待時間到 1000ms，確保工作緒完全停止
                  bool stopped = _workerTask?.Wait(1000) ?? true;
                  if (stopped)
                  {
                     _logger?.Invoke("[StopPollingWorker] 輪詢工作緒已停止 | Polling worker stopped");
                  }
                  else
                  {
                     _logger?.Invoke("[StopPollingWorker] 輪詢工作緒停止超時 | Polling worker stop timeout");
                  }
               }
               catch (Exception ex)
               {
                  _logger?.Invoke($"停止輪詢工作緒等待例外 | Exception while waiting for polling worker (Error: {ex.Message})");
               }

               try
               {
                  c.Dispose();
               }
               catch (Exception ex)
               {
                  _logger?.Invoke($"釋放 CancellationTokenSource 例外 | Exception disposing CancellationTokenSource (Error: {ex.Message})");
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
                  _logger?.Invoke($"輪詢週期逾時 | Polling cycle execution too long (Delay: {Math.Abs(delay.TotalMilliseconds):F0}ms)");
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
                  _logger?.Invoke($"輪詢執行例外 | Exception during polling (Error: {ex.Message})");
               }

               // 計算實際執行時間
               TimeSpan pollDuration = DateTime.UtcNow - pollStart;

               // 使用固定間隔模式：基於預期時間而非實際時間
               // 這樣可以補償執行時間，維持穩定的輪詢頻率
               nextPoll += _pollInterval;

               // 如果執行時間超過間隔，立即進行下一次輪詢
               if (nextPoll < DateTime.UtcNow)
               {
                  // 只有當實際延遲超過一個完整週期時才記錄警告
                  bool isSignificantlyLate = nextPoll + _pollInterval < DateTime.UtcNow;
                  if (isSignificantlyLate)
                  {
                     _logger?.Invoke(
                        $"輪詢執行超時 | Polling duration exceeded interval (Duration: {pollDuration.TotalMilliseconds:F0}ms, Interval: {_pollInterval.TotalMilliseconds:F0}ms)");
                     nextPoll = DateTime.UtcNow + _pollInterval;
                  }
               }
            }
            catch (OperationCanceledException)
            {
               break;
            }
            catch (Exception ex)
            {
               TryRaiseException(ex);
               _logger?.Invoke($"輪詢工作迴圈例外 | Exception in polling worker loop (Error: {ex.Message})");

               // 異常後使用輪詢間隔作為延遲，而非固定 500ms
               try
               {
                  await Task.Delay(_pollInterval, ct).ConfigureAwait(false);
               }
               catch (Exception ex2)
               {
                  _logger?.Invoke($"輪詢延遲等待例外 | Exception in polling loop delay (Error: {ex2.Message})");
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
         while (!ct.IsCancellationRequested && !_disposed)
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
               _logger?.Invoke($"心跳迴圈例外 | Exception in heartbeat loop (Error: {ex.Message})");
               try
               {
                  await Task.Delay(500, ct).ConfigureAwait(false);
               }
               catch (Exception ex2)
               {
                  _logger?.Invoke($"心跳延遲等待例外 | Exception in heartbeat loop delay (Error: {ex2.Message})");
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
            // 檢查是否已關閉連線，避免在 Close 後繼續輪詢
            if (!_status.IsConnected)
            {
               return;
            }

            foreach (var batch in batches)
            {
               try
               {
                  bool isBitDevice = IsBitDevice(batch.Kind);
                  if (isBitDevice)
                  {
                     PollBitDeviceBatch(batch);
                  }
                  else
                  {
                     PollWordDeviceBatch(batch);
                  }
               }
               catch (Exception ex)
               {
                  TryRaiseException(ex);
                  _logger?.Invoke($"輪詢批次處理例外 | Exception in polling batch processing (Error: {ex.Message})");
               }
            }
         }
      }

      /// <summary>
      /// 輪詢 Bit 類型設備（LB/LX/LY），使用 8 位元對齊讀取。
      /// </summary>
      private void PollBitDeviceBatch(BatchRead batch)
      {
         int path = _resolvedPath;
         int devCode = MapDeviceCode(batch.Kind);

         // 計算對齊範圍：將起始位址向下對齊到 8 的倍數
         int alignedStart = batch.Start / 8 * 8;
         int alignedEnd = (batch.Start + batch.Words - 1) / 8 * 8 + 7;
         int alignedWords = (alignedEnd - alignedStart + 1 + 7) / 8;

         var dest = new short[alignedWords];

         // 一次批量讀取
         int totalInBytes = alignedWords; // Each alignedWord represents 8 bits (1 byte)
         int size = totalInBytes;

         // Buffer logic: size is in bytes. short[] buffer needs ceil(size/2)
         // e.g., 65 bytes -> 33 shorts.
         var buffer = new short[(size + 1) / 2];

         int rc = _api.ReceiveEx(path, 1, 1, devCode, alignedStart, ref size, buffer);

         UpdateConnectionStatus(rc);

         if (rc == 0)
         {
            // 記錄原始 buffer（僅針對關鍵範圍，且只在內容變化時記錄）
            if (batch.Start <= 1024 && batch.Start + batch.Words > 1020)
            {
               var bufferPreview = string.Join(",", buffer.Take(Math.Min(8, buffer.Length)).Select(b => $"0x{b:X4}"));
               string bufferKey = $"{batch.Kind}:{batch.Start}";
               string lastBuffer;
               bool hasLast = _lastPollBuffers.TryGetValue(bufferKey, out lastBuffer);

               if (!hasLast || lastBuffer != bufferPreview)
               {
                  //_logger?.Invoke($"[PollBitDeviceBatch] Buffer 變更 | Buffer changed (BatchStart={batch.Start}, AlignedStart={alignedStart}, RawBuffer=[{bufferPreview}])");
                  _lastPollBuffers[bufferKey] = bufferPreview;
               }
            }

            // Unpack buffer (packed shorts) into dest (one short per byte)
            // Buffer: [Low:Byte0, High:Byte1], [Low:Byte2, High:Byte3]...
            for (int i = 0; i < alignedWords; i++)
            {
               int bufferIndex = i / 2;
               bool isLowerByte = i % 2 == 0;

               if (bufferIndex < buffer.Length)
               {
                  short raw = buffer[bufferIndex];
                  if (isLowerByte)
                  {
                     dest[i] = (short)(raw & 0xFF);
                  }
                  else
                  {
                     dest[i] = (short)((raw >> 8) & 0xFF);
                  }
               }
            }
         }
         else
         {
            _logger?.Invoke(string.Format("批次讀取失敗 | Batch read failed (Device: {0}0x{1:X4}, Size: {2}, RC: {3})", batch.Kind, alignedStart, totalInBytes, rc));
            return;
         }

         // 更新快取
         if (_status.IsConnected)
         {
            UpdateBitCache(batch, alignedStart, dest);
         }
      }

      /// <summary>
      /// 輪詢 Word 類型設備（LW）。
      /// </summary>
      private void PollWordDeviceBatch(BatchRead batch)
      {
         int path = _resolvedPath;
         int devCode = MapDeviceCode(batch.Kind);
         int sizeInBytes = batch.Words * 2;
         var dest = new short[batch.Words];

         int rc = _api.ReceiveEx(path, 0, 0, devCode, batch.Start, ref sizeInBytes, dest);

         UpdateConnectionStatus(rc);

         if (rc == 0)
         {
            // Dictionary 必須在鎖內訪問
            if (_deviceMemory.ContainsKey(batch.Kind))
            {
               var memory = _deviceMemory[batch.Kind];
               Array.Copy(dest, 0, memory, batch.Start, batch.Words);
            }
         }
         else
         {
            _logger?.Invoke($"批次讀取失敗 | Batch read failed (Device: {batch.Kind}{batch.Start:X4}, Words: {batch.Words}, RC: {rc})");
         }
      }

      /// <summary>
      /// 從對齊的 buffer 中提取位元值並更新快取。
      /// 注意：此方法必須在 _apiLock 保護下執行，以確保陣列不會在更新過程中被替換。
      /// </summary>
      private void UpdateBitCache(BatchRead batch, int alignedStart, short[] dest)
      {
         int helperHashCode = RuntimeHelpers.GetHashCode(this);

         short[] memory;
         if (!_deviceMemory.TryGetValue(batch.Kind, out memory))
         {
            _logger?.Invoke($"[UpdateBitCache] Kind 不存在 | Kind not found (HelperID={helperHashCode}, Kind={batch.Kind})");
            return;
         }

         int currentArrayId = RuntimeHelpers.GetHashCode(memory);

         // 只在 ArrayID 變化時記錄（按 Kind 追蹤）
         int lastArrayId;
         bool hasLast = _lastUpdateArrayIds.TryGetValue(batch.Kind, out lastArrayId);

         if (!hasLast || lastArrayId != currentArrayId)
         {
            //_logger?.Invoke($"[UpdateBitCache] ArrayID 變更 | ArrayID changed (HelperID={helperHashCode}, Kind={batch.Kind}, OldID={(hasLast ? lastArrayId.ToString() : "N/A")}, NewID={currentArrayId}, ArrayLen={memory.Length})");
            _lastUpdateArrayIds[batch.Kind] = currentArrayId;
         }

         // 直接更新陣列
         for (int i = 0; i < batch.Words; i++)
         {
            int actualAddress = batch.Start + i;
            int alignedIndex = (actualAddress - alignedStart) / 8;
            int bitPosition = (actualAddress - alignedStart) % 8;

            if (alignedIndex < dest.Length && actualAddress < memory.Length)
            {
               int bitValue = (dest[alignedIndex] >> bitPosition) & 1;
               memory[actualAddress] = (short)bitValue;
            }
         }
      }

      /// <summary>
      /// 更新連線狀態。
      /// </summary>
      private void UpdateConnectionStatus(int returnCode)
      {
         _status.IsConnected = returnCode == 0;
         _status.LastErrorCode = returnCode;
         _status.LastUpdated = DateTime.UtcNow;
      }

      private Task RunHeartbeatAsync(CancellationToken ct)
      {
         return Task.Run(() =>
         {
            if (_disposed)
            {
               return;
            }

            bool ok;
            bool handshakeCompleted = false;
            try
            {
               // 使用快取讀取狀態
               bool requestOn = GetBit(_heartbeatRequestFlagAddr);
               bool responseOn = GetBit(_heartbeatResponseFlagAddr);

               // 使用 bit mask: request bit << 1 | response bit
               int state = (requestOn ? 2 : 0) | (responseOn ? 1 : 0);

               // 追蹤上一個狀態
               int lastState = (_lastObservedRequest ?? false ? 2 : 0) | (_lastObservedResponse ?? false ? 1 : 0);

               // 第一次進來時條件 || 後續狀態變更條件
               bool stateChanged = !(_lastObservedRequest.HasValue && _lastObservedResponse.HasValue) || state != lastState;

               // 心跳進展邏輯：只要狀態有變化，或是處於 (0,0) 閒置狀態，就更新 Watchdog
               if (stateChanged || state == 0)
               {
                  _handshakeWatchdog = DateTime.UtcNow;
                  ok = true;

                  if (stateChanged)
                  {
                     _logger?.Invoke(
                        $"心跳狀態變更 | Heartbeat state changed (Current: {(requestOn ? 1 : 0)},{(responseOn ? 1 : 0)}, Last: {(_lastObservedRequest ?? false ? 1 : 0)},{(_lastObservedResponse ?? false ? 1 : 0)})");
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
                     _logger?.Invoke($"心跳狀態長時間未跳轉 | Heartbeat state not changed (State: {state}, Timeout: {timeout.TotalMilliseconds:F0}ms)");
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
                        _logger?.Invoke("心跳循環完成 | Heartbeat cycle completed (0,0)");
                     }

                     _lastObservedRequest = false;
                     _lastObservedResponse = false;
                     break;

                  case 2: // (1,0) - PLC 發起請求
                     if (lastState == 0 || !_lastObservedRequest.HasValue)
                     {
                        _logger?.Invoke("偵測到 PLC 請求 | PLC request detected (0,0) -> (1,0)");

                        try
                        {
                           lock (_apiLock)
                           {
                              int devCode = MapDeviceCode(_heartbeatResponseFlagAddr.Kind);
                              // 使用 (1, 1) 表示位元模式 | Use (1, 1) for bit mode
                              int r = _api.DevSetEx(_resolvedPath, 1, 1, devCode, _heartbeatResponseFlagAddr.Start);
                              _logger?.Invoke(
                                 $"執行回應追蹤設定 | Executing response flag set (Device: {_heartbeatResponseFlagAddr.Kind} 0x{_heartbeatResponseFlagAddr.Start:X4}, Value: 1)");

                              if (r != 0)
                              {
                                 ok = false;
                                 _logger?.Invoke($"回應旗號設定失敗 | Failed to set response flag (RC: {r})");
                              }
                           }
                        }
                        catch (Exception ex)
                        {
                           TryRaiseException(ex);
                           _logger?.Invoke($"回應旗號設定例外 | Exception while setting response flag (Error: {ex.Message})");
                           ok = false;
                        }

                        _lastObservedRequest = true;
                        _lastObservedResponse = false;
                     }

                     break;

                  case 3: // (1,1) - PC 已回應
                     _lastObservedRequest = true;
                     _lastObservedResponse = true;
                     break;

                  case 1: // (0,1) - PLC 已清除請求，等待 PC 清除回應
                     if (lastState == 3)
                     {
                        _logger?.Invoke("準備清除回應 | PLC cleared request, preparing to clear response (1,1) -> (0,1)");

                        try
                        {
                           lock (_apiLock)
                           {
                              int devCode = MapDeviceCode(_heartbeatResponseFlagAddr.Kind);
                              // 使用 (1, 1) 表示位元模式 | Use (1, 1) for bit mode
                              int r = _api.DevRstEx(_resolvedPath, 1, 1, devCode, _heartbeatResponseFlagAddr.Start);
                              _logger?.Invoke(
                                 $"執行回應旗號重置 | Executing response flag reset (Device: {_heartbeatResponseFlagAddr.Kind} 0x{_heartbeatResponseFlagAddr.Start:X4}, Value: 0)");

                              if (r != 0)
                              {
                                 ok = false;
                                 _logger?.Invoke($"回應旗號重置失敗 | Failed to reset response flag (RC: {r})");
                              }
                           }
                        }
                        catch (Exception ex)
                        {
                           TryRaiseException(ex);
                           _logger?.Invoke($"回應旗號重置例外 | Exception while resetting response flag (Error: {ex.Message})");
                           ok = false;
                        }

                        _lastObservedRequest = false;
                        _lastObservedResponse = true;
                     }

                     break;
               }
            }
            catch (Exception ex)
            {
               TryRaiseException(ex);
               _logger?.Invoke($"心跳邏輯處理例外 | Exception in heartbeat processing (Error: {ex.Message})");
               ok = false;
            }

            if (ok)
            {
               if (ConsecutiveHeartbeatFailuresCount > 0)
               {
                  ConsecutiveHeartbeatFailuresCount = 0;
               }

               if (handshakeCompleted)
               {
                  PostEvent(() => HeartbeatSucceeded?.Invoke());
               }
            }
            else
            {
               int currentCount = ++ConsecutiveHeartbeatFailuresCount;
               PostEvent(() => ConsecutiveHeartbeatFailures?.Invoke(currentCount));

               if (ConsecutiveHeartbeatFailuresCount >= _heartbeatFailThreshold)
               {
                  PostEvent(() => HeartbeatFailed?.Invoke());
                  PostEvent(() => Disconnected?.Invoke());

                  if (_reconnectAsync == null && _reconnectSync == null)
                  {
                     _logger?.Invoke("心跳中斷且無重連委派 | Heartbeat stopped, no reconnection delegate provided");
                     StopHeartbeat();
                     return;
                  }

                  _logger?.Invoke("啟動重新連線 | Heartbeat timed out, starting reconnection loop");
                  _ = Task.Run(async () =>
                  {
                     await AttemptReconnectLoop().ConfigureAwait(false);
                     if (!_disposed)
                     {
                        ConsecutiveHeartbeatFailuresCount = 0;
                        _handshakeWatchdog = DateTime.UtcNow;
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
            bool ok;
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
                  _logger?.Invoke("無重新連線委派 | No reconnection delegate provided");
                  break;
               }
            }
            catch (Exception ex)
            {
               TryRaiseException(ex);
               _logger?.Invoke($"重新連線嘗試例外 | Exception during reconnection attempt (Error: {ex.Message})");
               ok = false;
            }

            if (ok)
            {
               _logger?.Invoke($"重新連線成功 | Reconnection successful (Attempts: {attempt})");
               PostEvent(() => Reconnected?.Invoke());
               return;
            }

            int currentAttempt = attempt;
            PostEvent(() => ReconnectAttemptFailed?.Invoke(currentAttempt));
            _logger?.Invoke($"重新連線嘗試失敗 | Reconnection attempt failed (Attempt: {attempt})");

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

         _logger?.Invoke("重新連線結束仍未成功 | Reconnection loop ended without success");
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
               _logger?.Invoke($"UI 執行緒同步例外 | Exception in SynchronizationContext.Post (Error: {ex.Message})");
               try
               {
                  action();
               }
               catch (Exception ex2)
               {
                  _logger?.Invoke($"UI 同步備援執行例外 | Exception in fallback action execution (Error: {ex2.Message})");
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
               _logger?.Invoke($"事件執行例外 | Exception in direct action execution (Error: {ex.Message})");
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
            _logger?.Invoke($"例外通知事件處理例外 | Exception in ExceptionOccurred handler (Error: {ex2.Message})");
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
            _logger?.Invoke($"資源釋放停止輪詢例外 | Exception while stopping worker during dispose (Error: {ex.Message})");
         }
      }

      private sealed class BatchRead
      {
         #region Properties

         public string Kind { get; set; }
         public int Start { get; set; }
         public int Words { get; set; }

         #endregion
      }

      #region Utilities

      #endregion

      #region Events

      public event Action Disconnected;
      public event Action<Exception> ExceptionOccurred;
      public event Action HeartbeatFailed;                   // 連續失敗計數
      public event Action<int> ConsecutiveHeartbeatFailures; // 連續失敗計數
      public event Action HeartbeatSucceeded;
      public event Action<int> ReconnectAttemptFailed;
      public event Action Reconnected;
      public event Action Connected;

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
         _heartbeatCts = new CancellationTokenSource();
         _heartbeatTask = Task.Run(() => HeartbeatLoopAsync(_heartbeatCts.Token));
      }

      public void StopHeartbeat()
      {
         ConsecutiveHeartbeatFailuresCount = 0;

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
                  _logger?.Invoke($"停止心跳任務等待例外 | Exception while waiting for heartbeat task (Error: {ex.Message})");
               }

               _heartbeatCts.Dispose();
               _heartbeatCts = null;
               _heartbeatTask = null;
            }
         }
         catch (Exception ex)
         {
            _logger?.Invoke($"停止心跳執行例外 | Exception while stopping heartbeat (Error: {ex.Message})");
         }
      }

      public void StopTimeSync()
      {
         try
         {
            lock (_apiLock)
            {
               if (_timeSyncCts != null)
               {
                  _timeSyncCts.Cancel();
                  _timeSyncCts.Dispose();
                  _timeSyncCts = null;
               }
            }

            _logger?.Invoke("對時監控已停止 | TimeSync monitoring stopped");
         }
         catch (Exception ex)
         {
            _logger?.Invoke($"停止對時監控發生例外 | Exception while stopping TimeSync (Error: {ex.Message})");
         }
      }

      public void StartTimeSync(TimeSpan interval, string triggerAddr, string dataAddr)
      {
         StopTimeSync();

         _timeSyncCts = new CancellationTokenSource();
         var config = new TimeSyncSettings
         {
            TriggerAddress = triggerAddr,
            DataBaseAddress = dataAddr
         };

         _lastTimeSyncTrigger = false; // 重置觸發旗標
         _logger?.Invoke($"啟動對時監控 | TimeSync monitoring started (Interval: {interval.TotalMilliseconds}ms, Trigger: {triggerAddr}, Data: {dataAddr})");
         _timeSyncTask = Task.Run(() => TimeSyncLoopAsync(interval, config, _timeSyncCts.Token));
      }

      private async Task TimeSyncLoopAsync(TimeSpan interval, TimeSyncSettings config, CancellationToken ct)
      {
         while (!ct.IsCancellationRequested && !_disposed)
         {
            try
            {
               bool requestOn = GetBitFromAddressString(config.TriggerAddress, 0);

               // 記錄目前偵測狀態
               if (requestOn || _lastTimeSyncTrigger)
               {
                  _logger?.Invoke($"[Debug] 對時位元狀態: {config.TriggerAddress}={requestOn}, 上次狀態={_lastTimeSyncTrigger}");
               }

               // 正緣觸發 (Rising Edge): 只有在從 Off 變 On 時才執行對時
               if (requestOn && !_lastTimeSyncTrigger)
               {
                  _logger?.Invoke($"[Debug] 偵測到正緣觸發，執行對時... | Rising edge detected, syncing time...");
                  await SyncTimeAsync(config).ConfigureAwait(false);
               }

               _lastTimeSyncTrigger = requestOn;
            }
            catch (Exception ex)
            {
               _logger?.Invoke($"對時監控迴圈例外 | Exception in TimeSync loop (Error: {ex.Message})");
            }

            await Task.Delay(interval, ct).ConfigureAwait(false);
         }
      }

      private async Task SyncTimeAsync(TimeSyncSettings config)
      {
         try
         {
            // LW Address
            // 0000 Clock Update 系统时间：年 YYYY
            // 0001 Clock Update 系统时间：月 MM
            // 0002 Clock Update 系统时间：日 DD
            // 0003 Clock Update 系统时间：時 hh
            // 0004 Clock Update 系统时间：分 mm
            // 0005 Clock Update 系统时间：秒 ss
            // 0006 Clock Update 系统时间：週 week 日～六(0-6)
            var values = await ReadWordsAsync(config.DataBaseAddress, 7).ConfigureAwait(false);
            if (values == null || values.Count < 7)
            {
               _logger?.Invoke("對時資料讀取失敗 | Failed to read TimeSync data");
               return;
            }

            var st = new SystemTime
            {
               Year = (ushort)values[0],
               Month = (ushort)values[1],
               Day = (ushort)values[2],
               Hour = (ushort)values[3],
               Minute = (ushort)values[4],
               Second = (ushort)values[5],
               DayOfWeek = (ushort)values[6],
               Milliseconds = 0
            };

            bool success = SetLocalTime(ref st);
            if (success)
            {
               _logger?.Invoke($"系統時間已更新 | System time updated ({st.Year}/{st.Month}/{st.Day} {st.Hour}:{st.Minute}:{st.Second})");
            }
            else
            {
               int error = Marshal.GetLastWin32Error();
               string valStr = string.Join(",", values);
               string adminHint = error == 1314 ? " (請以管理員權限執行 | Please run as Administrator)" : "";
               _logger?.Invoke($"系統時間更新失敗 | Failed to set system time (Win32 Error: {error}{adminHint}, Values: {valStr})");
            }
         }
         catch (Exception ex)
         {
            _logger?.Invoke($"執行對時發生例外 | Exception during TimeSync (Error: {ex.Message})");
         }
      }

      /// <summary>
      /// 取得指定位址的 Bit 狀態 (字串位址版本)。
      /// </summary>
      private bool GetBitFromAddressString(string address, int index)
      {
         var parsed = LinkDeviceAddress.Parse(address, 1);
         return GetBit(parsed.Kind, parsed.Start + index);
      }

      /// <summary>
      /// 設定要掃描的區塊範圍。這會替換先前的所有掃描設定。
      /// </summary>
      public void SetScanRanges(IEnumerable<ScanRange> ranges)
      {
         if (ranges == null)
         {
            throw new ArgumentNullException(nameof(ranges));
         }

         // 先更新使用者範圍，再重新編譯計畫
         lock (_planLock)
         {
            _userScanRanges.Clear();
            _userScanRanges.AddRange(ranges);
         }

         // 在 _planLock 外部調用，讓 UpdatePollingPlan 獨立使用 _apiLock
         UpdatePollingPlan();

         // 確保輪詢工作已啟動
         EnsurePollingWorkerStarted();
      }

      /// <summary>
      /// 內部版本：直接在 _apiLock 內執行，供 OpenAsync 使用。
      /// </summary>
      private void UpdatePollingPlanInternal()
      {
         var newPlan = new List<BatchRead>();
         List<ScanRange> scanRangesCopy;
         lock (_planLock)
         {
            scanRangesCopy = _userScanRanges.ToList();
         }

         // 按 Kind 分組
         var groups = scanRangesCopy.GroupBy(r => r.Kind, StringComparer.OrdinalIgnoreCase);

         foreach (var g in groups)
         {
            string kind = g.Key.ToUpperInvariant();
            int maxEnd = g.Max(r => r.End);

            // 確保該 Kind 的快取已初始化且容量足夠
            short[] existingMemory = _deviceMemory.ContainsKey(kind) ? _deviceMemory[kind] : null;
            if (existingMemory == null)
            {
               // 初次建立：建議點位數為 16384
               int requiredSize = Math.Max(16384, maxEnd + 1);
               var newMemory = new short[requiredSize];

               _logger?.Invoke(string.Format("[UpdatePollingPlan] 初始化記憶體 | Initializing memory (Kind={0}, Size={1}, NewID={2})",
                  kind, requiredSize, RuntimeHelpers.GetHashCode(newMemory)));

               _deviceMemory[kind] = newMemory;
            }
            else if (existingMemory.Length <= maxEnd)
            {
               // 需要擴充容量：建立新陣列並複製舊資料
               int requiredSize = Math.Max(16384, maxEnd + 1);
               var newMemory = new short[requiredSize];
               Array.Copy(existingMemory, newMemory, existingMemory.Length);

               _logger?.Invoke(string.Format("[UpdatePollingPlan] 擴充記憶體 | Expanding memory (Kind={0}, OldSize={1}, NewSize={2}, OldID={3}, NewID={4})",
                  kind, existingMemory.Length, requiredSize,
                  RuntimeHelpers.GetHashCode(existingMemory),
                  RuntimeHelpers.GetHashCode(newMemory)));

               _deviceMemory[kind] = newMemory;
            }
            else
            {
               // 容量足夠，不需要重新配置
               _logger?.Invoke(string.Format("[UpdatePollingPlan] 記憶體容量足夠 | Memory capacity sufficient (Kind={0}, Size={1}, MaxEnd={2}, ArrayID={3})",
                  kind, existingMemory.Length, maxEnd, RuntimeHelpers.GetHashCode(existingMemory)));
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

         lock (_planLock)
         {
            _pollingPlan = newPlan;
         }

         _logger?.Invoke($"輪詢編譯完成 | Polling plan compiled (Total Batches: {newPlan.Count})");
      }

      private void UpdatePollingPlan()
      {
         var newPlan = new List<BatchRead>();
         List<ScanRange> scanRangesCopy;
         lock (_planLock)
         {
            scanRangesCopy = _userScanRanges.ToList();
         }

         // 按 Kind 分組
         var groups = scanRangesCopy.GroupBy(r => r.Kind, StringComparer.OrdinalIgnoreCase);

         lock (_apiLock)
         {
            foreach (var g in groups)
            {
               string kind = g.Key.ToUpperInvariant();
               int maxEnd = g.Max(r => r.End);

               // 確保該 Kind 的快取已初始化且容量足夠
               short[] existingMemory = _deviceMemory.ContainsKey(kind) ? _deviceMemory[kind] : null;
               if (existingMemory == null)
               {
                  // 初次建立：建議點位數為 16384
                  int requiredSize = Math.Max(16384, maxEnd + 1);
                  var newMemory = new short[requiredSize];

                  _logger?.Invoke(string.Format("[UpdatePollingPlan] 初始化記憶體 | Initializing memory (Kind={0}, Size={1}, NewID={2})",
                     kind, requiredSize, RuntimeHelpers.GetHashCode(newMemory)));

                  _deviceMemory[kind] = newMemory;
               }
               else if (existingMemory.Length <= maxEnd)
               {
                  // 需要擴充容量時，使用原地擴充而非替換陣列
                  int requiredSize = Math.Max(16384, maxEnd + 1);
                  var newMemory = new short[requiredSize];
                  Array.Copy(existingMemory, newMemory, existingMemory.Length);

                  _logger?.Invoke(string.Format("[UpdatePollingPlan] 擴充記憶體 | Expanding memory (Kind={0}, OldSize={1}, NewSize={2}, OldID={3}, NewID={4})",
                     kind, existingMemory.Length, requiredSize,
                     RuntimeHelpers.GetHashCode(existingMemory),
                     RuntimeHelpers.GetHashCode(newMemory)));

                  _deviceMemory[kind] = newMemory;

                  // 更新追蹤的 ArrayID，避免 GetBit 記錄誤報
                  _lastArrayIds[kind] = RuntimeHelpers.GetHashCode(newMemory);
                  _lastUpdateArrayIds[kind] = RuntimeHelpers.GetHashCode(newMemory);
               }
               else
               {
                  // 容量足夠，不需要重新配置
                  _logger?.Invoke(string.Format(
                     "[UpdatePollingPlan] 記憶體容量足夠，重用現有陣列 | Memory capacity sufficient, reusing existing array (Kind={0}, Size={1}, MaxEnd={2}, ArrayID={3})",
                     kind, existingMemory.Length, maxEnd, RuntimeHelpers.GetHashCode(existingMemory)));
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
         }

         lock (_planLock)
         {
            _pollingPlan = newPlan;
         }

         _logger?.Invoke($"輪詢編譯完成 | Polling plan compiled (Total Batches: {newPlan.Count})");
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

         lock (_apiLock)
         {
            if (_deviceMemory.ContainsKey(addr.Kind))
            {
               var memory = _deviceMemory[addr.Kind];
               // 讀取位元裝置與字組裝置的邏輯略有不同 (若 LinkDeviceAddress 已處理好 offset 則直接使用)
               // 這裡假設 addr.Start 與 addr.Length 是基於該裝置 Kind 的索引
               if (addr.Start + addr.Length <= memory.Length)
               {
                  var copy = new short[addr.Length];
                  Array.Copy(memory, addr.Start, copy, 0, addr.Length);
                  return copy;
               }
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

         lock (_apiLock)
         {
            // [Fix] 加入 Count 診斷，確認是否與 UpdateBitCache 看到的一致
            int dictCount = _deviceMemory.Count;
            int helperHashCode = RuntimeHelpers.GetHashCode(this);

            if (_deviceMemory.ContainsKey(kind))
            {
               var memory = _deviceMemory[kind];

               if (address >= 0 && address < memory.Length)
               {
                  short value = memory[address];
                  int currentArrayId = RuntimeHelpers.GetHashCode(memory);

                  // 只在 ArrayID 變化時記錄（按 Kind 追蹤）
                  int lastArrayId;
                  bool hasLast = _lastArrayIds.TryGetValue(kind, out lastArrayId);

                  if (!hasLast || lastArrayId != currentArrayId)
                  {
                     _logger?.Invoke(
                        $"[GetBit] ArrayID 變更 | ArrayID changed (HelperID={helperHashCode}, Kind={kind}, Addr={address}, DictCount={dictCount}, OldID={(hasLast ? lastArrayId.ToString() : "N/A")}, NewID={currentArrayId}, ArrayLen={memory.Length})");
                     _lastArrayIds[kind] = currentArrayId;
                  }

                  return value != 0;
               }
               else
               {
                  _logger?.Invoke($"[GetBit] 位址超出範圍 | Address out of range (HelperID={helperHashCode}, Kind={kind}, Addr={address}, DictCount={dictCount})");
               }
            }
            else
            {
               _logger?.Invoke($"[GetBit] Kind 不存在 | Kind not found (HelperID={helperHashCode}, Kind={kind}, DictCount={dictCount})");
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

         // 加鎖以確保在陣列替換或寫入期間讀取的安全性
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

      #region ICCLinkController Implementation

      public async Task OpenAsync(CancellationToken ct = default)
      {
         await Task.Run(() =>
         {
            _logger?.Invoke("[OpenAsync] 開始開啟連線 | Starting connection...");

            // [Fix] 先確保輪詢工作已完全停止
            if (_workerTask != null)
            {
               _logger?.Invoke("[OpenAsync] 偵測到輪詢工作仍在執行，先停止... | Polling worker still running, stopping...");
               StopPollingWorker();
            }

            // [Fix] 使用單一鎖保護整個初始化過程
            lock (_apiLock)
            {
               int path;
               // 使用 Settings 中的 Port 作為 Channel，Mode 設為 -1 (與 MelsecControlCard 對齊)
               int rc = _api.Open((short)_settings.Port, OpenMode, out path);
               if (rc != 0)
               {
                  throw MelsecException.FromCode(rc, nameof(_api.Open));
               }

               _resolvedPath = path;

               // [Fix] 清空快取，確保使用全新陣列（Close → Open 後避免舊資料干擾）
               int oldMemCount = _deviceMemory.Count;
               _deviceMemory.Clear();
               _lastArrayIds.Clear();
               _lastUpdateArrayIds.Clear();
               _lastPollBuffers.Clear();
               _logger?.Invoke($"[OpenAsync] 已清空記憶體快取 | Memory cache cleared (OldCount={oldMemCount})");

               _status.IsConnected = true;
               _status.Channel = _settings.Port;
               _status.LastUpdated = DateTime.UtcNow;
               _logger?.Invoke($"[OpenAsync] PLC 通訊路徑已開啟 | PLC path opened (Path={path}, Channel={_settings.Port})");

               // [Fix] 在 _apiLock 內重新初始化輪詢計畫，確保記憶體陣列在鎖內建立完成
               _logger?.Invoke("[OpenAsync] 準備重建輪詢計畫 | Rebuilding polling plan...");
               UpdatePollingPlanInternal();
               _logger?.Invoke($"[OpenAsync] 輪詢計畫重建完成 | Polling plan rebuilt (MemCount={_deviceMemory.Count})");
            }

            // [Fix] 在 _apiLock 外啟動輪詢工作，避免死鎖
            // 此時記憶體已完全初始化，可以安全地開始輪詢
            if (_workerTask == null)
            {
               _workerCts = new CancellationTokenSource();
               var workerToken = _workerCts.Token; // 先保存 Token，避免競爭條件
               _workerTask = Task.Run(() => PollingWorkerLoopAsync(workerToken));
               _logger?.Invoke("[OpenAsync] 輪詢工作已啟動 | Polling worker started");
            }

            PostEvent(() => Connected?.Invoke());
         }, ct).ConfigureAwait(false);
      }

      public async Task CloseAsync(CancellationToken ct = default)
      {
         _logger?.Invoke("[CloseAsync] 準備關閉連線 | Preparing to close connection...");

         // [Fix] 先停止輪詢和心跳，確保沒有執行緒在使用 _deviceMemory
         StopPollingWorker();
         StopHeartbeat();

         // [Fix] 額外等待以確保工作緒完全停止
         await Task.Delay(100, ct).ConfigureAwait(false);

         await Task.Run(() =>
         {
            // [Fix] 使用 _apiLock 保護整個關閉流程，確保與輪詢執行緒的互斥
            lock (_apiLock)
            {
               if (_resolvedPath >= 0)
               {
                  int rc = _api.Close(_resolvedPath);
                  _logger?.Invoke($"[CloseAsync] PLC 通訊路徑已關閉 | PLC path closed (Path={_resolvedPath}, RC={rc})");
                  _resolvedPath = -1;
               }

               // [Fix] 關閉時清空所有快取，確保 Open 後使用全新的陣列
               int oldMemCount = _deviceMemory.Count;
               _deviceMemory.Clear();
               _lastArrayIds.Clear();
               _lastUpdateArrayIds.Clear();
               _lastPollBuffers.Clear();
               _logger?.Invoke($"[CloseAsync] 已清空記憶體快取 | Memory cache cleared (OldCount={oldMemCount})");

               _status.IsConnected = false;
               _status.LastUpdated = DateTime.UtcNow;
            }
         }, ct).ConfigureAwait(false);
      }

      public async Task<bool> ReadBitsAsync(string address, CancellationToken ct = default)
      {
         // 解析位址，例如 LB0001
         var parsed = LinkDeviceAddress.Parse(address, 1);
         int deviceCode = MapDeviceCode(parsed.Kind);

         // 計算對齊到 8 的倍數的起始位址（向下取整）
         // 例如：LB0001 -> LB0000, LB0009 -> LB0008
         int alignedStart = parsed.Start / 8 * 8;

         // 計算目標位元在 8 位元組中的位置 (0-7)
         int bitPosition = parsed.Start - alignedStart;

         // 讀取位元軟元件，size 固定為 1
         int size = 1;
         short[] buffer = new short[1];

         await Task.Run(() =>
         {
            lock (_apiLock)
            {
               int rc = _api.ReceiveEx(_resolvedPath, 0, 0, deviceCode, alignedStart, ref size, buffer);
               if (rc != 0)
               {
                  throw MelsecException.FromCode(rc, nameof(_api.ReceiveEx));
               }
            }
         }, ct).ConfigureAwait(false);

         // 從 buffer[0] 中提取特定位元
         // buffer[0] 包含 8 個位元 (bit 0-7)
         // 例如：buffer[0] = 2 (二進制: 0000 0010) 表示 bit 1 = true
         int bitValue = (buffer[0] >> bitPosition) & 1;
         return bitValue != 0;
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

               // 同步更新快取，確保 GetBit/GetWord 能立即讀到新值 (若不在掃描範圍內則自動建立/擴充)
               short[] mem = _deviceMemory.ContainsKey(parsed.Kind) ? _deviceMemory[parsed.Kind] : null;
               if (mem == null || parsed.Start + src.Length > mem.Length)
               {
                  int newSize = Math.Max(16384, parsed.Start + src.Length);
                  var newMem = new short[newSize];
                  if (mem != null)
                  {
                     Array.Copy(mem, newMem, mem.Length);
                  }

                  Array.Copy(src, 0, newMem, parsed.Start, src.Length);
                  _deviceMemory[parsed.Kind] = newMem;
               }
               else
               {
                  Array.Copy(src, 0, mem, parsed.Start, src.Length);
                  _logger?.Invoke($"[Debug] WriteBits 快取更新: {address}, 值={string.Join(",", vals)} | Cache updated");
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

               // 同步更新快取，確保 GetBit/GetWord 能立即讀到新值 (若不在掃描範圍內則自動建立/擴充)
               short[] mem2 = _deviceMemory.ContainsKey(parsed.Kind) ? _deviceMemory[parsed.Kind] : null;
               if (mem2 == null || parsed.Start + src.Length > mem2.Length)
               {
                  int newSize = Math.Max(16384, parsed.Start + src.Length);
                  var newMem = new short[newSize];
                  if (mem2 != null)
                  {
                     Array.Copy(mem2, newMem, mem2.Length);
                  }

                  Array.Copy(src, 0, newMem, parsed.Start, src.Length);
                  _deviceMemory[parsed.Kind] = newMem;
               }
               else
               {
                  Array.Copy(src, 0, mem2, parsed.Start, src.Length);
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

      /// <summary>
      /// 判斷是否為 Bit 類型設備(LB、LX、LY)。
      /// </summary>
      private bool IsBitDevice(string kind)
      {
         string upperKind = kind.ToUpperInvariant();
         return upperKind == "LB" || upperKind == "LX" || upperKind == "LY";
      }

      internal int MapDeviceCode(string kind)
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

      #region Common Report (定期上報共通資料)

      // 上報事件（當資料變化並成功寫入 PLC 後觸發）
      public event Action<CommonReportStatus1> CommonReportStatus1Updated;
      public event Action<CommonReportStatus2> CommonReportStatus2Updated;
      public event Action<CommonReportAlarm> CommonReportAlarmUpdated;

      /// <summary>
      /// 重置定期上報的快取狀態，強制下一次更新時寫入 PLC
      /// </summary>
      public void ResetCommonReportCache()
      {
         lock (_commonReportLock)
         {
            _lastCommonReportStatus1 = null;
            _lastCommonReportStatus2 = null;
            _lastCommonReportAlarm = null;
         }
      }

      /// <summary>
      /// 更新定期上報共通資料 - Status1（機台狀態資料）
      /// 主程式定時呼叫此方法，內部會比對資料變化，只在變化時才寫入 PLC
      /// </summary>
      /// <param name="alarmStatus">警報狀態 (1=重大/2=輕警報/3=預報/4=無警報)</param>
      /// <param name="machineStatus">機台狀態 (1=初始化/2=準備/3=準備完成/4=生產/5=停機/6=停止)</param>
      /// <param name="actionStatus">機台動作狀態 (1=原點復歸/2=基板搬送/99=其他)</param>
      /// <param name="waitingStatus">基板等待狀態 (1=無等待/2=下游/3=上游/4=上下游/5=特殊/99=其他)</param>
      /// <param name="controlStatus">設備控制狀態 (1=自動/2=手動/3=條件設定/4=準備調整/5=品種切換/99=其他)</param>
      public void UpdateCommonReportStatus1(ushort alarmStatus, ushort machineStatus, ushort actionStatus, ushort waitingStatus, ushort controlStatus)
      {
         lock (_commonReportLock)
         {
            var newData = new CommonReportStatus1
            {
               AlarmStatus = alarmStatus,
               MachineStatus = machineStatus,
               ActionStatus = actionStatus,
               WaitingStatus = waitingStatus,
               ControlStatus = controlStatus
            };

            // 只在資料變化時才寫入 PLC
            if (newData.Equals(_lastCommonReportStatus1))
            {
               return; // 資料無變化，不執行寫入
            }

            try
            {
               // 準備寫入資料（5個 UINT16）
               short[] values = new short[5];
               values[0] = (short)alarmStatus;
               values[1] = (short)machineStatus;
               values[2] = (short)actionStatus;
               values[3] = (short)waitingStatus;
               values[4] = (short)controlStatus;

               // 寫入 PLC (LW1146-LW114A)
               lock (_apiLock)
               {
                  int path = _resolvedPath;
                  int devCode = MapDeviceCode("LW");
                  int startAddr = 0x1146; // LW1146
                  int sizeInBytes = values.Length * 2;

                  int rc = _api.SendEx(path, 0, 0, devCode, startAddr, ref sizeInBytes, values);

                  if (rc == 0)
                  {
                     _lastCommonReportStatus1 = newData;
                     //_logger?.Invoke($"定期上報 Status1 已更新 | Common Report Status1 updated ({newData})");

                     // 觸發事件
                     CommonReportStatus1Updated?.Invoke(newData);
                  }
                  else
                  {
                     _logger?.Invoke($"定期上報 Status1 寫入失敗 | Failed to write Common Report Status1 (RC: {rc})");
                  }
               }
            }
            catch (Exception ex)
            {
               _logger?.Invoke($"定期上報 Status1 發生例外 | Exception in UpdateCommonReportStatus1 (Error: {ex.Message})");
               TryRaiseException(ex);
            }
         }
      }

      /// <summary>
      /// 更新定期上報共通資料 - Status2（詳細狀態資料）
      /// 主程式定時呼叫此方法，內部會比對資料變化，只在變化時才寫入 PLC
      /// </summary>
      public void UpdateCommonReportStatus2(
         ushort redLightStatus, ushort yellowLightStatus, ushort greenLightStatus,
         ushort upstreamWaitingStatus, ushort downstreamWaitingStatus,
         ushort dischargeRate, ushort stopTime,
         uint processingCounter,
         ushort retainedBoardCount, ushort currentRecipeNo,
         ushort boardThicknessStatus, ushort uldFlag,
         string currentRecipeName)
      {
         lock (_commonReportLock)
         {
            var newData = new CommonReportStatus2
            {
               RedLightStatus = redLightStatus,
               YellowLightStatus = yellowLightStatus,
               GreenLightStatus = greenLightStatus,
               UpstreamWaitingStatus = upstreamWaitingStatus,
               DownstreamWaitingStatus = downstreamWaitingStatus,
               DischargeRate = dischargeRate,
               StopTime = stopTime,
               ProcessingCounter = processingCounter,
               RetainedBoardCount = retainedBoardCount,
               CurrentRecipeNo = currentRecipeNo,
               BoardThicknessStatus = boardThicknessStatus,
               UldFlag = uldFlag,
               CurrentRecipeName = currentRecipeName ?? string.Empty
            };

            // 只在資料變化時才寫入 PLC
            if (newData.Equals(_lastCommonReportStatus2))
            {
               return; // 資料無變化，不執行寫入
            }

            try
            {
               // 準備寫入資料（13個 UINT16 + 50個 UINT16 配方名稱）
               short[] values = new short[63];
               values[0] = (short)redLightStatus;
               values[1] = (short)yellowLightStatus;
               values[2] = (short)greenLightStatus;
               values[3] = (short)upstreamWaitingStatus;
               values[4] = (short)downstreamWaitingStatus;
               values[5] = (short)dischargeRate;
               values[6] = (short)stopTime;
               values[7] = (short)(processingCounter & 0xFFFF);         // 低位
               values[8] = (short)((processingCounter >> 16) & 0xFFFF); // 高位
               values[9] = (short)retainedBoardCount;
               values[10] = (short)currentRecipeNo;
               values[11] = (short)boardThicknessStatus;
               values[12] = (short)uldFlag;

               // 轉換配方名稱為 ASCII bytes（最多100字元）
               byte[] nameBytes = new byte[100];
               if (!string.IsNullOrEmpty(currentRecipeName))
               {
                  byte[] sourceBytes = System.Text.Encoding.ASCII.GetBytes(currentRecipeName);
                  int copyLength = Math.Min(sourceBytes.Length, 99); // 保留最後一個 byte 為 0x00
                  Array.Copy(sourceBytes, nameBytes, copyLength);
               }

               // 將 bytes 轉換為 shorts（100 bytes = 50 shorts）
               for (int i = 0; i < 50; i++)
               {
                  values[13 + i] = (short)((nameBytes[i * 2 + 1] << 8) | nameBytes[i * 2]);
               }

               // 寫入 PLC (LW114B-LW1189)
               lock (_apiLock)
               {
                  int path = _resolvedPath;
                  int devCode = MapDeviceCode("LW");
                  int startAddr = 0x114B; // LW114B
                  int sizeInBytes = values.Length * 2;

                  int rc = _api.SendEx(path, 0, 0, devCode, startAddr, ref sizeInBytes, values);

                  if (rc == 0)
                  {
                     _lastCommonReportStatus2 = newData;
                     _logger?.Invoke($"定期上報 Status2 已更新 | Common Report Status2 updated ({newData})");

                     // 觸發事件
                     CommonReportStatus2Updated?.Invoke(newData);
                  }
                  else
                  {
                     _logger?.Invoke($"定期上報 Status2 寫入失敗 | Failed to write Common Report Status2 (RC: {rc})");
                  }
               }
            }
            catch (Exception ex)
            {
               _logger?.Invoke($"定期上報 Status2 發生例外 | Exception in UpdateCommonReportStatus2 (Error: {ex.Message})");
               TryRaiseException(ex);
            }
         }
      }

      /// <summary>
      /// 更新定期上報共通資料 - Alarm（警報資料）
      /// 主程式定時呼叫此方法，內部會比對資料變化，只在變化時才寫入 PLC
      /// </summary>
      /// <param name="errorCodes">錯誤代碼陣列（12個 UINT16）</param>
      public void UpdateCommonReportAlarm(ushort[] errorCodes)
      {
         if (errorCodes == null || errorCodes.Length != 12)
         {
            _logger?.Invoke($"定期上報 Alarm 參數錯誤 | Invalid parameter for UpdateCommonReportAlarm (Expected 12 error codes, Got: {errorCodes?.Length ?? 0})");
            return;
         }

         lock (_commonReportLock)
         {
            var newData = new CommonReportAlarm
            {
               ErrorCodes = (ushort[])errorCodes.Clone() // 複製陣列避免外部修改
            };

            // 只在資料變化時才寫入 PLC
            if (newData.Equals(_lastCommonReportAlarm))
            {
               return; // 資料無變化，不執行寫入
            }

            try
            {
               // 準備寫入資料（12個 UINT16）
               short[] values = new short[12];
               for (int i = 0; i < 12; i++)
               {
                  values[i] = (short)errorCodes[i];
               }

               // 寫入 PLC (LW113A-LW1145)
               lock (_apiLock)
               {
                  int path = _resolvedPath;
                  int devCode = MapDeviceCode("LW");
                  int startAddr = 0x113A; // LW113A
                  int sizeInBytes = values.Length * 2;

                  int rc = _api.SendEx(path, 0, 0, devCode, startAddr, ref sizeInBytes, values);

                  if (rc == 0)
                  {
                     _lastCommonReportAlarm = newData;
                     _logger?.Invoke($"定期上報 Alarm 已更新 | Common Report Alarm updated ({newData})");

                     // 觸發事件
                     CommonReportAlarmUpdated?.Invoke(newData);
                  }
                  else
                  {
                     _logger?.Invoke($"定期上報 Alarm 寫入失敗 | Failed to write Common Report Alarm (RC: {rc})");
                  }
               }
            }
            catch (Exception ex)
            {
               _logger?.Invoke($"定期上報 Alarm 發生例外 | Exception in UpdateCommonReportAlarm (Error: {ex.Message})");
               TryRaiseException(ex);
            }
         }
      }

      #endregion
   }
}