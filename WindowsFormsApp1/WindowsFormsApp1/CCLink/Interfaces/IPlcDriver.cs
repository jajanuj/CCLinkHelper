using WindowsFormsApp1.CCLink.Models;

namespace WindowsFormsApp1.CCLink.Interfaces
{
   public interface IPlcDriver
   {
      #region Public Methods

      bool ReadBit(LinkDeviceAddress addr);
      void WriteBit(LinkDeviceAddress addr, bool value);
      short[] ReadWords(LinkDeviceAddress addr);
      void WriteWords(LinkDeviceAddress addr, short[] data);

      #endregion
   }
}
