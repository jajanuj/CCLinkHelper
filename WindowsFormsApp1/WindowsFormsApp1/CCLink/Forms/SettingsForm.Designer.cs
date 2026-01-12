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
         this.label4 = new System.Windows.Forms.Label();
         this.dgvRanges = new System.Windows.Forms.DataGridView();
         this.colKind = new System.Windows.Forms.DataGridViewComboBoxColumn();
         this.colStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.colEnd = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.btnSave = new System.Windows.Forms.Button();

         this.tabControl1.SuspendLayout();
         this.tabGeneral.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.dgvRanges)).BeginInit();
         this.SuspendLayout();

         // 
         // tabControl1
         // 
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
         this.tabGeneral.Controls.Add(this.label4);
         this.tabGeneral.Controls.Add(this.dgvRanges);
         this.tabGeneral.Location = new System.Drawing.Point(4, 22);
         this.tabGeneral.Name = "tabGeneral";
         this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
         this.tabGeneral.Size = new System.Drawing.Size(368, 394);
         this.tabGeneral.TabIndex = 0;
         this.tabGeneral.Text = "基本設定";
         this.tabGeneral.UseVisualStyleBackColor = true;

         // label4 (Scan Ranges)
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(10, 15);
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
         this.dgvRanges.Location = new System.Drawing.Point(10, 30);
         this.dgvRanges.Name = "dgvRanges";
         this.dgvRanges.RowTemplate.Height = 24;
         this.dgvRanges.Size = new System.Drawing.Size(350, 350);
         this.dgvRanges.TabIndex = 16;

         // colKind
         this.colKind.HeaderText = "Kind";
         this.colKind.Items.AddRange(new object[] { "LB", "LW", "M", "D", "X", "Y" });
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
         ((System.ComponentModel.ISupportInitialize)(this.dgvRanges)).EndInit();
         this.ResumeLayout(false);
      }

      protected System.Windows.Forms.TabControl tabControl1;
      protected System.Windows.Forms.TabPage tabGeneral;

      protected System.Windows.Forms.Label label4;
      protected System.Windows.Forms.DataGridView dgvRanges;
      protected System.Windows.Forms.DataGridViewComboBoxColumn colKind;
      protected System.Windows.Forms.DataGridViewTextBoxColumn colStart;
      protected System.Windows.Forms.DataGridViewTextBoxColumn colEnd;
      protected System.Windows.Forms.Button btnSave;
   }
}
