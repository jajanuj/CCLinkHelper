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
            this.tabPageStatus2 = new System.Windows.Forms.TabPage();
            this.tabPageAlarm = new System.Windows.Forms.TabPage();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();

            // Status 1 Controls
            this.cboAlarmStatus = new System.Windows.Forms.ComboBox();
            this.cboMachineStatus = new System.Windows.Forms.ComboBox();
            this.cboActionStatus = new System.Windows.Forms.ComboBox();
            this.cboWaitingStatus = new System.Windows.Forms.ComboBox();
            this.cboControlStatus = new System.Windows.Forms.ComboBox();
            this.lblAlarm = new System.Windows.Forms.Label();
            this.lblMachine = new System.Windows.Forms.Label();
            this.lblAction = new System.Windows.Forms.Label();
            this.lblWaiting = new System.Windows.Forms.Label();
            this.lblControl = new System.Windows.Forms.Label();

            // Status 2 Controls
            this.cmbRedLight = new System.Windows.Forms.ComboBox();
            this.cmbYellowLight = new System.Windows.Forms.ComboBox();
            this.cmbGreenLight = new System.Windows.Forms.ComboBox();
            this.cmbUpstreamWaiting = new System.Windows.Forms.ComboBox();
            this.cmbDownstreamWaiting = new System.Windows.Forms.ComboBox();
            this.nudDischargeRate = new System.Windows.Forms.NumericUpDown();
            this.nudStopTime = new System.Windows.Forms.NumericUpDown();
            this.nudProcessingCounter = new System.Windows.Forms.NumericUpDown();
            this.nudRetainedBoardCount = new System.Windows.Forms.NumericUpDown();
            this.nudCurrentRecipeNo = new System.Windows.Forms.NumericUpDown();
            this.nudBoardThickness = new System.Windows.Forms.NumericUpDown();
            this.txtRecipeName = new System.Windows.Forms.TextBox();
            this.lblRedLight = new System.Windows.Forms.Label();
            this.lblYellowLight = new System.Windows.Forms.Label();
            this.lblGreenLight = new System.Windows.Forms.Label();
            this.lblUpstreamWaiting = new System.Windows.Forms.Label();
            this.lblDownstreamWaiting = new System.Windows.Forms.Label();
            this.lblDischargeRate = new System.Windows.Forms.Label();
            this.lblStopTime = new System.Windows.Forms.Label();
            this.lblProcessingCounter = new System.Windows.Forms.Label();
            this.lblRetainedBoardCount = new System.Windows.Forms.Label();
            this.lblCurrentRecipeNo = new System.Windows.Forms.Label();
            this.lblBoardThickness = new System.Windows.Forms.Label();
            this.lblRecipeName = new System.Windows.Forms.Label();

            // Alarm Controls - Array initialization style manually unrolled
            this.nudError1 = new System.Windows.Forms.NumericUpDown();
            this.nudError2 = new System.Windows.Forms.NumericUpDown();
            this.nudError3 = new System.Windows.Forms.NumericUpDown();
            this.nudError4 = new System.Windows.Forms.NumericUpDown();
            this.nudError5 = new System.Windows.Forms.NumericUpDown();
            this.nudError6 = new System.Windows.Forms.NumericUpDown();
            this.nudError7 = new System.Windows.Forms.NumericUpDown();
            this.nudError8 = new System.Windows.Forms.NumericUpDown();
            this.nudError9 = new System.Windows.Forms.NumericUpDown();
            this.nudError10 = new System.Windows.Forms.NumericUpDown();
            this.nudError11 = new System.Windows.Forms.NumericUpDown();
            this.nudError12 = new System.Windows.Forms.NumericUpDown();
            this.lblError1 = new System.Windows.Forms.Label();
            this.lblError2 = new System.Windows.Forms.Label();
            this.lblError3 = new System.Windows.Forms.Label();
            this.lblError4 = new System.Windows.Forms.Label();
            this.lblError5 = new System.Windows.Forms.Label();
            this.lblError6 = new System.Windows.Forms.Label();
            this.lblError7 = new System.Windows.Forms.Label();
            this.lblError8 = new System.Windows.Forms.Label();
            this.lblError9 = new System.Windows.Forms.Label();
            this.lblError10 = new System.Windows.Forms.Label();
            this.lblError11 = new System.Windows.Forms.Label();
            this.lblError12 = new System.Windows.Forms.Label();
            
            this.tabControl1.SuspendLayout();
            this.tabPageStatus1.SuspendLayout();
            this.tabPageStatus2.SuspendLayout();
            this.tabPageAlarm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDischargeRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStopTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudProcessingCounter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRetainedBoardCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCurrentRecipeNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBoardThickness)).BeginInit();
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
            this.tabControl1.Size = new System.Drawing.Size(584, 460); // Adjusted Height
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
            this.tabPageStatus1.Location = new System.Drawing.Point(4, 25); // Font default padding
            this.tabPageStatus1.Name = "tabPageStatus1";
            this.tabPageStatus1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageStatus1.Size = new System.Drawing.Size(576, 431);
            this.tabPageStatus1.TabIndex = 0;
            this.tabPageStatus1.Text = "機台狀態 (Status 1)";
            this.tabPageStatus1.UseVisualStyleBackColor = true;

            // Status 1 Control Layout
            this.lblAlarm.AutoSize = true;
            this.lblAlarm.Location = new System.Drawing.Point(50, 30);
            this.lblAlarm.Text = "警報狀態:";
            this.cboAlarmStatus.Location = new System.Drawing.Point(160, 27);
            this.cboAlarmStatus.Size = new System.Drawing.Size(200, 24);
            this.cboAlarmStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.lblMachine.AutoSize = true;
            this.lblMachine.Location = new System.Drawing.Point(50, 70);
            this.lblMachine.Text = "機台狀態:";
            this.cboMachineStatus.Location = new System.Drawing.Point(160, 67);
            this.cboMachineStatus.Size = new System.Drawing.Size(200, 24);
            this.cboMachineStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.lblAction.AutoSize = true;
            this.lblAction.Location = new System.Drawing.Point(50, 110);
            this.lblAction.Text = "動作狀態:";
            this.cboActionStatus.Location = new System.Drawing.Point(160, 107);
            this.cboActionStatus.Size = new System.Drawing.Size(200, 24);
            this.cboActionStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.lblWaiting.AutoSize = true;
            this.lblWaiting.Location = new System.Drawing.Point(50, 150);
            this.lblWaiting.Text = "等待狀態:";
            this.cboWaitingStatus.Location = new System.Drawing.Point(160, 147);
            this.cboWaitingStatus.Size = new System.Drawing.Size(200, 24);
            this.cboWaitingStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.lblControl.AutoSize = true;
            this.lblControl.Location = new System.Drawing.Point(50, 190);
            this.lblControl.Text = "控制狀態:";
            this.cboControlStatus.Location = new System.Drawing.Point(160, 187);
            this.cboControlStatus.Size = new System.Drawing.Size(200, 24);
            this.cboControlStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            // 
            // tabPageStatus2
            // 
            this.tabPageStatus2.Controls.Add(this.lblRedLight);
            this.tabPageStatus2.Controls.Add(this.cmbRedLight);
            this.tabPageStatus2.Controls.Add(this.lblYellowLight);
            this.tabPageStatus2.Controls.Add(this.cmbYellowLight);
            this.tabPageStatus2.Controls.Add(this.lblGreenLight);
            this.tabPageStatus2.Controls.Add(this.cmbGreenLight);
            this.tabPageStatus2.Controls.Add(this.lblUpstreamWaiting);
            this.tabPageStatus2.Controls.Add(this.cmbUpstreamWaiting);
            this.tabPageStatus2.Controls.Add(this.lblDownstreamWaiting);
            this.tabPageStatus2.Controls.Add(this.cmbDownstreamWaiting);
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

            // Status 2 Layout (Directly using verified literals from previous fix)
            this.lblRedLight.Text = "紅燈狀態 (Red):";
            this.lblRedLight.Location = new System.Drawing.Point(30, 20);
            this.lblRedLight.Size = new System.Drawing.Size(140, 20);
            this.lblRedLight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmbRedLight.Location = new System.Drawing.Point(180, 20);
            this.cmbRedLight.Size = new System.Drawing.Size(300, 24);
            this.cmbRedLight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.lblYellowLight.Text = "黃燈狀態 (Yellow):";
            this.lblYellowLight.Location = new System.Drawing.Point(30, 52);
            this.lblYellowLight.Size = new System.Drawing.Size(140, 20);
            this.lblYellowLight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmbYellowLight.Location = new System.Drawing.Point(180, 52);
            this.cmbYellowLight.Size = new System.Drawing.Size(300, 24);
            this.cmbYellowLight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.lblGreenLight.Text = "綠燈狀態 (Green):";
            this.lblGreenLight.Location = new System.Drawing.Point(30, 84);
            this.lblGreenLight.Size = new System.Drawing.Size(140, 20);
            this.lblGreenLight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmbGreenLight.Location = new System.Drawing.Point(180, 84);
            this.cmbGreenLight.Size = new System.Drawing.Size(300, 24);
            this.cmbGreenLight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.lblUpstreamWaiting.Text = "上游等待 (Upstream):";
            this.lblUpstreamWaiting.Location = new System.Drawing.Point(30, 116);
            this.lblUpstreamWaiting.Size = new System.Drawing.Size(140, 20);
            this.lblUpstreamWaiting.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmbUpstreamWaiting.Location = new System.Drawing.Point(180, 116);
            this.cmbUpstreamWaiting.Size = new System.Drawing.Size(300, 24);
            this.cmbUpstreamWaiting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.lblDownstreamWaiting.Text = "下游等待 (Downstream):";
            this.lblDownstreamWaiting.Location = new System.Drawing.Point(30, 148);
            this.lblDownstreamWaiting.Size = new System.Drawing.Size(140, 20);
            this.lblDownstreamWaiting.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmbDownstreamWaiting.Location = new System.Drawing.Point(180, 148);
            this.cmbDownstreamWaiting.Size = new System.Drawing.Size(300, 24);
            this.cmbDownstreamWaiting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.lblDischargeRate.Text = "排出節拍 (Rate):";
            this.lblDischargeRate.Location = new System.Drawing.Point(30, 180);
            this.lblDischargeRate.Size = new System.Drawing.Size(140, 20);
            this.lblDischargeRate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.nudDischargeRate.Location = new System.Drawing.Point(180, 180);
            this.nudDischargeRate.Size = new System.Drawing.Size(300, 24);
            this.nudDischargeRate.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });

            this.lblStopTime.Text = "停止時間 (Stop Time):";
            this.lblStopTime.Location = new System.Drawing.Point(30, 212);
            this.lblStopTime.Size = new System.Drawing.Size(140, 20);
            this.lblStopTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.nudStopTime.Location = new System.Drawing.Point(180, 212);
            this.nudStopTime.Size = new System.Drawing.Size(300, 24);
            this.nudStopTime.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });

            this.lblProcessingCounter.Text = "處理計數 (Counter):";
            this.lblProcessingCounter.Location = new System.Drawing.Point(30, 244);
            this.lblProcessingCounter.Size = new System.Drawing.Size(140, 20);
            this.lblProcessingCounter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.nudProcessingCounter.Location = new System.Drawing.Point(180, 244);
            this.nudProcessingCounter.Size = new System.Drawing.Size(300, 24);
            this.nudProcessingCounter.Maximum = new decimal(new int[] { -1, -1, -1, 0 });

            this.lblRetainedBoardCount.Text = "滯留板數 (Retained):";
            this.lblRetainedBoardCount.Location = new System.Drawing.Point(30, 276);
            this.lblRetainedBoardCount.Size = new System.Drawing.Size(140, 20);
            this.lblRetainedBoardCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.nudRetainedBoardCount.Location = new System.Drawing.Point(180, 276);
            this.nudRetainedBoardCount.Size = new System.Drawing.Size(300, 24);
            this.nudRetainedBoardCount.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });

            this.lblCurrentRecipeNo.Text = "配方編號 (Recipe No):";
            this.lblCurrentRecipeNo.Location = new System.Drawing.Point(30, 308);
            this.lblCurrentRecipeNo.Size = new System.Drawing.Size(140, 20);
            this.lblCurrentRecipeNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.nudCurrentRecipeNo.Location = new System.Drawing.Point(180, 308);
            this.nudCurrentRecipeNo.Size = new System.Drawing.Size(300, 24);
            this.nudCurrentRecipeNo.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });

            this.lblBoardThickness.Text = "板厚狀態 (Thickness):";
            this.lblBoardThickness.Location = new System.Drawing.Point(30, 340);
            this.lblBoardThickness.Size = new System.Drawing.Size(140, 20);
            this.lblBoardThickness.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.nudBoardThickness.Location = new System.Drawing.Point(180, 340);
            this.nudBoardThickness.Size = new System.Drawing.Size(300, 24);
            this.nudBoardThickness.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });

            this.lblRecipeName.Text = "配方名稱 (Name):";
            this.lblRecipeName.Location = new System.Drawing.Point(30, 372);
            this.lblRecipeName.Size = new System.Drawing.Size(140, 20);
            this.lblRecipeName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.txtRecipeName.Location = new System.Drawing.Point(180, 372);
            this.txtRecipeName.Size = new System.Drawing.Size(300, 24);
            this.txtRecipeName.MaxLength = 100;

            // 
            // tabPageAlarm
            // 
            this.tabPageAlarm.Controls.Add(this.lblError1); this.tabPageAlarm.Controls.Add(this.nudError1);
            this.tabPageAlarm.Controls.Add(this.lblError2); this.tabPageAlarm.Controls.Add(this.nudError2);
            this.tabPageAlarm.Controls.Add(this.lblError3); this.tabPageAlarm.Controls.Add(this.nudError3);
            this.tabPageAlarm.Controls.Add(this.lblError4); this.tabPageAlarm.Controls.Add(this.nudError4);
            this.tabPageAlarm.Controls.Add(this.lblError5); this.tabPageAlarm.Controls.Add(this.nudError5);
            this.tabPageAlarm.Controls.Add(this.lblError6); this.tabPageAlarm.Controls.Add(this.nudError6);
            this.tabPageAlarm.Controls.Add(this.lblError7); this.tabPageAlarm.Controls.Add(this.nudError7);
            this.tabPageAlarm.Controls.Add(this.lblError8); this.tabPageAlarm.Controls.Add(this.nudError8);
            this.tabPageAlarm.Controls.Add(this.lblError9); this.tabPageAlarm.Controls.Add(this.nudError9);
            this.tabPageAlarm.Controls.Add(this.lblError10); this.tabPageAlarm.Controls.Add(this.nudError10);
            this.tabPageAlarm.Controls.Add(this.lblError11); this.tabPageAlarm.Controls.Add(this.nudError11);
            this.tabPageAlarm.Controls.Add(this.lblError12); this.tabPageAlarm.Controls.Add(this.nudError12);
            this.tabPageAlarm.Location = new System.Drawing.Point(4, 25);
            this.tabPageAlarm.Name = "tabPageAlarm";
            this.tabPageAlarm.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAlarm.Size = new System.Drawing.Size(576, 431);
            this.tabPageAlarm.TabIndex = 2;
            this.tabPageAlarm.Text = "警報設定 (Alarm)";
            this.tabPageAlarm.UseVisualStyleBackColor = true;

            // Alarm Layout (Using verified literals)
            this.lblError1.Text = "Code 01:"; this.lblError1.Location = new System.Drawing.Point(20, 20); this.lblError1.Size = new System.Drawing.Size(75, 20); this.lblError1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.nudError1.Location = new System.Drawing.Point(100, 20); this.nudError1.Size = new System.Drawing.Size(80, 24); this.nudError1.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });

            this.lblError2.Text = "Code 02:"; this.lblError2.Location = new System.Drawing.Point(20, 52); this.lblError2.Size = new System.Drawing.Size(75, 20); this.lblError2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.nudError2.Location = new System.Drawing.Point(100, 52); this.nudError2.Size = new System.Drawing.Size(80, 24); this.nudError2.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });

            this.lblError3.Text = "Code 03:"; this.lblError3.Location = new System.Drawing.Point(20, 84); this.lblError3.Size = new System.Drawing.Size(75, 20); this.lblError3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.nudError3.Location = new System.Drawing.Point(100, 84); this.nudError3.Size = new System.Drawing.Size(80, 24); this.nudError3.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });

            this.lblError4.Text = "Code 04:"; this.lblError4.Location = new System.Drawing.Point(20, 116); this.lblError4.Size = new System.Drawing.Size(75, 20); this.lblError4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.nudError4.Location = new System.Drawing.Point(100, 116); this.nudError4.Size = new System.Drawing.Size(80, 24); this.nudError4.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });

            this.lblError5.Text = "Code 05:"; this.lblError5.Location = new System.Drawing.Point(20, 148); this.lblError5.Size = new System.Drawing.Size(75, 20); this.lblError5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.nudError5.Location = new System.Drawing.Point(100, 148); this.nudError5.Size = new System.Drawing.Size(80, 24); this.nudError5.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });

            this.lblError6.Text = "Code 06:"; this.lblError6.Location = new System.Drawing.Point(20, 180); this.lblError6.Size = new System.Drawing.Size(75, 20); this.lblError6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.nudError6.Location = new System.Drawing.Point(100, 180); this.nudError6.Size = new System.Drawing.Size(80, 24); this.nudError6.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });

            this.lblError7.Text = "Code 07:"; this.lblError7.Location = new System.Drawing.Point(200, 20); this.lblError7.Size = new System.Drawing.Size(75, 20); this.lblError7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.nudError7.Location = new System.Drawing.Point(280, 20); this.nudError7.Size = new System.Drawing.Size(80, 24); this.nudError7.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });

            this.lblError8.Text = "Code 08:"; this.lblError8.Location = new System.Drawing.Point(200, 52); this.lblError8.Size = new System.Drawing.Size(75, 20); this.lblError8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.nudError8.Location = new System.Drawing.Point(280, 52); this.nudError8.Size = new System.Drawing.Size(80, 24); this.nudError8.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });

            this.lblError9.Text = "Code 09:"; this.lblError9.Location = new System.Drawing.Point(200, 84); this.lblError9.Size = new System.Drawing.Size(75, 20); this.lblError9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.nudError9.Location = new System.Drawing.Point(280, 84); this.nudError9.Size = new System.Drawing.Size(80, 24); this.nudError9.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });

            this.lblError10.Text = "Code 10:"; this.lblError10.Location = new System.Drawing.Point(200, 116); this.lblError10.Size = new System.Drawing.Size(75, 20); this.lblError10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.nudError10.Location = new System.Drawing.Point(280, 116); this.nudError10.Size = new System.Drawing.Size(80, 24); this.nudError10.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });

            this.lblError11.Text = "Code 11:"; this.lblError11.Location = new System.Drawing.Point(200, 148); this.lblError11.Size = new System.Drawing.Size(75, 20); this.lblError11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.nudError11.Location = new System.Drawing.Point(280, 148); this.nudError11.Size = new System.Drawing.Size(80, 24); this.nudError11.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });

            this.lblError12.Text = "Code 12:"; this.lblError12.Location = new System.Drawing.Point(200, 180); this.lblError12.Size = new System.Drawing.Size(75, 20); this.lblError12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.nudError12.Location = new System.Drawing.Point(280, 180); this.nudError12.Size = new System.Drawing.Size(80, 24); this.nudError12.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });


            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(180, 480);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 30);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "確定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);

            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(290, 480);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;

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
            this.tabPageAlarm.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudDischargeRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStopTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudProcessingCounter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRetainedBoardCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCurrentRecipeNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBoardThickness)).EndInit();
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
        private System.Windows.Forms.ComboBox cmbUpstreamWaiting;
        private System.Windows.Forms.ComboBox cmbDownstreamWaiting;
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
    }
}
