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
         this.tabControl1 = new System.Windows.Forms.TabControl();
         this.tabGeneral = new System.Windows.Forms.TabPage();
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
         this.label4 = new System.Windows.Forms.Label();
         this.dgvRanges = new System.Windows.Forms.DataGridView();
         this.colKind = new System.Windows.Forms.DataGridViewComboBoxColumn();
         this.colStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.colEnd = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.btnSave = new System.Windows.Forms.Button();

         this.tabControl1.SuspendLayout();
         this.tabGeneral.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numNetwork)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numRetryCount)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numRetryBackoff)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numStation)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dgvRanges)).BeginInit();
         this.SuspendLayout();

         // 
         // tabControl1
         // 
         this.tabControl1.Controls.Add(this.tabGeneral);
         this.tabControl1.Location = new System.Drawing.Point(12, 12);
         this.tabControl1.Name = "tabControl1";
         this.tabControl1.SelectedIndex = 0;
         this.tabControl1.Size = new System.Drawing.Size(376, 420);
         this.tabControl1.TabIndex = 0;
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
         this.tabGeneral.Controls.Add(this.label4);
         this.tabGeneral.Controls.Add(this.dgvRanges);
         this.tabGeneral.Location = new System.Drawing.Point(4, 22);
         this.tabGeneral.Name = "tabGeneral";
         this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
         this.tabGeneral.Size = new System.Drawing.Size(368, 394);
         this.tabGeneral.TabIndex = 0;
         this.tabGeneral.Text = "基本設定";
         this.tabGeneral.UseVisualStyleBackColor = true;

         // label5 (Port)
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(10, 17);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(27, 12);
         this.label5.TabIndex = 0;
         this.label5.Text = "Port:";
         // numPort
         this.numPort.Location = new System.Drawing.Point(70, 15);
         this.numPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
         this.numPort.Name = "numPort";
         this.numPort.Size = new System.Drawing.Size(80, 22);
         this.numPort.TabIndex = 1;
         // label6 (Network)
         this.label6.AutoSize = true;
         this.label6.Location = new System.Drawing.Point(10, 45);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(48, 12);
         this.label6.TabIndex = 2;
         this.label6.Text = "Network:";
         // numNetwork
         this.numNetwork.Location = new System.Drawing.Point(70, 43);
         this.numNetwork.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
         this.numNetwork.Name = "numNetwork";
         this.numNetwork.Size = new System.Drawing.Size(60, 22);
         this.numNetwork.TabIndex = 3;

         // label7 (Timeout)
         this.label7.AutoSize = true;
         this.label7.Location = new System.Drawing.Point(180, 17);
         this.label7.Name = "label7";
         this.label7.Size = new System.Drawing.Size(71, 12);
         this.label7.TabIndex = 4;
         this.label7.Text = "Timeout (ms):";
         // numTimeout
         this.numTimeout.Location = new System.Drawing.Point(260, 15);
         this.numTimeout.Maximum = new decimal(new int[] { 60000, 0, 0, 0 });
         this.numTimeout.Name = "numTimeout";
         this.numTimeout.Size = new System.Drawing.Size(80, 22);
         this.numTimeout.TabIndex = 5;
         this.numTimeout.Value = new decimal(new int[] { 3000, 0, 0, 0 });

         // label8 (Retry Count)
         this.label8.AutoSize = true;
         this.label8.Location = new System.Drawing.Point(180, 45);
         this.label8.Name = "label8";
         this.label8.Size = new System.Drawing.Size(65, 12);
         this.label8.TabIndex = 6;
         this.label8.Text = "Retry Count:";
         // numRetryCount
         this.numRetryCount.Location = new System.Drawing.Point(260, 43);
         this.numRetryCount.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
         this.numRetryCount.Name = "numRetryCount";
         this.numRetryCount.Size = new System.Drawing.Size(60, 22);
         this.numRetryCount.TabIndex = 7;
         this.numRetryCount.Value = new decimal(new int[] { 3, 0, 0, 0 });

         // label9 (Retry Backoff)
         this.label9.AutoSize = true;
         this.label9.Location = new System.Drawing.Point(180, 73);
         this.label9.Name = "label9";
         this.label9.Size = new System.Drawing.Size(76, 12);
         this.label9.TabIndex = 8;
         this.label9.Text = "Retry Backoff:";
         // numRetryBackoff
         this.numRetryBackoff.Location = new System.Drawing.Point(260, 71);
         this.numRetryBackoff.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
         this.numRetryBackoff.Name = "numRetryBackoff";
         this.numRetryBackoff.Size = new System.Drawing.Size(80, 22);
         this.numRetryBackoff.TabIndex = 9;
         this.numRetryBackoff.Value = new decimal(new int[] { 50, 0, 0, 0 });

         // label2 (Station)
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(10, 101);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(40, 12);
         this.label2.TabIndex = 10;
         this.label2.Text = "Station:";
         // numStation
         this.numStation.Location = new System.Drawing.Point(70, 99);
         this.numStation.Maximum = new decimal(new int[] { 64, 0, 0, 0 });
         this.numStation.Name = "numStation";
         this.numStation.Size = new System.Drawing.Size(60, 22);
         this.numStation.TabIndex = 11;

         // chkIsx64
         this.chkIsx64.AutoSize = true;
         this.chkIsx64.Location = new System.Drawing.Point(182, 101);
         this.chkIsx64.Name = "chkIsx64";
         this.chkIsx64.Size = new System.Drawing.Size(71, 16);
         this.chkIsx64.TabIndex = 12;
         this.chkIsx64.Text = "Is 64-bit?";
         this.chkIsx64.UseVisualStyleBackColor = true;

         // label10 (Endian)
         this.label10.AutoSize = true;
         this.label10.Location = new System.Drawing.Point(10, 129);
         this.label10.Name = "label10";
         this.label10.Size = new System.Drawing.Size(41, 12);
         this.label10.TabIndex = 13;
         this.label10.Text = "Endian:";
         // cmbEndian
         this.cmbEndian.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cmbEndian.FormattingEnabled = true;
         this.cmbEndian.Items.AddRange(new object[] { "Big", "Little" });
         this.cmbEndian.Location = new System.Drawing.Point(70, 127);
         this.cmbEndian.Name = "cmbEndian";
         this.cmbEndian.Size = new System.Drawing.Size(80, 20);
         this.cmbEndian.TabIndex = 14;

         // label4 (Scan Ranges)
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(10, 230);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(89, 12);
         this.label4.TabIndex = 15;
         this.label4.Text = "Scan Ranges (Hex):";

         // dgvRanges
         this.dgvRanges.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
         this.dgvRanges.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colKind,
            this.colStart,
            this.colEnd});
         this.dgvRanges.Location = new System.Drawing.Point(10, 245);
         this.dgvRanges.Name = "dgvRanges";
         this.dgvRanges.RowTemplate.Height = 24;
         this.dgvRanges.Size = new System.Drawing.Size(350, 140);
         this.dgvRanges.TabIndex = 16;

         // colKind
         this.colKind.HeaderText = "Kind";
         this.colKind.Items.AddRange(new object[] { "LB", "LW" });
         this.colKind.Name = "colKind";
         // colStart
         this.colStart.HeaderText = "Start (Hex)";
         this.colStart.Name = "colStart";
         // colEnd
         this.colEnd.HeaderText = "End (Hex)";
         this.colEnd.Name = "colEnd";

         // btnSave
         this.btnSave.Location = new System.Drawing.Point(307, 446);
         this.btnSave.Name = "btnSave";
         this.btnSave.Size = new System.Drawing.Size(75, 25);
         this.btnSave.TabIndex = 23;
         this.btnSave.Text = "Save";
         this.btnSave.UseVisualStyleBackColor = true;
         this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

         // SettingsForm
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(400, 480);
         this.Controls.Add(this.tabControl1);
         this.Controls.Add(this.btnSave);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "SettingsForm";
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
         this.Text = "連線與通訊設定";

         this.tabControl1.ResumeLayout(false);
         this.tabGeneral.ResumeLayout(false);
         this.tabGeneral.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numNetwork)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numRetryCount)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numRetryBackoff)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numStation)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dgvRanges)).EndInit();
         this.ResumeLayout(false);
      }

      protected System.Windows.Forms.TabControl tabControl1;
      protected System.Windows.Forms.TabPage tabGeneral;

      protected System.Windows.Forms.Label label5;
      protected System.Windows.Forms.NumericUpDown numPort;
      protected System.Windows.Forms.Label label6;
      protected System.Windows.Forms.NumericUpDown numNetwork;
      protected System.Windows.Forms.Label label7;
      protected System.Windows.Forms.NumericUpDown numTimeout;
      protected System.Windows.Forms.Label label8;
      protected System.Windows.Forms.NumericUpDown numRetryCount;
      protected System.Windows.Forms.Label label9;
      protected System.Windows.Forms.NumericUpDown numRetryBackoff;
      protected System.Windows.Forms.Label label2;
      protected System.Windows.Forms.NumericUpDown numStation;
      protected System.Windows.Forms.CheckBox chkIsx64;
      protected System.Windows.Forms.Label label10;
      protected System.Windows.Forms.ComboBox cmbEndian;
      protected System.Windows.Forms.Label label4;
      protected System.Windows.Forms.DataGridView dgvRanges;
      protected System.Windows.Forms.DataGridViewComboBoxColumn colKind;
      protected System.Windows.Forms.DataGridViewTextBoxColumn colStart;
      protected System.Windows.Forms.DataGridViewTextBoxColumn colEnd;
      protected System.Windows.Forms.Button btnSave;
   }
}
