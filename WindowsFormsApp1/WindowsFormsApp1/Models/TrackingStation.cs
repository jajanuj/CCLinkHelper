namespace WindowsFormsApp1.Models
{
    /// <summary>
    /// 追蹤站點列舉。
    /// </summary>
    public enum TrackingStation
    {
        /// <summary>
        /// 未定義。
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// 插框 Robot。
        /// </summary>
        LoadingRobot = 1,

        /// <summary>
        /// 插框站。
        /// </summary>
        Loading = 2,

        /// <summary>
        /// 拆框 Robot。
        /// </summary>
        UnloadingRobot = 3,

        /// <summary>
        /// 拆框站。
        /// </summary>
        Unloading = 4
    }
}
