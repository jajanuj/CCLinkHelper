namespace WindowsFormsApp1.Forms
{
   partial class HandshakeControlForm
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
         this.grpStbSignals = new System.Windows.Forms.GroupBox();
         this.btnToggleLastFlag = new System.Windows.Forms.Button();
         this.btnTogglePosition = new System.Windows.Forms.Button();
         this.btnToggleBoardReceiveComplete = new System.Windows.Forms.Button();
         this.btnToggleDischargeRequest = new System.Windows.Forms.Button();
         this.btnToggleDischargeNotice = new System.Windows.Forms.Button();
         this.btnToggleStopped = new System.Windows.Forms.Button();
         this.btnToggleRunning = new System.Windows.Forms.Button();
         this.lblStbLastFlag = new System.Windows.Forms.Label();
         this.lblStbBoardReceiveComplete = new System.Windows.Forms.Label();
         this.lblStbDischargeRequest = new System.Windows.Forms.Label();
         this.lblStbDischargeNotice = new System.Windows.Forms.Label();
         this.lblStbIsStopped = new System.Windows.Forms.Label();
         this.lblStbIsRunning = new System.Windows.Forms.Label();
         this.txtStbBoardPosition = new System.Windows.Forms.TextBox();
         this.label1 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.label4 = new System.Windows.Forms.Label();
         this.label5 = new System.Windows.Forms.Label();
         this.label6 = new System.Windows.Forms.Label();
         this.label7 = new System.Windows.Forms.Label();
         this.grpRbSignals = new System.Windows.Forms.GroupBox();
         this.lblRbCannotReceive = new System.Windows.Forms.Label();
         this.lblRbBoardReceiveComplete1 = new System.Windows.Forms.Label();
         this.lblRbDischargeRequest = new System.Windows.Forms.Label();
         this.lblRbReadyToReceiveNotice1 = new System.Windows.Forms.Label();
         this.lblRbIsStopped = new System.Windows.Forms.Label();
         this.lblRbIsRunning = new System.Windows.Forms.Label();
         this.label8 = new System.Windows.Forms.Label();
         this.label9 = new System.Windows.Forms.Label();
         this.label10 = new System.Windows.Forms.Label();
         this.label11 = new System.Windows.Forms.Label();
         this.label12 = new System.Windows.Forms.Label();
         this.label13 = new System.Windows.Forms.Label();
         this.grpStatus = new System.Windows.Forms.GroupBox();
         this.txtCurrentState = new System.Windows.Forms.TextBox();
         this.label14 = new System.Windows.Forms.Label();
         this.grpControl = new System.Windows.Forms.GroupBox();
         this.btnReset = new System.Windows.Forms.Button();
         this.btnStop = new System.Windows.Forms.Button();
         this.btnStart = new System.Windows.Forms.Button();
         this.grpLog = new System.Windows.Forms.GroupBox();
         this.rtbLog = new System.Windows.Forms.RichTextBox();
         this.btnClearLog = new System.Windows.Forms.Button();
         this.grpStbSignals.SuspendLayout();
         this.grpRbSignals.SuspendLayout();
         this.grpStatus.SuspendLayout();
         this.grpControl.SuspendLayout();
         this.grpLog.SuspendLayout();
         this.SuspendLayout();
         // 
         // grpStbSignals
         // 
         this.grpStbSignals.Controls.Add(this.btnToggleLastFlag);
         this.grpStbSignals.Controls.Add(this.btnTogglePosition);
         this.grpStbSignals.Controls.Add(this.btnToggleBoardReceiveComplete);
         this.grpStbSignals.Controls.Add(this.btnToggleDischargeRequest);
         this.grpStbSignals.Controls.Add(this.btnToggleDischargeNotice);
         this.grpStbSignals.Controls.Add(this.btnToggleStopped);
         this.grpStbSignals.Controls.Add(this.btnToggleRunning);
         this.grpStbSignals.Controls.Add(this.label7);
         this.grpStbSignals.Controls.Add(this.label6);
         this.grpStbSignals.Controls.Add(this.label5);
         this.grpStbSignals.Controls.Add(this.label4);
         this.grpStbSignals.Controls.Add(this.label3);
         this.grpStbSignals.Controls.Add(this.label2);
         this.grpStbSignals.Controls.Add(this.label1);
         this.grpStbSignals.Controls.Add(this.txtStbBoardPosition);
         this.grpStbSignals.Controls.Add(this.lblStbLastFlag);
         this.grpStbSignals.Controls.Add(this.lblStbBoardReceiveComplete);
         this.grpStbSignals.Controls.Add(this.lblStbDischargeRequest);
         this.grpStbSignals.Controls.Add(this.lblStbDischargeNotice);
         this.grpStbSignals.Controls.Add(this.lblStbIsStopped);
         this.grpStbSignals.Controls.Add(this.lblStbIsRunning);
         this.grpStbSignals.Location = new System.Drawing.Point(12, 12);
         this.grpStbSignals.Name = "grpStbSignals";
         this.grpStbSignals.Size = new System.Drawing.Size(400, 250);
         this.grpStbSignals.TabIndex = 0;
         this.grpStbSignals.TabStop = false;
         this.grpStbSignals.Text = "STB信號 (上游滑台)";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(20, 30);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(65, 12);
         this.label1.TabIndex = 0;
         this.label1.Text = "起動中:";
         // 
         // lblStbIsRunning
         // 
         this.lblStbIsRunning.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.lblStbIsRunning.Location = new System.Drawing.Point(150, 25);
         this.lblStbIsRunning.Name = "lblStbIsRunning";
         this.lblStbIsRunning.Size = new System.Drawing.Size(150, 23);
         this.lblStbIsRunning.TabIndex = 1;
         this.lblStbIsRunning.Text = "OFF";
         this.lblStbIsRunning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // btnToggleRunning
         // 
         this.btnToggleRunning.Location = new System.Drawing.Point(310, 25);
         this.btnToggleRunning.Name = "btnToggleRunning";
         this.btnToggleRunning.Size = new System.Drawing.Size(70, 23);
         this.btnToggleRunning.TabIndex = 2;
         this.btnToggleRunning.Tag = "LB1000";
         this.btnToggleRunning.Text = "切換";
         this.btnToggleRunning.UseVisualStyleBackColor = true;
         this.btnToggleRunning.Click += new System.EventHandler(this.btnToggleStbSignal_Click);
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(20, 60);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(65, 12);
         this.label2.TabIndex = 3;
         this.label2.Text = "停止中:";
         // 
         // lblStbIsStopped
         // 
         this.lblStbIsStopped.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.lblStbIsStopped.Location = new System.Drawing.Point(150, 55);
         this.lblStbIsStopped.Name = "lblStbIsStopped";
         this.lblStbIsStopped.Size = new System.Drawing.Size(150, 23);
         this.lblStbIsStopped.TabIndex = 4;
         this.lblStbIsStopped.Text = "OFF";
         this.lblStbIsStopped.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // btnToggleStopped
         // 
         this.btnToggleStopped.Location = new System.Drawing.Point(310, 55);
         this.btnToggleStopped.Name = "btnToggleStopped";
         this.btnToggleStopped.Size = new System.Drawing.Size(70, 23);
         this.btnToggleStopped.TabIndex = 5;
         this.btnToggleStopped.Tag = "LB1001";
         this.btnToggleStopped.Text = "切換";
         this.btnToggleStopped.UseVisualStyleBackColor = true;
         this.btnToggleStopped.Click += new System.EventHandler(this.btnToggleStbSignal_Click);
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(20, 90);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(77, 12);
         this.label3.TabIndex = 6;
         this.label3.Text = "排出予告:";
         // 
         // lblStbDischargeNotice
         // 
         this.lblStbDischargeNotice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.lblStbDischargeNotice.Location = new System.Drawing.Point(150, 85);
         this.lblStbDischargeNotice.Name = "lblStbDischargeNotice";
         this.lblStbDischargeNotice.Size = new System.Drawing.Size(150, 23);
         this.lblStbDischargeNotice.TabIndex = 7;
         this.lblStbDischargeNotice.Text = "OFF";
         this.lblStbDischargeNotice.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // btnToggleDischargeNotice
         // 
         this.btnToggleDischargeNotice.Location = new System.Drawing.Point(310, 85);
         this.btnToggleDischargeNotice.Name = "btnToggleDischargeNotice";
         this.btnToggleDischargeNotice.Size = new System.Drawing.Size(70, 23);
         this.btnToggleDischargeNotice.TabIndex = 8;
         this.btnToggleDischargeNotice.Tag = "LB1002";
         this.btnToggleDischargeNotice.Text = "切換";
         this.btnToggleDischargeNotice.UseVisualStyleBackColor = true;
         this.btnToggleDischargeNotice.Click += new System.EventHandler(this.btnToggleStbSignal_Click);
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(20, 120);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(77, 12);
         this.label4.TabIndex = 9;
         this.label4.Text = "排出要求:";
         // 
         // lblStbDischargeRequest
         // 
         this.lblStbDischargeRequest.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.lblStbDischargeRequest.Location = new System.Drawing.Point(150, 115);
         this.lblStbDischargeRequest.Name = "lblStbDischargeRequest";
         this.lblStbDischargeRequest.Size = new System.Drawing.Size(150, 23);
         this.lblStbDischargeRequest.TabIndex = 10;
         this.lblStbDischargeRequest.Text = "OFF";
         this.lblStbDischargeRequest.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // btnToggleDischargeRequest
         // 
         this.btnToggleDischargeRequest.Location = new System.Drawing.Point(310, 115);
         this.btnToggleDischargeRequest.Name = "btnToggleDischargeRequest";
         this.btnToggleDischargeRequest.Size = new System.Drawing.Size(70, 23);
         this.btnToggleDischargeRequest.TabIndex = 11;
         this.btnToggleDischargeRequest.Tag = "LB1003";
         this.btnToggleDischargeRequest.Text = "切換";
         this.btnToggleDischargeRequest.UseVisualStyleBackColor = true;
         this.btnToggleDischargeRequest.Click += new System.EventHandler(this.btnToggleStbSignal_Click);
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(20, 150);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(113, 12);
         this.label5.TabIndex = 12;
         this.label5.Text = "基板受取完了:";
         // 
         // lblStbBoardReceiveComplete
         // 
         this.lblStbBoardReceiveComplete.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.lblStbBoardReceiveComplete.Location = new System.Drawing.Point(150, 145);
         this.lblStbBoardReceiveComplete.Name = "lblStbBoardReceiveComplete";
         this.lblStbBoardReceiveComplete.Size = new System.Drawing.Size(150, 23);
         this.lblStbBoardReceiveComplete.TabIndex = 13;
         this.lblStbBoardReceiveComplete.Text = "OFF";
         this.lblStbBoardReceiveComplete.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // btnToggleBoardReceiveComplete
         // 
         this.btnToggleBoardReceiveComplete.Location = new System.Drawing.Point(310, 145);
         this.btnToggleBoardReceiveComplete.Name = "btnToggleBoardReceiveComplete";
         this.btnToggleBoardReceiveComplete.Size = new System.Drawing.Size(70, 23);
         this.btnToggleBoardReceiveComplete.TabIndex = 14;
         this.btnToggleBoardReceiveComplete.Tag = "LB1006";
         this.btnToggleBoardReceiveComplete.Text = "切換";
         this.btnToggleBoardReceiveComplete.UseVisualStyleBackColor = true;
         this.btnToggleBoardReceiveComplete.Click += new System.EventHandler(this.btnToggleStbSignal_Click);
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Location = new System.Drawing.Point(20, 180);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(125, 12);
         this.label6.TabIndex = 15;
         this.label6.Text = "基板受取位置:";
         // 
         // txtStbBoardPosition
         // 
         this.txtStbBoardPosition.Location = new System.Drawing.Point(150, 175);
         this.txtStbBoardPosition.Name = "txtStbBoardPosition";
         this.txtStbBoardPosition.ReadOnly = true;
         this.txtStbBoardPosition.Size = new System.Drawing.Size(150, 22);
         this.txtStbBoardPosition.TabIndex = 16;
         this.txtStbBoardPosition.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // btnTogglePosition
         // 
         this.btnTogglePosition.Location = new System.Drawing.Point(310, 175);
         this.btnTogglePosition.Name = "btnTogglePosition";
         this.btnTogglePosition.Size = new System.Drawing.Size(70, 23);
         this.btnTogglePosition.TabIndex = 17;
         this.btnTogglePosition.Tag = "LW1004";
         this.btnTogglePosition.Text = "設定";
         this.btnTogglePosition.UseVisualStyleBackColor = true;
         this.btnTogglePosition.Click += new System.EventHandler(this.btnToggleStbSignal_Click);
         // 
         // label7
         // 
         this.label7.AutoSize = true;
         this.label7.Location = new System.Drawing.Point(20, 210);
         this.label7.Name = "label7";
         this.label7.Size = new System.Drawing.Size(77, 12);
         this.label7.TabIndex = 18;
         this.label7.Text = "LastFlag:";
         // 
         // lblStbLastFlag
         // 
         this.lblStbLastFlag.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.lblStbLastFlag.Location = new System.Drawing.Point(150, 205);
         this.lblStbLastFlag.Name = "lblStbLastFlag";
         this.lblStbLastFlag.Size = new System.Drawing.Size(150, 23);
         this.lblStbLastFlag.TabIndex = 19;
         this.lblStbLastFlag.Text = "OFF";
         this.lblStbLastFlag.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // btnToggleLastFlag
         // 
         this.btnToggleLastFlag.Location = new System.Drawing.Point(310, 205);
         this.btnToggleLastFlag.Name = "btnToggleLastFlag";
         this.btnToggleLastFlag.Size = new System.Drawing.Size(70, 23);
         this.btnToggleLastFlag.TabIndex = 20;
         this.btnToggleLastFlag.Tag = "LB1007";
         this.btnToggleLastFlag.Text = "切換";
         this.btnToggleLastFlag.UseVisualStyleBackColor = true;
         this.btnToggleLastFlag.Click += new System.EventHandler(this.btnToggleStbSignal_Click);
         // 
         // grpRbSignals
         // 
         this.grpRbSignals.Controls.Add(this.label13);
         this.grpRbSignals.Controls.Add(this.label12);
         this.grpRbSignals.Controls.Add(this.label11);
         this.grpRbSignals.Controls.Add(this.label10);
         this.grpRbSignals.Controls.Add(this.label9);
         this.grpRbSignals.Controls.Add(this.label8);
         this.grpRbSignals.Controls.Add(this.lblRbCannotReceive);
         this.grpRbSignals.Controls.Add(this.lblRbBoardReceiveComplete1);
         this.grpRbSignals.Controls.Add(this.lblRbDischargeRequest);
         this.grpRbSignals.Controls.Add(this.lblRbReadyToReceiveNotice1);
         this.grpRbSignals.Controls.Add(this.lblRbIsStopped);
         this.grpRbSignals.Controls.Add(this.lblRbIsRunning);
         this.grpRbSignals.Location = new System.Drawing.Point(428, 12);
         this.grpRbSignals.Name = "grpRbSignals";
         this.grpRbSignals.Size = new System.Drawing.Size(350, 250);
         this.grpRbSignals.TabIndex = 1;
         this.grpRbSignals.TabStop = false;
         this.grpRbSignals.Text = "RB信號 (下游機械手臂)";
         // 
         // label8
         // 
         this.label8.AutoSize = true;
         this.label8.Location = new System.Drawing.Point(20, 30);
         this.label8.Name = "label8";
         this.label8.Size = new System.Drawing.Size(65, 12);
         this.label8.TabIndex = 0;
         this.label8.Text = "起動中:";
         // 
         // lblRbIsRunning
         // 
         this.lblRbIsRunning.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.lblRbIsRunning.Location = new System.Drawing.Point(150, 25);
         this.lblRbIsRunning.Name = "lblRbIsRunning";
         this.lblRbIsRunning.Size = new System.Drawing.Size(180, 23);
         this.lblRbIsRunning.TabIndex = 1;
         this.lblRbIsRunning.Text = "OFF";
         this.lblRbIsRunning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label9
         // 
         this.label9.AutoSize = true;
         this.label9.Location = new System.Drawing.Point(20, 60);
         this.label9.Name = "label9";
         this.label9.Size = new System.Drawing.Size( 65, 12);
         this.label9.TabIndex = 2;
         this.label9.Text = "停止中:";
         // 
         // lblRbIsStopped
         // 
         this.lblRbIsStopped.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.lblRbIsStopped.Location = new System.Drawing.Point(150, 55);
         this.lblRbIsStopped.Name = "lblRbIsStopped";
         this.lblRbIsStopped.Size = new System.Drawing.Size(180, 23);
         this.lblRbIsStopped.TabIndex = 3;
         this.lblRbIsStopped.Text = "OFF";
         this.lblRbIsStopped.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label10
         // 
         this.label10.AutoSize = true;
         this.label10.Location = new System.Drawing.Point(20, 90);
         this.label10.Name = "label10";
         this.label10.Size = new System.Drawing.Size(125, 12);
         this.label10.TabIndex = 4;
         this.label10.Text = "收料可能予告:";
         // 
         // lblRbReadyToReceiveNotice1
         // 
         this.lblRbReadyToReceiveNotice1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.lblRbReadyToReceiveNotice1.Location = new System.Drawing.Point(150, 85);
         this.lblRbReadyToReceiveNotice1.Name = "lblRbReadyToReceiveNotice1";
         this.lblRbReadyToReceiveNotice1.Size = new System.Drawing.Size(180, 23);
         this.lblRbReadyToReceiveNotice1.TabIndex = 5;
         this.lblRbReadyToReceiveNotice1.Text = "OFF";
         this.lblRbReadyToReceiveNotice1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label11
         // 
         this.label11.AutoSize = true;
         this.label11.Location = new System.Drawing.Point(20, 120);
         this.label11.Name = "label11";
         this.label11.Size = new System.Drawing.Size(77, 12);
         this.label11.TabIndex = 6;
         this.label11.Text = "排出要求:";
         // 
         // lblRbDischargeRequest
         // 
         this.lblRbDischargeRequest.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.lblRbDischargeRequest.Location = new System.Drawing.Point(150, 115);
         this.lblRbDischargeRequest.Name = "lblRbDischargeRequest";
         this.lblRbDischargeRequest.Size = new System.Drawing.Size(180, 23);
         this.lblRbDischargeRequest.TabIndex = 7;
         this.lblRbDischargeRequest.Text = "OFF";
         this.lblRbDischargeRequest.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label12
         // 
         this.label12.AutoSize = true;
         this.label12.Location = new System.Drawing.Point(20, 150);
         this.label12.Name = "label12";
         this.label12.Size = new System.Drawing.Size(113, 12);
         this.label12.TabIndex = 8;
         this.label12.Text = "基板受取完了:";
         // 
         // lblRbBoardReceiveComplete1
         // 
         this.lblRbBoardReceiveComplete1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.lblRbBoardReceiveComplete1.Location = new System.Drawing.Point(150, 145);
         this.lblRbBoardReceiveComplete1.Name = "lblRbBoardReceiveComplete1";
         this.lblRbBoardReceiveComplete1.Size = new System.Drawing.Size(180, 23);
         this.lblRbBoardReceiveComplete1.TabIndex = 9;
         this.lblRbBoardReceiveComplete1.Text = "OFF";
         this.lblRbBoardReceiveComplete1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label13
         // 
         this.label13.AutoSize = true;
         this.label13.Location = new System.Drawing.Point(20, 180);
         this.label13.Name = "label13";
         this.label13.Size = new System.Drawing.Size(77, 12);
         this.label13.TabIndex = 10;
         this.label13.Text = "受取不可:";
         // 
         // lblRbCannotReceive
         // 
         this.lblRbCannotReceive.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.lblRbCannotReceive.Location = new System.Drawing.Point(150, 175);
         this.lblRbCannotReceive.Name = "lblRbCannotReceive";
         this.lblRbCannotReceive.Size = new System.Drawing.Size(180, 23);
         this.lblRbCannotReceive.TabIndex = 11;
         this.lblRbCannotReceive.Text = "OFF";
         this.lblRbCannotReceive.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // grpStatus
         // 
         this.grpStatus.Controls.Add(this.label14);
         this.grpStatus.Controls.Add(this.txtCurrentState);
         this.grpStatus.Location = new System.Drawing.Point(12, 268);
         this.grpStatus.Name = "grpStatus";
         this.grpStatus.Size = new System.Drawing.Size(400, 80);
         this.grpStatus.TabIndex = 2;
         this.grpStatus.TabStop = false;
         this.grpStatus.Text = "狀態顯示";
         // 
         // label14
         // 
         this.label14.AutoSize = true;
         this.label14.Location = new System.Drawing.Point(20, 35);
         this.label14.Name = "label14";
         this.label14.Size = new System.Drawing.Size(77, 12);
         this.label14.TabIndex = 0;
         this.label14.Text = "當前狀態:";
         // 
         // txtCurrentState
         // 
         this.txtCurrentState.Font = new System.Drawing.Font("新細明體", 10F, System.Drawing.FontStyle.Bold);
         this.txtCurrentState.Location = new System.Drawing.Point(110, 30);
         this.txtCurrentState.Name = "txtCurrentState";
         this.txtCurrentState.ReadOnly = true;
         this.txtCurrentState.Size = new System.Drawing.Size(270, 23);
         this.txtCurrentState.TabIndex = 1;
         this.txtCurrentState.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         // 
         // grpControl
         // 
         this.grpControl.Controls.Add(this.btnReset);
         this.grpControl.Controls.Add(this.btnStop);
         this.grpControl.Controls.Add(this.btnStart);
         this.grpControl.Location = new System.Drawing.Point(428, 268);
         this.grpControl.Name = "grpControl";
         this.grpControl.Size = new System.Drawing.Size(350, 80);
         this.grpControl.TabIndex = 3;
         this.grpControl.TabStop = false;
         this.grpControl.Text = "控制按鈕";
         // 
         // btnStart
         // 
         this.btnStart.BackColor = System.Drawing.Color.LightGreen;
         this.btnStart.Font = new System.Drawing.Font("新細明體", 10F);
         this.btnStart.Location = new System.Drawing.Point(20, 30);
         this.btnStart.Name = "btnStart";
         this.btnStart.Size = new System.Drawing.Size(90, 35);
         this.btnStart.TabIndex = 0;
         this.btnStart.Text = "啟動";
         this.btnStart.UseVisualStyleBackColor = false;
         this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
         // 
         // btnStop
         // 
         this.btnStop.BackColor = System.Drawing.Color.LightCoral;
         this.btnStop.Font = new System.Drawing.Font("新細明體", 10F);
         this.btnStop.Location = new System.Drawing.Point(130, 30);
         this.btnStop.Name = "btnStop";
         this.btnStop.Size = new System.Drawing.Size(90, 35);
         this.btnStop.TabIndex = 1;
         this.btnStop.Text = "停止";
         this.btnStop.UseVisualStyleBackColor = false;
         this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
         // 
         // btnReset
         // 
         this.btnReset.BackColor = System.Drawing.Color.LightSkyBlue;
         this.btnReset.Font = new System.Drawing.Font("新細明體", 10F);
         this.btnReset.Location = new System.Drawing.Point(240, 30);
         this.btnReset.Name = "btnReset";
         this.btnReset.Size = new System.Drawing.Size(90, 35);
         this.btnReset.TabIndex = 2;
         this.btnReset.Text = "復位";
         this.btnReset.UseVisualStyleBackColor = false;
         this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
         // 
         // grpLog
         // 
         this.grpLog.Controls.Add(this.btnClearLog);
         this.grpLog.Controls.Add(this.rtbLog);
         this.grpLog.Location = new System.Drawing.Point(12, 354);
         this.grpLog.Name = "grpLog";
         this.grpLog.Size = new System.Drawing.Size(766, 250);
         this.grpLog.TabIndex = 4;
         this.grpLog.TabStop = false;
         this.grpLog.Text = "日誌記錄";
         // 
         // rtbLog
         // 
         this.rtbLog.Location = new System.Drawing.Point(20, 25);
         this.rtbLog.Name = "rtbLog";
         this.rtbLog.ReadOnly = true;
         this.rtbLog.Size = new System.Drawing.Size(726, 180);
         this.rtbLog.TabIndex = 0;
         this.rtbLog.Text = "";
         // 
         // btnClearLog
         // 
         this.btnClearLog.Location = new System.Drawing.Point(650, 212);
         this.btnClearLog.Name = "btnClearLog";
         this.btnClearLog.Size = new System.Drawing.Size(96, 25);
         this.btnClearLog.TabIndex = 1;
         this.btnClearLog.Text = "清除日誌";
         this.btnClearLog.UseVisualStyleBackColor = true;
         this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
         // 
         // HandshakeControlForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(790, 616);
         this.Controls.Add(this.grpLog);
         this.Controls.Add(this.grpControl);
         this.Controls.Add(this.grpStatus);
         this.Controls.Add(this.grpRbSignals);
         this.Controls.Add(this.grpStbSignals);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "HandshakeControlForm";
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "滑台-機械手臂交握監控";
         this.grpStbSignals.ResumeLayout(false);
         this.grpStbSignals.PerformLayout();
         this.grpRbSignals.ResumeLayout(false);
         this.grpRbSignals.PerformLayout();
         this.grpStatus.ResumeLayout(false);
         this.grpStatus.PerformLayout();
         this.grpControl.ResumeLayout(false);
         this.grpLog.ResumeLayout(false);
         this.ResumeLayout(false);
      }

      #endregion

      private System.Windows.Forms.GroupBox grpStbSignals;
      private System.Windows.Forms.Button btnToggleLastFlag;
      private System.Windows.Forms.Button btnTogglePosition;
      private System.Windows.Forms.Button btnToggleBoardReceiveComplete;
      private System.Windows.Forms.Button btnToggleDischargeRequest;
      private System.Windows.Forms.Button btnToggleDischargeNotice;
      private System.Windows.Forms.Button btnToggleStopped;
      private System.Windows.Forms.Button btnToggleRunning;
      private System.Windows.Forms.Label lblStbLastFlag;
      private System.Windows.Forms.Label lblStbBoardReceiveComplete;
      private System.Windows.Forms.Label lblStbDischargeRequest;
      private System.Windows.Forms.Label lblStbDischargeNotice;
      private System.Windows.Forms.Label lblStbIsStopped;
      private System.Windows.Forms.Label lblStbIsRunning;
      private System.Windows.Forms.TextBox txtStbBoardPosition;
      private System.Windows.Forms.Label label7;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.GroupBox grpRbSignals;
      private System.Windows.Forms.Label label13;
      private System.Windows.Forms.Label label12;
      private System.Windows.Forms.Label label11;
      private System.Windows.Forms.Label label10;
      private System.Windows.Forms.Label label9;
      private System.Windows.Forms.Label label8;
      private System.Windows.Forms.Label lblRbCannotReceive;
      private System.Windows.Forms.Label lblRbBoardReceiveComplete1;
      private System.Windows.Forms.Label lblRbDischargeRequest;
      private System.Windows.Forms.Label lblRbReadyToReceiveNotice1;
      private System.Windows.Forms.Label lblRbIsStopped;
      private System.Windows.Forms.Label lblRbIsRunning;
      private System.Windows.Forms.GroupBox grpStatus;
      private System.Windows.Forms.TextBox txtCurrentState;
      private System.Windows.Forms.Label label14;
      private System.Windows.Forms.GroupBox grpControl;
      private System.Windows.Forms.Button btnReset;
      private System.Windows.Forms.Button btnStop;
      private System.Windows.Forms.Button btnStart;
      private System.Windows.Forms.GroupBox grpLog;
      private System.Windows.Forms.Button btnClearLog;
      private System.Windows.Forms.RichTextBox rtbLog;
   }
}
