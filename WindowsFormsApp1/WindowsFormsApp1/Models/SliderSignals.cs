using System;

namespace WindowsFormsApp1.Models
{
    /// <summary>
    /// 滑台(STB)信號模型
    /// 代表從上游滑台讀取的所有輸入信號
    /// </summary>
    public class SliderSignals
    {
        /// <summary>
        /// Pin 1: 起動中 - 滑台設備已啟動
        /// </summary>
        public bool IsRunning { get; set; }

        /// <summary>
        /// Pin 2: 停止中 - 滑台設備處於停止狀態
        /// </summary>
        public bool IsStopped { get; set; }

        /// <summary>
        /// Pin 3: 排出予告 - 預告即將排出板材至滑台
        /// </summary>
        public bool DischargeNotice { get; set; }

        /// <summary>
        /// Pin 4: 排出要求 - 請求排出板材至指定位置
        /// </summary>
        public bool DischargeRequest { get; set; }

        /// <summary>
        /// Pin 5: 基板受取位置 - 指定板材的接收位置編號
        /// </summary>
        public short BoardReceivePosition { get; set; }

        /// <summary>
        /// Pin 6: 基板受取完了 - 確認板材已被下游設備接收
        /// </summary>
        public bool BoardReceiveComplete { get; set; }

        /// <summary>
        /// Pin 7: LastFlag - 標示批次的最後一片板材
        /// </summary>
        public bool LastFlag { get; set; }

        /// <summary>
        /// Pin 16: 非常停止 - 緊急停止信號
        /// </summary>
        public bool EmergencyStop { get; set; }

        /// <summary>
        /// 建立所有信號為預設值(OFF/0)的實例
        /// </summary>
        public SliderSignals()
        {
            IsRunning = false;
            IsStopped = false;
            DischargeNotice = false;
            DischargeRequest = false;
            BoardReceivePosition = 0;
            BoardReceiveComplete = false;
            LastFlag = false;
            EmergencyStop = false;
        }

        /// <summary>
        /// 取得信號的字串表示，用於日誌記錄
        /// </summary>
        public override string ToString()
        {
            return $"STB[起動:{IsRunning}, 停止:{IsStopped}, 排出予告:{DischargeNotice}, " +
                   $"排出要求:{DischargeRequest}, 位置:{BoardReceivePosition}, " +
                   $"受取完了:{BoardReceiveComplete}, Last:{LastFlag}, 緊急停止:{EmergencyStop}]";
        }
    }
}
