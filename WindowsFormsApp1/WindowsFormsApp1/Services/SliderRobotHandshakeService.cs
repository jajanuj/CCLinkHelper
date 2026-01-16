using System;
using System.Threading;
using System.Threading.Tasks;
using GRT.SDK.Framework.Timer;
using WindowsFormsApp1.CCLink.Interfaces;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.Services
{
   /// <summary>
   /// 滑台與機械手臂交握服務
   /// 負責管理STB與RB之間的自動化交握流程
   /// </summary>
   public class SliderRobotHandshakeService : IDisposable
   {
      #region Fields

      private readonly ICCLinkController _controller;
      private readonly Action<string> _logger;
      private readonly HandshakeSettings _settings;
      private CancellationTokenSource _cts;

      private int _currentRetryCount;

      private HandshakeState _currentState;
      private bool _isRunning;
      private Task _loopTask;
      private HandshakeState _previousState;
      private RobotSignals _rbSignals;
      private DateTime _stateStartTime;

      private SliderSignals _stbSignals;
      private CTimer _timeoutTimer;
      private int _timeoutSeconds;
      private string _timeoutDescription;

      #endregion

      #region Constructors

      /// <summary>
      /// 建構子
      /// </summary>
      /// <param name="controller">PLC控制器</param>
      /// <param name="settings">交握設定</param>
      /// <param name="logger">日誌記錄器</param>
      public SliderRobotHandshakeService(
         ICCLinkController controller,
         HandshakeSettings settings,
         Action<string> logger = null)
      {
         _controller = controller ?? throw new ArgumentNullException(nameof(controller));
         _settings = settings ?? throw new ArgumentNullException(nameof(settings));
         _logger = logger;

         // 驗證設定
         string validationError = _settings.Validate();
         if (validationError != null)
         {
            throw new ArgumentException($"設定驗證失敗: {validationError}", nameof(settings));
         }

         _currentState = HandshakeState.Idle;
         _previousState = HandshakeState.Idle;
         _timeoutTimer = new CTimer();
         _stbSignals = new SliderSignals();
         _rbSignals = new RobotSignals();
         _currentRetryCount = 0;
         _isRunning = false;
      }

      #endregion

      #region Delegates, Events

      /// <summary>
      /// 錯誤發生事件
      /// </summary>
      public event EventHandler<string> ErrorOccurred;

      /// <summary>
      /// 狀態變更事件
      /// </summary>
      public event EventHandler<HandshakeState> StateChanged;

      #endregion

      #region Properties

      /// <summary>
      /// 當前狀態
      /// </summary>
      public HandshakeState CurrentState => _currentState;

      /// <summary>
      /// 是否正在運行
      /// </summary>
      public bool IsRunning => _isRunning;

      /// <summary>
      /// STB信號(唯讀)
      /// </summary>
      public SliderSignals StbSignals => _stbSignals;

      /// <summary>
      /// RB信號(唯讀)
      /// </summary>
      public RobotSignals RbSignals => _rbSignals;

      /// <summary>
      /// 交握設定(唯讀)
      /// </summary>
      public HandshakeSettings Settings => _settings;

      /// <summary>
      /// PLC控制器(唯讀)
      /// </summary>
      public ICCLinkController Controller => _controller;

      #endregion

      #region Public Methods

      /// <summary>
      /// 啟動交握監控
      /// </summary>
      public void Start()
      {
         if (_isRunning)
         {
            Log("交握服務已在運行中");
            return;
         }

         _isRunning = true;
         _currentState = HandshakeState.Idle;
         _currentRetryCount = 0;
         _rbSignals.ResetAll();

         _cts = new CancellationTokenSource();
         _loopTask = Task.Run(() => HandshakeLoopAsync(_cts.Token), _cts.Token);

         Log("交握服務已啟動");
      }

      /// <summary>
      /// 停止交握監控
      /// </summary>
      public void Stop()
      {
         if (!_isRunning)
         {
            return;
         }

         _isRunning = false;
         _cts?.Cancel();

         try
         {
            _loopTask?.Wait(TimeSpan.FromSeconds(5));
         }
         catch (Exception ex)
         {
            Log($"停止交握服務時發生錯誤: {ex.Message}");
         }

         _rbSignals.ResetAll();
         WriteRbSignalsAsync().Wait(TimeSpan.FromSeconds(2));

         Log("交握服務已停止");
      }

      /// <summary>
      /// 手動復位 - 將所有RB輸出信號復位為OFF
      /// </summary>
      public async Task ManualResetAsync()
      {
         Log("執行手動復位...");

         _rbSignals.ResetAll();
         await WriteRbSignalsAsync();

         _currentState = HandshakeState.Idle;
         _currentRetryCount = 0;
         _timeoutTimer.Reset();
         OnStateChanged(_currentState);

         Log("手動復位完成");
      }

      #endregion

      #region Private Methods

      /// <summary>
      /// 主要狀態機循環
      /// </summary>
      private async Task HandshakeLoopAsync(CancellationToken ct)
      {
         Log("交握循環開始");

         while (!ct.IsCancellationRequested && _isRunning)
         {
            try
            {
               // 讀取STB信號
               await ReadStbSignalsAsync();

               // 檢查緊急停止
               if (_stbSignals.EmergencyStop)
               {
                  if (_currentState != HandshakeState.EmergencyStop)
                  {
                     ChangeState(HandshakeState.EmergencyStop);
                     OnError("收到緊急停止信號");
                  }
               }

               // 檢查逾時
               CheckTimeout();

               // 執行狀態機邏輯
               await ExecuteStateMachineAsync();

               // 寫入RB信號
               await WriteRbSignalsAsync();

               // 等待循環週期
               await Task.Delay(_settings.LoopCycleMs, ct);
            }
            catch (OperationCanceledException)
            {
               break;
            }
            catch (Exception ex)
            {
               Log($"交握循環錯誤: {ex.Message}");
               OnError($"交握循環錯誤: {ex.Message}");

               if (_currentState.IsNormalState())
               {
                  ChangeState(HandshakeState.CommunicationError);
               }

               await Task.Delay(1000, ct); // 錯誤後延遲1秒再繼續
            }
         }

         Log("交握循環結束");
      }

      /// <summary>
      /// 檢查逾時
      /// </summary>
      private void CheckTimeout()
      {
         if (_timeoutSeconds > 0 && _timeoutTimer.On(_timeoutSeconds * 1000))
         {
            Log($"逾時: {_timeoutDescription} 超過 {_timeoutSeconds} 秒");
            OnError($"逾時: {_timeoutDescription}");
            _timeoutSeconds = 0; // 停止計時
            ChangeState(HandshakeState.TimeoutError);
         }
      }

      /// <summary>
      /// 執行狀態機邏輯
      /// </summary>
      private async Task ExecuteStateMachineAsync()
      {
         switch (_currentState)
         {
            case HandshakeState.Idle:
               HandleIdle();
               break;

            case HandshakeState.WaitingForStbDischargeRequest:
               HandleWaitingForStbDischargeRequest();
               break;

            case HandshakeState.RbAcknowledgement:
               HandleRbAcknowledgement();
               break;

            case HandshakeState.WaitingForStbDischargeComplete:
               HandleWaitingForStbDischargeComplete();
               break;

            case HandshakeState.RbReceiveConfirmation:
               HandleRbReceiveConfirmation();
               break;

            case HandshakeState.WaitingForStbReset:
               HandleWaitingForStbReset();
               break;

            case HandshakeState.RbReset:
               HandleRbReset();
               break;

            case HandshakeState.Completed:
               HandleCompleted();
               break;

            // 異常狀態處理
            case HandshakeState.TimeoutError:
            case HandshakeState.SignalError:
            case HandshakeState.CommunicationError:
            case HandshakeState.IfError:
               await HandleErrorState();
               break;

            case HandshakeState.EmergencyStop:
               HandleEmergencyStop();
               break;
         }
      }

      // ===== 各狀態處理方法 (每個case保持簡單) =====

      /// <summary>
      /// 處理待機狀態
      /// </summary>
      private void HandleIdle()
      {
         // 復位所有RB輸出信號
         _rbSignals.ResetAll();

         // 切換到等待STB排出要求狀態
         ChangeState(HandshakeState.WaitingForStbDischargeRequest);
         LogVerbose("進入待機狀態，等待STB排出要求");
      }

      /// <summary>
      /// 處理等待STB排出要求狀態
      /// </summary>
      private void HandleWaitingForStbDischargeRequest()
      {
         // 檢查IF中異常: 起動中OFF + 排出要求ON
         if (!_stbSignals.IsRunning && _stbSignals.DischargeRequest)
         {
            ChangeState(HandshakeState.IfError);
            OnError("偵測到IF中異常: STB起動中OFF但排出要求ON");
            return;
         }

         // 檢查STB排出要求信號
         if (_stbSignals.DischargeRequest)
         {
            Log($"收到STB排出要求，位置: {_stbSignals.BoardReceivePosition}");

            if (_stbSignals.LastFlag)
            {
               Log("*** 偵測到LastFlag - 這是批次的最後一片板材 ***");
            }

            ChangeState(HandshakeState.RbAcknowledgement);
            StopTimeoutTimer();
         }
         else
         {
            // 啟動逾時Timer (僅在首次進入此狀態時)
            if (_timeoutSeconds == 0)
            {
               StartTimeoutTimer(_settings.T1TimeoutSeconds, "等待STB排出要求(OFF→ON)");
            }
         }
      }

      /// <summary>
      /// 處理RB確認並回應狀態
      /// </summary>
      private void HandleRbAcknowledgement()
      {
         // 設定RB回應信號
         _rbSignals.IsRunning = true;
         _rbSignals.IsStopped = false;
         _rbSignals.ReadyToReceiveNotice1 = true;
         _rbSignals.DischargeRequest = true;

         Log("RB確認並回應STB排出要求");
         ChangeState(HandshakeState.WaitingForStbDischargeComplete);
      }

      /// <summary>
      /// 處理等待STB排出完了狀態
      /// </summary>
      private void HandleWaitingForStbDischargeComplete()
      {
         // 檢查STB排出完了信號
         if (_stbSignals.BoardReceiveComplete)
         {
            Log("收到STB排出完了信號");
            ChangeState(HandshakeState.RbReceiveConfirmation);
            StopTimeoutTimer();
         }
         else
         {
            // 啟動逾時Timer
            if (_timeoutSeconds == 0)
            {
               StartTimeoutTimer(_settings.GeneralTimeoutSeconds, "等待STB排出完了");
            }
         }
      }

      /// <summary>
      /// 處理RB接收確認狀態
      /// </summary>
      private void HandleRbReceiveConfirmation()
      {
         // 設定RB接收完了信號
         _rbSignals.BoardReceiveComplete1 = true;

         Log("RB確認接收板材完成");
         ChangeState(HandshakeState.WaitingForStbReset);
      }

      /// <summary>
      /// 處理等待STB復位狀態
      /// </summary>
      private void HandleWaitingForStbReset()
      {
         // 檢查STB復位信號（排出要求 OFF）
         if (!_stbSignals.DischargeRequest)
         {
            Log("收到STB復位信號");
            ChangeState(HandshakeState.RbReset);
            StopTimeoutTimer();
         }
         else
         {
            // 啟動逾時Timer
            if (_timeoutSeconds == 0)
            {
               StartTimeoutTimer(_settings.T2TimeoutSeconds, "等待STB排出要求復位(ON→OFF)");
            }
         }
      }

      /// <summary>
      /// 處理RB復位狀態
      /// </summary>
      private void HandleRbReset()
      {
         // 復位RB所有輸出信號
         _rbSignals.DischargeRequest = false;
         _rbSignals.BoardReceiveComplete1 = false;
         _rbSignals.ReadyToReceiveNotice1 = false;

         Log("RB復位完成");
         ChangeState(HandshakeState.Completed);
      }

      /// <summary>
      /// 處理完成狀態
      /// </summary>
      private void HandleCompleted()
      {
         Log("交握流程完成");

         // 回到待機狀態，準備下一次交握
         _currentRetryCount = 0;
         ChangeState(HandshakeState.Idle);
      }

      /// <summary>
      /// 處理異常狀態
      /// </summary>
      private async Task HandleErrorState()
      {
         Log($"處理異常狀態: {_currentState.GetDescription()}");

         // 停止計時器
         StopTimeoutTimer();

         // 復位所有RB輸出信號
         _rbSignals.ResetAll();

         // 根據設定決定是否重試
         if (_settings.EnableAutoRetry && _currentRetryCount < _settings.RetryCount)
         {
            _currentRetryCount++;
            Log($"嘗試自動重試 ({_currentRetryCount}/{_settings.RetryCount})");

            await Task.Delay(1000); // 延遲1秒後重試

            ChangeState(HandshakeState.Idle);
         }
         else
         {
            if (_currentRetryCount >= _settings.RetryCount)
            {
               Log($"已達到最大重試次數({_settings.RetryCount})，等待手動復位");
            }
            else
            {
               Log("自動重試已停用，等待手動復位");
            }

            // 保持在異常狀態，等待手動介入
         }
      }

      /// <summary>
      /// 處理緊急停止狀態
      /// </summary>
      private void HandleEmergencyStop()
      {
         // 停止計時器
         StopTimeoutTimer();

         // 復位所有RB輸出信號
         _rbSignals.ResetAll();

         // 保持在緊急停止狀態，直到緊急停止信號解除
         if (!_stbSignals.EmergencyStop)
         {
            Log("緊急停止信號已解除");
            ChangeState(HandshakeState.Idle);
         }
      }

      // ===== 輔助方法 =====

      /// <summary>
      /// 啟動逾時計時器
      /// </summary>
      private void StartTimeoutTimer(int seconds, string description)
      {
         LogVerbose($"啟動逾時計時器: {description} - {seconds}秒");
         _timeoutTimer.Reset();
         _timeoutSeconds = seconds;
         _timeoutDescription = description;
      }

      /// <summary>
      /// 停止逾時計時器
      /// </summary>
      private void StopTimeoutTimer()
      {
         if (_timeoutSeconds > 0)
         {
            _timeoutSeconds = 0;
            _timeoutTimer.Reset();
            LogVerbose("逾時計時器已停止");
         }
      }

      /// <summary>
      /// 從PLC讀取STB信號
      /// </summary>
      private async Task ReadStbSignalsAsync()
      {
         var addr = _settings.Addresses;

         _stbSignals.IsRunning = await _controller.ReadBitsAsync(addr.StbIsRunning);
         _stbSignals.IsStopped = await _controller.ReadBitsAsync(addr.StbIsStopped);
         _stbSignals.DischargeNotice = await _controller.ReadBitsAsync(addr.StbDischargeNotice);
         _stbSignals.DischargeRequest = await _controller.ReadBitsAsync(addr.StbDischargeRequest);
         
         var posWords = await _controller.ReadWordsAsync(addr.StbBoardReceivePosition, 1);
         _stbSignals.BoardReceivePosition = posWords[0];
         
         _stbSignals.BoardReceiveComplete = await _controller.ReadBitsAsync(addr.StbBoardReceiveComplete);
         _stbSignals.LastFlag = await _controller.ReadBitsAsync(addr.StbLastFlag);
         _stbSignals.EmergencyStop = await _controller.ReadBitsAsync(addr.StbEmergencyStop);
      }

      /// <summary>
      /// 寫入RB信號至PLC
      /// </summary>
      private async Task WriteRbSignalsAsync()
      {
         var addr = _settings.Addresses;

         await _controller.WriteBitsAsync(addr.RbIsRunning, new[] { _rbSignals.IsRunning });
         await _controller.WriteBitsAsync(addr.RbIsStopped, new[] { _rbSignals.IsStopped });
         await _controller.WriteBitsAsync(addr.RbReadyToReceiveNotice1, new[] { _rbSignals.ReadyToReceiveNotice1 });
         await _controller.WriteBitsAsync(addr.RbDischargeRequest, new[] { _rbSignals.DischargeRequest });
         await _controller.WriteBitsAsync(addr.RbBoardReceiveComplete1, new[] { _rbSignals.BoardReceiveComplete1 });
         await _controller.WriteBitsAsync(addr.RbCannotReceive, new[] { _rbSignals.CannotReceive });
         await _controller.WriteBitsAsync(addr.RbManualMode, new[] { _rbSignals.ManualMode });
         await _controller.WriteBitsAsync(addr.RbDataCheckNg, new[] { _rbSignals.DataCheckNG });
         await _controller.WriteBitsAsync(addr.RbDataCheckOk, new[] { _rbSignals.DataCheckOK });
         await _controller.WriteWordsAsync(addr.RbBoardReceivePosition, new[] { _rbSignals.BoardReceivePosition });
      }

      /// <summary>
      /// 變更狀態
      /// </summary>
      private void ChangeState(HandshakeState newState)
      {
         if (_currentState == newState)
         {
            return;
         }

         _previousState = _currentState;
         _currentState = newState;
         _stateStartTime = DateTime.Now;

         Log($"狀態變更: {_previousState.GetDescription()} → {_currentState.GetDescription()}");
         OnStateChanged(newState);
      }

      /// <summary>
      /// 觸發狀態變更事件
      /// </summary>
      private void OnStateChanged(HandshakeState state)
      {
         StateChanged?.Invoke(this, state);
      }

      /// <summary>
      /// 觸發錯誤事件
      /// </summary>
      private void OnError(string message)
      {
         ErrorOccurred?.Invoke(this, message);
      }

      /// <summary>
      /// 記錄日誌
      /// </summary>
      private void Log(string message)
      {
         _logger?.Invoke($"[交握] {message}");
      }

      /// <summary>
      /// 記錄詳細日誌
      /// </summary>
      private void LogVerbose(string message)
      {
         if (_settings.EnableVerboseLogging)
         {
            _logger?.Invoke($"[交握][詳細] {message}");
         }
      }

      #endregion

      /// <summary>
      /// 釋放資源
      /// </summary>
      public void Dispose()
      {
         Stop();
         _cts?.Dispose();
      }
   }
}