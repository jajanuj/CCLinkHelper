using System;

namespace WindowsFormsApp1.Models
{
   /// <summary>
   /// 維護功能設定。
   /// </summary>
   [Serializable]
   public class MaintenanceSettings
   {
      #region Properties

      public int PlcToDeviceT1Timeout { get; set; }
      public int PlcToDeviceT2Timeout { get; set; }
      public int DeviceToPlcT1Timeout { get; set; }
      public int DeviceToPlcT2Timeout { get; set; }

      /// <summary>
      /// 基板位置資料起始位址
      /// </summary>
      public string AddrPositionDataBase { get; set; } = "LB184A"; // Base address for 5400 words (540 positions)

      public string AddrPlcToDeviceRequestFlag { get; set; } = "LB0106";
      public string AddrPlcToDeviceRequestTrackingData { get; set; } = "LW05BE";
      public string AddrPlcToDeviceRequestPosData { get; set; } = "LW05C8";
      public string AddrPlcToDeviceResponseOk { get; set; } = "LB0304";
      public string AddrPlcToDeviceResponseNg { get; set; } = "LB0305";

      public string AddrDeviceToPlcRequestFlag { get; set; } = "LB0306";
      public string AddrDeviceToPlcRequestTrackingData { get; set; } = "LW17D1";
      public string AddrDeviceToPlcRequestPosData { get; set; } = "LW17DB";
      public string AddrDeviceToPlcResponseFlag { get; set; } = "LB0107";

      #endregion
   }
}