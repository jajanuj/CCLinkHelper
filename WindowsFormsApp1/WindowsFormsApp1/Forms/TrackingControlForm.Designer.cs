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
            this.grpMonitor = new System.Windows.Forms.GroupBox();
            this.txtJudgeFlags = new System.Windows.Forms.TextBox();
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
            this.grpLog = new System.Windows.Forms.GroupBox();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.grpMonitor.SuspendLayout();
            this.grpOperation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudJudge3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudJudge2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudJudge1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLotNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLayerCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBoardId3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBoardId2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBoardId1)).BeginInit();
            this.grpLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpMonitor
            // 
            this.grpMonitor.Controls.Add(this.txtJudgeFlags);
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
            this.grpMonitor.Location = new System.Drawing.Point(12, 12);
            this.grpMonitor.Name = "grpMonitor";
            this.grpMonitor.Size = new System.Drawing.Size(560, 200);
            this.grpMonitor.TabIndex = 0;
            this.grpMonitor.TabStop = false;
            this.grpMonitor.Text = "站點監控 (Station Monitor)";
            // 
            // txtJudgeFlags
            // 
            this.txtJudgeFlags.Location = new System.Drawing.Point(100, 162);
            this.txtJudgeFlags.Name = "txtJudgeFlags";
            this.txtJudgeFlags.ReadOnly = true;
            this.txtJudgeFlags.Size = new System.Drawing.Size(440, 22);
            this.txtJudgeFlags.TabIndex = 12;
            // 
            // txtLotNo
            // 
            this.txtLotNo.Location = new System.Drawing.Point(100, 134);
            this.txtLotNo.Name = "txtLotNo";
            this.txtLotNo.ReadOnly = true;
            this.txtLotNo.Size = new System.Drawing.Size(440, 22);
            this.txtLotNo.TabIndex = 11;
            // 
            // txtLayerCount
            // 
            this.txtLayerCount.Location = new System.Drawing.Point(100, 106);
            this.txtLayerCount.Name = "txtLayerCount";
            this.txtLayerCount.ReadOnly = true;
            this.txtLayerCount.Size = new System.Drawing.Size(440, 22);
            this.txtLayerCount.TabIndex = 10;
            // 
            // txtBoardId
            // 
            this.txtBoardId.Location = new System.Drawing.Point(100, 78);
            this.txtBoardId.Name = "txtBoardId";
            this.txtBoardId.ReadOnly = true;
            this.txtBoardId.Size = new System.Drawing.Size(440, 22);
            this.txtBoardId.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 165);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "判斷旗標:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 137);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "批號:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 109);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "層數:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "基板序號:";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(456, 46);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(84, 23);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(100, 47);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.ReadOnly = true;
            this.txtAddress.Size = new System.Drawing.Size(340, 22);
            this.txtAddress.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "位址:";
            // 
            // cmbStation
            // 
            this.cmbStation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStation.FormattingEnabled = true;
            this.cmbStation.Location = new System.Drawing.Point(100, 19);
            this.cmbStation.Name = "cmbStation";
            this.cmbStation.Size = new System.Drawing.Size(340, 20);
            this.cmbStation.TabIndex = 1;
            this.cmbStation.SelectedIndexChanged += new System.EventHandler(this.cmbStation_SelectedIndexChanged);
            // 
            // lblStation
            // 
            this.lblStation.AutoSize = true;
            this.lblStation.Location = new System.Drawing.Point(16, 22);
            this.lblStation.Name = "lblStation";
            this.lblStation.Size = new System.Drawing.Size(48, 12);
            this.lblStation.TabIndex = 0;
            this.lblStation.Text = "站點:";
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
            this.grpOperation.Location = new System.Drawing.Point(12, 218);
            this.grpOperation.Name = "grpOperation";
            this.grpOperation.Size = new System.Drawing.Size(560, 280);
            this.grpOperation.TabIndex = 1;
            this.grpOperation.TabStop = false;
            this.grpOperation.Text = "資料操作 (Data Operation)";
            // 
            // nudJudge3
            // 
            this.nudJudge3.Location = new System.Drawing.Point(400, 213);
            this.nudJudge3.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudJudge3.Name = "nudJudge3";
            this.nudJudge3.Size = new System.Drawing.Size(140, 22);
            this.nudJudge3.TabIndex = 21;
            // 
            // nudJudge2
            // 
            this.nudJudge2.Location = new System.Drawing.Point(220, 213);
            this.nudJudge2.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudJudge2.Name = "nudJudge2";
            this.nudJudge2.Size = new System.Drawing.Size(140, 22);
            this.nudJudge2.TabIndex = 20;
            // 
            // nudJudge1
            // 
            this.nudJudge1.Location = new System.Drawing.Point(40, 213);
            this.nudJudge1.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudJudge1.Name = "nudJudge1";
            this.nudJudge1.Size = new System.Drawing.Size(140, 22);
            this.nudJudge1.TabIndex = 19;
            // 
            // nudLotNum
            // 
            this.nudLotNum.Location = new System.Drawing.Point(100, 186);
            this.nudLotNum.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.nudLotNum.Name = "nudLotNum";
            this.nudLotNum.Size = new System.Drawing.Size(440, 22);
            this.nudLotNum.TabIndex = 18;
            // 
            // txtLotChar
            // 
            this.txtLotChar.Location = new System.Drawing.Point(100, 158);
            this.txtLotChar.MaxLength = 1;
            this.txtLotChar.Name = "txtLotChar";
            this.txtLotChar.Size = new System.Drawing.Size(440, 22);
            this.txtLotChar.TabIndex = 17;
            // 
            // nudLayerCount
            // 
            this.nudLayerCount.Location = new System.Drawing.Point(100, 130);
            this.nudLayerCount.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudLayerCount.Name = "nudLayerCount";
            this.nudLayerCount.Size = new System.Drawing.Size(440, 22);
            this.nudLayerCount.TabIndex = 16;
            // 
            // nudBoardId3
            // 
            this.nudBoardId3.Location = new System.Drawing.Point(400, 102);
            this.nudBoardId3.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudBoardId3.Name = "nudBoardId3";
            this.nudBoardId3.Size = new System.Drawing.Size(140, 22);
            this.nudBoardId3.TabIndex = 15;
            // 
            // nudBoardId2
            // 
            this.nudBoardId2.Location = new System.Drawing.Point(220, 102);
            this.nudBoardId2.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudBoardId2.Name = "nudBoardId2";
            this.nudBoardId2.Size = new System.Drawing.Size(140, 22);
            this.nudBoardId2.TabIndex = 14;
            // 
            // nudBoardId1
            // 
            this.nudBoardId1.Location = new System.Drawing.Point(40, 102);
            this.nudBoardId1.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudBoardId1.Name = "nudBoardId1";
            this.nudBoardId1.Size = new System.Drawing.Size(140, 22);
            this.nudBoardId1.TabIndex = 13;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(16, 215);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(72, 12);
            this.label12.TabIndex = 12;
            this.label12.Text = "判斷旗標:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(16, 188);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(84, 12);
            this.label11.TabIndex = 11;
            this.label11.Text = "批號(數字):";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(16, 161);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(84, 12);
            this.label10.TabIndex = 10;
            this.label10.Text = "批號(字元):";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(16, 132);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(48, 12);
            this.label9.TabIndex = 9;
            this.label9.Text = "層數:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(16, 104);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(72, 12);
            this.label8.TabIndex = 8;
            this.label8.Text = "基板序號:";
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label7.Location = new System.Drawing.Point(18, 74);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(522, 2);
            this.label7.TabIndex = 7;
            // 
            // btnWriteTest
            // 
            this.btnWriteTest.Location = new System.Drawing.Point(18, 241);
            this.btnWriteTest.Name = "btnWriteTest";
            this.btnWriteTest.Size = new System.Drawing.Size(522, 28);
            this.btnWriteTest.TabIndex = 6;
            this.btnWriteTest.Text = "寫入測試資料 (Write Test Data)";
            this.btnWriteTest.UseVisualStyleBackColor = true;
            this.btnWriteTest.Click += new System.EventHandler(this.btnWriteTest_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label6.Location = new System.Drawing.Point(16, 53);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(181, 12);
            this.label6.TabIndex = 5;
            this.label6.Text = "---------- 測試區 ----------";
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.LightCoral;
            this.btnClear.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnClear.Location = new System.Drawing.Point(18, 19);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(522, 28);
            this.btnClear.TabIndex = 0;
            this.btnClear.Text = "清除追蹤資料 (Clear Data)";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // grpLog
            // 
            this.grpLog.Controls.Add(this.rtbLog);
            this.grpLog.Location = new System.Drawing.Point(12, 504);
            this.grpLog.Name = "grpLog";
            this.grpLog.Size = new System.Drawing.Size(560, 150);
            this.grpLog.TabIndex = 2;
            this.grpLog.TabStop = false;
            this.grpLog.Text = "日誌 (Log)";
            // 
            // rtbLog
            // 
            this.rtbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLog.Location = new System.Drawing.Point(3, 18);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.ReadOnly = true;
            this.rtbLog.Size = new System.Drawing.Size(554, 129);
            this.rtbLog.TabIndex = 0;
            this.rtbLog.Text = "";
            // 
            // TrackingControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 666);
            this.Controls.Add(this.grpLog);
            this.Controls.Add(this.grpOperation);
            this.Controls.Add(this.grpMonitor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TrackingControlForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "追蹤資料管理 (Tracking Data Management)";
            this.grpMonitor.ResumeLayout(false);
            this.grpMonitor.PerformLayout();
            this.grpOperation.ResumeLayout(false);
            this.grpOperation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudJudge3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudJudge2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudJudge1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLotNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLayerCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBoardId3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBoardId2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBoardId1)).EndInit();
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
    }
}
