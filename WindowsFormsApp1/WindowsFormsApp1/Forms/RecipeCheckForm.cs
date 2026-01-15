using System;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp1.CCLink.Interfaces;
using WindowsFormsApp1.Models;
using WindowsFormsApp1.Services;

namespace WindowsFormsApp1.Forms
{
   public partial class RecipeCheckForm : Form
   {
      #region Fields

      private readonly ICCLinkController _controller;
      private readonly PlcSimulator _simulator; // 新增模擬器引用
      private RecipeCheckClient _client;
      private RecipeCheckSettings _settings;

      #endregion

      #region Constructors

      public RecipeCheckForm(ICCLinkController controller, PlcSimulator simulator = null)
      {
         InitializeComponent();
         _controller = controller ?? throw new ArgumentNullException(nameof(controller));
         _simulator = simulator; // 儲存模擬器引用

         // 初始化設定
         _settings = new RecipeCheckSettings();

         // 綁定 PropertyGrid
         propertyGridSettings.SelectedObject = _settings;

         // 初始化 UI 狀態
         UpdateUiMode();
      }

      #endregion

      #region Private Methods

      private void rdoMode_CheckedChanged(object sender, EventArgs e)
      {
         UpdateUiMode();
      }

      private void UpdateUiMode()
      {
         bool isNumeric = rdoNumeric.Checked;
         txtRecipeNo.Enabled = isNumeric;
         txtRecipeName.Enabled = !isNumeric;

         // 更新設定中的模式
         _settings.Mode = isNumeric ? RecipeCheckMode.Numeric : RecipeCheckMode.String;
         propertyGridSettings.Refresh();
      }

      private async void btnSend_Click(object sender, EventArgs e)
      {
         try
         {
            btnSend.Enabled = false;
            lblResult.Text = "狀態: 檢查中...";
            lblResult.ForeColor = Color.Blue;
            Log("開始發送 Recipe Check 請求...");

            // 如果有模擬器，自動啟動模擬器流程
            if (_simulator != null)
            {
               Log("[模擬模式] 自動啟動模擬器流程...");
               _simulator.StartRecipeCheckMode(_settings);
            }

            // 準備追蹤資料
            short[] trackingData = ParsetrackingData(txtTrackingData.Text);

            // 準備 Recipe 參數
            ushort? recipeNo = null;
            string recipeName = null;

            if (rdoNumeric.Checked)
            {
               if (ushort.TryParse(txtRecipeNo.Text, out ushort no))
               {
                  recipeNo = no;
               }
               else
               {
                  MessageBox.Show("Recipe No. 必須是有效的數字 (0-65535)", "輸入錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                  return;
               }
            }
            else
            {
               recipeName = txtRecipeName.Text;
            }

            // 建立 Client
            _client = new RecipeCheckClient(_controller, _settings, Log);

            // 發送請求
            var response = await _client.SendRequestAsync(trackingData, recipeNo, recipeName);

            // 顯示結果
            DisplayResponse(response);
         }
         catch (Exception ex)
         {
            Log($"發生錯誤: {ex.Message}");
            lblResult.Text = "狀態: 錯誤";
            lblResult.ForeColor = Color.Red;
            MessageBox.Show($"執行失敗: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
         finally
         {
            btnSend.Enabled = true;
         }
      }

      private short[] ParsetrackingData(string input)
      {
         try
         {
            var parts = input.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var data = new short[10];

            for (int i = 0; i < 10 && i < parts.Length; i++)
            {
               if (short.TryParse(parts[i], out short val))
               {
                  data[i] = val;
               }
               else if (int.TryParse(parts[i], System.Globalization.NumberStyles.HexNumber, null, out int hexVal))
               {
                  // 支援十六進位輸入 (如 0xFFFF)
                  data[i] = (short)hexVal;
               }
            }

            return data;
         }
         catch
         {
            Log("追蹤資料解析失敗，使用預設值 (全0)");
            return new short[10];
         }
      }

      private void DisplayResponse(RecipeCheckResponse response)
      {
         lblResponseTime.Text = $"回應時間: {response.ResponseTime:HH:mm:ss.fff}";

         if (response.IsTimeout)
         {
            lblResult.Text = "狀態: 超時 (Timeout)";
            lblResult.ForeColor = Color.Red;
            lblThickness.Text = "板厚: N/A";
            lblResponseRecipe.Text = "Recipe: N/A";
         }
         else
         {
            lblResult.Text = response.IsOK ? "狀態: OK" : "狀態: NG";
            lblResult.ForeColor = response.IsOK ? Color.Green : Color.Red;

            lblThickness.Text = $"板厚: {response.BoardThickness}";

            if (_settings.Mode == RecipeCheckMode.Numeric)
            {
               lblResponseRecipe.Text = $"Recipe No.: {response.RecipeNo}";
            }
            else
            {
               lblResponseRecipe.Text = $"Recipe Name: {response.RecipeName}";
            }
         }
      }

      private void Log(string message)
      {
         if (InvokeRequired)
         {
            Invoke(new Action<string>(Log), message);
            return;
         }

         string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
         txtLog.AppendText($"[{timestamp}] {message}{Environment.NewLine}");
      }

      #endregion
   }
}