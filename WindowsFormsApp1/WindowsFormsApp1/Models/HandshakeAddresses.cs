namespace WindowsFormsApp1.Models
{
   /// <summary>
   /// 滑台與機械手臂交握流程的PLC地址配置
   /// </summary>
   public class HandshakeAddresses
   {
      #region Constructors

      /// <summary>
      /// 建立預設地址配置
      /// </summary>
      public HandshakeAddresses()
      {
         // 使用屬性初始化完成預設值設定
      }

      #endregion

      #region Properties

      // === STB → RB (輸入地址 - 從滑台讀取) ===

      /// <summary>
      /// STB Pin 1: 起動中
      /// </summary>
      public string StbIsRunning { get; set; } = "LB1000";

      /// <summary>
      /// STB Pin 2: 停止中
      /// </summary>
      public string StbIsStopped { get; set; } = "LB1001";

      /// <summary>
      /// STB Pin 3: 排出予告
      /// </summary>
      public string StbDischargeNotice { get; set; } = "LB1002";

      /// <summary>
      /// STB Pin 4: 排出要求
      /// </summary>
      public string StbDischargeRequest { get; set; } = "LB1003";

      /// <summary>
      /// STB Pin 5: 基板受取位置 (Word類型)
      /// </summary>
      public string StbBoardReceivePosition { get; set; } = "LW1004";

      /// <summary>
      /// STB Pin 6: 基板受取完了
      /// </summary>
      public string StbBoardReceiveComplete { get; set; } = "LB1006";

      /// <summary>
      /// STB Pin 7: LastFlag
      /// </summary>
      public string StbLastFlag { get; set; } = "LB1007";

      /// <summary>
      /// STB Pin 16: 非常停止
      /// </summary>
      public string StbEmergencyStop { get; set; } = "LB1016";

      // === RB → STB (輸出地址 - 寫入給滑台) ===

      /// <summary>
      /// RB Pin 1: 起動中
      /// </summary>
      public string RbIsRunning { get; set; } = "LB2000";

      /// <summary>
      /// RB Pin 2: 停止中
      /// </summary>
      public string RbIsStopped { get; set; } = "LB2001";

      /// <summary>
      /// RB Pin 3: 收料可能予告1
      /// </summary>
      public string RbReadyToReceiveNotice1 { get; set; } = "LB2002";

      /// <summary>
      /// RB Pin 4: 排出要求
      /// </summary>
      public string RbDischargeRequest { get; set; } = "LB2003";

      /// <summary>
      /// RB Pin 5: 基板受取完了1
      /// </summary>
      public string RbBoardReceiveComplete1 { get; set; } = "LB2004";

      /// <summary>
      /// RB Pin 6: 受取不可
      /// </summary>
      public string RbCannotReceive { get; set; } = "LB2005";

      /// <summary>
      /// RB Pin 7: 手動
      /// </summary>
      public string RbManualMode { get; set; } = "LB2006";

      /// <summary>
      /// RB Pin 26: データ確認NG
      /// </summary>
      public string RbDataCheckNg { get; set; } = "LB2026";

      /// <summary>
      /// RB Pin 27: データ確認OK
      /// </summary>
      public string RbDataCheckOk { get; set; } = "LB2027";

      /// <summary>
      /// RB Pin 28: 基板受取位置 (Word類型)
      /// </summary>
      public string RbBoardReceivePosition { get; set; } = "LW2028";

      #endregion

      #region Public Methods

      /// <summary>
      /// 複製地址配置
      /// </summary>
      public HandshakeAddresses Clone()
      {
         return new HandshakeAddresses
         {
            StbIsRunning = StbIsRunning,
            StbIsStopped = StbIsStopped,
            StbDischargeNotice = StbDischargeNotice,
            StbDischargeRequest = StbDischargeRequest,
            StbBoardReceivePosition = StbBoardReceivePosition,
            StbBoardReceiveComplete = StbBoardReceiveComplete,
            StbLastFlag = StbLastFlag,
            StbEmergencyStop = StbEmergencyStop,
            RbIsRunning = RbIsRunning,
            RbIsStopped = RbIsStopped,
            RbReadyToReceiveNotice1 = RbReadyToReceiveNotice1,
            RbDischargeRequest = RbDischargeRequest,
            RbBoardReceiveComplete1 = RbBoardReceiveComplete1,
            RbCannotReceive = RbCannotReceive,
            RbManualMode = RbManualMode,
            RbDataCheckNg = RbDataCheckNg,
            RbDataCheckOk = RbDataCheckOk,
            RbBoardReceivePosition = RbBoardReceivePosition
         };
      }

      #endregion
   }
}