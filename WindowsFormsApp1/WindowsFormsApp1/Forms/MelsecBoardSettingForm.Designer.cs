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
         this.paramHeartbeatInterval = new GRT.SDK.Framework.Component.ParamNumericUpDownUserControl();
         this.paramHeartbeatResponseAddress = new GRT.SDK.Framework.Component.ParamTextUserControl();
         this.paramHeartbeatRequestAddress = new GRT.SDK.Framework.Component.ParamTextUserControl();
         this.label11 = new System.Windows.Forms.Label();
         this.numTimeSync = new System.Windows.Forms.NumericUpDown();
         this.label12 = new System.Windows.Forms.Label();
         this.txtTrigger = new System.Windows.Forms.TextBox();
         this.label13 = new System.Windows.Forms.Label();
         this.txtData = new System.Windows.Forms.TextBox();
         this.tabTracking = new System.Windows.Forms.TabPage();
         this.label14 = new System.Windows.Forms.Label();
         this.txtLoadingRobotAddr = new System.Windows.Forms.TextBox();
         this.label15 = new System.Windows.Forms.Label();
         this.txtLoadingStationAddr = new System.Windows.Forms.TextBox();
         this.label16 = new System.Windows.Forms.Label();
         this.txtUnloadingRobotAddr = new System.Windows.Forms.TextBox();
         this.label17 = new System.Windows.Forms.Label();
         this.txtUnloadingStationAddr = new System.Windows.Forms.TextBox();
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
         this.tpMaintenance = new System.Windows.Forms.TabPage();
         this.paramMaintenanceT2Timeout = new GRT.SDK.Framework.Component.ParamNumericUpDownUserControl();
         this.paramMaintenanceT1Timeout = new GRT.SDK.Framework.Component.ParamNumericUpDownUserControl();
         this.tabControl1.SuspendLayout();
         this.tabGeneral.SuspendLayout();
         this.tabApp.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.numTimeSync)).BeginInit();
         this.tabTracking.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numNetwork)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numRetryCount)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numRetryBackoff)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numStation)).BeginInit();
         this.tpMaintenance.SuspendLayout();
         this.SuspendLayout();
         // 
         // tabControl1
         // 
         this.tabControl1.Controls.Add(this.tabApp);
         this.tabControl1.Controls.Add(this.tabTracking);
         this.tabControl1.Controls.Add(this.tpMaintenance);
         this.tabControl1.Location = new System.Drawing.Point(14, 16);
         this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
         this.tabControl1.Size = new System.Drawing.Size(439, 560);
         this.tabControl1.Controls.SetChildIndex(this.tpMaintenance, 0);
         this.tabControl1.Controls.SetChildIndex(this.tabTracking, 0);
         this.tabControl1.Controls.SetChildIndex(this.tabApp, 0);
         this.tabControl1.Controls.SetChildIndex(this.tabGeneral, 0);
         // 
         // tabGeneral
         // 
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
         this.tabGeneral.Location = new System.Drawing.Point(4, 25);
         this.tabGeneral.Margin = new System.Windows.Forms.Padding(4);
         this.tabGeneral.Padding = new System.Windows.Forms.Padding(4);
         this.tabGeneral.Size = new System.Drawing.Size(431, 531);
         this.tabGeneral.Controls.SetChildIndex(this.cmbEndian, 0);
         this.tabGeneral.Controls.SetChildIndex(this.label10, 0);
         this.tabGeneral.Controls.SetChildIndex(this.chkIsx64, 0);
         this.tabGeneral.Controls.SetChildIndex(this.numStation, 0);
         this.tabGeneral.Controls.SetChildIndex(this.label2, 0);
         this.tabGeneral.Controls.SetChildIndex(this.numRetryBackoff, 0);
         this.tabGeneral.Controls.SetChildIndex(this.label9, 0);
         this.tabGeneral.Controls.SetChildIndex(this.numRetryCount, 0);
         this.tabGeneral.Controls.SetChildIndex(this.label8, 0);
         this.tabGeneral.Controls.SetChildIndex(this.numTimeout, 0);
         this.tabGeneral.Controls.SetChildIndex(this.label7, 0);
         this.tabGeneral.Controls.SetChildIndex(this.numNetwork, 0);
         this.tabGeneral.Controls.SetChildIndex(this.label6, 0);
         this.tabGeneral.Controls.SetChildIndex(this.numPort, 0);
         this.tabGeneral.Controls.SetChildIndex(this.label5, 0);
         this.tabGeneral.Controls.SetChildIndex(this.label4, 0);
         // 
         // label4
         // 
         this.label4.Location = new System.Drawing.Point(12, 193);
         this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
         this.label4.Size = new System.Drawing.Size(115, 16);
         // 
         // btnSave
         // 
         this.btnSave.Location = new System.Drawing.Point(358, 595);
         this.btnSave.Margin = new System.Windows.Forms.Padding(4);
         this.btnSave.Size = new System.Drawing.Size(88, 33);
         // 
         // tabApp
         // 
         this.tabApp.Controls.Add(this.paramHeartbeatInterval);
         this.tabApp.Controls.Add(this.paramHeartbeatResponseAddress);
         this.tabApp.Controls.Add(this.paramHeartbeatRequestAddress);
         this.tabApp.Controls.Add(this.label11);
         this.tabApp.Controls.Add(this.numTimeSync);
         this.tabApp.Controls.Add(this.label12);
         this.tabApp.Controls.Add(this.txtTrigger);
         this.tabApp.Controls.Add(this.label13);
         this.tabApp.Controls.Add(this.txtData);
         this.tabApp.Location = new System.Drawing.Point(4, 25);
         this.tabApp.Margin = new System.Windows.Forms.Padding(4);
         this.tabApp.Name = "tabApp";
         this.tabApp.Padding = new System.Windows.Forms.Padding(4);
         this.tabApp.Size = new System.Drawing.Size(431, 531);
         this.tabApp.TabIndex = 1;
         this.tabApp.Text = "進階設定";
         this.tabApp.UseVisualStyleBackColor = true;
         // 
         // paramHeartbeatInterval
         // 
         this.paramHeartbeatInterval.Caption = "Heartbeat Inteval(ms)";
         this.paramHeartbeatInterval.CaptionWidth = 80F;
         this.paramHeartbeatInterval.DecimalPlaces = 0;
         this.paramHeartbeatInterval.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.paramHeartbeatInterval.Increment = 1D;
         this.paramHeartbeatInterval.Location = new System.Drawing.Point(15, 285);
         this.paramHeartbeatInterval.MaxNumber = 1000D;
         this.paramHeartbeatInterval.MinimumSize = new System.Drawing.Size(150, 30);
         this.paramHeartbeatInterval.MinNumber = 0D;
         this.paramHeartbeatInterval.Name = "paramHeartbeatInterval";
         this.paramHeartbeatInterval.Size = new System.Drawing.Size(300, 30);
         this.paramHeartbeatInterval.TabIndex = 12;
         this.paramHeartbeatInterval.Value = 300D;
         // 
         // paramHeartbeatResponseAddress
         // 
         this.paramHeartbeatResponseAddress.Caption = "Heartbeat Response Address";
         this.paramHeartbeatResponseAddress.CaptionWidth = 80F;
         this.paramHeartbeatResponseAddress.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.paramHeartbeatResponseAddress.Location = new System.Drawing.Point(15, 249);
         this.paramHeartbeatResponseAddress.MinimumSize = new System.Drawing.Size(150, 30);
         this.paramHeartbeatResponseAddress.Name = "paramHeartbeatResponseAddress";
         this.paramHeartbeatResponseAddress.ReadOnly = false;
         this.paramHeartbeatResponseAddress.ShowKeyboard = false;
         this.paramHeartbeatResponseAddress.Size = new System.Drawing.Size(300, 30);
         this.paramHeartbeatResponseAddress.TabIndex = 11;
         this.paramHeartbeatResponseAddress.TextValue = "";
         // 
         // paramHeartbeatRequestAddress
         // 
         this.paramHeartbeatRequestAddress.Caption = "Heartbeat Request Address";
         this.paramHeartbeatRequestAddress.CaptionWidth = 80F;
         this.paramHeartbeatRequestAddress.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.paramHeartbeatRequestAddress.Location = new System.Drawing.Point(15, 213);
         this.paramHeartbeatRequestAddress.MinimumSize = new System.Drawing.Size(150, 30);
         this.paramHeartbeatRequestAddress.Name = "paramHeartbeatRequestAddress";
         this.paramHeartbeatRequestAddress.ReadOnly = false;
         this.paramHeartbeatRequestAddress.ShowKeyboard = false;
         this.paramHeartbeatRequestAddress.Size = new System.Drawing.Size(300, 30);
         this.paramHeartbeatRequestAddress.TabIndex = 10;
         this.paramHeartbeatRequestAddress.TextValue = "";
         // 
         // label11
         // 
         this.label11.AutoSize = true;
         this.label11.Location = new System.Drawing.Point(12, 63);
         this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
         this.label11.Name = "label11";
         this.label11.Size = new System.Drawing.Size(68, 16);
         this.label11.TabIndex = 2;
         this.label11.Text = "TS Int (ms):";
         // 
         // numTimeSync
         // 
         this.numTimeSync.Location = new System.Drawing.Point(117, 60);
         this.numTimeSync.Margin = new System.Windows.Forms.Padding(4);
         this.numTimeSync.Maximum = new decimal(new int[] {
            3600000,
            0,
            0,
            0});
         this.numTimeSync.Name = "numTimeSync";
         this.numTimeSync.Size = new System.Drawing.Size(93, 23);
         this.numTimeSync.TabIndex = 3;
         this.numTimeSync.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
         // 
         // label12
         // 
         this.label12.AutoSize = true;
         this.label12.Location = new System.Drawing.Point(12, 103);
         this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
         this.label12.Name = "label12";
         this.label12.Size = new System.Drawing.Size(68, 16);
         this.label12.TabIndex = 4;
         this.label12.Text = "TS Trigger:";
         // 
         // txtTrigger
         // 
         this.txtTrigger.Location = new System.Drawing.Point(117, 100);
         this.txtTrigger.Margin = new System.Windows.Forms.Padding(4);
         this.txtTrigger.Name = "txtTrigger";
         this.txtTrigger.Size = new System.Drawing.Size(93, 23);
         this.txtTrigger.TabIndex = 5;
         // 
         // label13
         // 
         this.label13.AutoSize = true;
         this.label13.Location = new System.Drawing.Point(12, 143);
         this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
         this.label13.Name = "label13";
         this.label13.Size = new System.Drawing.Size(54, 16);
         this.label13.TabIndex = 6;
         this.label13.Text = "TS Data:";
         // 
         // txtData
         // 
         this.txtData.Location = new System.Drawing.Point(117, 140);
         this.txtData.Margin = new System.Windows.Forms.Padding(4);
         this.txtData.Name = "txtData";
         this.txtData.Size = new System.Drawing.Size(93, 23);
         this.txtData.TabIndex = 7;
         // 
         // tabTracking
         // 
         this.tabTracking.Controls.Add(this.label14);
         this.tabTracking.Controls.Add(this.txtLoadingRobotAddr);
         this.tabTracking.Controls.Add(this.label15);
         this.tabTracking.Controls.Add(this.txtLoadingStationAddr);
         this.tabTracking.Controls.Add(this.label16);
         this.tabTracking.Controls.Add(this.txtUnloadingRobotAddr);
         this.tabTracking.Controls.Add(this.label17);
         this.tabTracking.Controls.Add(this.txtUnloadingStationAddr);
         this.tabTracking.Location = new System.Drawing.Point(4, 25);
         this.tabTracking.Margin = new System.Windows.Forms.Padding(4);
         this.tabTracking.Name = "tabTracking";
         this.tabTracking.Padding = new System.Windows.Forms.Padding(4);
         this.tabTracking.Size = new System.Drawing.Size(431, 531);
         this.tabTracking.TabIndex = 2;
         this.tabTracking.Text = "追蹤設定";
         this.tabTracking.UseVisualStyleBackColor = true;
         // 
         // label14
         // 
         this.label14.AutoSize = true;
         this.label14.Location = new System.Drawing.Point(12, 23);
         this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
         this.label14.Name = "label14";
         this.label14.Size = new System.Drawing.Size(94, 16);
         this.label14.TabIndex = 0;
         this.label14.Text = "插框Robot位址:";
         // 
         // txtLoadingRobotAddr
         // 
         this.txtLoadingRobotAddr.Location = new System.Drawing.Point(140, 20);
         this.txtLoadingRobotAddr.Margin = new System.Windows.Forms.Padding(4);
         this.txtLoadingRobotAddr.Name = "txtLoadingRobotAddr";
         this.txtLoadingRobotAddr.Size = new System.Drawing.Size(116, 23);
         this.txtLoadingRobotAddr.TabIndex = 1;
         // 
         // label15
         // 
         this.label15.AutoSize = true;
         this.label15.Location = new System.Drawing.Point(12, 63);
         this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
         this.label15.Name = "label15";
         this.label15.Size = new System.Drawing.Size(70, 16);
         this.label15.TabIndex = 2;
         this.label15.Text = "插框站位址:";
         // 
         // txtLoadingStationAddr
         // 
         this.txtLoadingStationAddr.Location = new System.Drawing.Point(140, 60);
         this.txtLoadingStationAddr.Margin = new System.Windows.Forms.Padding(4);
         this.txtLoadingStationAddr.Name = "txtLoadingStationAddr";
         this.txtLoadingStationAddr.Size = new System.Drawing.Size(116, 23);
         this.txtLoadingStationAddr.TabIndex = 3;
         // 
         // label16
         // 
         this.label16.AutoSize = true;
         this.label16.Location = new System.Drawing.Point(12, 103);
         this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
         this.label16.Name = "label16";
         this.label16.Size = new System.Drawing.Size(94, 16);
         this.label16.TabIndex = 4;
         this.label16.Text = "拆框Robot位址:";
         // 
         // txtUnloadingRobotAddr
         // 
         this.txtUnloadingRobotAddr.Location = new System.Drawing.Point(140, 100);
         this.txtUnloadingRobotAddr.Margin = new System.Windows.Forms.Padding(4);
         this.txtUnloadingRobotAddr.Name = "txtUnloadingRobotAddr";
         this.txtUnloadingRobotAddr.Size = new System.Drawing.Size(116, 23);
         this.txtUnloadingRobotAddr.TabIndex = 5;
         // 
         // label17
         // 
         this.label17.AutoSize = true;
         this.label17.Location = new System.Drawing.Point(12, 143);
         this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
         this.label17.Name = "label17";
         this.label17.Size = new System.Drawing.Size(70, 16);
         this.label17.TabIndex = 6;
         this.label17.Text = "拆框站位址:";
         // 
         // txtUnloadingStationAddr
         // 
         this.txtUnloadingStationAddr.Location = new System.Drawing.Point(140, 140);
         this.txtUnloadingStationAddr.Margin = new System.Windows.Forms.Padding(4);
         this.txtUnloadingStationAddr.Name = "txtUnloadingStationAddr";
         this.txtUnloadingStationAddr.Size = new System.Drawing.Size(116, 23);
         this.txtUnloadingStationAddr.TabIndex = 7;
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(12, 23);
         this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(33, 16);
         this.label5.TabIndex = 0;
         this.label5.Text = "Port:";
         // 
         // numPort
         // 
         this.numPort.Location = new System.Drawing.Point(82, 20);
         this.numPort.Margin = new System.Windows.Forms.Padding(4);
         this.numPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
         this.numPort.Name = "numPort";
         this.numPort.Size = new System.Drawing.Size(93, 23);
         this.numPort.TabIndex = 1;
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Location = new System.Drawing.Point(12, 60);
         this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(58, 16);
         this.label6.TabIndex = 2;
         this.label6.Text = "Network:";
         // 
         // numNetwork
         // 
         this.numNetwork.Location = new System.Drawing.Point(82, 57);
         this.numNetwork.Margin = new System.Windows.Forms.Padding(4);
         this.numNetwork.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
         this.numNetwork.Name = "numNetwork";
         this.numNetwork.Size = new System.Drawing.Size(70, 23);
         this.numNetwork.TabIndex = 3;
         // 
         // label7
         // 
         this.label7.AutoSize = true;
         this.label7.Location = new System.Drawing.Point(210, 23);
         this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
         this.label7.Name = "label7";
         this.label7.Size = new System.Drawing.Size(84, 16);
         this.label7.TabIndex = 4;
         this.label7.Text = "Timeout (ms):";
         // 
         // numTimeout
         // 
         this.numTimeout.Location = new System.Drawing.Point(303, 20);
         this.numTimeout.Margin = new System.Windows.Forms.Padding(4);
         this.numTimeout.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
         this.numTimeout.Name = "numTimeout";
         this.numTimeout.Size = new System.Drawing.Size(93, 23);
         this.numTimeout.TabIndex = 5;
         this.numTimeout.Value = new decimal(new int[] {
            3000,
            0,
            0,
            0});
         // 
         // label8
         // 
         this.label8.AutoSize = true;
         this.label8.Location = new System.Drawing.Point(210, 60);
         this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
         this.label8.Name = "label8";
         this.label8.Size = new System.Drawing.Size(76, 16);
         this.label8.TabIndex = 6;
         this.label8.Text = "Retry Count:";
         // 
         // numRetryCount
         // 
         this.numRetryCount.Location = new System.Drawing.Point(303, 57);
         this.numRetryCount.Margin = new System.Windows.Forms.Padding(4);
         this.numRetryCount.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
         this.numRetryCount.Name = "numRetryCount";
         this.numRetryCount.Size = new System.Drawing.Size(70, 23);
         this.numRetryCount.TabIndex = 7;
         this.numRetryCount.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
         // 
         // label9
         // 
         this.label9.AutoSize = true;
         this.label9.Location = new System.Drawing.Point(210, 97);
         this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
         this.label9.Name = "label9";
         this.label9.Size = new System.Drawing.Size(84, 16);
         this.label9.TabIndex = 8;
         this.label9.Text = "Retry Backoff:";
         // 
         // numRetryBackoff
         // 
         this.numRetryBackoff.Location = new System.Drawing.Point(303, 95);
         this.numRetryBackoff.Margin = new System.Windows.Forms.Padding(4);
         this.numRetryBackoff.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
         this.numRetryBackoff.Name = "numRetryBackoff";
         this.numRetryBackoff.Size = new System.Drawing.Size(93, 23);
         this.numRetryBackoff.TabIndex = 9;
         this.numRetryBackoff.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(12, 135);
         this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(50, 16);
         this.label2.TabIndex = 10;
         this.label2.Text = "Station:";
         // 
         // numStation
         // 
         this.numStation.Location = new System.Drawing.Point(82, 132);
         this.numStation.Margin = new System.Windows.Forms.Padding(4);
         this.numStation.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
         this.numStation.Name = "numStation";
         this.numStation.Size = new System.Drawing.Size(70, 23);
         this.numStation.TabIndex = 11;
         // 
         // chkIsx64
         // 
         this.chkIsx64.AutoSize = true;
         this.chkIsx64.Location = new System.Drawing.Point(212, 135);
         this.chkIsx64.Margin = new System.Windows.Forms.Padding(4);
         this.chkIsx64.Name = "chkIsx64";
         this.chkIsx64.Size = new System.Drawing.Size(77, 20);
         this.chkIsx64.TabIndex = 12;
         this.chkIsx64.Text = "Is 64-bit?";
         this.chkIsx64.UseVisualStyleBackColor = true;
         // 
         // label10
         // 
         this.label10.AutoSize = true;
         this.label10.Location = new System.Drawing.Point(12, 172);
         this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
         this.label10.Name = "label10";
         this.label10.Size = new System.Drawing.Size(49, 16);
         this.label10.TabIndex = 13;
         this.label10.Text = "Endian:";
         // 
         // cmbEndian
         // 
         this.cmbEndian.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cmbEndian.FormattingEnabled = true;
         this.cmbEndian.Items.AddRange(new object[] {
            "Big",
            "Little"});
         this.cmbEndian.Location = new System.Drawing.Point(82, 169);
         this.cmbEndian.Margin = new System.Windows.Forms.Padding(4);
         this.cmbEndian.Name = "cmbEndian";
         this.cmbEndian.Size = new System.Drawing.Size(93, 24);
         this.cmbEndian.TabIndex = 14;
         // 
         // tpMaintenance
         // 
         this.tpMaintenance.Controls.Add(this.paramMaintenanceT2Timeout);
         this.tpMaintenance.Controls.Add(this.paramMaintenanceT1Timeout);
         this.tpMaintenance.Location = new System.Drawing.Point(4, 25);
         this.tpMaintenance.Name = "tpMaintenance";
         this.tpMaintenance.Padding = new System.Windows.Forms.Padding(3);
         this.tpMaintenance.Size = new System.Drawing.Size(431, 531);
         this.tpMaintenance.TabIndex = 3;
         this.tpMaintenance.Text = "維護設定";
         this.tpMaintenance.UseVisualStyleBackColor = true;
         // 
         // paramMaintenanceT2Timeout
         // 
         this.paramMaintenanceT2Timeout.Caption = "Maintenance T2 Timeout(ms)";
         this.paramMaintenanceT2Timeout.CaptionWidth = 80F;
         this.paramMaintenanceT2Timeout.DecimalPlaces = 0;
         this.paramMaintenanceT2Timeout.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.paramMaintenanceT2Timeout.Increment = 100D;
         this.paramMaintenanceT2Timeout.Location = new System.Drawing.Point(17, 57);
         this.paramMaintenanceT2Timeout.MaxNumber = 99000D;
         this.paramMaintenanceT2Timeout.MinimumSize = new System.Drawing.Size(150, 30);
         this.paramMaintenanceT2Timeout.MinNumber = 0D;
         this.paramMaintenanceT2Timeout.Name = "paramMaintenanceT2Timeout";
         this.paramMaintenanceT2Timeout.Size = new System.Drawing.Size(300, 30);
         this.paramMaintenanceT2Timeout.TabIndex = 14;
         this.paramMaintenanceT2Timeout.Value = 1000D;
         // 
         // paramMaintenanceT1Timeout
         // 
         this.paramMaintenanceT1Timeout.Caption = "Maintenance T1 Timeout(ms)";
         this.paramMaintenanceT1Timeout.CaptionWidth = 80F;
         this.paramMaintenanceT1Timeout.DecimalPlaces = 0;
         this.paramMaintenanceT1Timeout.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.paramMaintenanceT1Timeout.Increment = 100D;
         this.paramMaintenanceT1Timeout.Location = new System.Drawing.Point(17, 21);
         this.paramMaintenanceT1Timeout.MaxNumber = 99000D;
         this.paramMaintenanceT1Timeout.MinimumSize = new System.Drawing.Size(150, 30);
         this.paramMaintenanceT1Timeout.MinNumber = 0D;
         this.paramMaintenanceT1Timeout.Name = "paramMaintenanceT1Timeout";
         this.paramMaintenanceT1Timeout.Size = new System.Drawing.Size(300, 30);
         this.paramMaintenanceT1Timeout.TabIndex = 13;
         this.paramMaintenanceT1Timeout.Value = 1000D;
         // 
         // MelsecBoardSettingForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(467, 640);
         this.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
         this.Margin = new System.Windows.Forms.Padding(4);
         this.Name = "MelsecBoardSettingForm";
         this.Text = "MelsecBoardSettingForm";
         this.tabControl1.ResumeLayout(false);
         this.tabGeneral.ResumeLayout(false);
         this.tabGeneral.PerformLayout();
         this.tabApp.ResumeLayout(false);
         this.tabApp.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.numTimeSync)).EndInit();
         this.tabTracking.ResumeLayout(false);
         this.tabTracking.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numNetwork)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numRetryCount)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numRetryBackoff)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numStation)).EndInit();
         this.tpMaintenance.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.TabPage tabApp;
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

      // Tracking Controls
      private System.Windows.Forms.TabPage tabTracking;
      private System.Windows.Forms.Label label14;
      private System.Windows.Forms.TextBox txtLoadingRobotAddr;
      private System.Windows.Forms.Label label15;
      private System.Windows.Forms.TextBox txtLoadingStationAddr;
      private System.Windows.Forms.Label label16;
      private System.Windows.Forms.TextBox txtUnloadingRobotAddr;
      private System.Windows.Forms.Label label17;
      private System.Windows.Forms.TextBox txtUnloadingStationAddr;
      private GRT.SDK.Framework.Component.ParamTextUserControl paramHeartbeatRequestAddress;
      private GRT.SDK.Framework.Component.ParamTextUserControl paramHeartbeatResponseAddress;
      private GRT.SDK.Framework.Component.ParamNumericUpDownUserControl paramHeartbeatInterval;
      private System.Windows.Forms.TabPage tpMaintenance;
      private GRT.SDK.Framework.Component.ParamNumericUpDownUserControl paramMaintenanceT1Timeout;
      private GRT.SDK.Framework.Component.ParamNumericUpDownUserControl paramMaintenanceT2Timeout;
   }
}
