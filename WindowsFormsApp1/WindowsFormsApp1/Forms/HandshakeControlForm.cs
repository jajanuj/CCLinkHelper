using System;
using System.Drawing;
using System.Windows.Forms;
using GRT.SDK.Framework.Forms.DialogForm;
using WindowsFormsApp1.Models;
using WindowsFormsApp1.Services;

namespace WindowsFormsApp1.Forms
{
   /// <summary>
   /// 滑台與機械手臂交握監控介面
   /// </summary>
   public partial class HandshakeControlForm : Form
   {
      #region Fields

      private readonly HandshakeAddresses _addresses;

      private readonly SliderRobotHandshakeService _handshakeService;
      private Timer _uiUpdateTimer;

      #endregion

      #region Constructors

      /// <summary>
      /// 建構子
      /// </summary>
      /// <param name="handshakeService">交握服務</param>
      public HandshakeControlForm(SliderRobotHandshakeService handshakeService)
      {
         _handshakeService = handshakeService ?? throw new ArgumentNullException(nameof(handshakeService));
         _addresses = handshakeService.Settings.Addresses;

         InitializeComponent();

         // 訂閱事件
         _handshakeService.StateChanged += OnStateChanged;
         _handshakeService.ErrorOccurred += OnErrorOccurred;

         // 建立UI更新計時器
         _uiUpdateTimer = new Timer();
         _uiUpdateTimer.Interval = 200; // 200ms更新一次
         _uiUpdateTimer.Tick += UiUpdateTimer_Tick;
         _uiUpdateTimer.Start();

         // 初始化UI
         InitializeUI();
      }

      #endregion

      #region Private Methods

      /// <summary>
      /// 初始化UI
      /// </summary>
      private void InitializeUI()
      {
         // 設定初始狀態
         UpdateUI();
      }

      /// <summary>
      /// UI更新計時器 Tick事件
      /// </summary>
      private void UiUpdateTimer_Tick(object sender, EventArgs e)
      {
         if (InvokeRequired)
         {
            Invoke(new Action(UpdateUI));
         }
         else
         {
            UpdateUI();
         }
      }

      /// <summary>
      /// 更新UI顯示
      /// </summary>
      private void UpdateUI()
      {
         // 更新STB信號
         lblStbIsRunning.Text = _handshakeService.StbSignals.IsRunning ? "ON" : "OFF";
         lblStbIsRunning.BackColor = _handshakeService.StbSignals.IsRunning ? Color.LightGreen : Color.LightGray;

         lblStbIsStopped.Text = _handshakeService.StbSignals.IsStopped ? "ON" : "OFF";
         lblStbIsStopped.BackColor = _handshakeService.StbSignals.IsStopped ? Color.LightCoral : Color.LightGray;

         lblStbDischargeNotice.Text = _handshakeService.StbSignals.DischargeNotice ? "ON" : "OFF";
         lblStbDischargeNotice.BackColor = _handshakeService.StbSignals.DischargeNotice ? Color.LightGreen : Color.LightGray;

         lblStbDischargeRequest.Text = _handshakeService.StbSignals.DischargeRequest ? "ON" : "OFF";
         lblStbDischargeRequest.BackColor = _handshakeService.StbSignals.DischargeRequest ? Color.LightGreen : Color.LightGray;

         lblStbBoardReceiveComplete.Text = _handshakeService.StbSignals.BoardReceiveComplete ? "ON" : "OFF";
         lblStbBoardReceiveComplete.BackColor = _handshakeService.StbSignals.BoardReceiveComplete ? Color.LightGreen : Color.LightGray;

         lblStbLastFlag.Text = _handshakeService.StbSignals.LastFlag ? "ON" : "OFF";
         lblStbLastFlag.BackColor = _handshakeService.StbSignals.LastFlag ? Color.Yellow : Color.LightGray;

         txtStbBoardPosition.Text = _handshakeService.StbSignals.BoardReceivePosition.ToString();

         // 更新RB信號
         lblRbIsRunning.Text = _handshakeService.RbSignals.IsRunning ? "ON" : "OFF";
         lblRbIsRunning.BackColor = _handshakeService.RbSignals.IsRunning ? Color.LightGreen : Color.LightGray;

         lblRbIsStopped.Text = _handshakeService.RbSignals.IsStopped ? "ON" : "OFF";
         lblRbIsStopped.BackColor = _handshakeService.RbSignals.IsStopped ? Color.LightCoral : Color.LightGray;

         lblRbReadyToReceiveNotice1.Text = _handshakeService.RbSignals.ReadyToReceiveNotice1 ? "ON" : "OFF";
         lblRbReadyToReceiveNotice1.BackColor = _handshakeService.RbSignals.ReadyToReceiveNotice1 ? Color.LightGreen : Color.LightGray;

         lblRbDischargeRequest.Text = _handshakeService.RbSignals.DischargeRequest ? "ON" : "OFF";
         lblRbDischargeRequest.BackColor = _handshakeService.RbSignals.DischargeRequest ? Color.LightGreen : Color.LightGray;

         lblRbBoardReceiveComplete1.Text = _handshakeService.RbSignals.BoardReceiveComplete1 ? "ON" : "OFF";
         lblRbBoardReceiveComplete1.BackColor = _handshakeService.RbSignals.BoardReceiveComplete1 ? Color.LightGreen : Color.LightGray;

         lblRbCannotReceive.Text = _handshakeService.RbSignals.CannotReceive ? "ON" : "OFF";
         lblRbCannotReceive.BackColor = _handshakeService.RbSignals.CannotReceive ? Color.LightCoral : Color.LightGray;

         // 更新當前狀態
         txtCurrentState.Text = _handshakeService.CurrentState.GetDescription();
         txtCurrentState.BackColor = _handshakeService.CurrentState.IsErrorState() ? Color.LightCoral : Color.LightGreen;

         // 更新按鈕狀態
         btnStart.Enabled = !_handshakeService.IsRunning;
         btnStop.Enabled = _handshakeService.IsRunning;
      }

      /// <summary>
      /// 狀態變更事件處理
      /// </summary>
      private void OnStateChanged(object sender, HandshakeState newState)
      {
         if (InvokeRequired)
         {
            Invoke(new Action<object, HandshakeState>(OnStateChanged), sender, newState);
            return;
         }

         string message = $"[{DateTime.Now:HH:mm:ss}] 狀態變更: {newState.GetDescription()}";
         AppendLog(message, newState.IsErrorState() ? Color.Red : Color.Blue);
      }

      /// <summary>
      /// 錯誤發生事件處理
      /// </summary>
      private void OnErrorOccurred(object sender, string errorMessage)
      {
         if (InvokeRequired)
         {
            Invoke(new Action<object, string>(OnErrorOccurred), sender, errorMessage);
            return;
         }

         string message = $"[{DateTime.Now:HH:mm:ss}] 錯誤: {errorMessage}";
         AppendLog(message, Color.Red);
      }

      /// <summary>
      /// 新增日誌訊息
      /// </summary>
      private void AppendLog(string message, Color color)
      {
         rtbLog.SelectionStart = rtbLog.TextLength;
         rtbLog.SelectionLength = 0;
         rtbLog.SelectionColor = color;
         rtbLog.AppendText(message + Environment.NewLine);
         rtbLog.SelectionColor = rtbLog.ForeColor;
         rtbLog.ScrollToCaret();

         // 限制日誌行數
         if (rtbLog.Lines.Length > 1000)
         {
            string[] lines = rtbLog.Lines;
            rtbLog.Lines = new string[lines.Length - 500];
            Array.Copy(lines, 500, rtbLog.Lines, 0, lines.Length - 500);
         }
      }

      #endregion

      #region Event Handlers

      /// <summary>
      /// 啟動按鈕點擊事件
      /// </summary>
      private void btnStart_Click(object sender, EventArgs e)
      {
         try
         {
            _handshakeService.Start();
            AppendLog($"[{DateTime.Now:HH:mm:ss}] 交握服務已啟動", Color.Green);
         }
         catch (Exception ex)
         {
            MessageBox.Show($"啟動失敗: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      /// <summary>
      /// 停止按鈕點擊事件
      /// </summary>
      private void btnStop_Click(object sender, EventArgs e)
      {
         try
         {
            _handshakeService.Stop();
            AppendLog($"[{DateTime.Now:HH:mm:ss}] 交握服務已停止", Color.Orange);
         }
         catch (Exception ex)
         {
            MessageBox.Show($"停止失敗: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      /// <summary>
      /// 復位按鈕點擊事件
      /// </summary>
      private async void btnReset_Click(object sender, EventArgs e)
      {
         try
         {
            await _handshakeService.ManualResetAsync();
            AppendLog($"[{DateTime.Now:HH:mm:ss}] 手動復位完成", Color.Blue);
         }
         catch (Exception ex)
         {
            MessageBox.Show($"復位失敗: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      /// <summary>
      /// 清除日誌按鈕點擊事件
      /// </summary>
      private void btnClearLog_Click(object sender, EventArgs e)
      {
         rtbLog.Clear();
      }

      /// <summary>
      /// STB信號切換按鈕點擊事件
      /// </summary>
      private async void btnToggleStbSignal_Click(object sender, EventArgs e)
      {
         if (!(sender is Button btn))
         {
            return;
         }

         try
         {
            string address = btn.Tag as string;
            if (string.IsNullOrEmpty(address))
            {
               return;
            }

            // 讀取當前值
            bool currentValue;
            short currentPosition = 0;

            if (address == _addresses.StbBoardReceivePosition)
            {
               // Word類型信號 - 位置
               var values = await _handshakeService.Controller.ReadWordsAsync(address, 1);
               currentPosition = values[0];

               // 彈出對話框讓用戶輸入位置
               string input = "";
               var inputValue = new InputBoxForm();

               if (inputValue.ShowInputBoxForm("請輸入基板受取位置 (0-999):", ref input) != DialogResult.OK)
               {
                  return;
               }

               if (short.TryParse(input, out short newPosition))
               {
                  await _handshakeService.Controller.WriteWordsAsync(address, new[] { newPosition });
                  AppendLog($"[{DateTime.Now:HH:mm:ss}] STB位置設定: {newPosition}", Color.Purple);
               }
               else
               {
                  MessageBox.Show("請輸入有效的數字", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
               }
            }
            else
            {
               // Bit類型信號
               currentValue = await _handshakeService.Controller.ReadBitsAsync(address);

               // 切換到ON（如果是OFF）或OFF（如果是ON）
               bool newValue = !currentValue;
               await _handshakeService.Controller.WriteBitsAsync(address, new[] { newValue });

               string signalName = btn.Text.Replace("切換", "").Trim();
               AppendLog($"[{DateTime.Now:HH:mm:ss}] STB信號 '{signalName}' 切換: {(currentValue ? "ON" : "OFF")} → {(newValue ? "ON" : "OFF")}", Color.Purple);
            }
         }
         catch (Exception ex)
         {
            MessageBox.Show($"設定STB信號失敗: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      /// <summary>
      /// 窗體關閉事件
      /// </summary>
      protected override void OnFormClosing(FormClosingEventArgs e)
      {
         _uiUpdateTimer?.Stop();
         _uiUpdateTimer?.Dispose();

         // 取消訂閱事件
         if (_handshakeService != null)
         {
            _handshakeService.StateChanged -= OnStateChanged;
            _handshakeService.ErrorOccurred -= OnErrorOccurred;
         }

         base.OnFormClosing(e);
      }

      #endregion
   }
}