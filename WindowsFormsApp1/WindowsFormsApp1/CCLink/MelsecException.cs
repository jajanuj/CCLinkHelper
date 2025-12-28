using System;

namespace WindowsFormsApp1.CCLink
{
    /// <summary>
    /// MELSEC 錯誤例外封裝。統一攔截 Return Code 並分類。
    /// </summary>
    public enum MelsecErrorCategory { Transient, Permanent, Configuration, Hardware }

    /// <summary>
    /// 以錯誤碼建立人類可讀的例外，含建議動作與來源 API 名稱。
    /// </summary>
    public sealed class MelsecException : Exception
    {
        /// <summary>原始 Return Code。</summary>
        public int ReturnCode { get; }
        /// <summary>錯誤分類（暫時性/永久/設定/硬體）。</summary>
        public MelsecErrorCategory Category { get; }
        /// <summary>建議動作。</summary>
        public string Advice { get; }
        /// <summary>來源 API 名稱。</summary>
        public string ApiName { get; }

        public MelsecException(int code, string api, MelsecErrorCategory category, string advice, string message)
            : base(message)
        {
            ReturnCode = code; ApiName = api; Category = category; Advice = advice;
        }

        /// <summary>
        /// 由 Return Code 產生例外。0x4000 系列視為設定/環境問題。
        /// </summary>
        public static MelsecException FromCode(int code, string api)
        {
            var category = MelsecErrorCategory.Transient;
            var advice = "Retry or check connection.";
            var message = $"MELSEC API {api} failed with code 0x{code:X4}.";
            if ((code & 0x4000) == 0x4000) { category = MelsecErrorCategory.Configuration; advice = "Check path/channel/device parameters."; }
            return new MelsecException(code, api, category, advice, message);
        }
    }
}
