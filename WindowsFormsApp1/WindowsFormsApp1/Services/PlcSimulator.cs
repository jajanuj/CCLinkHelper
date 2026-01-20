using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WindowsFormsApp1.CCLink.Interfaces;
using WindowsFormsApp1.CCLink.Models;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.Services
{
   /// <summary>
   /// 測試模式
   /// </summary>
   public enum TestMode
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

      private readonly ICCLinkController _controller;
      private readonly Action<string> _logger;
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

      public PlcSimulator(ICCLinkController controller, LinkDeviceAddress requestAddr, LinkDeviceAddress responseAddr, Action<string> logger = null)
      {
         _controller = controller ?? throw new ArgumentNullException(nameof(controller));
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
      /// 測試模式
      /// </summary>
      public TestMode TestMode { get; set; } = TestMode.Normal;

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
      public void SetRequest(LinkDeviceAddress address, bool on)
      {
         try
         {
            // 直接設定快取，避免 WriteBitsAsync 的讀-改-寫延遲
            if (_controller is CCLink.Services.MelsecHelper helper)
            {
               helper.SetBitDirect(address.Kind, address.Start, on);
            }
            else
            {
               // 降級到 WriteBitsAsync（適用於非 MelsecHelper 的情況）
               string addr = $"{address.Kind}{address.Start:X4}";
               _controller.WriteBitsAsync(addr, new[] { on }).Wait();
            }

            _logger?.Invoke($"模擬 PLC 設定請求位元 | Simulator setting request flag (Device: {address.Kind} 0x{address.Start:X4}, Value: {(on ? 1 : 0)})");
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
      public void StartPulse(LinkDeviceAddress addrRequest, LinkDeviceAddress addrResponse, TimeSpan period, int pulseMs)
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
               _logger?.Invoke($"模擬 PLC 啟動成功 | Simulator initialized, starting heartbeat cycle");

               int step = 0; // 初始狀態

               while (!ct.IsCancellationRequested)
               {
                  try
                  {
                     switch (step)
                     {
                        case 0: // 檢查 Request 狀態 - 確保初始為 OFF
                           bool requestOn = GetBit(addrRequest);
                           if (requestOn)
                           {
                              _logger?.Invoke($"模擬 PLC 初始化清除 | Simulator initializing, clearing residual request flag");
                              SetRequest(addrRequest, false);
                              await Task.Delay(100, ct).ConfigureAwait(false);
                           }
                           else
                           {
                              step = 10; // 進入準備發送狀態
                           }

                           break;

                        case 10: // 設定 Request ON (模擬 PLC 發出心跳請求)
                           SetRequest(addrRequest, true);
                           step = 20; // 進入 ON 維持期
                           break;

                        case 20: // 維持 Request ON 一段時間 (Pulse Width)
                           await Task.Delay(pulseMs, ct).ConfigureAwait(false);
                           step = 30; // 準備清除
                           break;

                        case 30: // 清除 Request OFF
                           if (TestMode == TestMode.T1Timeout)
                           {
                              _logger?.Invoke($"模擬 PLC T1 測試模式：保持 Request ON 不清除 | Simulator T1 Test Mode: Keeping Request ON");
                              step = 40; // 直接進入等待週期剩餘時間
                              break;
                           }

                           SetRequest(addrRequest, false);
                           step = 40; // 進入 OFF 等待期 (週期剩餘時間)
                           break;

                        case 40: // 等待週期剩餘時間
                           // 計算 OFF 時間 = 總週期 - Pulse 時間
                           // 為簡化，這裡直接使用剩餘時間。
                           // 若需要精確週期控制，可調整 sleep 時間。
                           // 這裡簡單假設 duty cycle，等待 remaining time
                           var waitTime = (int)(period.TotalMilliseconds - pulseMs);
                           if (waitTime > 0)
                           {
                              await Task.Delay(waitTime, ct).ConfigureAwait(false);
                           }

                           step = 10; // 回到發送 Request
                           break;
                     }

                     // 每個 loop 短暫 sleep 避免 CPU 滿載 (視需要)
                     // 由於上述步驟已有 delay，這邊設為極短或移除
                     if (step == 0)
                     {
                        await Task.Delay(50, ct).ConfigureAwait(false);
                     }
                  }
                  catch (TaskCanceledException)
                  {
                     break;
                  }
                  catch (Exception ex)
                  {
                     _logger?.Invoke($"模擬 PLC 脈衝執行例外 | Exception: {ex.Message}");
                     await Task.Delay(500, ct).ConfigureAwait(false);
                     step = 0; // 重置
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
                           if (TestMode == TestMode.T1Timeout)
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
                           if (TestMode == TestMode.T2Timeout)
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

      public void StartRecipeCheckMode(RecipeCheckSettings settings = null)
      {
         var recipeSettings = settings ?? new RecipeCheckSettings();

         lock (_syncLock)
         {
            Stop(); // 確保完全停止

            _cts = new CancellationTokenSource();
            var ct = _cts.Token;

            _task = Task.Run(async () =>
            {
               _logger?.Invoke($"[Recipe Check 模擬器] 已啟動 | Recipe Check Simulator started");

               var random = new Random();

               try
               {
                  // 解析控制用的地址
                  var reqFlagAddr = ParseRecipeAddress(recipeSettings.RequestFlagAddress, 1);
                  var reqDataAddr = ParseRecipeAddress(recipeSettings.RequestDataAddress, 10);
                  var respOkAddr = ParseRecipeAddress(recipeSettings.ResponseOkAddress, 1);
                  var respNgAddr = ParseRecipeAddress(recipeSettings.ResponseNgAddress, 1);
                  var respThickAddr = ParseRecipeAddress(recipeSettings.ResponseThicknessAddress, 1);
                  var respRecipeNoAddr = ParseRecipeAddress(recipeSettings.ResponseRecipeNoAddress, 1);
                  var respRecipeNameAddr = ParseRecipeAddress(recipeSettings.ResponseRecipeNameAddress, 50);

                  while (!ct.IsCancellationRequested)
                  {
                     try
                     {
                        // 1. 監聽 Request Flag
                        bool requestFlag = GetBit(reqFlagAddr);

                        if (requestFlag)
                        {
                           _logger?.Invoke($"[Recipe Check 模擬器] 偵測到 Request (Addr: {reqFlagAddr}) | Detected Recipe Check Request");

                           // 2. 讀取追蹤資料 (10 words)
                           short[] trackingData = await ReadWordsAsync(reqDataAddr.Start, 10);

                           // 3. 檢查批號首字（offset 4 = LW6091）
                           bool autoOk = CheckLotNoPrefix(trackingData[4]);

                           bool isOk;
                           ushort boardThickness;
                           ushort recipeNo;
                           string recipeName;

                           if (autoOk)
                           {
                              // 批號 P/D 自動 OK
                              isOk = true;
                              boardThickness = (ushort)random.Next(100, 500);
                              recipeNo = 9999;
                              recipeName = "AUTO_OK_RECIPE";
                              _logger?.Invoke($"[Recipe Check 模擬器] 批號首字為 P/D，自動判定 OK | Auto OK due to lot prefix");
                           }
                           else
                           {
                              // 隨機 OK/NG
                              isOk = random.Next(0, 2) == 1;
                              boardThickness = (ushort)random.Next(100, 500);
                              recipeNo = (ushort)random.Next(1, 1000);
                              recipeName = $"RECIPE_{recipeNo:D4}";
                              _logger?.Invoke($"[Recipe Check 模擬器] 隨機判定 {(isOk ? "OK" : "NG")} | Random result: {(isOk ? "OK" : "NG")}");
                           }

                           // 4. 寫入 Response Data
                           await WriteWordAsync(respThickAddr.Start, boardThickness);
                           _logger?.Invoke($"[Recipe Check 模擬器] 回應板厚: {boardThickness} (Addr: {respThickAddr})");

                           if (recipeSettings.Mode == RecipeCheckMode.Numeric)
                           {
                              await WriteWordAsync(respRecipeNoAddr.Start, recipeNo);
                              _logger?.Invoke($"[Recipe Check 模擬器] 回應 Recipe No.: {recipeNo} (Addr: {respRecipeNoAddr})");
                           }
                           else
                           {
                              short[] recipeWords = ConvertStringToWords(recipeName, 50);
                              await WriteWordsAsync(respRecipeNameAddr.Start, recipeWords);
                              _logger?.Invoke($"[Recipe Check 模擬器] 回應 Recipe Name: {recipeName} (Addr: {respRecipeNameAddr})");
                           }

                           // 5. 設定 Response Flag
                           if (isOk)
                           {
                              SetRequest(respOkAddr, true); // OK
                              _logger?.Invoke($"[Recipe Check 模擬器] 設定 Response OK Flag (Addr: {respOkAddr})");
                           }
                           else
                           {
                              SetRequest(respNgAddr, true); // NG
                              _logger?.Invoke($"[Recipe Check 模擬器] 設定 Response NG Flag (Addr: {respNgAddr})");
                           }

                           // 6. 等待一段時間後清除 Request Flag
                           await Task.Delay(200, ct).ConfigureAwait(false);
                           SetRequest(reqFlagAddr, false);
                           _logger?.Invoke($"[Recipe Check 模擬器] 已清除 Request Flag (Addr: {reqFlagAddr})");
                        }

                        await Task.Delay(100, ct).ConfigureAwait(false);
                     }
                     catch (TaskCanceledException)
                     {
                        break;
                     }
                     catch (Exception ex)
                     {
                        _logger?.Invoke($"[Recipe Check 模擬器] 循環例外 | Loop Exception: {ex.Message}");
                        await Task.Delay(500, ct).ConfigureAwait(false);
                     }
                  }
               }
               catch (Exception ex)
               {
                  _logger?.Invoke($"[Recipe Check 模擬器] 啟動失敗 | Startup Exception: {ex.Message}");
               }

               _logger?.Invoke($"[Recipe Check 模擬器] 已停止 | Recipe Check Simulator stopped");
            }, ct);
         }
      }

      /// <summary>
      /// 啟動追蹤資料維護監聽模式：監控裝置端的 Request (LB0306) 並自動回應 (LB0107)。
      /// 模擬 LCS 端的行為。
      /// </summary>
      public void StartTrackingMaintenanceListenMode()
      {
         const int RequestFlagAddr = 0x0306;
         const int ResponseFlagAddr = 0x0107;
         const int TrackingDataStart = 0x05BE;
         const int PosNoAddr = 0x05C8;

         lock (_syncLock)
         {
            Stop(); // 確保完全停止

            _cts = new CancellationTokenSource();
            var ct = _cts.Token;

            _task = Task.Run(async () =>
            {
               _logger?.Invoke($"[追蹤資料維護模擬器] 已啟動 (監聽 LB{RequestFlagAddr:X4}) | Tracking Data Maint. Simulator started");

               while (!ct.IsCancellationRequested)
               {
                  try
                  {
                     // 1. 監聽 Request Flag (LB0306)
                     bool requestFlag = GetBit(new LinkDeviceAddress("LB", RequestFlagAddr, 1));

                     if (requestFlag)
                     {
                        _logger?.Invoke($"[追蹤資料維護模擬器] 偵測到 Request (LB{RequestFlagAddr:X4}) | Detected Request");

                        // 2. 讀取追蹤資料 (LW05BE-05C7) 與 Position No (LW05C8)
                        // 資料長度 10 words + 1 word = 讀取 11 words 比較快，或是分開讀
                        // 這裡分開讀取比較清晰
                        short[] trackingRaw = await ReadWordsAsync(TrackingDataStart, 10);
                        short[] posRaw = await ReadWordsAsync(PosNoAddr, 1);
                        ushort posNo = (ushort)posRaw[0];

                        // 解析一下資料方便 Log
                        var trackingData = TrackingData.FromRawData(trackingRaw);
                        _logger?.Invoke(
                           $"[追蹤資料維護模擬器] 收到資料 | Received Data - Board: {trackingData.FormatBoardId()}, Lot: {trackingData.FormatLotNo()}, Pos: {posNo}");

                        // 3. 模擬處理延遲
                        await Task.Delay(500, ct).ConfigureAwait(false);

                        // 4. 設定 Response Flag (LB0107) ON
                        SetResponseWithAddress(ResponseFlagAddr, true);
                        _logger?.Invoke($"[追蹤資料維護模擬器] 設定 Response ON (LB{ResponseFlagAddr:X4})");

                        // 5. 等待 Request Flag OFF
                        while (!ct.IsCancellationRequested)
                        {
                           bool req = GetBit(new LinkDeviceAddress("LB", RequestFlagAddr, 1));
                           if (!req)
                           {
                              _logger?.Invoke($"[追蹤資料維護模擬器] 偵測到 Request OFF");
                              break;
                           }

                           await Task.Delay(100, ct).ConfigureAwait(false);
                        }

                        // 6. 清除 Response Flag OFF
                        await Task.Delay(200, ct).ConfigureAwait(false);
                        SetResponseWithAddress(ResponseFlagAddr, false);
                        _logger?.Invoke($"[追蹤資料維護模擬器] 清除 Response OFF (LB{ResponseFlagAddr:X4})");
                     }

                     await Task.Delay(100, ct).ConfigureAwait(false);
                  }
                  catch (TaskCanceledException)
                  {
                     break;
                  }
                  catch (Exception ex)
                  {
                     _logger?.Invoke($"[追蹤資料維護模擬器] 例外 | Exception: {ex.Message}");
                     await Task.Delay(1000, ct).ConfigureAwait(false); // 發生錯誤時稍作等待
                  }
               }

               _logger?.Invoke($"[追蹤資料維護模擬器] 已停止 | Stopped");
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
            // 直接設定快取，避免 WriteBitsAsync 的讀-改-寫延遲
            if (_controller is CCLink.Services.MelsecHelper helper)
            {
               helper.SetBitDirect(_responseAddr.Kind, _responseAddr.Start, on);
            }
            else
            {
               string addr = $"{_responseAddr.Kind}{_responseAddr.Start:X4}";
               _controller.WriteBitsAsync(addr, new[] { on }).Wait();
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

      /// <summary>
      /// 模擬 LCS 發送 "追蹤資料維護" 請求。
      /// 流程：寫入 Data/Position -> Request ON -> Wait Response -> Request OFF
      /// </summary>
      /// <param name="data">Tracking Data (10 words)</param>
      /// <param name="positionNo">Position No</param>
      /// <returns>True if Response OK (LB0304), False if Response NG (LB0305) or Timeout</returns>
      public async Task<bool> TriggerTrackingMaintenanceRequest(TrackingData data, int positionNo)
      {
         const int TIMEOUT_MS = 5000;
         const int REQUEST_FLAG = 0x0106;
         const int REQUEST_DATA_START = 0x05BE;
         const int REQUEST_POS_ADDR = 0x05C8;
         const int RESPONSE_OK = 0x0304;
         const int RESPONSE_NG = 0x0305;

         try
         {
            _logger?.Invoke($"[模擬 LCS] 開始發送維護請求 Position: {positionNo} | Starting Maintenance Request");

            // 1. 寫入 Request Data (Tracking Data 10 Words + Position No 1 Word)
            short[] trackWords = data.ToRawData();
            await WriteWordsAsync(REQUEST_DATA_START, trackWords).ConfigureAwait(false);
            await WriteWordAsync(REQUEST_POS_ADDR, (ushort)positionNo).ConfigureAwait(false);

            // 2. 寫入 Request Flag (LB 0x0106) ON
            SetRequest(new LinkDeviceAddress("LB", REQUEST_FLAG, 1), true);
            var disOk = GetBit(new LinkDeviceAddress("LB", REQUEST_FLAG, 1));
            // 3. 等待回應 (Response OK or NG)
            var cts = new CancellationTokenSource(TIMEOUT_MS);
            bool isOk = false;
            bool isNg = false;

            try
            {
               await Task.Run(async () =>
               {
                  while (!cts.IsCancellationRequested)
                  {
                     isOk = GetBit(new LinkDeviceAddress("LB", RESPONSE_OK, 1));
                     isNg = GetBit(new LinkDeviceAddress("LB", RESPONSE_NG, 1));

                     if (isOk || isNg)
                     {
                        break;
                     }

                     await Task.Delay(50, cts.Token).ConfigureAwait(false);
                  }
               }, cts.Token);
            }
            catch (TaskCanceledException)
            {
               _logger?.Invoke($"[模擬 LCS] 等待回應逾時 | Wait for response timeout");
               SetRequest(new LinkDeviceAddress("LB", REQUEST_FLAG, 1), false);
               return false;
            }

            if (TestMode == TestMode.T1Timeout)
            {
               _logger?.Invoke($"[模擬 LCS] 測試模式 T1Timeout 啟用 | Test Mode T1Timeout Enabled");
               SetRequest(new LinkDeviceAddress("LB", REQUEST_FLAG, 1), false);
               return false;
            }

            // 4. 清除 Request Flag OFF
            SetRequest(new LinkDeviceAddress("LB", REQUEST_FLAG, 1), false);
            _logger?.Invoke($"[模擬 LCS] 收到回應: {(isOk ? "OK" : "NG")}，已清除 Request");

            return isOk;
         }
         catch (Exception ex)
         {
            _logger?.Invoke($"[模擬 LCS] 發生例外 | Exception: {ex.Message}");
            SetRequest(new LinkDeviceAddress("LB", REQUEST_FLAG, 1), false);
            return false;
         }
      }

      #endregion

      #region Private Methods

      /// <summary>
      /// 解析 Recipe Check 使用的地址字串。
      /// 特別處理 LW 為十進位，其他則依據 LinkDeviceAddress.Parse 的邏輯（優先十六進位）。
      /// </summary>
      private LinkDeviceAddress ParseRecipeAddress(string address, int length)
      {
         if (string.IsNullOrEmpty(address) || address.Length < 3)
         {
            return LinkDeviceAddress.Parse(address, length);
         }

         string prefix = address.Substring(0, 2).ToUpperInvariant();
         string numPart = address.Substring(2);

         if (prefix == "LW")
         {
            if (int.TryParse(numPart, out int decValue))
            {
               return new LinkDeviceAddress("LW", decValue, length);
            }
         }

         return LinkDeviceAddress.Parse(address, length);
      }

      /// <summary>
      /// 輔助方法：指定位址設定 Response (因為 SetResponse 唯讀 _responseAddr)
      /// </summary>
      private void SetResponseWithAddress(int address, bool on)
      {
         try
         {
            // 直接設定快取
            if (_controller is CCLink.Services.MelsecHelper helper)
            {
               helper.SetBitDirect("LB", address, on);
            }
            else
            {
               string addr = $"LB{address:X4}";
               _controller.WriteBitsAsync(addr, new[] { on }).Wait();
            }
         }
         catch (Exception ex)
         {
            _logger?.Invoke($"[模擬器] 設定 Response (LB{address:X4}) 失敗: {ex.Message}");
         }
      }

      /// <summary>
      /// 檢查批號首字是否為 P 或 D
      /// </summary>
      private bool CheckLotNoPrefix(short lotTextPart)
      {
         try
         {
            // 將 UINT16 轉換為 2 個 ASCII 字元
            byte[] bytes = BitConverter.GetBytes(lotTextPart);
            char firstChar = (char)bytes[1]; // 高位元組

            return firstChar == 'P' || firstChar == 'D' ||
                   firstChar == 'p' || firstChar == 'd';
         }
         catch
         {
            return false;
         }
      }

      /// <summary>
      /// 讀取多個 Word
      /// </summary>
      private async Task<short[]> ReadWordsAsync(int startAddress, int count)
      {
         string addr = $"LW{startAddress:X4}";
         var result = await _controller.ReadWordsAsync(addr, count);
         return result.ToArray();
      }

      /// <summary>
      /// 寫入單一 Word
      /// </summary>
      private async Task WriteWordAsync(int address, ushort value)
      {
         string addr = $"LW{address:X4}";
         await _controller.WriteWordsAsync(addr, new short[] { (short)value });
      }

      /// <summary>
      /// 寫入多個 Word
      /// </summary>
      private async Task WriteWordsAsync(int address, short[] values)
      {
         string addr = $"LW{address:X4}";
         await _controller.WriteWordsAsync(addr, values);
      }

      /// <summary>
      /// 將字串轉換為 PLC Word 陣列（ASCII）
      /// </summary>
      private short[] ConvertStringToWords(string text, int wordCount)
      {
         short[] words = new short[wordCount];
         if (string.IsNullOrEmpty(text))
         {
            return words;
         }

         byte[] bytes = System.Text.Encoding.ASCII.GetBytes(text);

         for (int i = 0; i < wordCount && i * 2 < bytes.Length; i++)
         {
            byte high = i * 2 + 1 < bytes.Length ? bytes[i * 2 + 1] : (byte)0;
            byte low = bytes[i * 2];
            words[i] = (short)((high << 8) | low);
         }

         return words;
      }

      /// <summary>
      /// 讀取指定位址的 bit 狀態
      /// </summary>
      private bool GetBit(LinkDeviceAddress addr)
      {
         try
         {
            string address = $"{addr.Kind}{addr.Start:X4}";
            return _controller.GetBit(address);
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
                  string addr = $"{_responseAddr.Kind}{_responseAddr.Start:X4}";
                  bool on = _controller.GetBit(addr);
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