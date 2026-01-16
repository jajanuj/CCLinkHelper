namespace WindowsFormsApp1.Models
{
   /// <summary>
   /// 滑台與機械手臂交握流程狀態枚舉
   /// </summary>
   public enum HandshakeState
   {
      /// <summary>
      /// 待機中 - 初始待機狀態，等待新的交握流程開始
      /// </summary>
      Idle = 0,

      /// <summary>
      /// 等待STB排出要求 - 等待滑台發送排出要求信號(OFF→ON)
      /// </summary>
      WaitingForStbDischargeRequest = 1,

      /// <summary>
      /// RB確認並回應 - 機械手臂確認收到排出要求並回應相關信號
      /// </summary>
      RbAcknowledgement = 2,

      /// <summary>
      /// 等待STB排出完了 - 等待滑台發送排出完成信號
      /// </summary>
      WaitingForStbDischargeComplete = 3,

      /// <summary>
      /// RB接收確認 - 機械手臂確認接收板材完成
      /// </summary>
      RbReceiveConfirmation = 4,

      /// <summary>
      /// 等待STB復位 - 等待滑台復位排出要求信號(ON→OFF)
      /// </summary>
      WaitingForStbReset = 5,

      /// <summary>
      /// RB復位 - 機械手臂復位所有輸出信號
      /// </summary>
      RbReset = 6,

      /// <summary>
      /// 完成 - 單次交握流程完成，準備回到待機狀態
      /// </summary>
      Completed = 7,

      // === 異常狀態 ===

      /// <summary>
      /// 逾時錯誤 - 等待STB信號超過設定的逾時時間
      /// </summary>
      TimeoutError = 100,

      /// <summary>
      /// 信號錯誤 - 偵測到衝突或不正確的信號組合
      /// </summary>
      SignalError = 101,

      /// <summary>
      /// 通訊錯誤 - PLC通訊發生錯誤
      /// </summary>
      CommunicationError = 102,

      /// <summary>
      /// 緊急停止 - 收到緊急停止信號，立即中斷所有動作
      /// </summary>
      EmergencyStop = 103,

      /// <summary>
      /// IF中異常 - 滑台在IF中發生異常(起動中OFF + 排出要求ON)
      /// </summary>
      IfError = 104
   }

   /// <summary>
   /// HandshakeState擴展方法
   /// </summary>
   public static class HandshakeStateExtensions
   {
      #region Public Methods

      /// <summary>
      /// 判斷是否為異常狀態
      /// </summary>
      public static bool IsErrorState(this HandshakeState state)
      {
         return (int)state >= 100;
      }

      /// <summary>
      /// 判斷是否為正常流程狀態
      /// </summary>
      public static bool IsNormalState(this HandshakeState state)
      {
         return (int)state < 100;
      }

      /// <summary>
      /// 取得狀態的中文描述
      /// </summary>
      public static string GetDescription(this HandshakeState state)
      {
         switch (state)
         {
            case HandshakeState.Idle:
               return "待機中";
            case HandshakeState.WaitingForStbDischargeRequest:
               return "等待STB排出要求";
            case HandshakeState.RbAcknowledgement:
               return "RB確認並回應";
            case HandshakeState.WaitingForStbDischargeComplete:
               return "等待STB排出完了";
            case HandshakeState.RbReceiveConfirmation:
               return "RB接收確認";
            case HandshakeState.WaitingForStbReset:
               return "等待STB復位";
            case HandshakeState.RbReset:
               return "RB復位中";
            case HandshakeState.Completed:
               return "完成";
            case HandshakeState.TimeoutError:
               return "逾時錯誤";
            case HandshakeState.SignalError:
               return "信號錯誤";
            case HandshakeState.CommunicationError:
               return "通訊錯誤";
            case HandshakeState.EmergencyStop:
               return "緊急停止";
            case HandshakeState.IfError:
               return "IF中異常";
            default:
               return state.ToString();
         }
      }

      #endregion
   }
}