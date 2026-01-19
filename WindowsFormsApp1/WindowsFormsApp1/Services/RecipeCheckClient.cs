using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsFormsApp1.CCLink.Interfaces;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.Services
{
   /// <summary>
   /// Recipe Check 客戶端（裝置端）
   /// 負責發送 Recipe Check 請求並接收 MPLC 回應
   /// </summary>
   public class RecipeCheckClient
   {
      #region Fields

      private readonly AppPlcService _appPlcService;
      private readonly ICCLinkController _controller;
      private readonly Action<string> _logger;
      private readonly RecipeCheckSettings _settings;

      #endregion

      #region Constructors

      /// <summary>
      /// 建構函數
      /// </summary>
      public RecipeCheckClient(AppPlcService appPlcService, RecipeCheckSettings settings, Action<string> logger = null)
      {
         _appPlcService = appPlcService ?? throw new ArgumentNullException(nameof(appPlcService));
         _controller = _appPlcService.Controller;
         _settings = settings ?? throw new ArgumentNullException(nameof(settings));
         _logger = logger;
      }

      #endregion

      #region Public Methods

      /// <summary>
      /// 發送 Recipe Check 請求
      /// </summary>
      /// <param name="trackingData">追蹤資料（10 word）</param>
      /// <param name="recipeNo">Recipe No.（數值模式）</param>
      /// <param name="recipeName">Recipe Name（字串模式）</param>
      public async Task<RecipeCheckResponse> SendRequestAsync(short[] trackingData, ushort? recipeNo = null, string recipeName = null)
      {
         try
         {
            if (trackingData == null || trackingData.Length != 10)
            {
               throw new ArgumentException("追蹤資料必須是 10 個 word", nameof(trackingData));
            }

            if (_settings.Mode == RecipeCheckMode.Numeric && !recipeNo.HasValue)
            {
               throw new ArgumentException("數值模式必須提供 Recipe No.", nameof(recipeNo));
            }

            if (_settings.Mode == RecipeCheckMode.String && string.IsNullOrEmpty(recipeName))
            {
               throw new ArgumentException("字串模式必須提供 Recipe Name", nameof(recipeName));
            }

            Log($"[Recipe Check] 開始發送請求（模式：{_settings.Mode}）");

            // 1. 寫入追蹤資料 (LW6087-6096)
            await _controller.WriteWordsAsync(_settings.RequestDataAddress, trackingData);
            Log($"[Recipe Check] 已寫入追蹤資料：{_settings.RequestDataAddress}");

            // 2. 根據模式寫入 Recipe No. 或 Recipe Name
            if (_settings.Mode == RecipeCheckMode.Numeric)
            {
               await _controller.WriteWordsAsync(_settings.RequestRecipeNoAddress, new short[] { (short)recipeNo.Value });
               Log($"[Recipe Check] 已寫入 Recipe No.: {recipeNo.Value}");
            }
            else
            {
               short[] recipeNameWords = ConvertStringToWords(recipeName, 50);
               await _controller.WriteWordsAsync(_settings.RequestRecipeNameAddress, recipeNameWords);
               Log($"[Recipe Check] 已寫入 Recipe Name: {recipeName}");
            }

            // 3. 設定 Request Flag (LB0303)
            await _controller.WriteBitsAsync(_settings.RequestFlagAddress, new bool[] { true });
            Log($"[Recipe Check] 已設定 Request Flag: {_settings.RequestFlagAddress}");

            // 4. 等待 Response
            var response = await WaitForResponseAsync();

            // 5. 清除 Request Flag
            await _controller.WriteBitsAsync(_settings.RequestFlagAddress, new bool[] { false });
            Log($"[Recipe Check] 已清除 Request Flag");

            return response;
         }
         catch (Exception ex)
         {
            Log($"[Recipe Check] 發送請求失敗：{ex.Message}");
            throw;
         }
      }

      #endregion

      #region Private Methods

      /// <summary>
      /// 等待 MPLC 回應
      /// </summary>
      private async Task<RecipeCheckResponse> WaitForResponseAsync()
      {
         var cts = new CancellationTokenSource(_settings.ResponseTimeoutMs);
         _ = DateTime.Now;

         try
         {
            Log($"[Recipe Check] 等待 MPLC 回應（超時：{_settings.ResponseTimeoutMs} ms）");

            while (!cts.Token.IsCancellationRequested)
            {
               // 檢查 Response Flag
               bool okFlag = await _controller.ReadBitsAsync(_settings.ResponseOkAddress);
               bool ngFlag = await _controller.ReadBitsAsync(_settings.ResponseNgAddress);

               if (okFlag || ngFlag)
               {
                  Log($"[Recipe Check] 收到回應：{(okFlag ? "OK" : "NG")}");

                  // 讀取 Response Data
                  var thicknessResult = await _controller.ReadWordsAsync(_settings.ResponseThicknessAddress, 1);
                  ushort boardThickness = (ushort)thicknessResult[0];
                  Log($"[Recipe Check] 板厚：{boardThickness}");

                  ushort? recipeNo = null;
                  string recipeName = null;

                  if (_settings.Mode == RecipeCheckMode.Numeric)
                  {
                     var recipeNoResult = await _controller.ReadWordsAsync(_settings.ResponseRecipeNoAddress, 1);
                     recipeNo = (ushort)recipeNoResult[0];
                     Log($"[Recipe Check] Recipe No.: {recipeNo.Value}");
                  }
                  else
                  {
                     var recipeWordsList = await _controller.ReadWordsAsync(_settings.ResponseRecipeNameAddress, 50);
                     recipeName = ConvertWordsToString(System.Linq.Enumerable.ToArray(recipeWordsList));
                     Log($"[Recipe Check] Recipe Name: {recipeName}");
                  }

                  // 清除 Response Flag
                  if (okFlag)
                  {
                     await _controller.WriteWordsAsync("LW119F", new short[] { (short)3 });
                     await _controller.WriteBitsAsync(_settings.ResponseOkAddress, new bool[] { false });
                  }

                  if (ngFlag)
                  {
                     await _controller.WriteBitsAsync(_settings.ResponseNgAddress, new bool[] { false });
                  }

                  return RecipeCheckResponse.CreateSuccessResponse(okFlag, boardThickness, recipeNo, recipeName);
               }

               await Task.Delay(100, cts.Token);
            }

            // 超時
            Log($"[Recipe Check] 等待回應超時（{_settings.ResponseTimeoutMs} ms）");
            return RecipeCheckResponse.CreateTimeoutResponse();
         }
         catch (OperationCanceledException)
         {
            Log($"[Recipe Check] 等待回應超時");
            await AlarmHelper.AddAlarmCodeAsync(_appPlcService, "C007");
            return RecipeCheckResponse.CreateTimeoutResponse();
         }
         catch (Exception ex)
         {
            Log($"[Recipe Check] 等待回應時發生錯誤：{ex.Message}");
            throw;
         }
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

         byte[] bytes = Encoding.ASCII.GetBytes(text);

         for (int i = 0; i < wordCount && i * 2 < bytes.Length; i++)
         {
            byte high = i * 2 + 1 < bytes.Length ? bytes[i * 2 + 1] : (byte)0;
            byte low = bytes[i * 2];
            words[i] = (short)((high << 8) | low);
         }

         return words;
      }

      /// <summary>
      /// 將 PLC Word 陣列轉換為字串（ASCII）
      /// </summary>
      private string ConvertWordsToString(short[] words)
      {
         if (words == null || words.Length == 0)
         {
            return string.Empty;
         }

         byte[] bytes = new byte[words.Length * 2];

         for (int i = 0; i < words.Length; i++)
         {
            bytes[i * 2] = (byte)(words[i] & 0xFF);
            bytes[i * 2 + 1] = (byte)((words[i] >> 8) & 0xFF);
         }

         // 移除尾端的 null 字元
         int length = Array.IndexOf(bytes, (byte)0);
         if (length < 0)
         {
            length = bytes.Length;
         }

         return Encoding.ASCII.GetString(bytes, 0, length);
      }

      /// <summary>
      /// 記錄日誌
      /// </summary>
      private void Log(string message)
      {
         _logger?.Invoke($"[{DateTime.Now:HH:mm:ss.fff}] {message}");
      }

      #endregion
   }
}