namespace WindowsFormsApp1.CCLink
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
         short rc = _api.mdDevRead(_path, devCode, addr.Start, addr.Length, dest);
         // treat non-zero as true for first bit/word
         return dest.Length > 0 && dest[0] != 0;
      }

      public void WriteBit(LinkDeviceAddress addr, bool value)
      {
         int devCode = MapDeviceCode(addr.Kind);
         if (value)
         {
            _api.mdDevSet(_path, 0, devCode, addr.Start);
         }
         else
         {
            _api.mdDevRst(_path, 0, devCode, addr.Start);
         }
      }

      public short[] ReadWords(LinkDeviceAddress addr)
      {
         var dest = new short[addr.Length];
         int devCode = MapDeviceCode(addr.Kind);
         _api.mdDevRead(_path, devCode, addr.Start, addr.Length, dest);
         return dest;
      }

      public void WriteWords(LinkDeviceAddress addr, short[] data)
      {
         int devCode = MapDeviceCode(addr.Kind);
         // IMelsecApiAdapter.mdDevWrite signature: mdDevWrite(int path, int deviceCode, int startAddr, int count, short[] src)
         _api.mdDevWrite(_path, devCode, addr.Start, data.Length, data);
      }
   }
}
