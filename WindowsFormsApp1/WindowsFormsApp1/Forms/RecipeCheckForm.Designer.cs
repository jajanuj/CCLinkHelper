
namespace WindowsFormsApp1.Forms
{
    partial class RecipeCheckForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpRequest = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtRecipeName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtRecipeNo = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.rdoString = new System.Windows.Forms.RadioButton();
            this.rdoNumeric = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTrackingData = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.grpResponse = new System.Windows.Forms.GroupBox();
            this.lblResponseTime = new System.Windows.Forms.Label();
            this.lblResponseRecipe = new System.Windows.Forms.Label();
            this.lblThickness = new System.Windows.Forms.Label();
            this.lblResult = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.grpSettings = new System.Windows.Forms.GroupBox();
            this.propertyGridSettings = new System.Windows.Forms.PropertyGrid();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.grpRequest.SuspendLayout();
            this.grpResponse.SuspendLayout();
            this.grpSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpRequest
            // 
            this.grpRequest.Controls.Add(this.label4);
            this.grpRequest.Controls.Add(this.txtRecipeName);
            this.grpRequest.Controls.Add(this.label3);
            this.grpRequest.Controls.Add(this.txtRecipeNo);
            this.grpRequest.Controls.Add(this.btnSend);
            this.grpRequest.Controls.Add(this.rdoString);
            this.grpRequest.Controls.Add(this.rdoNumeric);
            this.grpRequest.Controls.Add(this.label2);
            this.grpRequest.Controls.Add(this.txtTrackingData);
            this.grpRequest.Controls.Add(this.label1);
            this.grpRequest.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpRequest.Location = new System.Drawing.Point(0, 0);
            this.grpRequest.Name = "grpRequest";
            this.grpRequest.Size = new System.Drawing.Size(534, 200);
            this.grpRequest.TabIndex = 0;
            this.grpRequest.TabStop = false;
            this.grpRequest.Text = "Recipe Check 請求";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Gray;
            this.label4.Location = new System.Drawing.Point(105, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(269, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "格式：word1,word2... (請輸入10個數值，用逗號分隔)";
            // 
            // txtRecipeName
            // 
            this.txtRecipeName.Location = new System.Drawing.Point(107, 130);
            this.txtRecipeName.Name = "txtRecipeName";
            this.txtRecipeName.Size = new System.Drawing.Size(300, 22);
            this.txtRecipeName.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 133);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "Recipe Name:";
            // 
            // txtRecipeNo
            // 
            this.txtRecipeNo.Location = new System.Drawing.Point(107, 102);
            this.txtRecipeNo.Name = "txtRecipeNo";
            this.txtRecipeNo.Size = new System.Drawing.Size(100, 22);
            this.txtRecipeNo.TabIndex = 6;
            this.txtRecipeNo.Text = "1";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(420, 102);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(100, 50);
            this.btnSend.TabIndex = 5;
            this.btnSend.Text = "發送請求";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // rdoString
            // 
            this.rdoString.AutoSize = true;
            this.rdoString.Location = new System.Drawing.Point(220, 75);
            this.rdoString.Name = "rdoString";
            this.rdoString.Size = new System.Drawing.Size(71, 16);
            this.rdoString.TabIndex = 4;
            this.rdoString.Text = "字串模式";
            this.rdoString.UseVisualStyleBackColor = true;
            this.rdoString.CheckedChanged += new System.EventHandler(this.rdoMode_CheckedChanged);
            // 
            // rdoNumeric
            // 
            this.rdoNumeric.AutoSize = true;
            this.rdoNumeric.Checked = true;
            this.rdoNumeric.Location = new System.Drawing.Point(107, 75);
            this.rdoNumeric.Name = "rdoNumeric";
            this.rdoNumeric.Size = new System.Drawing.Size(71, 16);
            this.rdoNumeric.TabIndex = 3;
            this.rdoNumeric.TabStop = true;
            this.rdoNumeric.Text = "數值模式";
            this.rdoNumeric.UseVisualStyleBackColor = true;
            this.rdoNumeric.CheckedChanged += new System.EventHandler(this.rdoMode_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Recipe No.:";
            // 
            // txtTrackingData
            // 
            this.txtTrackingData.Location = new System.Drawing.Point(107, 25);
            this.txtTrackingData.Name = "txtTrackingData";
            this.txtTrackingData.Size = new System.Drawing.Size(413, 22);
            this.txtTrackingData.TabIndex = 1;
            this.txtTrackingData.Text = "0,0,0,0,0,0,0,0,0,0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "追蹤資料:";
            // 
            // grpResponse
            // 
            this.grpResponse.Controls.Add(this.lblResponseTime);
            this.grpResponse.Controls.Add(this.lblResponseRecipe);
            this.grpResponse.Controls.Add(this.lblThickness);
            this.grpResponse.Controls.Add(this.lblResult);
            this.grpResponse.Controls.Add(this.txtLog);
            this.grpResponse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpResponse.Location = new System.Drawing.Point(0, 200);
            this.grpResponse.Name = "grpResponse";
            this.grpResponse.Size = new System.Drawing.Size(534, 301);
            this.grpResponse.TabIndex = 1;
            this.grpResponse.TabStop = false;
            this.grpResponse.Text = "Response 結果顯示";
            // 
            // lblResponseTime
            // 
            this.lblResponseTime.AutoSize = true;
            this.lblResponseTime.Location = new System.Drawing.Point(23, 100);
            this.lblResponseTime.Name = "lblResponseTime";
            this.lblResponseTime.Size = new System.Drawing.Size(56, 12);
            this.lblResponseTime.TabIndex = 4;
            this.lblResponseTime.Text = "回應時間:";
            // 
            // lblResponseRecipe
            // 
            this.lblResponseRecipe.AutoSize = true;
            this.lblResponseRecipe.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblResponseRecipe.Location = new System.Drawing.Point(23, 75);
            this.lblResponseRecipe.Name = "lblResponseRecipe";
            this.lblResponseRecipe.Size = new System.Drawing.Size(95, 16);
            this.lblResponseRecipe.TabIndex = 3;
            this.lblResponseRecipe.Text = "Recipe: N/A";
            // 
            // lblThickness
            // 
            this.lblThickness.AutoSize = true;
            this.lblThickness.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblThickness.Location = new System.Drawing.Point(23, 50);
            this.lblThickness.Name = "lblThickness";
            this.lblThickness.Size = new System.Drawing.Size(76, 16);
            this.lblThickness.TabIndex = 2;
            this.lblThickness.Text = "板厚: N/A";
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Font = new System.Drawing.Font("PMingLiU", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblResult.Location = new System.Drawing.Point(23, 25);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(126, 19);
            this.lblResult.TabIndex = 1;
            this.lblResult.Text = "狀態: 等待中";
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Location = new System.Drawing.Point(6, 125);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(522, 170);
            this.txtLog.TabIndex = 0;
            // 
            // grpSettings
            // 
            this.grpSettings.Controls.Add(this.propertyGridSettings);
            this.grpSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSettings.Location = new System.Drawing.Point(0, 0);
            this.grpSettings.Name = "grpSettings";
            this.grpSettings.Size = new System.Drawing.Size(250, 501);
            this.grpSettings.TabIndex = 2;
            this.grpSettings.TabStop = false;
            this.grpSettings.Text = "PLC 地址設定";
            // 
            // propertyGridSettings
            // 
            this.propertyGridSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridSettings.Location = new System.Drawing.Point(3, 18);
            this.propertyGridSettings.Name = "propertyGridSettings";
            this.propertyGridSettings.Size = new System.Drawing.Size(244, 480);
            this.propertyGridSettings.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.grpResponse);
            this.splitContainer1.Panel1.Controls.Add(this.grpRequest);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.grpSettings);
            this.splitContainer1.Size = new System.Drawing.Size(784, 501);
            this.splitContainer1.SplitterDistance = 534;
            this.splitContainer1.TabIndex = 3;
            // 
            // RecipeCheckForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 501);
            this.Controls.Add(this.splitContainer1);
            this.Name = "RecipeCheckForm";
            this.Text = "Recipe Check 測試";
            this.grpRequest.ResumeLayout(false);
            this.grpRequest.PerformLayout();
            this.grpResponse.ResumeLayout(false);
            this.grpResponse.PerformLayout();
            this.grpSettings.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpRequest;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTrackingData;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rdoString;
        private System.Windows.Forms.RadioButton rdoNumeric;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtRecipeNo;
        private System.Windows.Forms.GroupBox grpResponse;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.Label lblResponseRecipe;
        private System.Windows.Forms.Label lblThickness;
        private System.Windows.Forms.GroupBox grpSettings;
        private System.Windows.Forms.PropertyGrid propertyGridSettings;
        private System.Windows.Forms.TextBox txtRecipeName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblResponseTime;
    }
}
