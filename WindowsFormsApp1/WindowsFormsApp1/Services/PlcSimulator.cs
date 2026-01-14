using System;
using System.Threading;
using System.Threading.Tasks;
using WindowsFormsApp1.CCLink.Interfaces;
using WindowsFormsApp1.CCLink.Models;

namespace WindowsFormsApp1.Services
{
   /// <summary>
   /// 連結報告測試模式
   /// </summary>
   public enum LinkReportTestMode
   {
      /// <summary>正常模式：完整握手流程</summary>
      Normal = 0,

      /// <summary>T1 逾時測試：收到 Request ON 後不回覆 Response</summary>
      T1Timeout = 1,

      /// <summary>T2 逾時測試：回覆 Response ON 後不清除</summary>
      T2Timeout = 2
   }

   /// <summary>
   /// 簡易 PLC 模擬器：可定期或手動在指定 request flag 上產生 pulse/toggle，並監控 response flag 的變化。
   /// 此模擬器直接使用 IMelsecApiAdapter 寫入/讀取位元（LB/LW 等），與 MelsecHelper 可一起搭配測試心跳流程。
   /// </summary>
   public sealed class PlcSimulator : IDisposable
   {
      #region Fields

      private readonly IMelsecApiAdapter _api;
      private readonly Action<string> _logger;
      private readonly int _path;
      private readonly LinkDeviceAddress _requestAddr;
      private readonly LinkDeviceAddress _responseAddr;

      private readonly object _syncLock = new object();

      private CancellationTokenSource _cts;

      private bool _disposed;
      private Task _monitorTask;

      private int _pollMs = 100; // monitor interval for response
      private Task _task;

      #endregion

      #region Constructors

      public PlcSimulator(IMelsecApiAdapter api, int path, LinkDeviceAddress requestAddr, LinkDeviceAddress responseAddr, Action<string> logger = null)
      {
         _api = api ?? throw new ArgumentNullException(nameof(api));
         _path = path;
         _requestAddr = requestAddr ?? throw new ArgumentNullException(nameof(requestAddr));
         _responseAddr = responseAddr ?? throw new ArgumentNullException(nameof(responseAddr));
         _logger = logger;
      }

      #endregion

      #region Delegates, Events

      public event Action<bool> RequestChanged;
      public event Action<bool> ResponseChanged;

      #endregion

      #region Properties

      /// <summary>
      /// 測試模式（可動態切換）
      /// </summary>
      public LinkReportTestMode TestMode { get; set; } = LinkReportTestMode.Normal;

      public int MonitorIntervalMs
      {
         get => _pollMs;
         set
         {
            if (value <= 0)
            {
               throw new ArgumentOutOfRangeException(nameof(value));
            }

            _pollMs = value;
         }
      }

      #endregion

      #region Public Methods

      /// <summary>
      /// 手動設定 request flag（true 設為 1，false 設為 0）。
      /// </summary>
      public void SetRequest(bool on)
      {
         try
         {
            int devCode = MapDeviceCode(_requestAddr.Kind);
            if (on)
            {
               // 使用 (1, 1) 表示位元模式 | Use (1, 1) for bit mode
               _api.DevSetEx(_path, 1, 1, devCode, _requestAddr.Start);
            }
            else
            {
               // 使用 (1, 1) 表示位元模式 | Use (1, 1) for bit mode
               _api.DevRstEx(_path, 1, 1, devCode, _requestAddr.Start);
            }

            _logger?.Invoke($"模擬 PLC 設定請求位元 | Simulator setting request flag (Device: {_requestAddr.Kind} 0x{_requestAddr.Start:X4}, Value: {(on ? 1 : 0)})");
            RequestChanged?.Invoke(on);
         }
         catch (Exception ex)
         {
            _logger?.Invoke($"模擬 PLC 設定請求例外 | Exception while setting simulator request (Error: {ex.Message})");
         }
      }

      /// <summary>
      /// 啟動週期性 pulse 模式：每個週期先將 request 設為 1，維持 pulseMs，然後清為 0，週期為 period。
      /// </summary>
      public void StartPulse(TimeSpan period, int pulseMs)
      {
         if (period.TotalMilliseconds <= 0)
         {
            throw new ArgumentOutOfRangeException(nameof(period));
         }

         if (pulseMs <= 0 || pulseMs > period.TotalMilliseconds)
         {
            throw new ArgumentOutOfRangeException(nameof(pulseMs));
         }

         lock (_syncLock)
         {
            Stop(); // 確保完全停止

            _cts = new CancellationTokenSource();
            var ct = _cts.Token;

            _task = Task.Run(async () =>
            {
               // 初始化：確保 request 為 0
               try
               {
                  bool requestOn = GetBit(_requestAddr);
                  if (requestOn)
                  {
                     _logger?.Invoke($"模擬 PLC 初始化清除 | Simulator initializing, clearing residual request flag");
                     SetRequest(false);
                     await Task.Delay(100, ct).ConfigureAwait(false);
                  }

                  _logger?.Invoke($"模擬 PLC 啟動成功 | Simulator initialized, starting heartbeat cycle");
               }
               catch (Exception ex)
               {
                  _logger?.Invoke($"模擬 PLC 初始化例外 | Exception during simulator initialization (Error: {ex.Message})");
               }

               var count = 0;

               // 主要心跳循環
               //while (!ct.IsCancellationRequested && count <= 2)
               while (!ct.IsCancellationRequested)
               {
                  try
                  {
                     DateTime cycleStart = DateTime.UtcNow;

                     // 步驟 1: 確認 request 為 0（等待上一次的 request 被清除）
                     bool requestOn = GetBit(_requestAddr);
                     if (requestOn)
                     {
                        _logger?.Invoke($"模擬 PLC 等待請求清除 | Simulator request bit still ON, waiting for clear");
                        await Task.Delay(100, ct).ConfigureAwait(false);
                        continue;
                     }

                     // 步驟 2: 設定 request ON (模擬 PLC 發出心跳請求)
                     SetRequest(true);

                     // 步驟 3: 維持 request ON 一段時間
                     await Task.Delay(pulseMs, ct).ConfigureAwait(false);

                     // 步驟 4: 清除 request (模擬 PLC 清除請求)
                     //SetRequest(false);

                     count++;

                     // 步驟 5: 等待剩餘的週期時間
                     TimeSpan elapsed = DateTime.UtcNow - cycleStart;
                     TimeSpan remaining = period - elapsed;
                     if (remaining > TimeSpan.Zero)
                     {
                        await Task.Delay(remaining, ct).ConfigureAwait(false);
                     }
                     else if (remaining < TimeSpan.Zero)
                     {
                        _logger?.Invoke(
                           $"模擬 PLC 週期超時 | Simulator cycle duration exceeded (Actual: {elapsed.TotalMilliseconds:F0}ms, Config: {period.TotalMilliseconds:F0}ms)");
                     }
                  }
                  catch (TaskCanceledException)
                  {
                     break;
                  }
                  catch (Exception ex)
                  {
                     _logger?.Invoke($"模擬 PLC 脈衝執行例外 | Exception in simulator pulse execution (Error: {ex.Message})");
                     // 發生錯誤時等待一下再繼續
                     try
                     {
                        await Task.Delay(500, ct).ConfigureAwait(false);
                     }
                     catch
                     {
                        break;
                     }
                  }
               }

               _logger?.Invoke($"模擬 PLC 心跳已停止 | Simulator heartbeat cycle stopped");
            }, ct);
         }
      }

      /// <summary>
      /// 啟動連結報告模擬模式：監控 EQ 發出的 Request，並自動發送 Response。
      /// 適用於模擬 LCS 端的行為。
      /// </summary>
      public void StartLinkReportMode()
      {
         lock (_syncLock)
         {
            Stop(); // 確保完全停止

            _cts = new CancellationTokenSource();
            var ct = _cts.Token;

            _task = Task.Run(async () =>
            {
               _logger?.Invoke($"模擬 PLC 連結報告模式啟動 | Simulator Link Report mode started");

               bool lastRequestState = false;
               int step = 0;              // 初始狀態
               int responseDelayMs = 100; // 固定延遲

               while (!ct.IsCancellationRequested)
               {
                  try
                  {
                     switch (step)
                     {
                        case 0: // 預備狀態 / 檢查 Request 狀態
                           bool requestOn = GetBit(_requestAddr);
                           if (requestOn && !lastRequestState)
                           {
                              // 偵測到上升沿
                              step = 20;
                           }
                           else if (!requestOn)
                           {
                              step = 0; // 持續監控
                           }

                           // 更新最後狀態以便偵測上升沿
                           lastRequestState = requestOn;
                           break;

                        case 20: // 檢測到 Request 上升沿（OFF -> ON）
                           _logger?.Invoke($"[模擬 PLC] 偵測到 EQ Request ON | Detected EQ Request ON (Addr: {_requestAddr})");

                           // T1 測試模式：不回覆 Response
                           if (TestMode == LinkReportTestMode.T1Timeout)
                           {
                              _logger?.Invoke($"[模擬 PLC] T1 測試模式：不回覆 Response | T1 Test: No Response");
                              // 停留在這一步，不進入下一步，或者可以回到 0 等待下一次（視需求而定，這裡選擇等待直到 Request 消失或其他重置條件）
                              // 為了能夠重置，我們檢查若 Request 變回 OFF 則重置
                              step = 888;
                           }
                           else
                           {
                              step = 30; // 進入延遲回應
                           }

                           break;

                        case 30: // 延遲後發送 Response ON
                           await Task.Delay(responseDelayMs, ct).ConfigureAwait(false);
                           SetResponse(true);
                           _logger?.Invoke($"[模擬 PLC] 已回應 Response ON | Sent Response ON (Addr: {_responseAddr})");

                           step = 40; // 等待 Request OFF
                           break;

                        case 40: // 檢查是否需要維持或清除
                           // T2 測試模式：回覆 Response ON 後不清除
                           if (TestMode == LinkReportTestMode.T2Timeout)
                           {
                              _logger?.Invoke($"[模擬 PLC] T2 測試模式：Response ON 將持續維持 | T2 Test: Response will stay ON");
                              step = 888; // 進入完成/凍結狀態
                           }
                           else
                           {
                              // 正常模式：等待 Request OFF
                              step = 50;
                           }

                           break;

                        case 50: // 等待 EQ Request OFF
                           if (!GetBit(_requestAddr))
                           {
                              _logger?.Invoke($"[模擬 PLC] 偵測到 EQ Request OFF | Detected EQ Request OFF");
                              step = 60; // 準備清除 Response
                           }

                           break;

                        case 60: // 清除 Response OFF
                           // 模擬處理/反應時間
                           await Task.Delay(100, ct).ConfigureAwait(false);
                           SetResponse(false);
                           _logger?.Invoke($"[模擬 PLC] 已清除 Response OFF | Cleared Response OFF");

                           step = 0;                 // 回到初始狀態
                           lastRequestState = false; // 重置狀態
                           break;

                        case 888: // 特殊狀態：等待 Request OFF 重置
                           if (!GetBit(_requestAddr))
                           {
                              lastRequestState = false;
                              step = 0; // 當 Request 消失時重置
                           }

                           break;
                     }

                     await Task.Delay(50, ct).ConfigureAwait(false);
                  }
                  catch (TaskCanceledException)
                  {
                     break;
                  }
                  catch (Exception ex)
                  {
                     _logger?.Invoke($"模擬 PLC 連結報告模式例外 | Exception: {ex.Message}");
                     await Task.Delay(500, ct).ConfigureAwait(false);
                     step = 0; // 發生錯誤重置
                  }
               }

               _logger?.Invoke($"模擬 PLC 連結報告模式已停止 | Simulator Link Report mode stopped");
            }, ct);
         }
      }

      /// <summary>
      /// 手動設定 response flag（true 設為 1，false 設為 0）。
      /// </summary>
      public void SetResponse(bool on)
      {
         try
         {
            int devCode = MapDeviceCode(_responseAddr.Kind);
            if (on)
            {
               _api.DevSetEx(_path, 1, 1, devCode, _responseAddr.Start);
            }
            else
            {
               _api.DevRstEx(_path, 1, 1, devCode, _responseAddr.Start);
            }

            _logger?.Invoke(
               $"模擬 PLC 設定回應位元 | Simulator setting response flag (Device: {_responseAddr.Kind} 0x{_responseAddr.Start:X4}, Value: {(on ? 1 : 0)})");
            ResponseChanged?.Invoke(on);
         }
         catch (Exception ex)
         {
            _logger?.Invoke($"模擬 PLC 設定回應例外 | Exception while setting simulator response (Error: {ex.Message})");
         }
      }

      /// <summary>
      /// 停止任何模擬行為與監控。
      /// </summary>
      public void Stop()
      {
         lock (_syncLock)
         {
            try
            {
               if (_cts != null)
               {
                  try
                  {
                     _cts.Cancel();
                  }
                  catch (Exception ex)
                  {
                     _logger?.Invoke($"模擬 PLC 停止取消例外 | Exception while canceling simulator token (Error: {ex.Message})");
                  }

                  try
                  {
                     // 增加超時時間，確保 Task 完全結束
                     if (_task != null && !_task.Wait(TimeSpan.FromSeconds(5)))
                     {
                        _logger?.Invoke("協助任務關閉超時 | Warning: Task did not end within 5s, forcing wait");
                        _task.Wait(); // 強制等待直到結束
                     }
                  }
                  catch (AggregateException ex)
                  {
                     _logger?.Invoke($"等待模擬 PLC 停止例外 | Exception while waiting for simulator task to stop (Error: {ex.InnerException?.Message ?? ex.Message})");
                  }

                  _cts.Dispose();
                  _cts = null;
               }
            }
            finally
            {
               _task = null;
            }

            StopMonitor();
         }
      }

      #endregion

      #region Private Methods

      /// <summary>
      /// 讀取指定位址的 bit 狀態
      /// </summary>
      private bool GetBit(LinkDeviceAddress addr)
      {
         try
         {
            int devCode = MapDeviceCode(addr.Kind);
            int size = 2;
            var buf = new short[1];
            int rc = _api.ReceiveEx(_path, 0, 0, devCode, addr.Start, ref size, buf);
            return rc == 0 && buf[0] != 0;
         }
         catch
         {
            return false;
         }
      }

      private void StartMonitor()
      {
         if (_monitorTask != null)
         {
            return;
         }

         var cts = new CancellationTokenSource();
         _monitorTask = Task.Run(async () =>
         {
            bool? last = null;
            while (!cts.IsCancellationRequested)
            {
               try
               {
                  var dest = new short[1];
                  int devCode = MapDeviceCode(_responseAddr.Kind);
                  int size = 1 * 2;
                  _api.ReceiveEx(_path, 0, 0, devCode, _responseAddr.Start, ref size, dest);
                  bool on = dest[0] != 0;
                  if (!last.HasValue || last.Value != on)
                  {
                     last = on;
                     _logger?.Invoke(
                        $"模擬 PLC 監控到回應變更 | Simulator detected response change (Device: {_responseAddr.Kind} 0x{_responseAddr.Start:X4}, Value: {(on ? 1 : 0)})");
                     ResponseChanged?.Invoke(on);
                  }
               }
               catch (Exception ex)
               {
                  _logger?.Invoke($"模擬 PLC 監控回應例外 | Exception in simulator response monitoring (Error: {ex.Message})");
               }

               try
               {
                  await Task.Delay(_pollMs, cts.Token).ConfigureAwait(false);
               }
               catch (TaskCanceledException)
               {
                  break;
               }
            }
         }, CancellationToken.None);

         // store cancellation for monitor as well via _cts so Stop stops both
         if (_cts == null)
         {
            _cts = new CancellationTokenSource();
         }
      }

      private void StopMonitor()
      {
         try
         {
            // monitor uses _monitorTask but no external cancellation token; we'll simply let it end when Stop is called
            _monitorTask = null;
         }
         catch (Exception ex)
         {
            _logger?.Invoke($"停止模擬 PLC 監控例外 | Exception while stopping simulator monitor (Error: {ex.Message})");
         }
      }

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

      public void Dispose()
      {
         if (_disposed)
         {
            return;
         }

         _disposed = true;
         Stop();
      }
   }
}