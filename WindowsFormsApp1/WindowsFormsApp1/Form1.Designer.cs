namespace WindowsFormsApp1
{
   partial class Form1
   {
      /// <summary>
      /// 設計工具所需的變數。
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
      private System.Windows.Forms.Button btnOpen;
      private System.Windows.Forms.Button btnRead;
      private System.Windows.Forms.Button btnWrite;
      private System.Windows.Forms.Button btnClose;
      private System.Windows.Forms.Button btnStartTimeSync;
      private System.Windows.Forms.Button btnForceTimeSync;
      private System.Windows.Forms.ListBox lstLog;
      private System.Windows.Forms.Label lblStatus;

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
         this.btnOpen = new System.Windows.Forms.Button();
         this.btnRead = new System.Windows.Forms.Button();
         this.btnWrite = new System.Windows.Forms.Button();
         this.btnClose = new System.Windows.Forms.Button();
         this.btnStartTimeSync = new System.Windows.Forms.Button();
         this.btnForceTimeSync = new System.Windows.Forms.Button();
         this.button1 = new System.Windows.Forms.Button();
         this.lstLog = new System.Windows.Forms.ListBox();
         this.lblStatus = new System.Windows.Forms.Label();
         this.btnStopSimulator = new System.Windows.Forms.Button();
         this.flowLayoutPanel1.SuspendLayout();
         this.SuspendLayout();
         // 
         // flowLayoutPanel1
         // 
         this.flowLayoutPanel1.AutoSize = true;
         this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
         this.flowLayoutPanel1.Controls.Add(this.btnOpen);
         this.flowLayoutPanel1.Controls.Add(this.btnRead);
         this.flowLayoutPanel1.Controls.Add(this.btnWrite);
         this.flowLayoutPanel1.Controls.Add(this.btnClose);
         this.flowLayoutPanel1.Controls.Add(this.btnStartTimeSync);
         this.flowLayoutPanel1.Controls.Add(this.btnForceTimeSync);
         this.flowLayoutPanel1.Controls.Add(this.button1);
         this.flowLayoutPanel1.Controls.Add(this.btnStopSimulator);
         this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
         this.flowLayoutPanel1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
         this.flowLayoutPanel1.Name = "flowLayoutPanel1";
         this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(10);
         this.flowLayoutPanel1.Size = new System.Drawing.Size(800, 112);
         this.flowLayoutPanel1.TabIndex = 0;
         // 
         // btnOpen
         // 
         this.btnOpen.AutoSize = true;
         this.btnOpen.Location = new System.Drawing.Point(20, 20);
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
         // btnForceTimeSync
         // 
         this.btnForceTimeSync.AutoSize = true;
         this.btnForceTimeSync.Location = new System.Drawing.Point(535, 20);
         this.btnForceTimeSync.Margin = new System.Windows.Forms.Padding(10);
         this.btnForceTimeSync.Name = "btnForceTimeSync";
         this.btnForceTimeSync.Size = new System.Drawing.Size(115, 26);
         this.btnForceTimeSync.TabIndex = 7;
         this.btnForceTimeSync.Text = "Force TimeSync";
         this.btnForceTimeSync.Click += new System.EventHandler(this.btnForceTimeSync_Click);
         // 
         // button1
         // 
         this.button1.Location = new System.Drawing.Point(20, 66);
         this.button1.Margin = new System.Windows.Forms.Padding(10);
         this.button1.Name = "button1";
         this.button1.Size = new System.Drawing.Size(132, 26);
         this.button1.TabIndex = 8;
         this.button1.Text = "Start Heartbeat";
         this.button1.UseVisualStyleBackColor = true;
         this.button1.Click += new System.EventHandler(this.btnStartHeartbeat_Click);
         // 
         // lstLog
         // 
         this.lstLog.Dock = System.Windows.Forms.DockStyle.Fill;
         this.lstLog.FormattingEnabled = true;
         this.lstLog.ItemHeight = 12;
         this.lstLog.Location = new System.Drawing.Point(0, 112);
         this.lstLog.Name = "lstLog";
         this.lstLog.Size = new System.Drawing.Size(800, 314);
         this.lstLog.TabIndex = 10;
         // 
         // lblStatus
         // 
         this.lblStatus.BackColor = System.Drawing.Color.LightGreen;
         this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.lblStatus.Location = new System.Drawing.Point(0, 426);
         this.lblStatus.Name = "lblStatus";
         this.lblStatus.Size = new System.Drawing.Size(800, 24);
         this.lblStatus.TabIndex = 11;
         this.lblStatus.Text = "Status";
         this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // btnStopSimulator
         // 
         this.btnStopSimulator.Location = new System.Drawing.Point(172, 66);
         this.btnStopSimulator.Margin = new System.Windows.Forms.Padding(10);
         this.btnStopSimulator.Name = "btnStopSimulator";
         this.btnStopSimulator.Size = new System.Drawing.Size(132, 26);
         this.btnStopSimulator.TabIndex = 9;
         this.btnStopSimulator.Text = "Stop Simulator";
         this.btnStopSimulator.UseVisualStyleBackColor = true;
         this.btnStopSimulator.Click += new System.EventHandler(this.btnStopSimulator_Click);
         // 
         // Form1
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
         this.ClientSize = new System.Drawing.Size(800, 450);
         this.Controls.Add(this.lstLog);
         this.Controls.Add(this.lblStatus);
         this.Controls.Add(this.flowLayoutPanel1);
         this.Name = "Form1";
         this.Text = "CC-Link Helper";
         this.flowLayoutPanel1.ResumeLayout(false);
         this.flowLayoutPanel1.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Button button1;
      private System.Windows.Forms.Button btnStopSimulator;
   }
}

