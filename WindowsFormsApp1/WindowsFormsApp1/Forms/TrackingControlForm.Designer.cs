namespace WindowsFormsApp1.Forms
{
    partial class TrackingControlForm
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TrackingControlForm));
         this.grpMonitor = new System.Windows.Forms.GroupBox();
         this.lblReasonCode = new System.Windows.Forms.Label();
         this.txtJudgeFlags = new System.Windows.Forms.TextBox();
         this.cboReasonCode = new System.Windows.Forms.ComboBox();
         this.txtLotNo = new System.Windows.Forms.TextBox();
         this.txtLayerCount = new System.Windows.Forms.TextBox();
         this.txtBoardId = new System.Windows.Forms.TextBox();
         this.label5 = new System.Windows.Forms.Label();
         this.label4 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.btnRefresh = new System.Windows.Forms.Button();
         this.txtAddress = new System.Windows.Forms.TextBox();
         this.label1 = new System.Windows.Forms.Label();
         this.cmbStation = new System.Windows.Forms.ComboBox();
         this.lblStation = new System.Windows.Forms.Label();
         this.grpOperation = new System.Windows.Forms.GroupBox();
         this.nudJudge3 = new System.Windows.Forms.NumericUpDown();
         this.nudJudge2 = new System.Windows.Forms.NumericUpDown();
         this.nudJudge1 = new System.Windows.Forms.NumericUpDown();
         this.nudLotNum = new System.Windows.Forms.NumericUpDown();
         this.txtLotChar = new System.Windows.Forms.TextBox();
         this.nudLayerCount = new System.Windows.Forms.NumericUpDown();
         this.nudBoardId3 = new System.Windows.Forms.NumericUpDown();
         this.nudBoardId2 = new System.Windows.Forms.NumericUpDown();
         this.nudBoardId1 = new System.Windows.Forms.NumericUpDown();
         this.label12 = new System.Windows.Forms.Label();
         this.label11 = new System.Windows.Forms.Label();
         this.label10 = new System.Windows.Forms.Label();
         this.label9 = new System.Windows.Forms.Label();
         this.label8 = new System.Windows.Forms.Label();
         this.label7 = new System.Windows.Forms.Label();
         this.btnWriteTest = new System.Windows.Forms.Button();
         this.label6 = new System.Windows.Forms.Label();
         this.btnClear = new System.Windows.Forms.Button();
         this.grpQuickWrite = new System.Windows.Forms.GroupBox();
         this.btnQuickLoadingRobot = new System.Windows.Forms.Button();
         this.btnQuickLoading = new System.Windows.Forms.Button();
         this.btnQuickUnloading = new System.Windows.Forms.Button();
         this.btnQuickUnloadingRobot = new System.Windows.Forms.Button();
         this.grpLcsSimulator = new System.Windows.Forms.GroupBox();
         this.lblSimPos = new System.Windows.Forms.Label();
         this.nudSimPos = new System.Windows.Forms.NumericUpDown();
         this.btnSimDataMaint = new System.Windows.Forms.Button();
         this.grpLog = new System.Windows.Forms.GroupBox();
         this.rtbLog = new System.Windows.Forms.RichTextBox();
         this.grpMonitor.SuspendLayout();
         this.grpOperation.SuspendLayout();
         this.grpLcsSimulator.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.nudJudge3)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudJudge2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudJudge1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudLotNum)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudLayerCount)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudBoardId3)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudBoardId2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudBoardId1)).BeginInit();
         this.grpQuickWrite.SuspendLayout();
         this.grpLog.SuspendLayout();
         this.SuspendLayout();
         // 
         // grpMonitor
         // 
         this.grpMonitor.Controls.Add(this.lblReasonCode);
         this.grpMonitor.Controls.Add(this.txtJudgeFlags);
         this.grpMonitor.Controls.Add(this.cboReasonCode);
         this.grpMonitor.Controls.Add(this.txtLotNo);
         this.grpMonitor.Controls.Add(this.txtLayerCount);
         this.grpMonitor.Controls.Add(this.txtBoardId);
         this.grpMonitor.Controls.Add(this.label5);
         this.grpMonitor.Controls.Add(this.label4);
         this.grpMonitor.Controls.Add(this.label3);
         this.grpMonitor.Controls.Add(this.label2);
         this.grpMonitor.Controls.Add(this.btnRefresh);
         this.grpMonitor.Controls.Add(this.txtAddress);
         this.grpMonitor.Controls.Add(this.label1);
         this.grpMonitor.Controls.Add(this.cmbStation);
         this.grpMonitor.Controls.Add(this.lblStation);
         resources.ApplyResources(this.grpMonitor, "grpMonitor");
         this.grpMonitor.Name = "grpMonitor";
         this.grpMonitor.TabStop = false;
         // 
         // lblReasonCode
         // 
         resources.ApplyResources(this.lblReasonCode, "lblReasonCode");
         this.lblReasonCode.Name = "lblReasonCode";
         // 
         // txtJudgeFlags
         // 
         resources.ApplyResources(this.txtJudgeFlags, "txtJudgeFlags");
         this.txtJudgeFlags.Name = "txtJudgeFlags";
         this.txtJudgeFlags.ReadOnly = true;
         // 
         // cboReasonCode
         // 
         this.cboReasonCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cboReasonCode.FormattingEnabled = true;
         this.cboReasonCode.Items.AddRange(new object[] {
            resources.GetString("cboReasonCode.Items"),
            resources.GetString("cboReasonCode.Items1"),
            resources.GetString("cboReasonCode.Items2")});
         resources.ApplyResources(this.cboReasonCode, "cboReasonCode");
         this.cboReasonCode.Name = "cboReasonCode";
         // 
         // txtLotNo
         // 
         resources.ApplyResources(this.txtLotNo, "txtLotNo");
         this.txtLotNo.Name = "txtLotNo";
         this.txtLotNo.ReadOnly = true;
         // 
         // txtLayerCount
         // 
         resources.ApplyResources(this.txtLayerCount, "txtLayerCount");
         this.txtLayerCount.Name = "txtLayerCount";
         this.txtLayerCount.ReadOnly = true;
         // 
         // txtBoardId
         // 
         resources.ApplyResources(this.txtBoardId, "txtBoardId");
         this.txtBoardId.Name = "txtBoardId";
         this.txtBoardId.ReadOnly = true;
         // 
         // label5
         // 
         resources.ApplyResources(this.label5, "label5");
         this.label5.Name = "label5";
         // 
         // label4
         // 
         resources.ApplyResources(this.label4, "label4");
         this.label4.Name = "label4";
         // 
         // label3
         // 
         resources.ApplyResources(this.label3, "label3");
         this.label3.Name = "label3";
         // 
         // label2
         // 
         resources.ApplyResources(this.label2, "label2");
         this.label2.Name = "label2";
         // 
         // btnRefresh
         // 
         resources.ApplyResources(this.btnRefresh, "btnRefresh");
         this.btnRefresh.Name = "btnRefresh";
         this.btnRefresh.UseVisualStyleBackColor = true;
         this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
         // 
         // txtAddress
         // 
         resources.ApplyResources(this.txtAddress, "txtAddress");
         this.txtAddress.Name = "txtAddress";
         this.txtAddress.ReadOnly = true;
         // 
         // label1
         // 
         resources.ApplyResources(this.label1, "label1");
         this.label1.Name = "label1";
         // 
         // cmbStation
         // 
         this.cmbStation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cmbStation.FormattingEnabled = true;
         resources.ApplyResources(this.cmbStation, "cmbStation");
         this.cmbStation.Name = "cmbStation";
         this.cmbStation.SelectedIndexChanged += new System.EventHandler(this.cmbStation_SelectedIndexChanged);
         // 
         // lblStation
         // 
         resources.ApplyResources(this.lblStation, "lblStation");
         this.lblStation.Name = "lblStation";
         // 
         // grpOperation
         // 
         this.grpOperation.Controls.Add(this.nudJudge3);
         this.grpOperation.Controls.Add(this.nudJudge2);
         this.grpOperation.Controls.Add(this.nudJudge1);
         this.grpOperation.Controls.Add(this.nudLotNum);
         this.grpOperation.Controls.Add(this.txtLotChar);
         this.grpOperation.Controls.Add(this.nudLayerCount);
         this.grpOperation.Controls.Add(this.nudBoardId3);
         this.grpOperation.Controls.Add(this.nudBoardId2);
         this.grpOperation.Controls.Add(this.nudBoardId1);
         this.grpOperation.Controls.Add(this.label12);
         this.grpOperation.Controls.Add(this.label11);
         this.grpOperation.Controls.Add(this.label10);
         this.grpOperation.Controls.Add(this.label9);
         this.grpOperation.Controls.Add(this.label8);
         this.grpOperation.Controls.Add(this.label7);
         this.grpOperation.Controls.Add(this.btnWriteTest);
         this.grpOperation.Controls.Add(this.label6);
         this.grpOperation.Controls.Add(this.btnClear);
         resources.ApplyResources(this.grpOperation, "grpOperation");
         this.grpOperation.Name = "grpOperation";
         this.grpOperation.TabStop = false;
         // 
         // nudJudge3
         // 
         resources.ApplyResources(this.nudJudge3, "nudJudge3");
         this.nudJudge3.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
         this.nudJudge3.Name = "nudJudge3";
         // 
         // nudJudge2
         // 
         resources.ApplyResources(this.nudJudge2, "nudJudge2");
         this.nudJudge2.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
         this.nudJudge2.Name = "nudJudge2";
         // 
         // nudJudge1
         // 
         resources.ApplyResources(this.nudJudge1, "nudJudge1");
         this.nudJudge1.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
         this.nudJudge1.Name = "nudJudge1";
         // 
         // nudLotNum
         // 
         resources.ApplyResources(this.nudLotNum, "nudLotNum");
         this.nudLotNum.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
         this.nudLotNum.Name = "nudLotNum";
         // 
         // txtLotChar
         // 
         resources.ApplyResources(this.txtLotChar, "txtLotChar");
         this.txtLotChar.Name = "txtLotChar";
         // 
         // nudLayerCount
         // 
         resources.ApplyResources(this.nudLayerCount, "nudLayerCount");
         this.nudLayerCount.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
         this.nudLayerCount.Name = "nudLayerCount";
         // 
         // nudBoardId3
         // 
         resources.ApplyResources(this.nudBoardId3, "nudBoardId3");
         this.nudBoardId3.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
         this.nudBoardId3.Name = "nudBoardId3";
         // 
         // nudBoardId2
         // 
         resources.ApplyResources(this.nudBoardId2, "nudBoardId2");
         this.nudBoardId2.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
         this.nudBoardId2.Name = "nudBoardId2";
         // 
         // nudBoardId1
         // 
         resources.ApplyResources(this.nudBoardId1, "nudBoardId1");
         this.nudBoardId1.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
         this.nudBoardId1.Name = "nudBoardId1";
         // 
         // label12
         // 
         resources.ApplyResources(this.label12, "label12");
         this.label12.Name = "label12";
         // 
         // label11
         // 
         resources.ApplyResources(this.label11, "label11");
         this.label11.Name = "label11";
         // 
         // label10
         // 
         resources.ApplyResources(this.label10, "label10");
         this.label10.Name = "label10";
         // 
         // label9
         // 
         resources.ApplyResources(this.label9, "label9");
         this.label9.Name = "label9";
         // 
         // label8
         // 
         resources.ApplyResources(this.label8, "label8");
         this.label8.Name = "label8";
         // 
         // label7
         // 
         this.label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         resources.ApplyResources(this.label7, "label7");
         this.label7.Name = "label7";
         // 
         // btnWriteTest
         // 
         resources.ApplyResources(this.btnWriteTest, "btnWriteTest");
         this.btnWriteTest.Name = "btnWriteTest";
         this.btnWriteTest.UseVisualStyleBackColor = true;
         this.btnWriteTest.Click += new System.EventHandler(this.btnWriteTest_Click);
         // 
         // label6
         // 
         resources.ApplyResources(this.label6, "label6");
         this.label6.Name = "label6";
         // 
         // btnClear
         // 
         this.btnClear.BackColor = System.Drawing.Color.LightCoral;
         resources.ApplyResources(this.btnClear, "btnClear");
         this.btnClear.Name = "btnClear";
         this.btnClear.UseVisualStyleBackColor = false;
         this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
         // 
         // grpQuickWrite
         // 
         this.grpQuickWrite.Controls.Add(this.btnQuickLoadingRobot);
         this.grpQuickWrite.Controls.Add(this.btnQuickLoading);
         this.grpQuickWrite.Controls.Add(this.btnQuickUnloading);
         this.grpQuickWrite.Controls.Add(this.btnQuickUnloadingRobot);
         resources.ApplyResources(this.grpQuickWrite, "grpQuickWrite");
         this.grpQuickWrite.Name = "grpQuickWrite";
         this.grpQuickWrite.TabStop = false;
         // 
         // btnQuickLoadingRobot
         // 
         this.btnQuickLoadingRobot.BackColor = System.Drawing.Color.LightGreen;
         resources.ApplyResources(this.btnQuickLoadingRobot, "btnQuickLoadingRobot");
         this.btnQuickLoadingRobot.Name = "btnQuickLoadingRobot";
         this.btnQuickLoadingRobot.UseVisualStyleBackColor = false;
         this.btnQuickLoadingRobot.Click += new System.EventHandler(this.btnQuickLoadingRobot_Click);
         // 
         // btnQuickLoading
         // 
         this.btnQuickLoading.BackColor = System.Drawing.Color.LightGreen;
         resources.ApplyResources(this.btnQuickLoading, "btnQuickLoading");
         this.btnQuickLoading.Name = "btnQuickLoading";
         this.btnQuickLoading.UseVisualStyleBackColor = false;
         this.btnQuickLoading.Click += new System.EventHandler(this.btnQuickLoading_Click);
         // 
         // btnQuickUnloading
         // 
         this.btnQuickUnloading.BackColor = System.Drawing.Color.LightGreen;
         resources.ApplyResources(this.btnQuickUnloading, "btnQuickUnloading");
         this.btnQuickUnloading.Name = "btnQuickUnloading";
         this.btnQuickUnloading.UseVisualStyleBackColor = false;
         this.btnQuickUnloading.Click += new System.EventHandler(this.btnQuickUnloading_Click);
         // 
         // btnQuickUnloadingRobot
         // 
         this.btnQuickUnloadingRobot.BackColor = System.Drawing.Color.LightGreen;
         resources.ApplyResources(this.btnQuickUnloadingRobot, "btnQuickUnloadingRobot");
         this.btnQuickUnloadingRobot.Name = "btnQuickUnloadingRobot";
         this.btnQuickUnloadingRobot.UseVisualStyleBackColor = false;
         this.btnQuickUnloadingRobot.Click += new System.EventHandler(this.btnQuickUnloadingRobot_Click);
         // 
         // grpLcsSimulator
         // 
         this.grpLcsSimulator.Controls.Add(this.btnSimDataMaint);
         this.grpLcsSimulator.Controls.Add(this.nudSimPos);
         this.grpLcsSimulator.Controls.Add(this.lblSimPos);
         this.grpLcsSimulator.Location = new System.Drawing.Point(533, 410); // Adjust location as needed
         this.grpLcsSimulator.Name = "grpLcsSimulator";
         this.grpLcsSimulator.Size = new System.Drawing.Size(515, 70);
         this.grpLcsSimulator.TabIndex = 20;
         this.grpLcsSimulator.TabStop = false;
         this.grpLcsSimulator.Text = "LCS 模擬器 / Maintenance Request Simulator";
         // 
         // lblSimPos
         // 
         this.lblSimPos.AutoSize = true;
         this.lblSimPos.Location = new System.Drawing.Point(19, 30);
         this.lblSimPos.Name = "lblSimPos";
         this.lblSimPos.Size = new System.Drawing.Size(70, 15);
         this.lblSimPos.TabIndex = 0;
         this.lblSimPos.Text = "Position No:";
         // 
         // nudSimPos
         // 
         this.nudSimPos.Location = new System.Drawing.Point(92, 26);
         this.nudSimPos.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
         this.nudSimPos.Name = "nudSimPos";
         this.nudSimPos.Size = new System.Drawing.Size(80, 25);
         this.nudSimPos.TabIndex = 1;
         // 
         // btnSimDataMaint
         // 
         this.btnSimDataMaint.BackColor = System.Drawing.Color.LightSkyBlue;
         this.btnSimDataMaint.Location = new System.Drawing.Point(200, 21);
         this.btnSimDataMaint.Name = "btnSimDataMaint";
         this.btnSimDataMaint.Size = new System.Drawing.Size(180, 35);
         this.btnSimDataMaint.TabIndex = 2;
         this.btnSimDataMaint.Text = "發送維護請求 (LCS Request)";
         this.btnSimDataMaint.UseVisualStyleBackColor = false;
         this.btnSimDataMaint.Click += new System.EventHandler(this.btnSimDataMaint_Click);
         // 
         // grpLog
         // 
         this.grpLog.Controls.Add(this.rtbLog);
         resources.ApplyResources(this.grpLog, "grpLog");
         this.grpLog.Name = "grpLog";
         this.grpLog.TabStop = false;
         // 
         // rtbLog
         // 
         resources.ApplyResources(this.rtbLog, "rtbLog");
         this.rtbLog.Name = "rtbLog";
         this.rtbLog.ReadOnly = true;
         // 
         // TrackingControlForm
         // 
         resources.ApplyResources(this, "$this");
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.grpQuickWrite);
         this.Controls.Add(this.grpLcsSimulator);
         this.Controls.Add(this.grpLog);
         this.Controls.Add(this.grpOperation);
         this.Controls.Add(this.grpMonitor);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "TrackingControlForm";
         this.grpMonitor.ResumeLayout(false);
         this.grpMonitor.PerformLayout();
         this.grpOperation.ResumeLayout(false);
         this.grpOperation.PerformLayout();
         this.grpLcsSimulator.ResumeLayout(false);
         this.grpLcsSimulator.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.nudJudge3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudJudge2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudJudge1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudLotNum)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudLayerCount)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudBoardId3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudBoardId2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudBoardId1)).EndInit();
         this.grpQuickWrite.ResumeLayout(false);
         this.grpLcsSimulator.ResumeLayout(false);
         this.grpLcsSimulator.PerformLayout();
         this.grpLog.ResumeLayout(false);
         this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpMonitor;
        private System.Windows.Forms.ComboBox cmbStation;
        private System.Windows.Forms.Label lblStation;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.GroupBox grpOperation;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.GroupBox grpLog;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.TextBox txtJudgeFlags;
        private System.Windows.Forms.TextBox txtLotNo;
        private System.Windows.Forms.TextBox txtLayerCount;
        private System.Windows.Forms.TextBox txtBoardId;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnWriteTest;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nudBoardId1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown nudJudge3;
        private System.Windows.Forms.NumericUpDown nudJudge2;
        private System.Windows.Forms.NumericUpDown nudJudge1;
        private System.Windows.Forms.NumericUpDown nudLotNum;
        private System.Windows.Forms.TextBox txtLotChar;
        private System.Windows.Forms.NumericUpDown nudLayerCount;
        private System.Windows.Forms.NumericUpDown nudBoardId3;
        private System.Windows.Forms.NumericUpDown nudBoardId2;
        private System.Windows.Forms.GroupBox grpQuickWrite;
        private System.Windows.Forms.Button btnQuickLoadingRobot;
        private System.Windows.Forms.Button btnQuickLoading;
        private System.Windows.Forms.Button btnQuickUnloading;
        private System.Windows.Forms.Button btnQuickUnloadingRobot;
      private System.Windows.Forms.ComboBox cboReasonCode;
      private System.Windows.Forms.Label lblReasonCode;
      private System.Windows.Forms.GroupBox grpLcsSimulator;
      private System.Windows.Forms.NumericUpDown nudSimPos;
      private System.Windows.Forms.Label lblSimPos;
      private System.Windows.Forms.Button btnSimDataMaint;
   }
}
