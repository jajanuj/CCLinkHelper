using WindowsFormsApp1.CCLink.Interfaces;
using WindowsFormsApp1.CCLink.Models;

namespace WindowsFormsApp1.CCLink.Drivers
{
   // Simple adapter that wraps IMelsecApiAdapter for common operations
   public class MelsecPlcDriver : IPlcDriver
   {
      #region Fields

      private readonly IMelsecApiAdapter _api;
      private readonly int _path;

      #endregion

      #region Constructors

      public MelsecPlcDriver(IMelsecApiAdapter api, int path)
      {
         _api = api;
         _path = path;
      }

      #endregion

      #region Private Methods

      private static int MapDeviceCode(string kind)
      {
         if (string.IsNullOrWhiteSpace(kind))
         {
            throw new System.ArgumentException(nameof(kind));
         }

         switch (kind.ToUpperInvariant())
         {
            case "LB": return CCLinkConstants.DEV_LB;
            case "LW": return CCLinkConstants.DEV_LW;
            case "LX": return CCLinkConstants.DEV_LX;
            case "LY": return CCLinkConstants.DEV_LY;
            default: throw new System.ArgumentException($"Unsupported device kind: {kind}");
         }
      }

      #endregion

      public bool ReadBit(LinkDeviceAddress addr)
      {
         var dest = new short[addr.Length];
         int devCode = MapDeviceCode(addr.Kind);
         int size = addr.Length * 2;
         int rc = _api.ReceiveEx(_path, 0, 0, devCode, addr.Start, ref size, dest);
         // treat non-zero as true for first bit/word
         return dest.Length > 0 && dest[0] != 0;
      }

      public void WriteBit(LinkDeviceAddress addr, bool value)
      {
         int devCode = MapDeviceCode(addr.Kind);
         if (value)
         {
            _api.DevSetEx(_path, 0, 0, devCode, addr.Start);
         }
         else
         {
            _api.DevRstEx(_path, 0, 0, devCode, addr.Start);
         }
      }

      public short[] ReadWords(LinkDeviceAddress addr)
      {
         var dest = new short[addr.Length];
         int devCode = MapDeviceCode(addr.Kind);
         int size = addr.Length * 2;
         _api.ReceiveEx(_path, 0, 0, devCode, addr.Start, ref size, dest);
         return dest;
      }

      public void WriteWords(LinkDeviceAddress addr, short[] data)
      {
         int devCode = MapDeviceCode(addr.Kind);
         int size = data.Length * 2;
         _api.SendEx(_path, 0, 0, devCode, addr.Start, ref size, data);
      }
   }
}
