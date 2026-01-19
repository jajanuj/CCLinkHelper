namespace WindowsFormsApp1.Forms
{
    partial class CommonReportSettingsForm
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
         this.tabControl1 = new System.Windows.Forms.TabControl();
         this.tabPageStatus1 = new System.Windows.Forms.TabPage();
         this.lblControl = new System.Windows.Forms.Label();
         this.lblWaiting = new System.Windows.Forms.Label();
         this.lblAction = new System.Windows.Forms.Label();
         this.lblMachine = new System.Windows.Forms.Label();
         this.lblAlarm = new System.Windows.Forms.Label();
         this.cboControlStatus = new System.Windows.Forms.ComboBox();
         this.cboWaitingStatus = new System.Windows.Forms.ComboBox();
         this.cboActionStatus = new System.Windows.Forms.ComboBox();
         this.cboMachineStatus = new System.Windows.Forms.ComboBox();
         this.cboAlarmStatus = new System.Windows.Forms.ComboBox();
         this.tabPageStatus2 = new System.Windows.Forms.TabPage();
         this.lblRedLight = new System.Windows.Forms.Label();
         this.cmbRedLight = new System.Windows.Forms.ComboBox();
         this.lblYellowLight = new System.Windows.Forms.Label();
         this.cmbYellowLight = new System.Windows.Forms.ComboBox();
         this.lblGreenLight = new System.Windows.Forms.Label();
         this.cmbGreenLight = new System.Windows.Forms.ComboBox();
         this.lblUpstreamWaiting = new System.Windows.Forms.Label();
         this.lblDownstreamWaiting = new System.Windows.Forms.Label();
         this.lblDischargeRate = new System.Windows.Forms.Label();
         this.nudDischargeRate = new System.Windows.Forms.NumericUpDown();
         this.lblStopTime = new System.Windows.Forms.Label();
         this.nudStopTime = new System.Windows.Forms.NumericUpDown();
         this.lblProcessingCounter = new System.Windows.Forms.Label();
         this.nudProcessingCounter = new System.Windows.Forms.NumericUpDown();
         this.lblRetainedBoardCount = new System.Windows.Forms.Label();
         this.nudRetainedBoardCount = new System.Windows.Forms.NumericUpDown();
         this.lblCurrentRecipeNo = new System.Windows.Forms.Label();
         this.nudCurrentRecipeNo = new System.Windows.Forms.NumericUpDown();
         this.lblBoardThickness = new System.Windows.Forms.Label();
         this.nudBoardThickness = new System.Windows.Forms.NumericUpDown();
         this.lblRecipeName = new System.Windows.Forms.Label();
         this.txtRecipeName = new System.Windows.Forms.TextBox();
         this.tabPageAlarm = new System.Windows.Forms.TabPage();
         this.lblError1 = new System.Windows.Forms.Label();
         this.nudError1 = new System.Windows.Forms.NumericUpDown();
         this.lblError2 = new System.Windows.Forms.Label();
         this.nudError2 = new System.Windows.Forms.NumericUpDown();
         this.lblError3 = new System.Windows.Forms.Label();
         this.nudError3 = new System.Windows.Forms.NumericUpDown();
         this.lblError4 = new System.Windows.Forms.Label();
         this.nudError4 = new System.Windows.Forms.NumericUpDown();
         this.lblError5 = new System.Windows.Forms.Label();
         this.nudError5 = new System.Windows.Forms.NumericUpDown();
         this.lblError6 = new System.Windows.Forms.Label();
         this.nudError6 = new System.Windows.Forms.NumericUpDown();
         this.lblError7 = new System.Windows.Forms.Label();
         this.nudError7 = new System.Windows.Forms.NumericUpDown();
         this.lblError8 = new System.Windows.Forms.Label();
         this.nudError8 = new System.Windows.Forms.NumericUpDown();
         this.lblError9 = new System.Windows.Forms.Label();
         this.nudError9 = new System.Windows.Forms.NumericUpDown();
         this.lblError10 = new System.Windows.Forms.Label();
         this.nudError10 = new System.Windows.Forms.NumericUpDown();
         this.lblError11 = new System.Windows.Forms.Label();
         this.nudError11 = new System.Windows.Forms.NumericUpDown();
         this.lblError12 = new System.Windows.Forms.Label();
         this.nudError12 = new System.Windows.Forms.NumericUpDown();
         this.btnOK = new System.Windows.Forms.Button();
         this.btnCancel = new System.Windows.Forms.Button();
         this.nudUpstreamWaiting = new System.Windows.Forms.NumericUpDown();
         this.nudDownstreamWaiting = new System.Windows.Forms.NumericUpDown();
         this.tabControl1.SuspendLayout();
         this.tabPageStatus1.SuspendLayout();
         this.tabPageStatus2.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.nudDischargeRate)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudStopTime)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudProcessingCounter)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudRetainedBoardCount)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudCurrentRecipeNo)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudBoardThickness)).BeginInit();
         this.tabPageAlarm.SuspendLayout();
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
         ((System.ComponentModel.ISupportInitialize)(this.nudUpstreamWaiting)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudDownstreamWaiting)).BeginInit();
         this.SuspendLayout();
         // 
         // tabControl1
         // 
         this.tabControl1.Controls.Add(this.tabPageStatus1);
         this.tabControl1.Controls.Add(this.tabPageStatus2);
         this.tabControl1.Controls.Add(this.tabPageAlarm);
         this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
         this.tabControl1.Location = new System.Drawing.Point(0, 0);
         this.tabControl1.Name = "tabControl1";
         this.tabControl1.SelectedIndex = 0;
         this.tabControl1.Size = new System.Drawing.Size(584, 460);
         this.tabControl1.TabIndex = 0;
         // 
         // tabPageStatus1
         // 
         this.tabPageStatus1.Controls.Add(this.lblControl);
         this.tabPageStatus1.Controls.Add(this.lblWaiting);
         this.tabPageStatus1.Controls.Add(this.lblAction);
         this.tabPageStatus1.Controls.Add(this.lblMachine);
         this.tabPageStatus1.Controls.Add(this.lblAlarm);
         this.tabPageStatus1.Controls.Add(this.cboControlStatus);
         this.tabPageStatus1.Controls.Add(this.cboWaitingStatus);
         this.tabPageStatus1.Controls.Add(this.cboActionStatus);
         this.tabPageStatus1.Controls.Add(this.cboMachineStatus);
         this.tabPageStatus1.Controls.Add(this.cboAlarmStatus);
         this.tabPageStatus1.Location = new System.Drawing.Point(4, 25);
         this.tabPageStatus1.Name = "tabPageStatus1";
         this.tabPageStatus1.Padding = new System.Windows.Forms.Padding(3);
         this.tabPageStatus1.Size = new System.Drawing.Size(576, 431);
         this.tabPageStatus1.TabIndex = 0;
         this.tabPageStatus1.Text = "機台狀態 (Status 1)";
         this.tabPageStatus1.UseVisualStyleBackColor = true;
         // 
         // lblControl
         // 
         this.lblControl.AutoSize = true;
         this.lblControl.Location = new System.Drawing.Point(50, 190);
         this.lblControl.Name = "lblControl";
         this.lblControl.Size = new System.Drawing.Size(58, 16);
         this.lblControl.TabIndex = 0;
         this.lblControl.Text = "控制狀態:";
         // 
         // lblWaiting
         // 
         this.lblWaiting.AutoSize = true;
         this.lblWaiting.Location = new System.Drawing.Point(50, 150);
         this.lblWaiting.Name = "lblWaiting";
         this.lblWaiting.Size = new System.Drawing.Size(58, 16);
         this.lblWaiting.TabIndex = 1;
         this.lblWaiting.Text = "等待狀態:";
         // 
         // lblAction
         // 
         this.lblAction.AutoSize = true;
         this.lblAction.Location = new System.Drawing.Point(50, 110);
         this.lblAction.Name = "lblAction";
         this.lblAction.Size = new System.Drawing.Size(58, 16);
         this.lblAction.TabIndex = 2;
         this.lblAction.Text = "動作狀態:";
         // 
         // lblMachine
         // 
         this.lblMachine.AutoSize = true;
         this.lblMachine.Location = new System.Drawing.Point(50, 70);
         this.lblMachine.Name = "lblMachine";
         this.lblMachine.Size = new System.Drawing.Size(58, 16);
         this.lblMachine.TabIndex = 3;
         this.lblMachine.Text = "機台狀態:";
         // 
         // lblAlarm
         // 
         this.lblAlarm.AutoSize = true;
         this.lblAlarm.Location = new System.Drawing.Point(50, 30);
         this.lblAlarm.Name = "lblAlarm";
         this.lblAlarm.Size = new System.Drawing.Size(58, 16);
         this.lblAlarm.TabIndex = 4;
         this.lblAlarm.Text = "警報狀態:";
         // 
         // cboControlStatus
         // 
         this.cboControlStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cboControlStatus.Location = new System.Drawing.Point(160, 187);
         this.cboControlStatus.Name = "cboControlStatus";
         this.cboControlStatus.Size = new System.Drawing.Size(200, 24);
         this.cboControlStatus.TabIndex = 5;
         // 
         // cboWaitingStatus
         // 
         this.cboWaitingStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cboWaitingStatus.Location = new System.Drawing.Point(160, 147);
         this.cboWaitingStatus.Name = "cboWaitingStatus";
         this.cboWaitingStatus.Size = new System.Drawing.Size(200, 24);
         this.cboWaitingStatus.TabIndex = 6;
         // 
         // cboActionStatus
         // 
         this.cboActionStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cboActionStatus.Location = new System.Drawing.Point(160, 107);
         this.cboActionStatus.Name = "cboActionStatus";
         this.cboActionStatus.Size = new System.Drawing.Size(200, 24);
         this.cboActionStatus.TabIndex = 7;
         // 
         // cboMachineStatus
         // 
         this.cboMachineStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cboMachineStatus.Location = new System.Drawing.Point(160, 67);
         this.cboMachineStatus.Name = "cboMachineStatus";
         this.cboMachineStatus.Size = new System.Drawing.Size(200, 24);
         this.cboMachineStatus.TabIndex = 8;
         // 
         // cboAlarmStatus
         // 
         this.cboAlarmStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cboAlarmStatus.Location = new System.Drawing.Point(160, 27);
         this.cboAlarmStatus.Name = "cboAlarmStatus";
         this.cboAlarmStatus.Size = new System.Drawing.Size(200, 24);
         this.cboAlarmStatus.TabIndex = 9;
         // 
         // tabPageStatus2
         // 
         this.tabPageStatus2.Controls.Add(this.nudDownstreamWaiting);
         this.tabPageStatus2.Controls.Add(this.nudUpstreamWaiting);
         this.tabPageStatus2.Controls.Add(this.lblRedLight);
         this.tabPageStatus2.Controls.Add(this.cmbRedLight);
         this.tabPageStatus2.Controls.Add(this.lblYellowLight);
         this.tabPageStatus2.Controls.Add(this.cmbYellowLight);
         this.tabPageStatus2.Controls.Add(this.lblGreenLight);
         this.tabPageStatus2.Controls.Add(this.cmbGreenLight);
         this.tabPageStatus2.Controls.Add(this.lblUpstreamWaiting);
         this.tabPageStatus2.Controls.Add(this.lblDownstreamWaiting);
         this.tabPageStatus2.Controls.Add(this.lblDischargeRate);
         this.tabPageStatus2.Controls.Add(this.nudDischargeRate);
         this.tabPageStatus2.Controls.Add(this.lblStopTime);
         this.tabPageStatus2.Controls.Add(this.nudStopTime);
         this.tabPageStatus2.Controls.Add(this.lblProcessingCounter);
         this.tabPageStatus2.Controls.Add(this.nudProcessingCounter);
         this.tabPageStatus2.Controls.Add(this.lblRetainedBoardCount);
         this.tabPageStatus2.Controls.Add(this.nudRetainedBoardCount);
         this.tabPageStatus2.Controls.Add(this.lblCurrentRecipeNo);
         this.tabPageStatus2.Controls.Add(this.nudCurrentRecipeNo);
         this.tabPageStatus2.Controls.Add(this.lblBoardThickness);
         this.tabPageStatus2.Controls.Add(this.nudBoardThickness);
         this.tabPageStatus2.Controls.Add(this.lblRecipeName);
         this.tabPageStatus2.Controls.Add(this.txtRecipeName);
         this.tabPageStatus2.Location = new System.Drawing.Point(4, 25);
         this.tabPageStatus2.Name = "tabPageStatus2";
         this.tabPageStatus2.Padding = new System.Windows.Forms.Padding(3);
         this.tabPageStatus2.Size = new System.Drawing.Size(576, 431);
         this.tabPageStatus2.TabIndex = 1;
         this.tabPageStatus2.Text = "詳細狀態 (Status 2)";
         this.tabPageStatus2.UseVisualStyleBackColor = true;
         // 
         // lblRedLight
         // 
         this.lblRedLight.Location = new System.Drawing.Point(30, 20);
         this.lblRedLight.Name = "lblRedLight";
         this.lblRedLight.Size = new System.Drawing.Size(140, 20);
         this.lblRedLight.TabIndex = 0;
         this.lblRedLight.Text = "紅燈狀態 (Red):";
         this.lblRedLight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // cmbRedLight
         // 
         this.cmbRedLight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cmbRedLight.Location = new System.Drawing.Point(180, 20);
         this.cmbRedLight.Name = "cmbRedLight";
         this.cmbRedLight.Size = new System.Drawing.Size(300, 24);
         this.cmbRedLight.TabIndex = 1;
         // 
         // lblYellowLight
         // 
         this.lblYellowLight.Location = new System.Drawing.Point(30, 52);
         this.lblYellowLight.Name = "lblYellowLight";
         this.lblYellowLight.Size = new System.Drawing.Size(140, 20);
         this.lblYellowLight.TabIndex = 2;
         this.lblYellowLight.Text = "黃燈狀態 (Yellow):";
         this.lblYellowLight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // cmbYellowLight
         // 
         this.cmbYellowLight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cmbYellowLight.Location = new System.Drawing.Point(180, 52);
         this.cmbYellowLight.Name = "cmbYellowLight";
         this.cmbYellowLight.Size = new System.Drawing.Size(300, 24);
         this.cmbYellowLight.TabIndex = 3;
         // 
         // lblGreenLight
         // 
         this.lblGreenLight.Location = new System.Drawing.Point(30, 84);
         this.lblGreenLight.Name = "lblGreenLight";
         this.lblGreenLight.Size = new System.Drawing.Size(140, 20);
         this.lblGreenLight.TabIndex = 4;
         this.lblGreenLight.Text = "綠燈狀態 (Green):";
         this.lblGreenLight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // cmbGreenLight
         // 
         this.cmbGreenLight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cmbGreenLight.Location = new System.Drawing.Point(180, 84);
         this.cmbGreenLight.Name = "cmbGreenLight";
         this.cmbGreenLight.Size = new System.Drawing.Size(300, 24);
         this.cmbGreenLight.TabIndex = 5;
         // 
         // lblUpstreamWaiting
         // 
         this.lblUpstreamWaiting.Location = new System.Drawing.Point(30, 116);
         this.lblUpstreamWaiting.Name = "lblUpstreamWaiting";
         this.lblUpstreamWaiting.Size = new System.Drawing.Size(140, 20);
         this.lblUpstreamWaiting.TabIndex = 6;
         this.lblUpstreamWaiting.Text = "上游等待 (Upstream):";
         this.lblUpstreamWaiting.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // lblDownstreamWaiting
         // 
         this.lblDownstreamWaiting.Location = new System.Drawing.Point(30, 148);
         this.lblDownstreamWaiting.Name = "lblDownstreamWaiting";
         this.lblDownstreamWaiting.Size = new System.Drawing.Size(140, 20);
         this.lblDownstreamWaiting.TabIndex = 8;
         this.lblDownstreamWaiting.Text = "下游等待 (Downstream):";
         this.lblDownstreamWaiting.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // lblDischargeRate
         // 
         this.lblDischargeRate.Location = new System.Drawing.Point(30, 180);
         this.lblDischargeRate.Name = "lblDischargeRate";
         this.lblDischargeRate.Size = new System.Drawing.Size(140, 20);
         this.lblDischargeRate.TabIndex = 10;
         this.lblDischargeRate.Text = "排出節拍 (Rate):";
         this.lblDischargeRate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // nudDischargeRate
         // 
         this.nudDischargeRate.Location = new System.Drawing.Point(180, 180);
         this.nudDischargeRate.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
         this.nudDischargeRate.Name = "nudDischargeRate";
         this.nudDischargeRate.Size = new System.Drawing.Size(300, 23);
         this.nudDischargeRate.TabIndex = 11;
         // 
         // lblStopTime
         // 
         this.lblStopTime.Location = new System.Drawing.Point(30, 212);
         this.lblStopTime.Name = "lblStopTime";
         this.lblStopTime.Size = new System.Drawing.Size(140, 20);
         this.lblStopTime.TabIndex = 12;
         this.lblStopTime.Text = "停止時間 (Stop Time):";
         this.lblStopTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // nudStopTime
         // 
         this.nudStopTime.DecimalPlaces = 1;
         this.nudStopTime.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
         this.nudStopTime.Location = new System.Drawing.Point(180, 212);
         this.nudStopTime.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
         this.nudStopTime.Name = "nudStopTime";
         this.nudStopTime.Size = new System.Drawing.Size(300, 23);
         this.nudStopTime.TabIndex = 13;
         // 
         // lblProcessingCounter
         // 
         this.lblProcessingCounter.Location = new System.Drawing.Point(30, 244);
         this.lblProcessingCounter.Name = "lblProcessingCounter";
         this.lblProcessingCounter.Size = new System.Drawing.Size(140, 20);
         this.lblProcessingCounter.TabIndex = 14;
         this.lblProcessingCounter.Text = "處理計數 (Counter):";
         this.lblProcessingCounter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // nudProcessingCounter
         // 
         this.nudProcessingCounter.Location = new System.Drawing.Point(180, 244);
         this.nudProcessingCounter.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
         this.nudProcessingCounter.Name = "nudProcessingCounter";
         this.nudProcessingCounter.Size = new System.Drawing.Size(300, 23);
         this.nudProcessingCounter.TabIndex = 15;
         // 
         // lblRetainedBoardCount
         // 
         this.lblRetainedBoardCount.Location = new System.Drawing.Point(30, 276);
         this.lblRetainedBoardCount.Name = "lblRetainedBoardCount";
         this.lblRetainedBoardCount.Size = new System.Drawing.Size(140, 20);
         this.lblRetainedBoardCount.TabIndex = 16;
         this.lblRetainedBoardCount.Text = "滯留板數 (Retained):";
         this.lblRetainedBoardCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // nudRetainedBoardCount
         // 
         this.nudRetainedBoardCount.Location = new System.Drawing.Point(180, 276);
         this.nudRetainedBoardCount.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
         this.nudRetainedBoardCount.Name = "nudRetainedBoardCount";
         this.nudRetainedBoardCount.Size = new System.Drawing.Size(300, 23);
         this.nudRetainedBoardCount.TabIndex = 17;
         // 
         // lblCurrentRecipeNo
         // 
         this.lblCurrentRecipeNo.Location = new System.Drawing.Point(30, 308);
         this.lblCurrentRecipeNo.Name = "lblCurrentRecipeNo";
         this.lblCurrentRecipeNo.Size = new System.Drawing.Size(140, 20);
         this.lblCurrentRecipeNo.TabIndex = 18;
         this.lblCurrentRecipeNo.Text = "配方編號 (Recipe No):";
         this.lblCurrentRecipeNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // nudCurrentRecipeNo
         // 
         this.nudCurrentRecipeNo.Location = new System.Drawing.Point(180, 308);
         this.nudCurrentRecipeNo.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
         this.nudCurrentRecipeNo.Name = "nudCurrentRecipeNo";
         this.nudCurrentRecipeNo.Size = new System.Drawing.Size(300, 23);
         this.nudCurrentRecipeNo.TabIndex = 19;
         // 
         // lblBoardThickness
         // 
         this.lblBoardThickness.Location = new System.Drawing.Point(30, 340);
         this.lblBoardThickness.Name = "lblBoardThickness";
         this.lblBoardThickness.Size = new System.Drawing.Size(140, 20);
         this.lblBoardThickness.TabIndex = 20;
         this.lblBoardThickness.Text = "板厚狀態 (Thickness):";
         this.lblBoardThickness.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // nudBoardThickness
         // 
         this.nudBoardThickness.Location = new System.Drawing.Point(180, 340);
         this.nudBoardThickness.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
         this.nudBoardThickness.Name = "nudBoardThickness";
         this.nudBoardThickness.Size = new System.Drawing.Size(300, 23);
         this.nudBoardThickness.TabIndex = 21;
         // 
         // lblRecipeName
         // 
         this.lblRecipeName.Location = new System.Drawing.Point(30, 372);
         this.lblRecipeName.Name = "lblRecipeName";
         this.lblRecipeName.Size = new System.Drawing.Size(140, 20);
         this.lblRecipeName.TabIndex = 22;
         this.lblRecipeName.Text = "配方名稱 (Name):";
         this.lblRecipeName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // txtRecipeName
         // 
         this.txtRecipeName.Location = new System.Drawing.Point(180, 372);
         this.txtRecipeName.MaxLength = 100;
         this.txtRecipeName.Name = "txtRecipeName";
         this.txtRecipeName.Size = new System.Drawing.Size(300, 23);
         this.txtRecipeName.TabIndex = 23;
         // 
         // tabPageAlarm
         // 
         this.tabPageAlarm.Controls.Add(this.lblError1);
         this.tabPageAlarm.Controls.Add(this.nudError1);
         this.tabPageAlarm.Controls.Add(this.lblError2);
         this.tabPageAlarm.Controls.Add(this.nudError2);
         this.tabPageAlarm.Controls.Add(this.lblError3);
         this.tabPageAlarm.Controls.Add(this.nudError3);
         this.tabPageAlarm.Controls.Add(this.lblError4);
         this.tabPageAlarm.Controls.Add(this.nudError4);
         this.tabPageAlarm.Controls.Add(this.lblError5);
         this.tabPageAlarm.Controls.Add(this.nudError5);
         this.tabPageAlarm.Controls.Add(this.lblError6);
         this.tabPageAlarm.Controls.Add(this.nudError6);
         this.tabPageAlarm.Controls.Add(this.lblError7);
         this.tabPageAlarm.Controls.Add(this.nudError7);
         this.tabPageAlarm.Controls.Add(this.lblError8);
         this.tabPageAlarm.Controls.Add(this.nudError8);
         this.tabPageAlarm.Controls.Add(this.lblError9);
         this.tabPageAlarm.Controls.Add(this.nudError9);
         this.tabPageAlarm.Controls.Add(this.lblError10);
         this.tabPageAlarm.Controls.Add(this.nudError10);
         this.tabPageAlarm.Controls.Add(this.lblError11);
         this.tabPageAlarm.Controls.Add(this.nudError11);
         this.tabPageAlarm.Controls.Add(this.lblError12);
         this.tabPageAlarm.Controls.Add(this.nudError12);
         this.tabPageAlarm.Location = new System.Drawing.Point(4, 25);
         this.tabPageAlarm.Name = "tabPageAlarm";
         this.tabPageAlarm.Padding = new System.Windows.Forms.Padding(3);
         this.tabPageAlarm.Size = new System.Drawing.Size(576, 431);
         this.tabPageAlarm.TabIndex = 2;
         this.tabPageAlarm.Text = "警報設定 (Alarm)";
         this.tabPageAlarm.UseVisualStyleBackColor = true;
         // 
         // lblError1
         // 
         this.lblError1.Location = new System.Drawing.Point(20, 20);
         this.lblError1.Name = "lblError1";
         this.lblError1.Size = new System.Drawing.Size(75, 20);
         this.lblError1.TabIndex = 0;
         this.lblError1.Text = "Code 01:";
         this.lblError1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // nudError1
         // 
         this.nudError1.Location = new System.Drawing.Point(100, 20);
         this.nudError1.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
         this.nudError1.Name = "nudError1";
         this.nudError1.Size = new System.Drawing.Size(80, 23);
         this.nudError1.TabIndex = 1;
         // 
         // lblError2
         // 
         this.lblError2.Location = new System.Drawing.Point(20, 52);
         this.lblError2.Name = "lblError2";
         this.lblError2.Size = new System.Drawing.Size(75, 20);
         this.lblError2.TabIndex = 2;
         this.lblError2.Text = "Code 02:";
         this.lblError2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // nudError2
         // 
         this.nudError2.Location = new System.Drawing.Point(100, 52);
         this.nudError2.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
         this.nudError2.Name = "nudError2";
         this.nudError2.Size = new System.Drawing.Size(80, 23);
         this.nudError2.TabIndex = 3;
         // 
         // lblError3
         // 
         this.lblError3.Location = new System.Drawing.Point(20, 84);
         this.lblError3.Name = "lblError3";
         this.lblError3.Size = new System.Drawing.Size(75, 20);
         this.lblError3.TabIndex = 4;
         this.lblError3.Text = "Code 03:";
         this.lblError3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // nudError3
         // 
         this.nudError3.Location = new System.Drawing.Point(100, 84);
         this.nudError3.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
         this.nudError3.Name = "nudError3";
         this.nudError3.Size = new System.Drawing.Size(80, 23);
         this.nudError3.TabIndex = 5;
         // 
         // lblError4
         // 
         this.lblError4.Location = new System.Drawing.Point(20, 116);
         this.lblError4.Name = "lblError4";
         this.lblError4.Size = new System.Drawing.Size(75, 20);
         this.lblError4.TabIndex = 6;
         this.lblError4.Text = "Code 04:";
         this.lblError4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // nudError4
         // 
         this.nudError4.Location = new System.Drawing.Point(100, 116);
         this.nudError4.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
         this.nudError4.Name = "nudError4";
         this.nudError4.Size = new System.Drawing.Size(80, 23);
         this.nudError4.TabIndex = 7;
         // 
         // lblError5
         // 
         this.lblError5.Location = new System.Drawing.Point(20, 148);
         this.lblError5.Name = "lblError5";
         this.lblError5.Size = new System.Drawing.Size(75, 20);
         this.lblError5.TabIndex = 8;
         this.lblError5.Text = "Code 05:";
         this.lblError5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // nudError5
         // 
         this.nudError5.Location = new System.Drawing.Point(100, 148);
         this.nudError5.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
         this.nudError5.Name = "nudError5";
         this.nudError5.Size = new System.Drawing.Size(80, 23);
         this.nudError5.TabIndex = 9;
         // 
         // lblError6
         // 
         this.lblError6.Location = new System.Drawing.Point(20, 180);
         this.lblError6.Name = "lblError6";
         this.lblError6.Size = new System.Drawing.Size(75, 20);
         this.lblError6.TabIndex = 10;
         this.lblError6.Text = "Code 06:";
         this.lblError6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // nudError6
         // 
         this.nudError6.Location = new System.Drawing.Point(100, 180);
         this.nudError6.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
         this.nudError6.Name = "nudError6";
         this.nudError6.Size = new System.Drawing.Size(80, 23);
         this.nudError6.TabIndex = 11;
         // 
         // lblError7
         // 
         this.lblError7.Location = new System.Drawing.Point(200, 20);
         this.lblError7.Name = "lblError7";
         this.lblError7.Size = new System.Drawing.Size(75, 20);
         this.lblError7.TabIndex = 12;
         this.lblError7.Text = "Code 07:";
         this.lblError7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // nudError7
         // 
         this.nudError7.Location = new System.Drawing.Point(280, 20);
         this.nudError7.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
         this.nudError7.Name = "nudError7";
         this.nudError7.Size = new System.Drawing.Size(80, 23);
         this.nudError7.TabIndex = 13;
         // 
         // lblError8
         // 
         this.lblError8.Location = new System.Drawing.Point(200, 52);
         this.lblError8.Name = "lblError8";
         this.lblError8.Size = new System.Drawing.Size(75, 20);
         this.lblError8.TabIndex = 14;
         this.lblError8.Text = "Code 08:";
         this.lblError8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // nudError8
         // 
         this.nudError8.Location = new System.Drawing.Point(280, 52);
         this.nudError8.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
         this.nudError8.Name = "nudError8";
         this.nudError8.Size = new System.Drawing.Size(80, 23);
         this.nudError8.TabIndex = 15;
         // 
         // lblError9
         // 
         this.lblError9.Location = new System.Drawing.Point(200, 84);
         this.lblError9.Name = "lblError9";
         this.lblError9.Size = new System.Drawing.Size(75, 20);
         this.lblError9.TabIndex = 16;
         this.lblError9.Text = "Code 09:";
         this.lblError9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // nudError9
         // 
         this.nudError9.Location = new System.Drawing.Point(280, 84);
         this.nudError9.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
         this.nudError9.Name = "nudError9";
         this.nudError9.Size = new System.Drawing.Size(80, 23);
         this.nudError9.TabIndex = 17;
         // 
         // lblError10
         // 
         this.lblError10.Location = new System.Drawing.Point(200, 116);
         this.lblError10.Name = "lblError10";
         this.lblError10.Size = new System.Drawing.Size(75, 20);
         this.lblError10.TabIndex = 18;
         this.lblError10.Text = "Code 10:";
         this.lblError10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // nudError10
         // 
         this.nudError10.Location = new System.Drawing.Point(280, 116);
         this.nudError10.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
         this.nudError10.Name = "nudError10";
         this.nudError10.Size = new System.Drawing.Size(80, 23);
         this.nudError10.TabIndex = 19;
         // 
         // lblError11
         // 
         this.lblError11.Location = new System.Drawing.Point(200, 148);
         this.lblError11.Name = "lblError11";
         this.lblError11.Size = new System.Drawing.Size(75, 20);
         this.lblError11.TabIndex = 20;
         this.lblError11.Text = "Code 11:";
         this.lblError11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // nudError11
         // 
         this.nudError11.Location = new System.Drawing.Point(280, 148);
         this.nudError11.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
         this.nudError11.Name = "nudError11";
         this.nudError11.Size = new System.Drawing.Size(80, 23);
         this.nudError11.TabIndex = 21;
         // 
         // lblError12
         // 
         this.lblError12.Location = new System.Drawing.Point(200, 180);
         this.lblError12.Name = "lblError12";
         this.lblError12.Size = new System.Drawing.Size(75, 20);
         this.lblError12.TabIndex = 22;
         this.lblError12.Text = "Code 12:";
         this.lblError12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // nudError12
         // 
         this.nudError12.Location = new System.Drawing.Point(280, 180);
         this.nudError12.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
         this.nudError12.Name = "nudError12";
         this.nudError12.Size = new System.Drawing.Size(80, 23);
         this.nudError12.TabIndex = 23;
         // 
         // btnOK
         // 
         this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.btnOK.Location = new System.Drawing.Point(180, 480);
         this.btnOK.Name = "btnOK";
         this.btnOK.Size = new System.Drawing.Size(90, 30);
         this.btnOK.TabIndex = 1;
         this.btnOK.Text = "確定";
         this.btnOK.UseVisualStyleBackColor = true;
         this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
         // 
         // btnCancel
         // 
         this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.btnCancel.Location = new System.Drawing.Point(290, 480);
         this.btnCancel.Name = "btnCancel";
         this.btnCancel.Size = new System.Drawing.Size(90, 30);
         this.btnCancel.TabIndex = 2;
         this.btnCancel.Text = "取消";
         this.btnCancel.UseVisualStyleBackColor = true;
         // 
         // nudUpstreamWaiting
         // 
         this.nudUpstreamWaiting.DecimalPlaces = 1;
         this.nudUpstreamWaiting.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
         this.nudUpstreamWaiting.Location = new System.Drawing.Point(180, 116);
         this.nudUpstreamWaiting.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            65536});
         this.nudUpstreamWaiting.Name = "nudUpstreamWaiting";
         this.nudUpstreamWaiting.Size = new System.Drawing.Size(300, 23);
         this.nudUpstreamWaiting.TabIndex = 24;
         // 
         // nudDownstreamWaiting
         // 
         this.nudDownstreamWaiting.DecimalPlaces = 1;
         this.nudDownstreamWaiting.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
         this.nudDownstreamWaiting.Location = new System.Drawing.Point(180, 148);
         this.nudDownstreamWaiting.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            65536});
         this.nudDownstreamWaiting.Name = "nudDownstreamWaiting";
         this.nudDownstreamWaiting.Size = new System.Drawing.Size(300, 23);
         this.nudDownstreamWaiting.TabIndex = 25;
         // 
         // CommonReportSettingsForm
         // 
         this.AcceptButton = this.btnOK;
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.CancelButton = this.btnCancel;
         this.ClientSize = new System.Drawing.Size(584, 530);
         this.Controls.Add(this.btnCancel);
         this.Controls.Add(this.btnOK);
         this.Controls.Add(this.tabControl1);
         this.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "CommonReportSettingsForm";
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "定期上報設定 (Common Reporting Settings)";
         this.tabControl1.ResumeLayout(false);
         this.tabPageStatus1.ResumeLayout(false);
         this.tabPageStatus1.PerformLayout();
         this.tabPageStatus2.ResumeLayout(false);
         this.tabPageStatus2.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.nudDischargeRate)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudStopTime)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudProcessingCounter)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudRetainedBoardCount)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudCurrentRecipeNo)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudBoardThickness)).EndInit();
         this.tabPageAlarm.ResumeLayout(false);
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
         ((System.ComponentModel.ISupportInitialize)(this.nudUpstreamWaiting)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudDownstreamWaiting)).EndInit();
         this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageStatus1;
        private System.Windows.Forms.TabPage tabPageStatus2;
        private System.Windows.Forms.TabPage tabPageAlarm;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;

        // Status 1 Controls
        private System.Windows.Forms.ComboBox cboAlarmStatus;
        private System.Windows.Forms.ComboBox cboMachineStatus;
        private System.Windows.Forms.ComboBox cboActionStatus;
        private System.Windows.Forms.ComboBox cboWaitingStatus;
        private System.Windows.Forms.ComboBox cboControlStatus;
        private System.Windows.Forms.Label lblAlarm;
        private System.Windows.Forms.Label lblMachine;
        private System.Windows.Forms.Label lblAction;
        private System.Windows.Forms.Label lblWaiting;
        private System.Windows.Forms.Label lblControl;

        // Status 2 Controls
        private System.Windows.Forms.ComboBox cmbRedLight;
        private System.Windows.Forms.ComboBox cmbYellowLight;
        private System.Windows.Forms.ComboBox cmbGreenLight;
        private System.Windows.Forms.NumericUpDown nudDischargeRate;
        private System.Windows.Forms.NumericUpDown nudStopTime;
        private System.Windows.Forms.NumericUpDown nudProcessingCounter;
        private System.Windows.Forms.NumericUpDown nudRetainedBoardCount;
        private System.Windows.Forms.NumericUpDown nudCurrentRecipeNo;
        private System.Windows.Forms.NumericUpDown nudBoardThickness;
        private System.Windows.Forms.TextBox txtRecipeName;
        private System.Windows.Forms.Label lblRedLight;
        private System.Windows.Forms.Label lblYellowLight;
        private System.Windows.Forms.Label lblGreenLight;
        private System.Windows.Forms.Label lblUpstreamWaiting;
        private System.Windows.Forms.Label lblDownstreamWaiting;
        private System.Windows.Forms.Label lblDischargeRate;
        private System.Windows.Forms.Label lblStopTime;
        private System.Windows.Forms.Label lblProcessingCounter;
        private System.Windows.Forms.Label lblRetainedBoardCount;
        private System.Windows.Forms.Label lblCurrentRecipeNo;
        private System.Windows.Forms.Label lblBoardThickness;
        private System.Windows.Forms.Label lblRecipeName;

        // Alarm Controls
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
      private System.Windows.Forms.NumericUpDown nudDownstreamWaiting;
      private System.Windows.Forms.NumericUpDown nudUpstreamWaiting;
   }
}
