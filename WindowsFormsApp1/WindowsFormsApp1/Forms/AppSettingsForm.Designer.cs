namespace WindowsFormsApp1.Forms
{
   partial class AppSettingsForm
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
         this.tabApp = new System.Windows.Forms.TabPage();
         this.label3 = new System.Windows.Forms.Label();
         this.numHeartbeat = new System.Windows.Forms.NumericUpDown();
         this.label11 = new System.Windows.Forms.Label();
         this.numTimeSync = new System.Windows.Forms.NumericUpDown();
         this.label12 = new System.Windows.Forms.Label();
         this.txtTrigger = new System.Windows.Forms.TextBox();
         this.label13 = new System.Windows.Forms.Label();
         this.txtData = new System.Windows.Forms.TextBox();

         ((System.ComponentModel.ISupportInitialize)(this.numHeartbeat)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numTimeSync)).BeginInit();
         this.tabApp.SuspendLayout();
         this.SuspendLayout();

         // 
         // tabApp
         // 
         this.tabApp.Controls.Add(this.label3);
         this.tabApp.Controls.Add(this.numHeartbeat);
         this.tabApp.Controls.Add(this.label11);
         this.tabApp.Controls.Add(this.numTimeSync);
         this.tabApp.Controls.Add(this.label12);
         this.tabApp.Controls.Add(this.txtTrigger);
         this.tabApp.Controls.Add(this.label13);
         this.tabApp.Controls.Add(this.txtData);
         this.tabApp.Location = new System.Drawing.Point(4, 22);
         this.tabApp.Name = "tabApp";
         this.tabApp.Padding = new System.Windows.Forms.Padding(3);
         this.tabApp.Size = new System.Drawing.Size(368, 394);
         this.tabApp.TabIndex = 1;
         this.tabApp.Text = "進階設定";
         this.tabApp.UseVisualStyleBackColor = true;

         // 
         // label3 (Heartbeat Interval)
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(10, 17);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(62, 12);
         this.label3.TabIndex = 0;
         this.label3.Text = "HB Int (ms):";
         // 
         // numHeartbeat
         // 
         this.numHeartbeat.Location = new System.Drawing.Point(100, 15);
         this.numHeartbeat.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
         this.numHeartbeat.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
         this.numHeartbeat.Name = "numHeartbeat";
         this.numHeartbeat.Size = new System.Drawing.Size(80, 22);
         this.numHeartbeat.TabIndex = 1;
         this.numHeartbeat.Value = new decimal(new int[] { 300, 0, 0, 0 });

         // 
         // label11 (TimeSync Interval)
         // 
         this.label11.AutoSize = true;
         this.label11.Location = new System.Drawing.Point(10, 47);
         this.label11.Name = "label11";
         this.label11.Size = new System.Drawing.Size(63, 12);
         this.label11.TabIndex = 2;
         this.label11.Text = "TS Int (ms):";
         // 
         // numTimeSync
         // 
         this.numTimeSync.Location = new System.Drawing.Point(100, 45);
         this.numTimeSync.Maximum = new decimal(new int[] { 3600000, 0, 0, 0 });
         this.numTimeSync.Name = "numTimeSync";
         this.numTimeSync.Size = new System.Drawing.Size(80, 22);
         this.numTimeSync.TabIndex = 3;
         this.numTimeSync.Value = new decimal(new int[] { 1000, 0, 0, 0 });

         // 
         // label12 (Trigger)
         // 
         this.label12.AutoSize = true;
         this.label12.Location = new System.Drawing.Point(10, 77);
         this.label12.Name = "label12";
         this.label12.Size = new System.Drawing.Size(61, 12);
         this.label12.TabIndex = 4;
         this.label12.Text = "TS Trigger:";
         // 
         // txtTrigger
         // 
         this.txtTrigger.Location = new System.Drawing.Point(100, 75);
         this.txtTrigger.Name = "txtTrigger";
         this.txtTrigger.Size = new System.Drawing.Size(80, 22);
         this.txtTrigger.TabIndex = 5;

         // 
         // label13 (Data)
         // 
         this.label13.AutoSize = true;
         this.label13.Location = new System.Drawing.Point(10, 107);
         this.label13.Name = "label13";
         this.label13.Size = new System.Drawing.Size(46, 12);
         this.label13.TabIndex = 6;
         this.label13.Text = "TS Data:";
         // 
         // txtData
         // 
         this.txtData.Location = new System.Drawing.Point(100, 105);
         this.txtData.Name = "txtData";
         this.txtData.Size = new System.Drawing.Size(80, 22);
         this.txtData.TabIndex = 7;

         // 
         // AppSettingsForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(400, 480);
         this.Name = "AppSettingsForm";
         this.Text = "AppSettingsForm";
         
         // Add inherited tab control page
         this.tabControl1.Controls.Add(this.tabApp);

         ((System.ComponentModel.ISupportInitialize)(this.numHeartbeat)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numTimeSync)).EndInit();
         this.tabApp.ResumeLayout(false);
         this.tabApp.PerformLayout();
         this.ResumeLayout(false);
      }

      #endregion

      private System.Windows.Forms.TabPage tabApp;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.NumericUpDown numHeartbeat;
      private System.Windows.Forms.Label label11;
      private System.Windows.Forms.NumericUpDown numTimeSync;
      private System.Windows.Forms.Label label12;
      private System.Windows.Forms.TextBox txtTrigger;
      private System.Windows.Forms.Label label13;
      private System.Windows.Forms.TextBox txtData;
   }
}
