using System;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
   public partial class CommonReportStatus2SettingsForm : Form
   {
      public ushort RedLightStatus { get; private set; }
      public ushort YellowLightStatus { get; private set; }
      public ushort GreenLightStatus { get; private set; }
      public ushort UpstreamWaitingStatus { get; private set; }
      public ushort DownstreamWaitingStatus { get; private set; }
      public ushort DischargeRate { get; private set; }
      public ushort StopTime { get; private set; }
      public uint ProcessingCounter { get; private set; }
      public ushort RetainedBoardCount { get; private set; }
      public ushort CurrentRecipeNo { get; private set; }
      public ushort BoardThicknessStatus { get; private set; }
      public string CurrentRecipeName { get; private set; }

      public CommonReportStatus2SettingsForm(
         ushort redLightStatus, ushort yellowLightStatus, ushort greenLightStatus,
         ushort upstreamWaitingStatus, ushort downstreamWaitingStatus,
         ushort dischargeRate, ushort stopTime,
         uint processingCounter,
         ushort retainedBoardCount, ushort currentRecipeNo,
         ushort boardThicknessStatus,
         string currentRecipeName)
      {
         InitializeComponent();
         InitializeComboBoxItems();
         LoadCurrentValues(redLightStatus, yellowLightStatus, greenLightStatus,
            upstreamWaitingStatus, downstreamWaitingStatus,
            dischargeRate, stopTime, processingCounter,
            retainedBoardCount, currentRecipeNo, boardThicknessStatus, currentRecipeName);
      }

      private void InitializeComboBoxItems()
      {
         // Initialize Light Statuses (0-3)
         var lightItems = new object[]
         {
            new ComboBoxItem(0, "0 - 熄燈"),
            new ComboBoxItem(1, "1 - 常亮"),
            new ComboBoxItem(2, "2 - 閃爍"),
            new ComboBoxItem(3, "3 - 旋轉")
         };
         
         cmbRedLight.Items.AddRange(lightItems);
         cmbYellowLight.Items.AddRange(lightItems);
         cmbGreenLight.Items.AddRange(lightItems);

         // Initialize Waiting Statuses (Same as Status1)
         var waitingItems = new object[]
         {
            new ComboBoxItem(1, "1 - 無等待"),
            new ComboBoxItem(2, "2 - 下游"),
            new ComboBoxItem(3, "3 - 上游"),
            new ComboBoxItem(4, "4 - 上下游"),
            new ComboBoxItem(5, "5 - 特殊"),
            new ComboBoxItem(99, "99 - 其他")
         };

         cmbUpstreamWaiting.Items.AddRange(waitingItems);
         cmbDownstreamWaiting.Items.AddRange(waitingItems);
      }

      private void LoadCurrentValues(
         ushort redLightStatus, ushort yellowLightStatus, ushort greenLightStatus,
         ushort upstreamWaitingStatus, ushort downstreamWaitingStatus,
         ushort dischargeRate, ushort stopTime,
         uint processingCounter,
         ushort retainedBoardCount, ushort currentRecipeNo,
         ushort boardThicknessStatus,
         string currentRecipeName)
      {
         SetComboBoxValue(cmbRedLight, redLightStatus);
         SetComboBoxValue(cmbYellowLight, yellowLightStatus);
         SetComboBoxValue(cmbGreenLight, greenLightStatus);
         SetComboBoxValue(cmbUpstreamWaiting, upstreamWaitingStatus);
         SetComboBoxValue(cmbDownstreamWaiting, downstreamWaitingStatus);

         nudDischargeRate.Value = dischargeRate;
         nudStopTime.Value = stopTime;
         nudProcessingCounter.Value = processingCounter;
         nudRetainedBoardCount.Value = retainedBoardCount;
         nudCurrentRecipeNo.Value = currentRecipeNo;
         nudBoardThickness.Value = boardThicknessStatus;
         txtRecipeName.Text = currentRecipeName;
      }

      private void SetComboBoxValue(ComboBox comboBox, ushort value)
      {
         foreach (ComboBoxItem item in comboBox.Items)
         {
            if (item.Value == value)
            {
               comboBox.SelectedItem = item;
               return;
            }
         }
         if (comboBox.Items.Count > 0)
         {
            comboBox.SelectedIndex = 0;
         }
      }

      private void BtnOK_Click(object sender, EventArgs e)
      {
         RedLightStatus = ((ComboBoxItem)cmbRedLight.SelectedItem).Value;
         YellowLightStatus = ((ComboBoxItem)cmbYellowLight.SelectedItem).Value;
         GreenLightStatus = ((ComboBoxItem)cmbGreenLight.SelectedItem).Value;
         UpstreamWaitingStatus = ((ComboBoxItem)cmbUpstreamWaiting.SelectedItem).Value;
         DownstreamWaitingStatus = ((ComboBoxItem)cmbDownstreamWaiting.SelectedItem).Value;

         DischargeRate = (ushort)nudDischargeRate.Value;
         StopTime = (ushort)nudStopTime.Value;
         ProcessingCounter = (uint)nudProcessingCounter.Value;
         RetainedBoardCount = (ushort)nudRetainedBoardCount.Value;
         CurrentRecipeNo = (ushort)nudCurrentRecipeNo.Value;
         BoardThicknessStatus = (ushort)nudBoardThickness.Value;
         CurrentRecipeName = txtRecipeName.Text;
      }
   }
}
