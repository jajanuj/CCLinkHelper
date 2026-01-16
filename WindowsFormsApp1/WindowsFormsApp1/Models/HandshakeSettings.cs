namespace WindowsFormsApp1.Models
{
   /// <summary>
   /// 滑台與機械手臂交握流程設定
   /// </summary>
   public class HandshakeSettings
   {
      #region Constructors

      /// <summary>
      /// 建立預設設定
      /// </summary>
      public HandshakeSettings()
      {
         // 使用屬性初始化完成預設值設定
      }

      #endregion

      #region Properties

      /// <summary>
      /// T1逾時秒數 - 等待STB排出要求從OFF→ON的逾時時間
      /// </summary>
      public int T1TimeoutSeconds { get; set; } = 5;

      /// <summary>
      /// T2逾時秒數 - 等待STB排出要求從ON→OFF的逾時時間
      /// </summary>
      public int T2TimeoutSeconds { get; set; } = 5;

      /// <summary>
      /// 一般逾時秒數 - 其他階段的預設逾時時間
      /// </summary>
      public int GeneralTimeoutSeconds { get; set; } = 10;

      /// <summary>
      /// 重試次數 - 發生逾時或錯誤時的自動重試次數
      /// </summary>
      public int RetryCount { get; set; } = 3;

      /// <summary>
      /// 啟用自動重試 - 是否在發生錯誤時自動重試
      /// </summary>
      public bool EnableAutoRetry { get; set; } = true;

      /// <summary>
      /// 循環週期毫秒 - 狀態機執行循環的週期(毫秒)
      /// </summary>
      public int LoopCycleMs { get; set; } = 100;

      /// <summary>
      /// PLC地址配置
      /// </summary>
      public HandshakeAddresses Addresses { get; set; } = new HandshakeAddresses();

      /// <summary>
      /// 啟用詳細日誌 - 是否記錄詳細的除錯日誌
      /// </summary>
      public bool EnableVerboseLogging { get; set; } = false;

      #endregion

      #region Public Methods

      /// <summary>
      /// 複製設定
      /// </summary>
      public HandshakeSettings Clone()
      {
         return new HandshakeSettings
         {
            T1TimeoutSeconds = T1TimeoutSeconds,
            T2TimeoutSeconds = T2TimeoutSeconds,
            GeneralTimeoutSeconds = GeneralTimeoutSeconds,
            RetryCount = RetryCount,
            EnableAutoRetry = EnableAutoRetry,
            LoopCycleMs = LoopCycleMs,
            Addresses = Addresses.Clone(),
            EnableVerboseLogging = EnableVerboseLogging
         };
      }

      /// <summary>
      /// 驗證設定值的有效性
      /// </summary>
      /// <returns>驗證錯誤訊息，若無錯誤則返回null</returns>
      public string Validate()
      {
         if (T1TimeoutSeconds <= 0)
         {
            return "T1逾時秒數必須大於0";
         }

         if (T2TimeoutSeconds <= 0)
         {
            return "T2逾時秒數必須大於0";
         }

         if (GeneralTimeoutSeconds <= 0)
         {
            return "一般逾時秒數必須大於0";
         }

         if (RetryCount < 0)
         {
            return "重試次數不能為負數";
         }

         if (LoopCycleMs <= 0)
         {
            return "循環週期必須大於0毫秒";
         }

         if (Addresses == null)
         {
            return "地址配置不能為空";
         }

         return null; // 驗證通過
      }

      #endregion
   }
}