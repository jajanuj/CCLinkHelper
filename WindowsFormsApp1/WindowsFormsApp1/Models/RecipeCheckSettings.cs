namespace WindowsFormsApp1.Models
{
    /// <summary>
    /// Recipe Check 設定
    /// </summary>
    public class RecipeCheckSettings
    {
        /// <summary>檢查模式</summary>
        public RecipeCheckMode Mode { get; set; } = RecipeCheckMode.Numeric;

        /// <summary>Request Flag 地址</summary>
        public string RequestFlagAddress { get; set; } = "LB0303";

        /// <summary>Request Data 起始地址（追蹤資料）</summary>
        public string RequestDataAddress { get; set; } = "LW6087";

        /// <summary>Request Data Recipe No. 地址（數值模式）</summary>
        public string RequestRecipeNoAddress { get; set; } = "LW1155";

        /// <summary>Request Data Recipe Name 地址（字串模式）</summary>
        public string RequestRecipeNameAddress { get; set; } = "LW1159";

        /// <summary>Response Flag OK 地址</summary>
        public string ResponseOkAddress { get; set; } = "LB0103";

        /// <summary>Response Flag NG 地址</summary>
        public string ResponseNgAddress { get; set; } = "LB0104";

        /// <summary>Response Data 板厚地址</summary>
        public string ResponseThicknessAddress { get; set; } = "LW1498";

        /// <summary>Response Data Recipe No. 地址（數值模式）</summary>
        public string ResponseRecipeNoAddress { get; set; } = "LW1500";

        /// <summary>Response Data Recipe Name 地址（字串模式）</summary>
        public string ResponseRecipeNameAddress { get; set; } = "LW1502";

        /// <summary>Response 超時時間（毫秒）</summary>
        public int ResponseTimeoutMs { get; set; } = 5000;
    }
}
