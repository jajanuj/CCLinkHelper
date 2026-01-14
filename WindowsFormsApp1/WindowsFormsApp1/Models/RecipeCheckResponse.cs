using System;

namespace WindowsFormsApp1.Models
{
    /// <summary>
    /// Recipe Check 回應資料
    /// </summary>
    public class RecipeCheckResponse
    {
        /// <summary>檢查結果：OK or NG</summary>
        public bool IsOK { get; set; }

        /// <summary>板厚資訊</summary>
        public ushort BoardThickness { get; set; }

        /// <summary>Recipe No.（數值模式）</summary>
        public ushort? RecipeNo { get; set; }

        /// <summary>Recipe Name（字串模式）</summary>
        public string RecipeName { get; set; }

        /// <summary>回應時間</summary>
        public DateTime ResponseTime { get; set; }

        /// <summary>是否超時</summary>
        public bool IsTimeout { get; set; }

        /// <summary>
        /// 建立超時回應
        /// </summary>
        public static RecipeCheckResponse CreateTimeoutResponse()
        {
            return new RecipeCheckResponse
            {
                IsOK = false,
                IsTimeout = true,
                ResponseTime = DateTime.Now
            };
        }

        /// <summary>
        /// 建立成功回應
        /// </summary>
        public static RecipeCheckResponse CreateSuccessResponse(bool isOk, ushort boardThickness, ushort? recipeNo = null, string recipeName = null)
        {
            return new RecipeCheckResponse
            {
                IsOK = isOk,
                BoardThickness = boardThickness,
                RecipeNo = recipeNo,
                RecipeName = recipeName,
                ResponseTime = DateTime.Now,
                IsTimeout = false
            };
        }
    }
}
