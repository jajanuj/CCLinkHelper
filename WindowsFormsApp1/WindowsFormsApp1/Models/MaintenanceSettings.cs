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

      public int MaintenanceT1Timeout { get; set; }
      public int MaintenanceT2Timeout { get; set; }

      public int MaintenanceEqToPlcT1Timeout { get; set; }
      public int MaintenanceEqToPlcT2Timeout { get; set; }

      #endregion
   }
}