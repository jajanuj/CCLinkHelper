namespace WindowsFormsApp1
{
   partial class CommonReportStatus1SettingsForm
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
         this.cboAlarmStatus = new System.Windows.Forms.ComboBox();
         this.cboMachineStatus = new System.Windows.Forms.ComboBox();
         this.cboActionStatus = new System.Windows.Forms.ComboBox();
         this.cboWaitingStatus = new System.Windows.Forms.ComboBox();
         this.cboControlStatus = new System.Windows.Forms.ComboBox();
         this.btnOk = new System.Windows.Forms.Button();
         this.btnCancel = new System.Windows.Forms.Button();
         this.lblAlarm = new System.Windows.Forms.Label();
         this.lblMachine = new System.Windows.Forms.Label();
         this.lblAction = new System.Windows.Forms.Label();
         this.lblWaiting = new System.Windows.Forms.Label();
         this.lblControl = new System.Windows.Forms.Label();
         this.SuspendLayout();
         // 
         // cboAlarmStatus
         // 
         this.cboAlarmStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cboAlarmStatus.DropDownWidth = 121;
         this.cboAlarmStatus.FormattingEnabled = true;
         this.cboAlarmStatus.Location = new System.Drawing.Point(133, 25);
         this.cboAlarmStatus.Name = "cboAlarmStatus";
         this.cboAlarmStatus.Size = new System.Drawing.Size(100, 24);
         this.cboAlarmStatus.TabIndex = 0;
         // 
         // cboMachineStatus
         // 
         this.cboMachineStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cboMachineStatus.FormattingEnabled = true;
         this.cboMachineStatus.Location = new System.Drawing.Point(133, 55);
         this.cboMachineStatus.Name = "cboMachineStatus";
         this.cboMachineStatus.Size = new System.Drawing.Size(100, 24);
         this.cboMachineStatus.TabIndex = 1;
         // 
         // cboActionStatus
         // 
         this.cboActionStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cboActionStatus.FormattingEnabled = true;
         this.cboActionStatus.Location = new System.Drawing.Point(133, 85);
         this.cboActionStatus.Name = "cboActionStatus";
         this.cboActionStatus.Size = new System.Drawing.Size(100, 24);
         this.cboActionStatus.TabIndex = 2;
         // 
         // cboWaitingStatus
         // 
         this.cboWaitingStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cboWaitingStatus.FormattingEnabled = true;
         this.cboWaitingStatus.Location = new System.Drawing.Point(133, 115);
         this.cboWaitingStatus.Name = "cboWaitingStatus";
         this.cboWaitingStatus.Size = new System.Drawing.Size(100, 24);
         this.cboWaitingStatus.TabIndex = 3;
         // 
         // cboControlStatus
         // 
         this.cboControlStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cboControlStatus.FormattingEnabled = true;
         this.cboControlStatus.Location = new System.Drawing.Point(133, 145);
         this.cboControlStatus.Name = "cboControlStatus";
         this.cboControlStatus.Size = new System.Drawing.Size(100, 24);
         this.cboControlStatus.TabIndex = 4;
         // 
         // btnOk
         // 
         this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.btnOk.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.btnOk.Location = new System.Drawing.Point(20, 188);
         this.btnOk.Name = "btnOk";
         this.btnOk.Size = new System.Drawing.Size(88, 31);
         this.btnOk.TabIndex = 5;
         this.btnOk.Text = "確定";
         this.btnOk.UseVisualStyleBackColor = true;
         this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
         // 
         // btnCancel
         // 
         this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.btnCancel.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.btnCancel.Location = new System.Drawing.Point(145, 188);
         this.btnCancel.Name = "btnCancel";
         this.btnCancel.Size = new System.Drawing.Size(88, 31);
         this.btnCancel.TabIndex = 6;
         this.btnCancel.Text = "取消";
         this.btnCancel.UseVisualStyleBackColor = true;
         // 
         // lblAlarm
         // 
         this.lblAlarm.AutoSize = true;
         this.lblAlarm.Location = new System.Drawing.Point(53, 28);
         this.lblAlarm.Name = "lblAlarm";
         this.lblAlarm.Size = new System.Drawing.Size(55, 16);
         this.lblAlarm.TabIndex = 7;
         this.lblAlarm.Text = "警報狀態";
         this.lblAlarm.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // lblMachine
         // 
         this.lblMachine.AutoSize = true;
         this.lblMachine.Location = new System.Drawing.Point(53, 58);
         this.lblMachine.Name = "lblMachine";
         this.lblMachine.Size = new System.Drawing.Size(55, 16);
         this.lblMachine.TabIndex = 8;
         this.lblMachine.Text = "機台狀態";
         this.lblMachine.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // lblAction
         // 
         this.lblAction.AutoSize = true;
         this.lblAction.Location = new System.Drawing.Point(53, 88);
         this.lblAction.Name = "lblAction";
         this.lblAction.Size = new System.Drawing.Size(55, 16);
         this.lblAction.TabIndex = 9;
         this.lblAction.Text = "動作狀態";
         this.lblAction.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // lblWaiting
         // 
         this.lblWaiting.AutoSize = true;
         this.lblWaiting.Location = new System.Drawing.Point(53, 118);
         this.lblWaiting.Name = "lblWaiting";
         this.lblWaiting.Size = new System.Drawing.Size(55, 16);
         this.lblWaiting.TabIndex = 10;
         this.lblWaiting.Text = "等待狀態";
         this.lblWaiting.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // lblControl
         // 
         this.lblControl.AutoSize = true;
         this.lblControl.Location = new System.Drawing.Point(53, 148);
         this.lblControl.Name = "lblControl";
         this.lblControl.Size = new System.Drawing.Size(55, 16);
         this.lblControl.TabIndex = 11;
         this.lblControl.Text = "控制狀態";
         this.lblControl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // CommonReportStatus1SettingsForm
         // 
         this.AcceptButton = this.btnOk;
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.CancelButton = this.btnCancel;
         this.ClientSize = new System.Drawing.Size(316, 273);
         this.Controls.Add(this.lblControl);
         this.Controls.Add(this.lblWaiting);
         this.Controls.Add(this.lblAction);
         this.Controls.Add(this.lblMachine);
         this.Controls.Add(this.lblAlarm);
         this.Controls.Add(this.btnCancel);
         this.Controls.Add(this.btnOk);
         this.Controls.Add(this.cboControlStatus);
         this.Controls.Add(this.cboWaitingStatus);
         this.Controls.Add(this.cboActionStatus);
         this.Controls.Add(this.cboMachineStatus);
         this.Controls.Add(this.cboAlarmStatus);
         this.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.Margin = new System.Windows.Forms.Padding(4);
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "CommonReportStatus1SettingsForm";
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "定期上報設定";
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.ComboBox cboAlarmStatus;
      private System.Windows.Forms.ComboBox cboMachineStatus;
      private System.Windows.Forms.ComboBox cboActionStatus;
      private System.Windows.Forms.ComboBox cboWaitingStatus;
      private System.Windows.Forms.ComboBox cboControlStatus;
      private System.Windows.Forms.Button btnOk;
      private System.Windows.Forms.Button btnCancel;
      private System.Windows.Forms.Label lblAlarm;
      private System.Windows.Forms.Label lblMachine;
      private System.Windows.Forms.Label lblAction;
      private System.Windows.Forms.Label lblWaiting;
      private System.Windows.Forms.Label lblControl;
   }
}