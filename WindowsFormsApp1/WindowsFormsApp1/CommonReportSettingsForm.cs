using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WindowsFormsApp1.CCLink.Models;

namespace WindowsFormsApp1
{
    public partial class CommonReportSettingsForm : Form
    {
        public CommonReportStatus1 Status1Result { get; private set; }
        public CommonReportStatus2 Status2Result { get; private set; }
        public CommonReportAlarm AlarmResult { get; private set; }

        public CommonReportSettingsForm(CommonReportStatus1 s1, CommonReportStatus2 s2, CommonReportAlarm alarm)
        {
            InitializeComponent();
            InitializeControls(s1, s2, alarm);
        }

        private void InitializeControls(CommonReportStatus1 s1, CommonReportStatus2 s2, CommonReportAlarm alarm)
        {
            // === Status 1 ===
            InitComboBox(cboAlarmStatus, s1.AlarmStatus, new[] {
                new ComboBoxItem(1, "1: 重大警報"),
                new ComboBoxItem(2, "2: 輕微警報"),
                new ComboBoxItem(3, "3: 預報"),
                new ComboBoxItem(4, "4: 無警報")
            });

            InitComboBox(cboMachineStatus, s1.MachineStatus, new[] {
                new ComboBoxItem(1, "1: 初始化"),
                new ComboBoxItem(2, "2: 準備"),
                new ComboBoxItem(3, "3: 準備完成"),
                new ComboBoxItem(4, "4: 生產"),
                new ComboBoxItem(5, "5: 停機"),
                new ComboBoxItem(6, "6: 停止")
            });

            InitComboBox(cboActionStatus, s1.ActionStatus, new[] {
                new ComboBoxItem(1, "1: 原點復歸中"),
                new ComboBoxItem(2, "2: 基板搬送中"),
                new ComboBoxItem(99, "99: 其他")
            });

            InitComboBox(cboWaitingStatus, s1.WaitingStatus, new[] {
                new ComboBoxItem(1, "1: 無等待"),
                new ComboBoxItem(2, "2: 下游等待"),
                new ComboBoxItem(3, "3: 上游等待"),
                new ComboBoxItem(4, "4: 上下游等待"),
                new ComboBoxItem(5, "5: 特殊等待"),
                new ComboBoxItem(99, "99: 其他")
            });

            InitComboBox(cboControlStatus, s1.ControlStatus, new[] {
                new ComboBoxItem(1, "1: 自動"),
                new ComboBoxItem(2, "2: 手動"),
                new ComboBoxItem(3, "3: 條件設定"),
                new ComboBoxItem(4, "4: 準備調整"),
                new ComboBoxItem(5, "5: 品種切換"),
                new ComboBoxItem(99, "99: 其他")
            });

            // === Status 2 ===
            var lightItems = new[] {
                new ComboBoxItem(0, "0: 熄燈"),
                new ComboBoxItem(1, "1: 常亮"),
                new ComboBoxItem(2, "2: 閃爍"),
                new ComboBoxItem(3, "3: 旋轉")
            };
            InitComboBox(cmbRedLight, s2.RedLightStatus, lightItems);
            InitComboBox(cmbYellowLight, s2.YellowLightStatus, lightItems);
            InitComboBox(cmbGreenLight, s2.GreenLightStatus, lightItems);

            var waitItems = new[] {
                new ComboBoxItem(0, "0: 無等待"),
                new ComboBoxItem(1, "1: 等待中")
            };
            InitComboBox(cmbUpstreamWaiting, s2.UpstreamWaitingStatus, waitItems);
            InitComboBox(cmbDownstreamWaiting, s2.DownstreamWaitingStatus, waitItems);

            nudDischargeRate.Value = s2.DischargeRate;
            nudStopTime.Value = s2.StopTime;
            nudProcessingCounter.Value = s2.ProcessingCounter;
            nudRetainedBoardCount.Value = s2.RetainedBoardCount;
            nudCurrentRecipeNo.Value = s2.CurrentRecipeNo;
            nudBoardThickness.Value = s2.BoardThicknessStatus;
            txtRecipeName.Text = s2.CurrentRecipeName;

            // === Alarm ===
            var errorNuds = new[] {
                nudError1, nudError2, nudError3, nudError4, nudError5, nudError6,
                nudError7, nudError8, nudError9, nudError10, nudError11, nudError12
            };

            for (int i = 0; i < 12 && i < alarm.ErrorCodes.Length; i++)
            {
                if (i < errorNuds.Length)
                    errorNuds[i].Value = alarm.ErrorCodes[i];
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // Build Status 1 Result
            Status1Result = new CommonReportStatus1(
                GetComboValue(cboAlarmStatus),
                GetComboValue(cboMachineStatus),
                GetComboValue(cboActionStatus),
                GetComboValue(cboWaitingStatus),
                GetComboValue(cboControlStatus)
            );

            // Build Status 2 Result
            Status2Result = new CommonReportStatus2(
                GetComboValue(cmbRedLight),
                GetComboValue(cmbYellowLight),
                GetComboValue(cmbGreenLight),
                GetComboValue(cmbUpstreamWaiting),
                GetComboValue(cmbDownstreamWaiting),
                (ushort)nudDischargeRate.Value,
                (ushort)nudStopTime.Value,
                (uint)nudProcessingCounter.Value,
                (ushort)nudRetainedBoardCount.Value,
                (ushort)nudCurrentRecipeNo.Value,
                (ushort)nudBoardThickness.Value,
                0, // UldFlag
                txtRecipeName.Text
            );

            // Build Alarm Result
            var errorNuds = new[] {
                nudError1, nudError2, nudError3, nudError4, nudError5, nudError6,
                nudError7, nudError8, nudError9, nudError10, nudError11, nudError12
            };
            var codes = new ushort[12];
            for(int i=0; i<12; i++)
            {
                if (i < errorNuds.Length)
                    codes[i] = (ushort)errorNuds[i].Value;
            }
            AlarmResult = new CommonReportAlarm(codes);
        }

        private void InitComboBox(ComboBox cmb, ushort currentValue, ComboBoxItem[] items)
        {
            cmb.Items.Clear();
            foreach (var item in items)
            {
                cmb.Items.Add(item);
                if (item.Value == currentValue)
                {
                    cmb.SelectedItem = item;
                }
            }
            if (cmb.SelectedIndex == -1 && cmb.Items.Count > 0)
                cmb.SelectedIndex = 0;
        }

        private ushort GetComboValue(ComboBox cmb)
        {
            if (cmb.SelectedItem is ComboBoxItem item)
            {
                return item.Value;
            }
            return 0;
        }

        private class ComboBoxItem
        {
            public ushort Value { get; }
            public string Display { get; }

            public ComboBoxItem(ushort value, string display)
            {
                Value = value;
                Display = display;
            }

            public override string ToString()
            {
                return Display;
            }
        }
    }
}
