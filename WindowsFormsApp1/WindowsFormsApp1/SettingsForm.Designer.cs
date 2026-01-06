namespace WindowsFormsApp1
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
         this.colKind = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.colStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.colEnd = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.btnSave = new System.Windows.Forms.Button();
         this.label4 = new System.Windows.Forms.Label();
         ((System.ComponentModel.ISupportInitialize)(this.numStation)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numHeartbeat)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dgvRanges)).BeginInit();
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
         this.txtIp.Size = new System.Drawing.Size(120, 22);
         this.txtIp.TabIndex = 1;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(12, 43);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(40, 12);
         this.label2.TabIndex = 2;
         this.label2.Text = "Station:";
         // 
         // numStation
         // 
         this.numStation.Location = new System.Drawing.Point(80, 41);
         this.numStation.Maximum = new decimal(new int[] { 64, 0, 0, 0 });
         this.numStation.Name = "numStation";
         this.numStation.Size = new System.Drawing.Size(60, 22);
         this.numStation.TabIndex = 3;
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(12, 71);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(62, 12);
         this.label3.TabIndex = 4;
         this.label3.Text = "HB Int (ms):";
         // 
         // numHeartbeat
         // 
         this.numHeartbeat.Location = new System.Drawing.Point(80, 69);
         this.numHeartbeat.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
         this.numHeartbeat.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
         this.numHeartbeat.Name = "numHeartbeat";
         this.numHeartbeat.Size = new System.Drawing.Size(80, 22);
         this.numHeartbeat.TabIndex = 5;
         this.numHeartbeat.Value = new decimal(new int[] { 1000, 0, 0, 0 });
         // 
         // dgvRanges
         // 
         this.dgvRanges.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
         this.dgvRanges.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colKind,
            this.colStart,
            this.colEnd});
         this.dgvRanges.Location = new System.Drawing.Point(12, 110);
         this.dgvRanges.Name = "dgvRanges";
         this.dgvRanges.RowTemplate.Height = 24;
         this.dgvRanges.Size = new System.Drawing.Size(360, 150);
         this.dgvRanges.TabIndex = 6;
         // 
         // colKind
         // 
         this.colKind.HeaderText = "Kind (LB/LW)";
         this.colKind.Name = "colKind";
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
         this.btnSave.Location = new System.Drawing.Point(297, 266);
         this.btnSave.Name = "btnSave";
         this.btnSave.Size = new System.Drawing.Size(75, 23);
         this.btnSave.TabIndex = 7;
         this.btnSave.Text = "儲存並啟動";
         this.btnSave.UseVisualStyleBackColor = true;
         this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(12, 95);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(89, 12);
         this.label4.TabIndex = 8;
         this.label4.Text = "Scan Ranges (Hex):";
         // 
         // SettingsForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(384, 301);
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
         this.Text = "連線設定 (首次啟動)";
         ((System.ComponentModel.ISupportInitialize)(this.numStation)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numHeartbeat)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dgvRanges)).EndInit();
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
      private System.Windows.Forms.DataGridViewTextBoxColumn colKind;
      private System.Windows.Forms.DataGridViewTextBoxColumn colStart;
      private System.Windows.Forms.DataGridViewTextBoxColumn colEnd;
      private System.Windows.Forms.Label label4;
   }
}
