namespace WindowsFormsApp1
{
   partial class CommonReportStatus2SettingsForm
   {
      private System.ComponentModel.IContainer components = null;

      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      private void InitializeComponent()
      {
         this.cmbRedLight = new System.Windows.Forms.ComboBox();
         this.cmbYellowLight = new System.Windows.Forms.ComboBox();
         this.cmbGreenLight = new System.Windows.Forms.ComboBox();
         this.cmbUpstreamWaiting = new System.Windows.Forms.ComboBox();
         this.cmbDownstreamWaiting = new System.Windows.Forms.ComboBox();
         this.nudDischargeRate = new System.Windows.Forms.NumericUpDown();
         this.nudStopTime = new System.Windows.Forms.NumericUpDown();
         this.nudProcessingCounter = new System.Windows.Forms.NumericUpDown();
         this.nudRetainedBoardCount = new System.Windows.Forms.NumericUpDown();
         this.nudCurrentRecipeNo = new System.Windows.Forms.NumericUpDown();
         this.nudBoardThickness = new System.Windows.Forms.NumericUpDown();
         this.txtRecipeName = new System.Windows.Forms.TextBox();
         this.btnOK = new System.Windows.Forms.Button();
         this.btnCancel = new System.Windows.Forms.Button();
         this.lblRedLight = new System.Windows.Forms.Label();
         this.lblYellowLight = new System.Windows.Forms.Label();
         this.lblGreenLight = new System.Windows.Forms.Label();
         this.lblUpstreamWaiting = new System.Windows.Forms.Label();
         this.lblDownstreamWaiting = new System.Windows.Forms.Label();
         this.lblDischargeRate = new System.Windows.Forms.Label();
         this.lblStopTime = new System.Windows.Forms.Label();
         this.lblProcessingCounter = new System.Windows.Forms.Label();
         this.lblRetainedBoardCount = new System.Windows.Forms.Label();
         this.lblCurrentRecipeNo = new System.Windows.Forms.Label();
         this.lblBoardThickness = new System.Windows.Forms.Label();
         this.lblRecipeName = new System.Windows.Forms.Label();
         ((System.ComponentModel.ISupportInitialize)(this.nudDischargeRate)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudStopTime)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudProcessingCounter)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudRetainedBoardCount)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudCurrentRecipeNo)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudBoardThickness)).BeginInit();
         this.SuspendLayout();

         // Form Settings
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(550, 480);
         this.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "定期上報 Status2 設定";

         // Red Light
         this.lblRedLight.Text = "紅燈狀態 (Red):";
         this.lblRedLight.Location = new System.Drawing.Point(30, 20);
         this.lblRedLight.Size = new System.Drawing.Size(140, 20);
         this.lblRedLight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         
         this.cmbRedLight.Location = new System.Drawing.Point(180, 20);
         this.cmbRedLight.Size = new System.Drawing.Size(300, 24);
         this.cmbRedLight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

         // Yellow Light
         this.lblYellowLight.Text = "黃燈狀態 (Yellow):";
         this.lblYellowLight.Location = new System.Drawing.Point(30, 52);
         this.lblYellowLight.Size = new System.Drawing.Size(140, 20);
         this.lblYellowLight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

         this.cmbYellowLight.Location = new System.Drawing.Point(180, 52);
         this.cmbYellowLight.Size = new System.Drawing.Size(300, 24);
         this.cmbYellowLight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

         // Green Light
         this.lblGreenLight.Text = "綠燈狀態 (Green):";
         this.lblGreenLight.Location = new System.Drawing.Point(30, 84);
         this.lblGreenLight.Size = new System.Drawing.Size(140, 20);
         this.lblGreenLight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

         this.cmbGreenLight.Location = new System.Drawing.Point(180, 84);
         this.cmbGreenLight.Size = new System.Drawing.Size(300, 24);
         this.cmbGreenLight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

         // Upstream Waiting
         this.lblUpstreamWaiting.Text = "上游等待 (Upstream):";
         this.lblUpstreamWaiting.Location = new System.Drawing.Point(30, 116);
         this.lblUpstreamWaiting.Size = new System.Drawing.Size(140, 20);
         this.lblUpstreamWaiting.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

         this.cmbUpstreamWaiting.Location = new System.Drawing.Point(180, 116);
         this.cmbUpstreamWaiting.Size = new System.Drawing.Size(300, 24);
         this.cmbUpstreamWaiting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

         // Downstream Waiting
         this.lblDownstreamWaiting.Text = "下游等待 (Downstream):";
         this.lblDownstreamWaiting.Location = new System.Drawing.Point(30, 148);
         this.lblDownstreamWaiting.Size = new System.Drawing.Size(140, 20);
         this.lblDownstreamWaiting.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

         this.cmbDownstreamWaiting.Location = new System.Drawing.Point(180, 148);
         this.cmbDownstreamWaiting.Size = new System.Drawing.Size(300, 24);
         this.cmbDownstreamWaiting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

         // Discharge Rate
         this.lblDischargeRate.Text = "排出節拍 (Rate):";
         this.lblDischargeRate.Location = new System.Drawing.Point(30, 180);
         this.lblDischargeRate.Size = new System.Drawing.Size(140, 20);
         this.lblDischargeRate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

         this.nudDischargeRate.Location = new System.Drawing.Point(180, 180);
         this.nudDischargeRate.Size = new System.Drawing.Size(300, 24);
         this.nudDischargeRate.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });

         // Stop Time
         this.lblStopTime.Text = "停止時間 (Stop Time):";
         this.lblStopTime.Location = new System.Drawing.Point(30, 212);
         this.lblStopTime.Size = new System.Drawing.Size(140, 20);
         this.lblStopTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

         this.nudStopTime.Location = new System.Drawing.Point(180, 212);
         this.nudStopTime.Size = new System.Drawing.Size(300, 24);
         this.nudStopTime.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });

         // Processing Counter
         this.lblProcessingCounter.Text = "處理計數 (Counter):";
         this.lblProcessingCounter.Location = new System.Drawing.Point(30, 244);
         this.lblProcessingCounter.Size = new System.Drawing.Size(140, 20);
         this.lblProcessingCounter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

         this.nudProcessingCounter.Location = new System.Drawing.Point(180, 244);
         this.nudProcessingCounter.Size = new System.Drawing.Size(300, 24);
         this.nudProcessingCounter.Maximum = new decimal(new int[] { -1, -1, -1, 0 }); // UINT32 Max

         // Retained Board Count
         this.lblRetainedBoardCount.Text = "滯留板數 (Retained):";
         this.lblRetainedBoardCount.Location = new System.Drawing.Point(30, 276);
         this.lblRetainedBoardCount.Size = new System.Drawing.Size(140, 20);
         this.lblRetainedBoardCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

         this.nudRetainedBoardCount.Location = new System.Drawing.Point(180, 276);
         this.nudRetainedBoardCount.Size = new System.Drawing.Size(300, 24);
         this.nudRetainedBoardCount.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });

         // Current Recipe No
         this.lblCurrentRecipeNo.Text = "配方編號 (Recipe No):";
         this.lblCurrentRecipeNo.Location = new System.Drawing.Point(30, 308);
         this.lblCurrentRecipeNo.Size = new System.Drawing.Size(140, 20);
         this.lblCurrentRecipeNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

         this.nudCurrentRecipeNo.Location = new System.Drawing.Point(180, 308);
         this.nudCurrentRecipeNo.Size = new System.Drawing.Size(300, 24);
         this.nudCurrentRecipeNo.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });

         // Board Thickness
         this.lblBoardThickness.Text = "板厚狀態 (Thickness):";
         this.lblBoardThickness.Location = new System.Drawing.Point(30, 340);
         this.lblBoardThickness.Size = new System.Drawing.Size(140, 20);
         this.lblBoardThickness.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

         this.nudBoardThickness.Location = new System.Drawing.Point(180, 340);
         this.nudBoardThickness.Size = new System.Drawing.Size(300, 24);
         this.nudBoardThickness.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });

         // Recipe Name
         this.lblRecipeName.Text = "配方名稱 (Name):";
         this.lblRecipeName.Location = new System.Drawing.Point(30, 372);
         this.lblRecipeName.Size = new System.Drawing.Size(140, 20);
         this.lblRecipeName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

         this.txtRecipeName.Location = new System.Drawing.Point(180, 372);
         this.txtRecipeName.Size = new System.Drawing.Size(300, 24);
         this.txtRecipeName.MaxLength = 100;

         // Buttons
         this.btnOK.Location = new System.Drawing.Point(180, 414);
         this.btnOK.Size = new System.Drawing.Size(90, 30);
         this.btnOK.Text = "確定";
         this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);

         this.btnCancel.Location = new System.Drawing.Point(290, 414);
         this.btnCancel.Size = new System.Drawing.Size(90, 30);
         this.btnCancel.Text = "取消";
         this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;

         this.AcceptButton = this.btnOK;
         this.CancelButton = this.btnCancel;

         this.Controls.Add(this.lblRedLight);
         this.Controls.Add(this.cmbRedLight);
         this.Controls.Add(this.lblYellowLight);
         this.Controls.Add(this.cmbYellowLight);
         this.Controls.Add(this.lblGreenLight);
         this.Controls.Add(this.cmbGreenLight);
         this.Controls.Add(this.lblUpstreamWaiting);
         this.Controls.Add(this.cmbUpstreamWaiting);
         this.Controls.Add(this.lblDownstreamWaiting);
         this.Controls.Add(this.cmbDownstreamWaiting);
         this.Controls.Add(this.lblDischargeRate);
         this.Controls.Add(this.nudDischargeRate);
         this.Controls.Add(this.lblStopTime);
         this.Controls.Add(this.nudStopTime);
         this.Controls.Add(this.lblProcessingCounter);
         this.Controls.Add(this.nudProcessingCounter);
         this.Controls.Add(this.lblRetainedBoardCount);
         this.Controls.Add(this.nudRetainedBoardCount);
         this.Controls.Add(this.lblCurrentRecipeNo);
         this.Controls.Add(this.nudCurrentRecipeNo);
         this.Controls.Add(this.lblBoardThickness);
         this.Controls.Add(this.nudBoardThickness);
         this.Controls.Add(this.lblRecipeName);
         this.Controls.Add(this.txtRecipeName);
         this.Controls.Add(this.btnOK);
         this.Controls.Add(this.btnCancel);

         ((System.ComponentModel.ISupportInitialize)(this.nudDischargeRate)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudStopTime)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudProcessingCounter)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudRetainedBoardCount)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudCurrentRecipeNo)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudBoardThickness)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();
      }

      #endregion

      private System.Windows.Forms.ComboBox cmbRedLight;
      private System.Windows.Forms.ComboBox cmbYellowLight;
      private System.Windows.Forms.ComboBox cmbGreenLight;
      private System.Windows.Forms.ComboBox cmbUpstreamWaiting;
      private System.Windows.Forms.ComboBox cmbDownstreamWaiting;
      private System.Windows.Forms.NumericUpDown nudDischargeRate;
      private System.Windows.Forms.NumericUpDown nudStopTime;
      private System.Windows.Forms.NumericUpDown nudProcessingCounter;
      private System.Windows.Forms.NumericUpDown nudRetainedBoardCount;
      private System.Windows.Forms.NumericUpDown nudCurrentRecipeNo;
      private System.Windows.Forms.NumericUpDown nudBoardThickness;
      private System.Windows.Forms.TextBox txtRecipeName;
      private System.Windows.Forms.Button btnOK;
      private System.Windows.Forms.Button btnCancel;
      private System.Windows.Forms.Label lblRedLight;
      private System.Windows.Forms.Label lblYellowLight;
      private System.Windows.Forms.Label lblGreenLight;
      private System.Windows.Forms.Label lblUpstreamWaiting;
      private System.Windows.Forms.Label lblDownstreamWaiting;
      private System.Windows.Forms.Label lblDischargeRate;
      private System.Windows.Forms.Label lblStopTime;
      private System.Windows.Forms.Label lblProcessingCounter;
      private System.Windows.Forms.Label lblRetainedBoardCount;
      private System.Windows.Forms.Label lblCurrentRecipeNo;
      private System.Windows.Forms.Label lblBoardThickness;
      private System.Windows.Forms.Label lblRecipeName;

      private class ComboBoxItem
      {
         public ushort Value { get; }
         public string Display { get; }

         public ComboBoxItem(ushort value, string display)
         {
            Value = value;
            Display = display;
         }

         public override string ToString()
         {
            return Display;
         }
      }
   }
}
