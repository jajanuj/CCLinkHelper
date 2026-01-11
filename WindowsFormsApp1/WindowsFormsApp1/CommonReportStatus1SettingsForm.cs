using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
   public partial class CommonReportStatus1SettingsForm : Form
   {
      #region Constructors

      public CommonReportStatus1SettingsForm(ushort alarmStatus = 4, ushort machineStatus = 4,
         ushort actionStatus = 2, ushort waitingStatus = 1, ushort controlStatus = 1)
      {
         InitializeComponent();
         InitializeComboBox();
         LoadCurrentValues(alarmStatus, machineStatus, actionStatus, waitingStatus, controlStatus);
      }

      #endregion

      #region Properties

      public ushort AlarmStatus { get; private set; }
      public ushort MachineStatus { get; private set; }
      public ushort ActionStatus { get; private set; }
      public ushort WaitingStatus { get; private set; }
      public ushort ControlStatus { get; private set; }

      #endregion

      #region Private Methods

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

      private void LoadCurrentValues(ushort alarmStatus, ushort machineStatus,
         ushort actionStatus, ushort waitingStatus, ushort controlStatus)
      {
         SetComboBoxValue(cboAlarmStatus, alarmStatus);
         SetComboBoxValue(cboMachineStatus, machineStatus);
         SetComboBoxValue(cboActionStatus, actionStatus);
         SetComboBoxValue(cboWaitingStatus, waitingStatus);
         SetComboBoxValue(cboControlStatus, controlStatus);
      }

      private void InitializeComboBox()
      {
         cboAlarmStatus.Items.AddRange(new object[]
         {
            new ComboBoxItem(1, "1 - 重大警報"),
            new ComboBoxItem(2, "2 - 輕警報"),
            new ComboBoxItem(3, "3 - 預報"),
            new ComboBoxItem(4, "4 - 無警報")
         });

         cboMachineStatus.Items.AddRange(new object[]
         {
            new ComboBoxItem(1, "1 - 初始化"),
            new ComboBoxItem(2, "2 - 準備"),
            new ComboBoxItem(3, "3 - 準備完成"),
            new ComboBoxItem(4, "4 - 生產"),
            new ComboBoxItem(5, "5 - 停機"),
            new ComboBoxItem(6, "6 - 停止")
         });

         cboActionStatus.Items.AddRange(new object[]
         {
            new ComboBoxItem(1, "1 - 原點復歸"),
            new ComboBoxItem(2, "2 - 基板搬送"),
            new ComboBoxItem(99, "99 - 其他")
         });
         cboWaitingStatus.Items.AddRange(new object[]
         {
            new ComboBoxItem(1, "1 - 無等待"),
            new ComboBoxItem(2, "2 - 下游"),
            new ComboBoxItem(3, "3 - 上游"),
            new ComboBoxItem(4, "4 - 上下游"),
            new ComboBoxItem(5, "5 - 特殊"),
            new ComboBoxItem(99, "99 - 其他")
         });
         cboControlStatus.Items.AddRange(new object[]
         {
            new ComboBoxItem(1, "1 - 自動"),
            new ComboBoxItem(2, "2 - 手動"),
            new ComboBoxItem(3, "3 - 條件設定"),
            new ComboBoxItem(4, "4 - 準備調整"),
            new ComboBoxItem(5, "5 - 品種切換"),
            new ComboBoxItem(99, "99 - 其他")
         });
      }

      private void btnOk_Click(object sender, EventArgs e)
      {
         AlarmStatus = ((ComboBoxItem)cboAlarmStatus.SelectedItem).Value;
         MachineStatus = ((ComboBoxItem)cboMachineStatus.SelectedItem).Value;
         ActionStatus = ((ComboBoxItem)cboActionStatus.SelectedItem).Value;
         WaitingStatus = ((ComboBoxItem)cboWaitingStatus.SelectedItem).Value;
         ControlStatus = ((ComboBoxItem)cboControlStatus.SelectedItem).Value;
      }

      #endregion
   }

   public class ComboBoxItem
   {
      #region Constructors

      public ComboBoxItem(ushort value, string display)
      {
         Value = value;
         Display = display;
      }

      #endregion

      #region Properties

      public ushort Value { get; }
      public string Display { get; }

      #endregion

      #region Public Methods

      public override string ToString()
      {
         return Display;
      }

      #endregion
   }
}