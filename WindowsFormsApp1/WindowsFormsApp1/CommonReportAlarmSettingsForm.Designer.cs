namespace WindowsFormsApp1
{
   partial class CommonReportAlarmSettingsForm
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
         this.nudError1 = new System.Windows.Forms.NumericUpDown();
         this.nudError2 = new System.Windows.Forms.NumericUpDown();
         this.nudError3 = new System.Windows.Forms.NumericUpDown();
         this.nudError4 = new System.Windows.Forms.NumericUpDown();
         this.nudError5 = new System.Windows.Forms.NumericUpDown();
         this.nudError6 = new System.Windows.Forms.NumericUpDown();
         this.nudError7 = new System.Windows.Forms.NumericUpDown();
         this.nudError8 = new System.Windows.Forms.NumericUpDown();
         this.nudError9 = new System.Windows.Forms.NumericUpDown();
         this.nudError10 = new System.Windows.Forms.NumericUpDown();
         this.nudError11 = new System.Windows.Forms.NumericUpDown();
         this.nudError12 = new System.Windows.Forms.NumericUpDown();
         this.lblError1 = new System.Windows.Forms.Label();
         this.lblError2 = new System.Windows.Forms.Label();
         this.lblError3 = new System.Windows.Forms.Label();
         this.lblError4 = new System.Windows.Forms.Label();
         this.lblError5 = new System.Windows.Forms.Label();
         this.lblError6 = new System.Windows.Forms.Label();
         this.lblError7 = new System.Windows.Forms.Label();
         this.lblError8 = new System.Windows.Forms.Label();
         this.lblError9 = new System.Windows.Forms.Label();
         this.lblError10 = new System.Windows.Forms.Label();
         this.lblError11 = new System.Windows.Forms.Label();
         this.lblError12 = new System.Windows.Forms.Label();
         this.btnOK = new System.Windows.Forms.Button();
         this.btnCancel = new System.Windows.Forms.Button();

         ((System.ComponentModel.ISupportInitialize)(this.nudError1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudError2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudError3)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudError4)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudError5)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudError6)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudError7)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudError8)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudError9)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudError10)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudError11)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudError12)).BeginInit();
         this.SuspendLayout();

         // Form Settings
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(400, 300);
         this.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "定期上報 Alarm 設定";

         // Error 1
         this.lblError1.Text = "Code 01:";
         this.lblError1.Location = new System.Drawing.Point(20, 20);
         this.lblError1.Size = new System.Drawing.Size(75, 20);
         this.lblError1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         this.nudError1.Location = new System.Drawing.Point(100, 20);
         this.nudError1.Size = new System.Drawing.Size(80, 24);
         this.nudError1.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });

         // Error 2
         this.lblError2.Text = "Code 02:";
         this.lblError2.Location = new System.Drawing.Point(20, 52);
         this.lblError2.Size = new System.Drawing.Size(75, 20);
         this.lblError2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         this.nudError2.Location = new System.Drawing.Point(100, 52);
         this.nudError2.Size = new System.Drawing.Size(80, 24);
         this.nudError2.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });

         // Error 3
         this.lblError3.Text = "Code 03:";
         this.lblError3.Location = new System.Drawing.Point(20, 84);
         this.lblError3.Size = new System.Drawing.Size(75, 20);
         this.lblError3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         this.nudError3.Location = new System.Drawing.Point(100, 84);
         this.nudError3.Size = new System.Drawing.Size(80, 24);
         this.nudError3.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });

         // Error 4
         this.lblError4.Text = "Code 04:";
         this.lblError4.Location = new System.Drawing.Point(20, 116);
         this.lblError4.Size = new System.Drawing.Size(75, 20);
         this.lblError4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         this.nudError4.Location = new System.Drawing.Point(100, 116);
         this.nudError4.Size = new System.Drawing.Size(80, 24);
         this.nudError4.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });

         // Error 5
         this.lblError5.Text = "Code 05:";
         this.lblError5.Location = new System.Drawing.Point(20, 148);
         this.lblError5.Size = new System.Drawing.Size(75, 20);
         this.lblError5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         this.nudError5.Location = new System.Drawing.Point(100, 148);
         this.nudError5.Size = new System.Drawing.Size(80, 24);
         this.nudError5.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });

         // Error 6
         this.lblError6.Text = "Code 06:";
         this.lblError6.Location = new System.Drawing.Point(20, 180);
         this.lblError6.Size = new System.Drawing.Size(75, 20);
         this.lblError6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         this.nudError6.Location = new System.Drawing.Point(100, 180);
         this.nudError6.Size = new System.Drawing.Size(80, 24);
         this.nudError6.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });

         // Error 7 (Column 2)
         this.lblError7.Text = "Code 07:";
         this.lblError7.Location = new System.Drawing.Point(200, 20);
         this.lblError7.Size = new System.Drawing.Size(75, 20);
         this.lblError7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         this.nudError7.Location = new System.Drawing.Point(280, 20);
         this.nudError7.Size = new System.Drawing.Size(80, 24);
         this.nudError7.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });

         // Error 8
         this.lblError8.Text = "Code 08:";
         this.lblError8.Location = new System.Drawing.Point(200, 52);
         this.lblError8.Size = new System.Drawing.Size(75, 20);
         this.lblError8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         this.nudError8.Location = new System.Drawing.Point(280, 52);
         this.nudError8.Size = new System.Drawing.Size(80, 24);
         this.nudError8.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });

         // Error 9
         this.lblError9.Text = "Code 09:";
         this.lblError9.Location = new System.Drawing.Point(200, 84);
         this.lblError9.Size = new System.Drawing.Size(75, 20);
         this.lblError9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         this.nudError9.Location = new System.Drawing.Point(280, 84);
         this.nudError9.Size = new System.Drawing.Size(80, 24);
         this.nudError9.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });

         // Error 10
         this.lblError10.Text = "Code 10:";
         this.lblError10.Location = new System.Drawing.Point(200, 116);
         this.lblError10.Size = new System.Drawing.Size(75, 20);
         this.lblError10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         this.nudError10.Location = new System.Drawing.Point(280, 116);
         this.nudError10.Size = new System.Drawing.Size(80, 24);
         this.nudError10.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });

         // Error 11
         this.lblError11.Text = "Code 11:";
         this.lblError11.Location = new System.Drawing.Point(200, 148);
         this.lblError11.Size = new System.Drawing.Size(75, 20);
         this.lblError11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         this.nudError11.Location = new System.Drawing.Point(280, 148);
         this.nudError11.Size = new System.Drawing.Size(80, 24);
         this.nudError11.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });

         // Error 12
         this.lblError12.Text = "Code 12:";
         this.lblError12.Location = new System.Drawing.Point(200, 180);
         this.lblError12.Size = new System.Drawing.Size(75, 20);
         this.lblError12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         this.nudError12.Location = new System.Drawing.Point(280, 180);
         this.nudError12.Size = new System.Drawing.Size(80, 24);
         this.nudError12.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });

         // Buttons
         this.btnOK.Location = new System.Drawing.Point(100, 222);
         this.btnOK.Size = new System.Drawing.Size(90, 30);
         this.btnOK.Text = "確定";
         this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);

         this.btnCancel.Location = new System.Drawing.Point(200, 222);
         this.btnCancel.Size = new System.Drawing.Size(90, 30);
         this.btnCancel.Text = "取消";
         this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;

         this.AcceptButton = this.btnOK;
         this.CancelButton = this.btnCancel;

         // Add Controls
         this.Controls.Add(this.lblError1); this.Controls.Add(this.nudError1);
         this.Controls.Add(this.lblError2); this.Controls.Add(this.nudError2);
         this.Controls.Add(this.lblError3); this.Controls.Add(this.nudError3);
         this.Controls.Add(this.lblError4); this.Controls.Add(this.nudError4);
         this.Controls.Add(this.lblError5); this.Controls.Add(this.nudError5);
         this.Controls.Add(this.lblError6); this.Controls.Add(this.nudError6);
         this.Controls.Add(this.lblError7); this.Controls.Add(this.nudError7);
         this.Controls.Add(this.lblError8); this.Controls.Add(this.nudError8);
         this.Controls.Add(this.lblError9); this.Controls.Add(this.nudError9);
         this.Controls.Add(this.lblError10); this.Controls.Add(this.nudError10);
         this.Controls.Add(this.lblError11); this.Controls.Add(this.nudError11);
         this.Controls.Add(this.lblError12); this.Controls.Add(this.nudError12);
         this.Controls.Add(this.btnOK);
         this.Controls.Add(this.btnCancel);

         ((System.ComponentModel.ISupportInitialize)(this.nudError1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudError2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudError3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudError4)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudError5)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudError6)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudError7)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudError8)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudError9)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudError10)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudError11)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudError12)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();
      }

      #endregion

      private System.Windows.Forms.NumericUpDown nudError1;
      private System.Windows.Forms.NumericUpDown nudError2;
      private System.Windows.Forms.NumericUpDown nudError3;
      private System.Windows.Forms.NumericUpDown nudError4;
      private System.Windows.Forms.NumericUpDown nudError5;
      private System.Windows.Forms.NumericUpDown nudError6;
      private System.Windows.Forms.NumericUpDown nudError7;
      private System.Windows.Forms.NumericUpDown nudError8;
      private System.Windows.Forms.NumericUpDown nudError9;
      private System.Windows.Forms.NumericUpDown nudError10;
      private System.Windows.Forms.NumericUpDown nudError11;
      private System.Windows.Forms.NumericUpDown nudError12;
      private System.Windows.Forms.Label lblError1;
      private System.Windows.Forms.Label lblError2;
      private System.Windows.Forms.Label lblError3;
      private System.Windows.Forms.Label lblError4;
      private System.Windows.Forms.Label lblError5;
      private System.Windows.Forms.Label lblError6;
      private System.Windows.Forms.Label lblError7;
      private System.Windows.Forms.Label lblError8;
      private System.Windows.Forms.Label lblError9;
      private System.Windows.Forms.Label lblError10;
      private System.Windows.Forms.Label lblError11;
      private System.Windows.Forms.Label lblError12;
      private System.Windows.Forms.Button btnOK;
      private System.Windows.Forms.Button btnCancel;
   }
}
