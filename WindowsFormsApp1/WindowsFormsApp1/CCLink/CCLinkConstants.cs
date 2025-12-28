namespace WindowsFormsApp1.CCLink
{
    /// <summary>
    /// 常數定義：裝置代碼與預設位址等。
    /// </summary>
    internal static class CCLinkConstants
    {
        // 裝置代碼，對應 MELSEC API (手冊)
        public const int DEV_LB = 23;
        public const int DEV_LW = 24;
        public const int DEV_LX = 21;
        public const int DEV_LY = 22;

        // 預設位址（可由上層覆寫）        public const int DefaultRequestFlagAddress = 300; // LB0300        public const int DefaultResponseFlagAddress = 100; // LB0100        public const int DefaultRequestDataAddress = 400; // LW0400        public const int DefaultRequestDataLength = 7;    // Year..Week 共 7 個 UINT16    }
}