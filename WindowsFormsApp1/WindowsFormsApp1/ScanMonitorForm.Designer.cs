namespace WindowsFormsApp1
{
   partial class ScanMonitorForm
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
         this.dgvMonitor = new System.Windows.Forms.DataGridView();
         this.colAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.colCurrentValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.colNewValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.panelControls = new System.Windows.Forms.Panel();
         this.lblLastUpdate = new System.Windows.Forms.Label();
         this.btnRefresh = new System.Windows.Forms.Button();
         this.btnWriteAll = new System.Windows.Forms.Button();
         this.btnWriteSelected = new System.Windows.Forms.Button();
         this.btnStopUpdate = new System.Windows.Forms.Button();
         this.btnStartUpdate = new System.Windows.Forms.Button();
         this.nudUpdateInterval = new System.Windows.Forms.NumericUpDown();
         this.lblUpdateInterval = new System.Windows.Forms.Label();
         this.chkHexFormat = new System.Windows.Forms.CheckBox();
         ((System.ComponentModel.ISupportInitialize)(this.dgvMonitor)).BeginInit();
         this.panelControls.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.nudUpdateInterval)).BeginInit();
         this.SuspendLayout();
         // 
         // dgvMonitor
         // 
         this.dgvMonitor.AllowUserToAddRows = false;
         this.dgvMonitor.AllowUserToDeleteRows = false;
         this.dgvMonitor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
         this.dgvMonitor.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colAddress,
            this.colType,
            this.colCurrentValue,
            this.colNewValue});
         this.dgvMonitor.Dock = System.Windows.Forms.DockStyle.Fill;
         this.dgvMonitor.Location = new System.Drawing.Point(0, 80);
         this.dgvMonitor.Name = "dgvMonitor";
         this.dgvMonitor.RowHeadersWidth = 51;
         this.dgvMonitor.Size = new System.Drawing.Size(800, 370);
         this.dgvMonitor.TabIndex = 0;
         // 
         // colAddress
         // 
         this.colAddress.HeaderText = "Address";
         this.colAddress.MinimumWidth = 6;
         this.colAddress.Name = "colAddress";
         this.colAddress.ReadOnly = true;
         this.colAddress.Width = 120;
         // 
         // colType
         // 
         this.colType.HeaderText = "Type";
         this.colType.MinimumWidth = 6;
         this.colType.Name = "colType";
         this.colType.ReadOnly = true;
         this.colType.Width = 80;
         // 
         // colCurrentValue
         // 
         this.colCurrentValue.HeaderText = "Current Value";
         this.colCurrentValue.MinimumWidth = 6;
         this.colCurrentValue.Name = "colCurrentValue";
         this.colCurrentValue.ReadOnly = true;
         this.colCurrentValue.Width = 150;
         // 
         // colNewValue
         // 
         this.colNewValue.HeaderText = "New Value";
         this.colNewValue.MinimumWidth = 6;
         this.colNewValue.Name = "colNewValue";
         this.colNewValue.Width = 150;
         // 
         // panelControls
         // 
         this.panelControls.Controls.Add(this.lblLastUpdate);
         this.panelControls.Controls.Add(this.btnRefresh);
         this.panelControls.Controls.Add(this.btnWriteAll);
         this.panelControls.Controls.Add(this.btnWriteSelected);
         this.panelControls.Controls.Add(this.btnStopUpdate);
         this.panelControls.Controls.Add(this.btnStartUpdate);
         this.panelControls.Controls.Add(this.nudUpdateInterval);
         this.panelControls.Controls.Add(this.lblUpdateInterval);
         this.panelControls.Controls.Add(this.chkHexFormat);
         this.panelControls.Dock = System.Windows.Forms.DockStyle.Top;
         this.panelControls.Location = new System.Drawing.Point(0, 0);
         this.panelControls.Name = "panelControls";
         this.panelControls.Size = new System.Drawing.Size(800, 80);
         this.panelControls.TabIndex = 1;
         // 
         // lblLastUpdate
         // 
         this.lblLastUpdate.AutoSize = true;
         this.lblLastUpdate.Location = new System.Drawing.Point(12, 55);
         this.lblLastUpdate.Name = "lblLastUpdate";
         this.lblLastUpdate.Size = new System.Drawing.Size(107, 12);
         this.lblLastUpdate.TabIndex = 7;
         this.lblLastUpdate.Text = "Last Update: Never";
         // 
         // btnRefresh
         // 
         this.btnRefresh.Location = new System.Drawing.Point(310, 10);
         this.btnRefresh.Name = "btnRefresh";
         this.btnRefresh.Size = new System.Drawing.Size(90, 30);
         this.btnRefresh.TabIndex = 6;
         this.btnRefresh.Text = "Refresh";
         this.btnRefresh.UseVisualStyleBackColor = true;
         this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
         // 
         // btnWriteAll
         // 
         this.btnWriteAll.Location = new System.Drawing.Point(540, 10);
         this.btnWriteAll.Name = "btnWriteAll";
         this.btnWriteAll.Size = new System.Drawing.Size(110, 30);
         this.btnWriteAll.TabIndex = 5;
         this.btnWriteAll.Text = "Write All";
         this.btnWriteAll.UseVisualStyleBackColor = true;
         this.btnWriteAll.Click += new System.EventHandler(this.btnWriteAll_Click);
         // 
         // btnWriteSelected
         // 
         this.btnWriteSelected.Location = new System.Drawing.Point(410, 10);
         this.btnWriteSelected.Name = "btnWriteSelected";
         this.btnWriteSelected.Size = new System.Drawing.Size(120, 30);
         this.btnWriteSelected.TabIndex = 4;
         this.btnWriteSelected.Text = "Write Selected";
         this.btnWriteSelected.UseVisualStyleBackColor = true;
         this.btnWriteSelected.Click += new System.EventHandler(this.btnWriteSelected_Click);
         // 
         // btnStopUpdate
         // 
         this.btnStopUpdate.Enabled = false;
         this.btnStopUpdate.Location = new System.Drawing.Point(210, 10);
         this.btnStopUpdate.Name = "btnStopUpdate";
         this.btnStopUpdate.Size = new System.Drawing.Size(90, 30);
         this.btnStopUpdate.TabIndex = 3;
         this.btnStopUpdate.Text = "Stop";
         this.btnStopUpdate.UseVisualStyleBackColor = true;
         this.btnStopUpdate.Click += new System.EventHandler(this.btnStopUpdate_Click);
         // 
         // btnStartUpdate
         // 
         this.btnStartUpdate.Location = new System.Drawing.Point(110, 10);
         this.btnStartUpdate.Name = "btnStartUpdate";
         this.btnStartUpdate.Size = new System.Drawing.Size(90, 30);
         this.btnStartUpdate.TabIndex = 2;
         this.btnStartUpdate.Text = "Start";
         this.btnStartUpdate.UseVisualStyleBackColor = true;
         this.btnStartUpdate.Click += new System.EventHandler(this.btnStartUpdate_Click);
         // 
         // nudUpdateInterval
         // 
         this.nudUpdateInterval.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
         this.nudUpdateInterval.Location = new System.Drawing.Point(12, 25);
         this.nudUpdateInterval.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
         this.nudUpdateInterval.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
         this.nudUpdateInterval.Name = "nudUpdateInterval";
         this.nudUpdateInterval.Size = new System.Drawing.Size(80, 22);
         this.nudUpdateInterval.TabIndex = 1;
         this.nudUpdateInterval.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
         // 
         // lblUpdateInterval
         // 
         this.lblUpdateInterval.AutoSize = true;
         this.lblUpdateInterval.Location = new System.Drawing.Point(12, 10);
         this.lblUpdateInterval.Name = "lblUpdateInterval";
         this.lblUpdateInterval.Size = new System.Drawing.Size(88, 12);
         this.lblUpdateInterval.TabIndex = 0;
         this.lblUpdateInterval.Text = "Interval (ms):";
         // 
         // chkHexFormat
         // 
         this.chkHexFormat.AutoSize = true;
         this.chkHexFormat.Location = new System.Drawing.Point(660, 15);
         this.chkHexFormat.Name = "chkHexFormat";
         this.chkHexFormat.Size = new System.Drawing.Size(120, 16);
         this.chkHexFormat.TabIndex = 8;
         this.chkHexFormat.Text = "Hex Format (0x)";
         this.chkHexFormat.UseVisualStyleBackColor = true;
         this.chkHexFormat.CheckedChanged += new System.EventHandler(this.chkHexFormat_CheckedChanged);
         // 
         // ScanMonitorForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
         this.ClientSize = new System.Drawing.Size(800, 450);
         this.Controls.Add(this.dgvMonitor);
         this.Controls.Add(this.panelControls);
         this.Name = "ScanMonitorForm";
         this.Text = "Scan Area Monitor";
         ((System.ComponentModel.ISupportInitialize)(this.dgvMonitor)).EndInit();
         this.panelControls.ResumeLayout(false);
         this.panelControls.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.nudUpdateInterval)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.DataGridView dgvMonitor;
      private System.Windows.Forms.Panel panelControls;
      private System.Windows.Forms.NumericUpDown nudUpdateInterval;
      private System.Windows.Forms.Label lblUpdateInterval;
      private System.Windows.Forms.Button btnStartUpdate;
      private System.Windows.Forms.Button btnStopUpdate;
      private System.Windows.Forms.Button btnWriteSelected;
      private System.Windows.Forms.Button btnWriteAll;
      private System.Windows.Forms.Button btnRefresh;
      private System.Windows.Forms.Label lblLastUpdate;
      private System.Windows.Forms.DataGridViewTextBoxColumn colAddress;
      private System.Windows.Forms.DataGridViewTextBoxColumn colType;
      private System.Windows.Forms.DataGridViewTextBoxColumn colCurrentValue;
      private System.Windows.Forms.DataGridViewTextBoxColumn colNewValue;
      private System.Windows.Forms.CheckBox chkHexFormat;
   }
}
