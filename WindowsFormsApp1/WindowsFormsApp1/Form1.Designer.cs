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
      private System.Windows.Forms.Button btnStartHeartbeat;
      private System.Windows.Forms.Button btnStopHeartbeat;
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
         this.components = new System.ComponentModel.Container();
         this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
         this.btnOpen = new System.Windows.Forms.Button();
         this.btnRead = new System.Windows.Forms.Button();
         this.btnWrite = new System.Windows.Forms.Button();
         this.btnClose = new System.Windows.Forms.Button();
         this.btnStartHeartbeat = new System.Windows.Forms.Button();
         this.btnStopHeartbeat = new System.Windows.Forms.Button();
         this.btnStartTimeSync = new System.Windows.Forms.Button();
         this.btnForceTimeSync = new System.Windows.Forms.Button();
         this.lstLog = new System.Windows.Forms.ListBox();
         this.lblStatus = new System.Windows.Forms.Label();
         this.SuspendLayout();
         // 
         // flowLayoutPanel1
         // 
         this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
         this.flowLayoutPanel1.AutoSize = true;
         this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
         this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(10);
         this.flowLayoutPanel1.Name = "flowLayoutPanel1";
         this.flowLayoutPanel1.TabIndex = 0;
         // 
         // btnOpen
         // 
         this.btnOpen.Name = "btnOpen";
         this.btnOpen.Text = "Open";
         this.btnOpen.AutoSize = true;
         this.btnOpen.Margin = new System.Windows.Forms.Padding(10);
         this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
         // 
         // btnRead
         // 
         this.btnRead.Name = "btnRead";
         this.btnRead.Text = "Read";
         this.btnRead.AutoSize = true;
         this.btnRead.Margin = new System.Windows.Forms.Padding(10);
         this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
         // 
         // btnWrite
         // 
         this.btnWrite.Name = "btnWrite";
         this.btnWrite.Text = "Write";
         this.btnWrite.AutoSize = true;
         this.btnWrite.Margin = new System.Windows.Forms.Padding(10);
         // 
         // btnClose
         // 
         this.btnClose.Name = "btnClose";
         this.btnClose.Text = "Close";
         this.btnClose.AutoSize = true;
         this.btnClose.Margin = new System.Windows.Forms.Padding(10);
         // 
         // btnStartHeartbeat
         // 
         this.btnStartHeartbeat.Name = "btnStartHeartbeat";
         this.btnStartHeartbeat.Text = "Start Heartbeat";
         this.btnStartHeartbeat.AutoSize = true;
         this.btnStartHeartbeat.Margin = new System.Windows.Forms.Padding(10);
         this.btnStartHeartbeat.Click += new System.EventHandler(this.btnStartHeartbeat_Click);
         // 
         // btnStopHeartbeat
         // 
         this.btnStopHeartbeat.Name = "btnStopHeartbeat";
         this.btnStopHeartbeat.Text = "Stop Heartbeat";
         this.btnStopHeartbeat.AutoSize = true;
         this.btnStopHeartbeat.Margin = new System.Windows.Forms.Padding(10);
         this.btnStopHeartbeat.Click += new System.EventHandler(this.btnStopHeartbeat_Click);
         // 
         // btnStartTimeSync
         // 
         this.btnStartTimeSync.Name = "btnStartTimeSync";
         this.btnStartTimeSync.Text = "Start TimeSync";
         this.btnStartTimeSync.AutoSize = true;
         this.btnStartTimeSync.Margin = new System.Windows.Forms.Padding(10);
         this.btnStartTimeSync.Click += new System.EventHandler(this.btnStartTimeSync_Click);
         // 
         // btnForceTimeSync
         // 
         this.btnForceTimeSync.Name = "btnForceTimeSync";
         this.btnForceTimeSync.Text = "Force TimeSync";
         this.btnForceTimeSync.AutoSize = true;
         this.btnForceTimeSync.Margin = new System.Windows.Forms.Padding(10);
         this.btnForceTimeSync.Click += new System.EventHandler(this.btnForceTimeSync_Click);
         // 
         // lstLog
         // 
         this.lstLog.Dock = System.Windows.Forms.DockStyle.Fill;
         this.lstLog.FormattingEnabled = true;
         this.lstLog.Name = "lstLog";
         this.lstLog.TabIndex = 10;
         // 
         // lblStatus
         // 
         this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.lblStatus.Height = 24;
         this.lblStatus.Text = "Status";
         this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.lblStatus.BackColor = System.Drawing.Color.LightGreen;
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
         // 將按鈕加入面板
         this.flowLayoutPanel1.Controls.Add(this.btnOpen);
         this.flowLayoutPanel1.Controls.Add(this.btnRead);
         this.flowLayoutPanel1.Controls.Add(this.btnWrite);
         this.flowLayoutPanel1.Controls.Add(this.btnClose);
         this.flowLayoutPanel1.Controls.Add(this.btnStartHeartbeat);
         this.flowLayoutPanel1.Controls.Add(this.btnStopHeartbeat);
         this.flowLayoutPanel1.Controls.Add(this.btnStartTimeSync);
         this.flowLayoutPanel1.Controls.Add(this.btnForceTimeSync);
         this.ResumeLayout(false);
         this.PerformLayout();
      }

      #endregion
   }
}

