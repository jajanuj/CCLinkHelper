namespace WindowsFormsApp1
{
   partial class Form1
   {
      /// <summary>
      /// 設計工具所需的變數。
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
      private System.Windows.Forms.GroupBox grpConnectionMode;
      private System.Windows.Forms.RadioButton rbMockMode;
      private System.Windows.Forms.RadioButton rbRealMode;
      private System.Windows.Forms.Button btnOpen;
      private System.Windows.Forms.Button btnRead;
      private System.Windows.Forms.Button btnWrite;
      private System.Windows.Forms.Button btnClose;
      private System.Windows.Forms.Button btnStartTimeSync;
      private System.Windows.Forms.Button btnStopTimeSync;
      private System.Windows.Forms.Button btnForceTimeSync;
      private System.Windows.Forms.ListBox lstLog;
      private System.Windows.Forms.Label lblStatus;
      private System.Windows.Forms.GroupBox grpManualTime;
      private System.Windows.Forms.DateTimePicker dtpDate;
      private System.Windows.Forms.DateTimePicker dtpTime;
      private System.Windows.Forms.Button btnSetTimeToPlc;
      private System.Windows.Forms.Button btnSyncFromPc;

      /// <summary>
      /// 清除任何使用中的資源。
      /// </summary>
      /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Windows Form 設計工具產生的程式碼

      /// <summary>
      /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
      /// 這個方法的內容。
      /// </summary>
      private void InitializeComponent()
      {
         this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
         this.grpConnectionMode = new System.Windows.Forms.GroupBox();
         this.rbMockMode = new System.Windows.Forms.RadioButton();
         this.rbRealMode = new System.Windows.Forms.RadioButton();
         this.btnOpen = new System.Windows.Forms.Button();
         this.btnRead = new System.Windows.Forms.Button();
         this.btnWrite = new System.Windows.Forms.Button();
         this.btnClose = new System.Windows.Forms.Button();
         this.btnStartTimeSync = new System.Windows.Forms.Button();
         this.btnStopTimeSync = new System.Windows.Forms.Button();
         this.btnForceTimeSync = new System.Windows.Forms.Button();
         this.btnStartHeartbeat = new System.Windows.Forms.Button();
         this.btnStopHeartbeat = new System.Windows.Forms.Button();
         this.btnStopSimulator = new System.Windows.Forms.Button();
         this.btnPlcSettings = new System.Windows.Forms.Button();
         this.btnScanMonitor = new System.Windows.Forms.Button();
         this.grpManualTime = new System.Windows.Forms.GroupBox();
         this.dtpDate = new System.Windows.Forms.DateTimePicker();
         this.dtpTime = new System.Windows.Forms.DateTimePicker();
         this.btnSyncFromPc = new System.Windows.Forms.Button();
         this.btnSetTimeToPlc = new System.Windows.Forms.Button();
         this.lstLog = new System.Windows.Forms.ListBox();
         this.lblStatus = new System.Windows.Forms.Label();
         this.grpManualTime.SuspendLayout();
         this.grpConnectionMode.SuspendLayout();
         this.flowLayoutPanel1.SuspendLayout();
         this.SuspendLayout();
         //
         // flowLayoutPanel1
         //
         this.flowLayoutPanel1.AutoSize = true;
         this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
         this.flowLayoutPanel1.Controls.Add(this.grpConnectionMode);
         this.flowLayoutPanel1.Controls.Add(this.btnOpen);
         this.flowLayoutPanel1.Controls.Add(this.btnRead);
         this.flowLayoutPanel1.Controls.Add(this.btnWrite);
         this.flowLayoutPanel1.Controls.Add(this.btnClose);
         this.flowLayoutPanel1.Controls.Add(this.btnStartTimeSync);
         this.flowLayoutPanel1.Controls.Add(this.btnStopTimeSync);
         this.flowLayoutPanel1.Controls.Add(this.btnForceTimeSync);
         this.flowLayoutPanel1.Controls.Add(this.btnStartHeartbeat);
         this.flowLayoutPanel1.Controls.Add(this.btnStopHeartbeat);
         this.flowLayoutPanel1.Controls.Add(this.btnStopSimulator);
         this.flowLayoutPanel1.Controls.Add(this.btnPlcSettings);
         this.flowLayoutPanel1.Controls.Add(this.btnScanMonitor);
         this.flowLayoutPanel1.Controls.Add(this.grpManualTime);
         this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
         this.flowLayoutPanel1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
         this.flowLayoutPanel1.Name = "flowLayoutPanel1";
         this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(10);
         this.flowLayoutPanel1.Size = new System.Drawing.Size(832, 112);
         this.flowLayoutPanel1.TabIndex = 0;
         //
         // grpConnectionMode
         //
         this.grpConnectionMode.Controls.Add(this.rbMockMode);
         this.grpConnectionMode.Controls.Add(this.rbRealMode);
         this.grpConnectionMode.Location = new System.Drawing.Point(20, 20);
         this.grpConnectionMode.Margin = new System.Windows.Forms.Padding(10);
         this.grpConnectionMode.Name = "grpConnectionMode";
         this.grpConnectionMode.Size = new System.Drawing.Size(200, 80);
         this.grpConnectionMode.TabIndex = 15;
         this.grpConnectionMode.TabStop = false;
         this.grpConnectionMode.Text = "連接模式";
         //
         // rbMockMode
         //
         this.rbMockMode.AutoSize = true;
         this.rbMockMode.Checked = true;
         this.rbMockMode.Location = new System.Drawing.Point(15, 25);
         this.rbMockMode.Name = "rbMockMode";
         this.rbMockMode.Size = new System.Drawing.Size(109, 18);
         this.rbMockMode.TabIndex = 0;
         this.rbMockMode.TabStop = true;
         this.rbMockMode.Text = "模擬模式 (Mock)";
         this.rbMockMode.UseVisualStyleBackColor = true;
         //
         // rbRealMode
         //
         this.rbRealMode.AutoSize = true;
         this.rbRealMode.Location = new System.Drawing.Point(15, 50);
         this.rbRealMode.Name = "rbRealMode";
         this.rbRealMode.Size = new System.Drawing.Size(109, 18);
         this.rbRealMode.TabIndex = 1;
         this.rbRealMode.Text = "實際連接 (Real)";
         this.rbRealMode.UseVisualStyleBackColor = true;
         //
         // btnOpen
         //
         this.btnOpen.AutoSize = true;
         this.btnOpen.Location = new System.Drawing.Point(240, 20);
         this.btnOpen.Margin = new System.Windows.Forms.Padding(10);
         this.btnOpen.Name = "btnOpen";
         this.btnOpen.Size = new System.Drawing.Size(75, 26);
         this.btnOpen.TabIndex = 0;
         this.btnOpen.Text = "Open";
         this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
         //
         // btnRead
         //
         this.btnRead.AutoSize = true;
         this.btnRead.Location = new System.Drawing.Point(115, 20);
         this.btnRead.Margin = new System.Windows.Forms.Padding(10);
         this.btnRead.Name = "btnRead";
         this.btnRead.Size = new System.Drawing.Size(75, 26);
         this.btnRead.TabIndex = 1;
         this.btnRead.Text = "Read";
         this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
         //
         // btnWrite
         //
         this.btnWrite.AutoSize = true;
         this.btnWrite.Location = new System.Drawing.Point(210, 20);
         this.btnWrite.Margin = new System.Windows.Forms.Padding(10);
         this.btnWrite.Name = "btnWrite";
         this.btnWrite.Size = new System.Drawing.Size(75, 26);
         this.btnWrite.TabIndex = 2;
         this.btnWrite.Text = "Write";
         //
         // btnClose
         //
         this.btnClose.AutoSize = true;
         this.btnClose.Location = new System.Drawing.Point(305, 20);
         this.btnClose.Margin = new System.Windows.Forms.Padding(10);
         this.btnClose.Name = "btnClose";
         this.btnClose.Size = new System.Drawing.Size(75, 26);
         this.btnClose.TabIndex = 3;
         this.btnClose.Text = "Close";
         this.btnClose.Enabled = false;
         this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
         //
         // btnStartTimeSync
         //
         this.btnStartTimeSync.AutoSize = true;
         this.btnStartTimeSync.Location = new System.Drawing.Point(400, 20);
         this.btnStartTimeSync.Margin = new System.Windows.Forms.Padding(10);
         this.btnStartTimeSync.Name = "btnStartTimeSync";
         this.btnStartTimeSync.Size = new System.Drawing.Size(115, 26);
         this.btnStartTimeSync.TabIndex = 6;
         this.btnStartTimeSync.Text = "Start TimeSync";
         this.btnStartTimeSync.Click += new System.EventHandler(this.btnStartTimeSync_Click);
         //
         // btnStopTimeSync
         //
         this.btnStopTimeSync.AutoSize = true;
         this.btnStopTimeSync.Location = new System.Drawing.Point(535, 20);
         this.btnStopTimeSync.Margin = new System.Windows.Forms.Padding(10);
         this.btnStopTimeSync.Name = "btnStopTimeSync";
         this.btnStopTimeSync.Size = new System.Drawing.Size(108, 26);
         this.btnStopTimeSync.TabIndex = 12;
         this.btnStopTimeSync.Text = "Stop TimeSync";
         this.btnStopTimeSync.Click += new System.EventHandler(this.btnStopTimeSync_Click);
         //
         // btnForceTimeSync
         //
         this.btnForceTimeSync.Location = new System.Drawing.Point(663, 20);
         this.btnForceTimeSync.Margin = new System.Windows.Forms.Padding(10);
         this.btnForceTimeSync.Name = "btnForceTimeSync";
         this.btnForceTimeSync.Size = new System.Drawing.Size(137, 23);
         this.btnForceTimeSync.TabIndex = 13;
         this.btnForceTimeSync.Text = "Force Time Sync";
         this.btnForceTimeSync.Click += new System.EventHandler(this.btnForceTimeSync_Click);
         //
         // btnStartHeartbeat
         //
         this.btnStartHeartbeat.Location = new System.Drawing.Point(20, 66);
         this.btnStartHeartbeat.Margin = new System.Windows.Forms.Padding(10);
         this.btnStartHeartbeat.Name = "btnStartHeartbeat";
         this.btnStartHeartbeat.Size = new System.Drawing.Size(132, 26);
         this.btnStartHeartbeat.TabIndex = 8;
         this.btnStartHeartbeat.Text = "Start Heartbeat";
         this.btnStartHeartbeat.UseVisualStyleBackColor = true;
         this.btnStartHeartbeat.Click += new System.EventHandler(this.btnStartHeartbeat_Click);
         //
         // btnStopHeartbeat
         //
         this.btnStopHeartbeat.Location = new System.Drawing.Point(172, 66);
         this.btnStopHeartbeat.Margin = new System.Windows.Forms.Padding(10);
         this.btnStopHeartbeat.Name = "btnStopHeartbeat";
         this.btnStopHeartbeat.Size = new System.Drawing.Size(132, 26);
         this.btnStopHeartbeat.TabIndex = 11;
         this.btnStopHeartbeat.Text = "Stop Heartbeat";
         this.btnStopHeartbeat.UseVisualStyleBackColor = true;
         this.btnStopHeartbeat.Click += new System.EventHandler(this.btnStopHeartbeat_Click);
         //
         // btnStopSimulator
         //
         this.btnStopSimulator.Enabled = false;
         this.btnStopSimulator.Location = new System.Drawing.Point(324, 66);
         this.btnStopSimulator.Margin = new System.Windows.Forms.Padding(10);
         this.btnStopSimulator.Name = "btnStopSimulator";
         this.btnStopSimulator.Size = new System.Drawing.Size(132, 26);
         this.btnStopSimulator.TabIndex = 9;
         this.btnStopSimulator.Text = "Stop Simulator";
         this.btnStopSimulator.UseVisualStyleBackColor = true;
         this.btnStopSimulator.Click += new System.EventHandler(this.btnStopSimulator_Click);
         //
         // btnPlcSettings
         //
         this.btnPlcSettings.Location = new System.Drawing.Point(476, 66);
         this.btnPlcSettings.Margin = new System.Windows.Forms.Padding(10);
         this.btnPlcSettings.Name = "btnPlcSettings";
         this.btnPlcSettings.Size = new System.Drawing.Size(132, 26);
         this.btnPlcSettings.TabIndex = 10;
         this.btnPlcSettings.Text = "Show Settings";
         this.btnPlcSettings.UseVisualStyleBackColor = true;
         this.btnPlcSettings.Click += new System.EventHandler(this.btnPlcSettings_Click);
         //
         // btnScanMonitor
         //
         this.btnScanMonitor.Location = new System.Drawing.Point(628, 66);
         this.btnScanMonitor.Margin = new System.Windows.Forms.Padding(10);
         this.btnScanMonitor.Name = "btnScanMonitor";
         this.btnScanMonitor.Size = new System.Drawing.Size(132, 26);
         this.btnScanMonitor.TabIndex = 14;
         this.btnScanMonitor.Text = "Scan Monitor";
         this.btnScanMonitor.UseVisualStyleBackColor = true;
         this.btnScanMonitor.Click += new System.EventHandler(this.btnScanMonitor_Click);
         //
         // grpManualTime
         //
         this.grpManualTime.Controls.Add(this.dtpDate);
         this.grpManualTime.Controls.Add(this.dtpTime);
         this.grpManualTime.Controls.Add(this.btnSyncFromPc);
         this.grpManualTime.Controls.Add(this.btnSetTimeToPlc);
         this.grpManualTime.Location = new System.Drawing.Point(618, 66);
         this.grpManualTime.Margin = new System.Windows.Forms.Padding(10);
         this.grpManualTime.Name = "grpManualTime";
         this.grpManualTime.Size = new System.Drawing.Size(300, 80);
         this.grpManualTime.TabIndex = 11;
         this.grpManualTime.TabStop = false;
         this.grpManualTime.Text = "手動設定時間";
         // 
         // dtpDate
         // 
         this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
         this.dtpDate.Location = new System.Drawing.Point(6, 21);
         this.dtpDate.Name = "dtpDate";
         this.dtpDate.Size = new System.Drawing.Size(100, 22);
         this.dtpDate.TabIndex = 0;
         // 
         // dtpTime
         // 
         this.dtpTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
         this.dtpTime.Location = new System.Drawing.Point(6, 49);
         this.dtpTime.Name = "dtpTime";
         this.dtpTime.ShowUpDown = true;
         this.dtpTime.Size = new System.Drawing.Size(100, 22);
         this.dtpTime.TabIndex = 1;
         // 
         // btnSyncFromPc
         // 
         this.btnSyncFromPc.Location = new System.Drawing.Point(112, 19);
         this.btnSyncFromPc.Name = "btnSyncFromPc";
         this.btnSyncFromPc.Size = new System.Drawing.Size(150, 25);
         this.btnSyncFromPc.TabIndex = 2;
         this.btnSyncFromPc.Text = "同步電腦時間";
         this.btnSyncFromPc.UseVisualStyleBackColor = true;
         this.btnSyncFromPc.Click += new System.EventHandler(this.btnSyncFromPc_Click);
         // 
         // btnSetTimeToPlc
         // 
         this.btnSetTimeToPlc.Location = new System.Drawing.Point(112, 47);
         this.btnSetTimeToPlc.Name = "btnSetTimeToPlc";
         this.btnSetTimeToPlc.Size = new System.Drawing.Size(150, 25);
         this.btnSetTimeToPlc.TabIndex = 3;
         this.btnSetTimeToPlc.Text = "寫入時間至 PLC";
         this.btnSetTimeToPlc.UseVisualStyleBackColor = true;
         this.btnSetTimeToPlc.Click += new System.EventHandler(this.btnSetTimeToPlc_Click);
         // 
         // lstLog
         // 
         this.lstLog.Dock = System.Windows.Forms.DockStyle.Fill;
         this.lstLog.FormattingEnabled = true;
         this.lstLog.ItemHeight = 12;
         this.lstLog.Location = new System.Drawing.Point(0, 112);
         this.lstLog.Name = "lstLog";
         this.lstLog.Size = new System.Drawing.Size(832, 314);
         this.lstLog.TabIndex = 10;
         // 
         // lblStatus
         // 
         this.lblStatus.BackColor = System.Drawing.Color.LightGreen;
         this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.lblStatus.Location = new System.Drawing.Point(0, 426);
         this.lblStatus.Name = "lblStatus";
         this.lblStatus.Size = new System.Drawing.Size(832, 24);
         this.lblStatus.TabIndex = 11;
         this.lblStatus.Text = "Status";
         this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // Form1
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
         this.ClientSize = new System.Drawing.Size(832, 450);
         this.Controls.Add(this.lstLog);
         this.Controls.Add(this.lblStatus);
         this.Controls.Add(this.flowLayoutPanel1);
         this.Name = "Form1";
         this.Text = "CC-Link Helper";
         this.grpConnectionMode.ResumeLayout(false);
         this.grpConnectionMode.PerformLayout();
         this.grpManualTime.ResumeLayout(false);
         this.flowLayoutPanel1.ResumeLayout(false);
         this.flowLayoutPanel1.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Button btnStartHeartbeat;
      private System.Windows.Forms.Button btnStopSimulator;
      private System.Windows.Forms.Button btnPlcSettings;
      private System.Windows.Forms.Button btnStopHeartbeat;
      private System.Windows.Forms.Button btnScanMonitor;
   }
}

