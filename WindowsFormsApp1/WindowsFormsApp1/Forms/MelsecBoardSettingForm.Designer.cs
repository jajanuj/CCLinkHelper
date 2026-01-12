namespace WindowsFormsApp1.Forms
{
   partial class MelsecBoardSettingForm
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

         this.label5 = new System.Windows.Forms.Label();
         this.numPort = new System.Windows.Forms.NumericUpDown();
         this.label6 = new System.Windows.Forms.Label();
         this.numNetwork = new System.Windows.Forms.NumericUpDown();
         this.label7 = new System.Windows.Forms.Label();
         this.numTimeout = new System.Windows.Forms.NumericUpDown();
         this.label8 = new System.Windows.Forms.Label();
         this.numRetryCount = new System.Windows.Forms.NumericUpDown();
         this.label9 = new System.Windows.Forms.Label();
         this.numRetryBackoff = new System.Windows.Forms.NumericUpDown();
         this.label2 = new System.Windows.Forms.Label();
         this.numStation = new System.Windows.Forms.NumericUpDown();
         this.chkIsx64 = new System.Windows.Forms.CheckBox();
         this.label10 = new System.Windows.Forms.Label();
         this.cmbEndian = new System.Windows.Forms.ComboBox();

         ((System.ComponentModel.ISupportInitialize)(this.numHeartbeat)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numTimeSync)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numNetwork)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numRetryCount)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numRetryBackoff)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numStation)).BeginInit();
         this.tabApp.SuspendLayout();
         this.tabGeneral.SuspendLayout();
         this.SuspendLayout();

         // 
         // tabGeneral (Inherited)
         // 
         // 
         // tabGeneral (Inherited)
         // 
         // Reposition dgvRanges to make room for Board settings
         this.dgvRanges.Location = new System.Drawing.Point(10, 160);
         this.dgvRanges.Size = new System.Drawing.Size(350, 220);
         // Reposition label4 (Scan Ranges label) to match grid
         this.label4.Location = new System.Drawing.Point(10, 145);

         this.tabGeneral.Controls.Add(this.label5);
         this.tabGeneral.Controls.Add(this.numPort);
         this.tabGeneral.Controls.Add(this.label6);
         this.tabGeneral.Controls.Add(this.numNetwork);
         this.tabGeneral.Controls.Add(this.label7);
         this.tabGeneral.Controls.Add(this.numTimeout);
         this.tabGeneral.Controls.Add(this.label8);
         this.tabGeneral.Controls.Add(this.numRetryCount);
         this.tabGeneral.Controls.Add(this.label9);
         this.tabGeneral.Controls.Add(this.numRetryBackoff);
         this.tabGeneral.Controls.Add(this.label2);
         this.tabGeneral.Controls.Add(this.numStation);
         this.tabGeneral.Controls.Add(this.chkIsx64);
         this.tabGeneral.Controls.Add(this.label10);
         this.tabGeneral.Controls.Add(this.cmbEndian);

         // label5 (Port)
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(10, 17);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(27, 12);
         this.label5.TabIndex = 0;
         this.label5.Text = "Port:";
         // numPort
         this.numPort.Location = new System.Drawing.Point(70, 15);
         this.numPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
         this.numPort.Name = "numPort";
         this.numPort.Size = new System.Drawing.Size(80, 22);
         this.numPort.TabIndex = 1;
         // label6 (Network)
         this.label6.AutoSize = true;
         this.label6.Location = new System.Drawing.Point(10, 45);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(48, 12);
         this.label6.TabIndex = 2;
         this.label6.Text = "Network:";
         // numNetwork
         this.numNetwork.Location = new System.Drawing.Point(70, 43);
         this.numNetwork.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
         this.numNetwork.Name = "numNetwork";
         this.numNetwork.Size = new System.Drawing.Size(60, 22);
         this.numNetwork.TabIndex = 3;

         // label7 (Timeout)
         this.label7.AutoSize = true;
         this.label7.Location = new System.Drawing.Point(180, 17);
         this.label7.Name = "label7";
         this.label7.Size = new System.Drawing.Size(71, 12);
         this.label7.TabIndex = 4;
         this.label7.Text = "Timeout (ms):";
         // numTimeout
         this.numTimeout.Location = new System.Drawing.Point(260, 15);
         this.numTimeout.Maximum = new decimal(new int[] { 60000, 0, 0, 0 });
         this.numTimeout.Name = "numTimeout";
         this.numTimeout.Size = new System.Drawing.Size(80, 22);
         this.numTimeout.TabIndex = 5;
         this.numTimeout.Value = new decimal(new int[] { 3000, 0, 0, 0 });

         // label8 (Retry Count)
         this.label8.AutoSize = true;
         this.label8.Location = new System.Drawing.Point(180, 45);
         this.label8.Name = "label8";
         this.label8.Size = new System.Drawing.Size(65, 12);
         this.label8.TabIndex = 6;
         this.label8.Text = "Retry Count:";
         // numRetryCount
         this.numRetryCount.Location = new System.Drawing.Point(260, 43);
         this.numRetryCount.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
         this.numRetryCount.Name = "numRetryCount";
         this.numRetryCount.Size = new System.Drawing.Size(60, 22);
         this.numRetryCount.TabIndex = 7;
         this.numRetryCount.Value = new decimal(new int[] { 3, 0, 0, 0 });

         // label9 (Retry Backoff)
         this.label9.AutoSize = true;
         this.label9.Location = new System.Drawing.Point(180, 73);
         this.label9.Name = "label9";
         this.label9.Size = new System.Drawing.Size(76, 12);
         this.label9.TabIndex = 8;
         this.label9.Text = "Retry Backoff:";
         // numRetryBackoff
         this.numRetryBackoff.Location = new System.Drawing.Point(260, 71);
         this.numRetryBackoff.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
         this.numRetryBackoff.Name = "numRetryBackoff";
         this.numRetryBackoff.Size = new System.Drawing.Size(80, 22);
         this.numRetryBackoff.TabIndex = 9;
         this.numRetryBackoff.Value = new decimal(new int[] { 50, 0, 0, 0 });

         // label2 (Station)
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(10, 101);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(40, 12);
         this.label2.TabIndex = 10;
         this.label2.Text = "Station:";
         // numStation
         this.numStation.Location = new System.Drawing.Point(70, 99);
         this.numStation.Maximum = new decimal(new int[] { 64, 0, 0, 0 });
         this.numStation.Name = "numStation";
         this.numStation.Size = new System.Drawing.Size(60, 22);
         this.numStation.TabIndex = 11;

         // chkIsx64
         this.chkIsx64.AutoSize = true;
         this.chkIsx64.Location = new System.Drawing.Point(182, 101);
         this.chkIsx64.Name = "chkIsx64";
         this.chkIsx64.Size = new System.Drawing.Size(71, 16);
         this.chkIsx64.TabIndex = 12;
         this.chkIsx64.Text = "Is 64-bit?";
         this.chkIsx64.UseVisualStyleBackColor = true;

         // label10 (Endian)
         this.label10.AutoSize = true;
         this.label10.Location = new System.Drawing.Point(10, 129);
         this.label10.Name = "label10";
         this.label10.Size = new System.Drawing.Size(41, 12);
         this.label10.TabIndex = 13;
         this.label10.Text = "Endian:";
         // cmbEndian
         this.cmbEndian.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cmbEndian.FormattingEnabled = true;
         this.cmbEndian.Items.AddRange(new object[] { "Big", "Little" });
         this.cmbEndian.Location = new System.Drawing.Point(70, 127);
         this.cmbEndian.Name = "cmbEndian";
         this.cmbEndian.Size = new System.Drawing.Size(80, 20);
         this.cmbEndian.TabIndex = 14;

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
         // MelsecBoardSettingForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(400, 480);
         this.Name = "MelsecBoardSettingForm";
         this.Text = "MelsecBoardSettingForm";
         
         // Add inherited tab control page
         this.tabControl1.Controls.Add(this.tabApp);

         ((System.ComponentModel.ISupportInitialize)(this.numHeartbeat)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numTimeSync)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numNetwork)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numRetryCount)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numRetryBackoff)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numStation)).EndInit();
         this.tabApp.ResumeLayout(false);
         this.tabApp.PerformLayout();
         this.tabGeneral.ResumeLayout(false);
         this.tabGeneral.PerformLayout();
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

      // Board Controls
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.NumericUpDown numPort;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.NumericUpDown numNetwork;
      private System.Windows.Forms.Label label7;
      private System.Windows.Forms.NumericUpDown numTimeout;
      private System.Windows.Forms.Label label8;
      private System.Windows.Forms.NumericUpDown numRetryCount;
      private System.Windows.Forms.Label label9;
      private System.Windows.Forms.NumericUpDown numRetryBackoff;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.NumericUpDown numStation;
      private System.Windows.Forms.CheckBox chkIsx64;
      private System.Windows.Forms.Label label10;
      private System.Windows.Forms.ComboBox cmbEndian;
   }
}
