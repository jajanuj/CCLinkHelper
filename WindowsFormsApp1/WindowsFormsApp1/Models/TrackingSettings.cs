using System;

namespace WindowsFormsApp1.Models
{
    /// <summary>
    /// 追蹤資料位址設定
    /// </summary>
    [Serializable]
    public class TrackingSettings
    {
        /// <summary>
        /// 插框 Robot 追蹤資料起始位址
        /// </summary>
        public string LoadingRobotAddress { get; set; } = "LW0000";

        /// <summary>
        /// 插框站追蹤資料起始位址
        /// </summary>
        public string LoadingStationAddress { get; set; } = "LW0010";

        /// <summary>
        /// 拆框 Robot 追蹤資料起始位址
        /// </summary>
        public string UnloadingRobotAddress { get; set; } = "LW0020";

        /// <summary>
        /// 拆框站追蹤資料起始位址
        /// </summary>
        public string UnloadingStationAddress { get; set; } = "LW0030";

        /// <summary>
        /// 根據站點取得對應位址
        /// </summary>
        public string GetAddress(TrackingStation station)
        {
            switch (station)
            {
                case TrackingStation.LoadingRobot:
                    return LoadingRobotAddress;
                case TrackingStation.Loading:
                    return LoadingStationAddress;
                case TrackingStation.UnloadingRobot:
                    return UnloadingRobotAddress;
                case TrackingStation.Unloading:
                    return UnloadingStationAddress;
                default:
                    throw new ArgumentException($"不支援的站點: {station}");
            }
        }
    }
}
