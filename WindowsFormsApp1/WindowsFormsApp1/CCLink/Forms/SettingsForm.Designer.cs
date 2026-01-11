namespace WindowsFormsApp1.CCLink.Forms
{
   partial class SettingsForm
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

      private void InitializeComponent()
      {
         this.label1 = new System.Windows.Forms.Label();
         this.txtIp = new System.Windows.Forms.TextBox();
         this.label2 = new System.Windows.Forms.Label();
         this.numStation = new System.Windows.Forms.NumericUpDown();
         this.label3 = new System.Windows.Forms.Label();
         this.numHeartbeat = new System.Windows.Forms.NumericUpDown();
         this.dgvRanges = new System.Windows.Forms.DataGridView();
         this.colKind = new System.Windows.Forms.DataGridViewComboBoxColumn();
         this.colStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.colEnd = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.btnSave = new System.Windows.Forms.Button();
         this.label4 = new System.Windows.Forms.Label();
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
         this.cmbEndian = new System.Windows.Forms.ComboBox();
         this.chkIsx64 = new System.Windows.Forms.CheckBox();
         this.label10 = new System.Windows.Forms.Label();
         this.label11 = new System.Windows.Forms.Label();
         this.numTimeSync = new System.Windows.Forms.NumericUpDown();
         this.label12 = new System.Windows.Forms.Label();
         this.txtTrigger = new System.Windows.Forms.TextBox();
         this.label13 = new System.Windows.Forms.Label();
         this.txtData = new System.Windows.Forms.TextBox();
         ((System.ComponentModel.ISupportInitialize)(this.numStation)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numHeartbeat)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dgvRanges)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numNetwork)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numRetryCount)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numRetryBackoff)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numTimeSync)).BeginInit();
         this.SuspendLayout();
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(12, 15);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(42, 12);
         this.label1.TabIndex = 0;
         this.label1.Text = "PLC IP:";
         // 
         // txtIp
         // 
         this.txtIp.Location = new System.Drawing.Point(80, 12);
         this.txtIp.Name = "txtIp";
         this.txtIp.Size = new System.Drawing.Size(100, 22);
         this.txtIp.TabIndex = 1;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(12, 127);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(40, 12);
         this.label2.TabIndex = 6;
         this.label2.Text = "Station:";
         // 
         // numStation
         // 
         this.numStation.Location = new System.Drawing.Point(80, 125);
         this.numStation.Maximum = new decimal(new int[] { 64, 0, 0, 0 });
         this.numStation.Name = "numStation";
         this.numStation.Size = new System.Drawing.Size(60, 22);
         this.numStation.TabIndex = 7;
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(200, 15);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(62, 12);
         this.label3.TabIndex = 11;
         this.label3.Text = "HB Int (ms):";
         // 
         // numHeartbeat
         // 
         this.numHeartbeat.Location = new System.Drawing.Point(300, 13);
         this.numHeartbeat.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
         this.numHeartbeat.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
         this.numHeartbeat.Name = "numHeartbeat";
         this.numHeartbeat.Size = new System.Drawing.Size(80, 22);
         this.numHeartbeat.TabIndex = 12;
         this.numHeartbeat.Value = new decimal(new int[] { 300, 0, 0, 0 });
         // 
         // dgvRanges
         // 
         this.dgvRanges.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
         this.dgvRanges.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colKind,
            this.colStart,
            this.colEnd});
         this.dgvRanges.Location = new System.Drawing.Point(12, 280);
         this.dgvRanges.Name = "dgvRanges";
         this.dgvRanges.RowTemplate.Height = 24;
         this.dgvRanges.Size = new System.Drawing.Size(370, 160);
         this.dgvRanges.TabIndex = 22;
         // 
         // colKind
         // 
         this.colKind.HeaderText = "Kind";
         this.colKind.Items.AddRange(new object[] { "LB", "LW" });
         this.colKind.Name = "colKind";
         this.colKind.Resizable = System.Windows.Forms.DataGridViewTriState.True;
         this.colKind.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
         // 
         // colStart
         // 
         this.colStart.HeaderText = "Start (Hex)";
         this.colStart.Name = "colStart";
         // 
         // colEnd
         // 
         this.colEnd.HeaderText = "End (Hex)";
         this.colEnd.Name = "colEnd";
         // 
         // btnSave
         // 
         this.btnSave.Location = new System.Drawing.Point(307, 446);
         this.btnSave.Name = "btnSave";
         this.btnSave.Size = new System.Drawing.Size(75, 25);
         this.btnSave.TabIndex = 23;
         this.btnSave.Text = "儲存並啟動";
         this.btnSave.UseVisualStyleBackColor = true;
         this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(12, 265);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(89, 12);
         this.label4.TabIndex = 21;
         this.label4.Text = "Scan Ranges (Hex):";
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(12, 43);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(27, 12);
         this.label5.TabIndex = 2;
         this.label5.Text = "Port:";
         // 
         // numPort
         // 
         this.numPort.Location = new System.Drawing.Point(80, 41);
         this.numPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
         this.numPort.Name = "numPort";
         this.numPort.Size = new System.Drawing.Size(80, 22);
         this.numPort.TabIndex = 3;
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Location = new System.Drawing.Point(12, 71);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(48, 12);
         this.label6.TabIndex = 4;
         this.label6.Text = "Network:";
         // 
         // numNetwork
         // 
         this.numNetwork.Location = new System.Drawing.Point(80, 69);
         this.numNetwork.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
         this.numNetwork.Name = "numNetwork";
         this.numNetwork.Size = new System.Drawing.Size(60, 22);
         this.numNetwork.TabIndex = 5;
         // 
         // label7
         // 
         this.label7.AutoSize = true;
         this.label7.Location = new System.Drawing.Point(200, 43);
         this.label7.Name = "label7";
         this.label7.Size = new System.Drawing.Size(71, 12);
         this.label7.TabIndex = 13;
         this.label7.Text = "Timeout (ms):";
         // 
         // numTimeout
         // 
         this.numTimeout.Location = new System.Drawing.Point(300, 41);
         this.numTimeout.Maximum = new decimal(new int[] { 60000, 0, 0, 0 });
         this.numTimeout.Name = "numTimeout";
         this.numTimeout.Size = new System.Drawing.Size(80, 22);
         this.numTimeout.TabIndex = 14;
         this.numTimeout.Value = new decimal(new int[] { 3000, 0, 0, 0 });
         // 
         // label8
         // 
         this.label8.AutoSize = true;
         this.label8.Location = new System.Drawing.Point(200, 71);
         this.label8.Name = "label8";
         this.label8.Size = new System.Drawing.Size(65, 12);
         this.label8.TabIndex = 15;
         this.label8.Text = "Retry Count:";
         // 
         // numRetryCount
         // 
         this.numRetryCount.Location = new System.Drawing.Point(300, 69);
         this.numRetryCount.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
         this.numRetryCount.Name = "numRetryCount";
         this.numRetryCount.Size = new System.Drawing.Size(60, 22);
         this.numRetryCount.TabIndex = 16;
         this.numRetryCount.Value = new decimal(new int[] { 3, 0, 0, 0 });
         // 
         // label9
         // 
         this.label9.AutoSize = true;
         this.label9.Location = new System.Drawing.Point(200, 99);
         this.label9.Name = "label9";
         this.label9.Size = new System.Drawing.Size(76, 12);
         this.label9.TabIndex = 17;
         this.label9.Text = "Retry Backoff:";
         // 
         // numRetryBackoff
         // 
         this.numRetryBackoff.Location = new System.Drawing.Point(300, 97);
         this.numRetryBackoff.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
         this.numRetryBackoff.Name = "numRetryBackoff";
         this.numRetryBackoff.Size = new System.Drawing.Size(80, 22);
         this.numRetryBackoff.TabIndex = 18;
         this.numRetryBackoff.Value = new decimal(new int[] { 50, 0, 0, 0 });
         // 
         // label10
         // 
         this.label10.AutoSize = true;
         this.label10.Location = new System.Drawing.Point(12, 155);
         this.label10.Name = "label10";
         this.label10.Size = new System.Drawing.Size(41, 12);
         this.label10.TabIndex = 8;
         this.label10.Text = "Endian:";
         // 
         // cmbEndian
         // 
         this.cmbEndian.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cmbEndian.FormattingEnabled = true;
         this.cmbEndian.Items.AddRange(new object[] { "Big", "Little" });
         this.cmbEndian.Location = new System.Drawing.Point(80, 153);
         this.cmbEndian.Name = "cmbEndian";
         this.cmbEndian.Size = new System.Drawing.Size(80, 20);
         this.cmbEndian.TabIndex = 9;
         // 
         // chkIsx64
         // 
         this.chkIsx64.AutoSize = true;
         this.chkIsx64.Location = new System.Drawing.Point(202, 127);
         this.chkIsx64.Name = "chkIsx64";
         this.chkIsx64.Size = new System.Drawing.Size(71, 16);
         this.chkIsx64.TabIndex = 19;
         this.chkIsx64.Text = "Is 64-bit?";
         this.chkIsx64.UseVisualStyleBackColor = true;
         // 
         // label11
         // 
         this.label11.AutoSize = true;
         this.label11.Location = new System.Drawing.Point(200, 155);
         this.label11.Name = "label11";
         this.label11.Size = new System.Drawing.Size(63, 12);
         this.label11.TabIndex = 10;
         this.label11.Text = "TS Int (ms):";
         // 
         // numTimeSync
         // 
         this.numTimeSync.Location = new System.Drawing.Point(300, 153);
         this.numTimeSync.Maximum = new decimal(new int[] { 3600000, 0, 0, 0 });
         this.numTimeSync.Name = "numTimeSync";
         this.numTimeSync.Size = new System.Drawing.Size(80, 22);
         this.numTimeSync.TabIndex = 10;
         this.numTimeSync.Value = new decimal(new int[] { 1000, 0, 0, 0 });
         // 
         // label12
         // 
         this.label12.AutoSize = true;
         this.label12.Location = new System.Drawing.Point(12, 185);
         this.label12.Name = "label12";
         this.label12.Size = new System.Drawing.Size(61, 12);
         this.label12.TabIndex = 24;
         this.label12.Text = "TS Trigger:";
         // 
         // txtTrigger
         // 
         this.txtTrigger.Location = new System.Drawing.Point(80, 182);
         this.txtTrigger.Name = "txtTrigger";
         this.txtTrigger.Size = new System.Drawing.Size(80, 22);
         this.txtTrigger.TabIndex = 25;
         // 
         // label13
         // 
         this.label13.AutoSize = true;
         this.label13.Location = new System.Drawing.Point(200, 185);
         this.label13.Name = "label13";
         this.label13.Size = new System.Drawing.Size(46, 12);
         this.label13.TabIndex = 26;
         this.label13.Text = "TS Data:";
         // 
         // txtData
         // 
         this.txtData.Location = new System.Drawing.Point(300, 182);
         this.txtData.Name = "txtData";
         this.txtData.Size = new System.Drawing.Size(80, 22);
         this.txtData.TabIndex = 27;
         // 
         // SettingsForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(400, 480);
         this.Controls.Add(this.txtData);
         this.Controls.Add(this.label13);
         this.Controls.Add(this.txtTrigger);
         this.Controls.Add(this.label12);
         this.Controls.Add(this.numTimeSync);
         this.Controls.Add(this.label11);
         this.Controls.Add(this.chkIsx64);
         this.Controls.Add(this.cmbEndian);
         this.Controls.Add(this.label10);
         this.Controls.Add(this.numRetryBackoff);
         this.Controls.Add(this.label9);
         this.Controls.Add(this.numRetryCount);
         this.Controls.Add(this.label8);
         this.Controls.Add(this.numTimeout);
         this.Controls.Add(this.label7);
         this.Controls.Add(this.numNetwork);
         this.Controls.Add(this.label6);
         this.Controls.Add(this.numPort);
         this.Controls.Add(this.label5);
         this.Controls.Add(this.label4);
         this.Controls.Add(this.btnSave);
         this.Controls.Add(this.dgvRanges);
         this.Controls.Add(this.numHeartbeat);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.numStation);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.txtIp);
         this.Controls.Add(this.label1);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "SettingsForm";
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
         this.Text = "連線與通訊設定";
         ((System.ComponentModel.ISupportInitialize)(this.numStation)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numHeartbeat)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dgvRanges)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numNetwork)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numRetryCount)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numRetryBackoff)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numTimeSync)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TextBox txtIp;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.NumericUpDown numStation;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.NumericUpDown numHeartbeat;
      private System.Windows.Forms.DataGridView dgvRanges;
      private System.Windows.Forms.Button btnSave;
      private System.Windows.Forms.DataGridViewComboBoxColumn colKind;
      private System.Windows.Forms.DataGridViewTextBoxColumn colStart;
      private System.Windows.Forms.DataGridViewTextBoxColumn colEnd;
      private System.Windows.Forms.Label label4;
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
      private System.Windows.Forms.Label label10;
      private System.Windows.Forms.ComboBox cmbEndian;
      private System.Windows.Forms.CheckBox chkIsx64;
      private System.Windows.Forms.Label label11;
      private System.Windows.Forms.NumericUpDown numTimeSync;
      private System.Windows.Forms.Label label12;
      private System.Windows.Forms.TextBox txtTrigger;
      private System.Windows.Forms.Label label13;
      private System.Windows.Forms.TextBox txtData;
   }
}
