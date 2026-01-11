using System;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
   public partial class CommonReportAlarmSettingsForm : Form
   {
      public ushort[] ErrorCodes { get; private set; }

      public CommonReportAlarmSettingsForm(ushort[] currentCodes)
      {
         InitializeComponent();
         // Create a copy to avoid external modification until OK
         ErrorCodes = currentCodes != null ? (ushort[])currentCodes.Clone() : new ushort[12];
         LoadCurrentValues();
      }

      private void LoadCurrentValues()
      {
         if (ErrorCodes == null || ErrorCodes.Length < 12) return;

         nudError1.Value = ErrorCodes[0];
         nudError2.Value = ErrorCodes[1];
         nudError3.Value = ErrorCodes[2];
         nudError4.Value = ErrorCodes[3];
         nudError5.Value = ErrorCodes[4];
         nudError6.Value = ErrorCodes[5];
         nudError7.Value = ErrorCodes[6];
         nudError8.Value = ErrorCodes[7];
         nudError9.Value = ErrorCodes[8];
         nudError10.Value = ErrorCodes[9];
         nudError11.Value = ErrorCodes[10];
         nudError12.Value = ErrorCodes[11];
      }

      private void BtnOK_Click(object sender, EventArgs e)
      {
         ErrorCodes = new ushort[12];
         ErrorCodes[0] = (ushort)nudError1.Value;
         ErrorCodes[1] = (ushort)nudError2.Value;
         ErrorCodes[2] = (ushort)nudError3.Value;
         ErrorCodes[3] = (ushort)nudError4.Value;
         ErrorCodes[4] = (ushort)nudError5.Value;
         ErrorCodes[5] = (ushort)nudError6.Value;
         ErrorCodes[6] = (ushort)nudError7.Value;
         ErrorCodes[7] = (ushort)nudError8.Value;
         ErrorCodes[8] = (ushort)nudError9.Value;
         ErrorCodes[9] = (ushort)nudError10.Value;
         ErrorCodes[10] = (ushort)nudError11.Value;
         ErrorCodes[11] = (ushort)nudError12.Value;
      }
   }
}
