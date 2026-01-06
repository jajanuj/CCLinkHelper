using System;
using System.Threading;
using System.Threading.Tasks;
using WindowsFormsApp1.CCLink.Interfaces;
using WindowsFormsApp1.CCLink.Models;

namespace WindowsFormsApp1.CCLink.Services
{
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
               _api.DevSetEx(_path, 0, 0, devCode, _requestAddr.Start);
            }
            else
            {
               _api.DevRstEx(_path, 0, 0, devCode, _requestAddr.Start);
            }

            _logger?.Invoke($"模擬 PLC：設定 Request {_requestAddr.Kind} 0x{_requestAddr.Start:X} = {(on ? 1 : 0)}");
            RequestChanged?.Invoke(on);
         }
         catch (Exception ex)
         {
            _logger?.Invoke($"模擬 PLC 設定 Request 發生例外: {ex.Message}");
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

         Stop();

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
                  _logger?.Invoke($"模擬 PLC：初始化中，清除殘留的 request");
                  SetRequest(false);
                  await Task.Delay(100, ct).ConfigureAwait(false);
               }

               _logger?.Invoke($"模擬 PLC：初始化完成，開始心跳週期");
            }
            catch (Exception ex)
            {
               _logger?.Invoke($"模擬 PLC：初始化時發生例外: {ex.Message}");
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
                     _logger?.Invoke($"模擬 PLC：⚠️ request 仍為 ON，等待清除");
                     await Task.Delay(100, ct).ConfigureAwait(false);
                     continue;
                  }

                  // 步驟 2: 設定 request ON (模擬 PLC 發出心跳請求)
                  SetRequest(true);

                  // 步驟 3: 維持 request ON 一段時間
                  await Task.Delay(pulseMs, ct).ConfigureAwait(false);

                  // 步驟 4: 清除 request (模擬 PLC 清除請求)
                  SetRequest(false);

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
                     _logger?.Invoke($"模擬 PLC：⚠️ 週期時間過長，實際 {elapsed.TotalMilliseconds:F0}ms，設定 {period.TotalMilliseconds:F0}ms");
                  }
               }
               catch (TaskCanceledException)
               {
                  break;
               }
               catch (Exception ex)
               {
                  _logger?.Invoke($"模擬 PLC pulse 執行例外: {ex.Message}");
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

            _logger?.Invoke($"模擬 PLC：心跳循環已停止");
         }, ct);
      }

      /// <summary>
      /// 停止任何模擬行為與監控。
      /// </summary>
      public void Stop()
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
                  _logger?.Invoke($"模擬 PLC Stop 取消令牌時發生例外: {ex.Message}");
               }

               try
               {
                  _task?.Wait(200);
               }
               catch (Exception ex)
               {
                  _logger?.Invoke($"等待模擬 PLC 工作緒停止時發生例外: {ex.Message}");
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
                     _logger?.Invoke($"模擬 PLC 監控到 Response {_responseAddr.Kind} 0x{_responseAddr.Start:X} = {(on ? 1 : 0)}");
                     ResponseChanged?.Invoke(on);
                  }
               }
               catch (Exception ex)
               {
                  _logger?.Invoke($"模擬 PLC 監控 Response 時發生例外: {ex.Message}");
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
            _logger?.Invoke($"停止模擬 PLC 監控時發生例外: {ex.Message}");
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