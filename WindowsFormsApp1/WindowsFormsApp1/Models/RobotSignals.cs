using System;

namespace WindowsFormsApp1.Models
{
    /// <summary>
    /// 機械手臂(RB)信號模型
    /// 代表向上游滑台輸出的所有信號
    /// </summary>
    public class RobotSignals
    {
        /// <summary>
        /// Pin 1: 起動中 - 機械手臂設備已啟動
        /// </summary>
        public bool IsRunning { get; set; }

        /// <summary>
        /// Pin 2: 停止中 - 機械手臂設備處於停止狀態
        /// </summary>
        public bool IsStopped { get; set; }

        /// <summary>
        /// Pin 3: 收料可能予告1 - 預告下游設備準備好接收板材
        /// </summary>
        public bool ReadyToReceiveNotice1 { get; set; }

        /// <summary>
        /// Pin 4: 排出要求 - 請求上游排出板材
        /// </summary>
        public bool DischargeRequest { get; set; }

        /// <summary>
        /// Pin 5: 基板受取完了1 - 確認已接收板材
        /// </summary>
        public bool BoardReceiveComplete1 { get; set; }

        /// <summary>
        /// Pin 6: 受取不可 - 表示目前無法接收板材
        /// </summary>
        public bool CannotReceive { get; set; }

        /// <summary>
        /// Pin 7: 手動 - 滑台手動控制模式信號
        /// </summary>
        public bool ManualMode { get; set; }

        /// <summary>
        /// Pin 26: データ確認NG - 上游的資料確認失敗
        /// </summary>
        public bool DataCheckNG { get; set; }

        /// <summary>
        /// Pin 27: データ確認OK - 上游的資料確認成功
        /// </summary>
        public bool DataCheckOK { get; set; }

        /// <summary>
        /// Pin 28: 基板受取位置 - 滑台設定的板材接收位置
        /// </summary>
        public short BoardReceivePosition { get; set; }

        /// <summary>
        /// 建立所有信號為預設值(OFF/0)的實例
        /// </summary>
        public RobotSignals()
        {
            IsRunning = false;
            IsStopped = false;
            ReadyToReceiveNotice1 = false;
            DischargeRequest = false;
            BoardReceiveComplete1 = false;
            CannotReceive = false;
            ManualMode = false;
            DataCheckNG = false;
            DataCheckOK = false;
            BoardReceivePosition = 0;
        }

        /// <summary>
        /// 取得信號的字串表示，用於日誌記錄
        /// </summary>
        public override string ToString()
        {
            return $"RB[起動:{IsRunning}, 停止:{IsStopped}, 收料予告:{ReadyToReceiveNotice1}, " +
                   $"排出要求:{DischargeRequest}, 受取完了:{BoardReceiveComplete1}, " +
                   $"受取不可:{CannotReceive}, 手動:{ManualMode}, 位置:{BoardReceivePosition}]";
        }

        /// <summary>
        /// 復位所有輸出信號為OFF
        /// </summary>
        public void ResetAll()
        {
            IsRunning = false;
            IsStopped = false;
            ReadyToReceiveNotice1 = false;
            DischargeRequest = false;
            BoardReceiveComplete1 = false;
            CannotReceive = false;
            ManualMode = false;
            DataCheckNG = false;
            DataCheckOK = false;
            BoardReceivePosition = 0;
        }
    }
}
