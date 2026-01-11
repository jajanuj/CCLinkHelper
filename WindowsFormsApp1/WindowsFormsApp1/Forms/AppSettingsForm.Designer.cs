namespace WindowsFormsApp1.Forms
{
   partial class AppSettingsForm
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
         this.label3 = new System.Windows.Forms.Label();
         this.numHeartbeat = new System.Windows.Forms.NumericUpDown();
         this.label11 = new System.Windows.Forms.Label();
         this.numTimeSync = new System.Windows.Forms.NumericUpDown();
         this.label12 = new System.Windows.Forms.Label();
         this.txtTrigger = new System.Windows.Forms.TextBox();
         this.label13 = new System.Windows.Forms.Label();
         this.txtData = new System.Windows.Forms.TextBox();
         ((System.ComponentModel.ISupportInitialize)(this.numHeartbeat)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numTimeSync)).BeginInit();
         this.SuspendLayout();
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
         // AppSettingsForm
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
         this.Controls.Add(this.numHeartbeat);
         this.Controls.Add(this.label3);
         this.Name = "AppSettingsForm";
         this.Text = "AppSettingsForm";
         this.Controls.SetChildIndex(this.label3, 0);
         this.Controls.SetChildIndex(this.numHeartbeat, 0);
         this.Controls.SetChildIndex(this.label11, 0);
         this.Controls.SetChildIndex(this.numTimeSync, 0);
         this.Controls.SetChildIndex(this.label12, 0);
         this.Controls.SetChildIndex(this.txtTrigger, 0);
         this.Controls.SetChildIndex(this.label13, 0);
         this.Controls.SetChildIndex(this.txtData, 0);
         ((System.ComponentModel.ISupportInitialize)(this.numHeartbeat)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numTimeSync)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.NumericUpDown numHeartbeat;
      private System.Windows.Forms.Label label11;
      private System.Windows.Forms.NumericUpDown numTimeSync;
      private System.Windows.Forms.Label label12;
      private System.Windows.Forms.TextBox txtTrigger;
      private System.Windows.Forms.Label label13;
      private System.Windows.Forms.TextBox txtData;
   }
}
