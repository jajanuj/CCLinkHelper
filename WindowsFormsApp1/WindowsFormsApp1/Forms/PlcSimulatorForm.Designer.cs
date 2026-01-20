namespace WindowsFormsApp1.Forms
{
   partial class PlcSimulatorForm
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
         this.btnStartHeartbeat = new System.Windows.Forms.Button();
         this.btnStopHeartbeat = new System.Windows.Forms.Button();
         this.chkHeartbeatT1Timeout = new System.Windows.Forms.CheckBox();
         this.SuspendLayout();
         // 
         // btnStartHeartbeat
         // 
         this.btnStartHeartbeat.Location = new System.Drawing.Point(40, 40);
         this.btnStartHeartbeat.Name = "btnStartHeartbeat";
         this.btnStartHeartbeat.Size = new System.Drawing.Size(103, 23);
         this.btnStartHeartbeat.TabIndex = 0;
         this.btnStartHeartbeat.Text = "開始心跳流程";
         this.btnStartHeartbeat.UseVisualStyleBackColor = true;
         this.btnStartHeartbeat.Click += new System.EventHandler(this.btnStartHeartbeat_Click);
         // 
         // btnStopHeartbeat
         // 
         this.btnStopHeartbeat.Location = new System.Drawing.Point(149, 40);
         this.btnStopHeartbeat.Name = "btnStopHeartbeat";
         this.btnStopHeartbeat.Size = new System.Drawing.Size(103, 23);
         this.btnStopHeartbeat.TabIndex = 1;
         this.btnStopHeartbeat.Text = "停止心跳流程";
         this.btnStopHeartbeat.UseVisualStyleBackColor = true;
         this.btnStopHeartbeat.Click += new System.EventHandler(this.btnStopHeartbeat_Click);
         // 
         // chkHeartbeatT1Timeout
         // 
         this.chkHeartbeatT1Timeout.AutoSize = true;
         this.chkHeartbeatT1Timeout.Location = new System.Drawing.Point(268, 42);
         this.chkHeartbeatT1Timeout.Name = "chkHeartbeatT1Timeout";
         this.chkHeartbeatT1Timeout.Size = new System.Drawing.Size(90, 20);
         this.chkHeartbeatT1Timeout.TabIndex = 2;
         this.chkHeartbeatT1Timeout.Text = "T1 Timeout";
         this.chkHeartbeatT1Timeout.UseVisualStyleBackColor = true;
         this.chkHeartbeatT1Timeout.CheckedChanged += new System.EventHandler(this.chkHeartbeatT1Timeout_CheckedChanged);
         // 
         // PlcSimulatorForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(784, 478);
         this.Controls.Add(this.chkHeartbeatT1Timeout);
         this.Controls.Add(this.btnStopHeartbeat);
         this.Controls.Add(this.btnStartHeartbeat);
         this.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.Name = "PlcSimulatorForm";
         this.Text = "PlcSimulatorForm";
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Button btnStartHeartbeat;
      private System.Windows.Forms.Button btnStopHeartbeat;
      private System.Windows.Forms.CheckBox chkHeartbeatT1Timeout;
   }
}