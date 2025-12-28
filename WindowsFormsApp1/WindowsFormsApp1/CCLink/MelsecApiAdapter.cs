using System.Runtime.InteropServices;

namespace WindowsFormsApp1.CCLink
{
   internal static class MelsecApi
   {
      #region Constant

      private const string DllName32 = "MDFUNC32.DLL";
      private const string DllName64 = "MDFUNC64.DLL";

      #endregion

      [DllImport(DllName32, EntryPoint = "mdClose", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdClose32(int path);

      [DllImport(DllName64, EntryPoint = "mdClose", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdClose64(int path);

      [DllImport(DllName32, EntryPoint = "mdDevRead", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdDevRead32(int path, int deviceCode, int startAddr, int count, short[] dest);

      [DllImport(DllName64, EntryPoint = "mdDevRead", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdDevRead64(int path, int deviceCode, int startAddr, int count, short[] dest);

      [DllImport(DllName32, EntryPoint = "mdDevRst", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdDevRst32(int path, int stno, int devtyp, int devno);

      [DllImport(DllName64, EntryPoint = "mdDevRst", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdDevRst64(int path, int stno, int devtyp, int devno);

      [DllImport(DllName32, EntryPoint = "mdDevSet", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdDevSet32(int path, int stno, int devtyp, int devno);

      [DllImport(DllName64, EntryPoint = "mdDevSet", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdDevSet64(int path, int stno, int devtyp, int devno);

      [DllImport(DllName32, EntryPoint = "mdDevWrite", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdDevWrite32(int path, int deviceCode, int startAddr, int count, short[] src);

      [DllImport(DllName64, EntryPoint = "mdDevWrite", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdDevWrite64(int path, int deviceCode, int startAddr, int count, short[] src);

      [DllImport(DllName32, EntryPoint = "mdGetLastError", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdGetLastError32(int path, out int code);

      [DllImport(DllName64, EntryPoint = "mdGetLastError", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdGetLastError64(int path, out int code);

      [DllImport(DllName32, EntryPoint = "mdGetStatus", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdGetStatus32(int path, out int statusCode);

      [DllImport(DllName64, EntryPoint = "mdGetStatus", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdGetStatus64(int path, out int statusCode);

      [DllImport(DllName32, EntryPoint = "mdOpen", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdOpen32(short chan, short mode, out int path);

      [DllImport(DllName64, EntryPoint = "mdOpen", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdOpen64(short chan, short mode, out int path);

      [DllImport(DllName32, EntryPoint = "mdRandREx", CallingConvention = CallingConvention.StdCall)]
      internal static extern int mdRandREx32(int path, int stno, int[] dev, short[] buf, int bufsize);

      [DllImport(DllName64, EntryPoint = "mdRandREx", CallingConvention = CallingConvention.StdCall)]
      internal static extern int mdRandREx64(int path, int stno, int[] dev, short[] buf, int bufsize);

      [DllImport(DllName32, EntryPoint = "mdRandWEx", CallingConvention = CallingConvention.StdCall)]
      internal static extern int mdRandWEx32(int path, int stno, int[] dev, short[] buf, int bufsize);

      [DllImport(DllName64, EntryPoint = "mdRandWEx", CallingConvention = CallingConvention.StdCall)]
      internal static extern int mdRandWEx64(int path, int stno, int[] dev, short[] buf, int bufsize);

      [DllImport(DllName32, EntryPoint = "mdReceive", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdReceive32(int path, byte[] buffer, int length, out int received);

      [DllImport(DllName64, EntryPoint = "mdReceive", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdReceive64(int path, byte[] buffer, int length, out int received);

      [DllImport(DllName32, EntryPoint = "mdReceiveEx", CallingConvention = CallingConvention.StdCall)]
      internal static extern int mdReceiveEx32(int path, int netno, int stno, int devtyp, int devno, ref int size, short[] data);

      [DllImport(DllName64, EntryPoint = "mdReceiveEx", CallingConvention = CallingConvention.StdCall)]
      internal static extern int mdReceiveEx64(int path, int netno, int stno, int devtyp, int devno, ref int size, short[] data);

      [DllImport(DllName32, EntryPoint = "mdSend", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdSend32(int path, byte[] buffer, int length);

      [DllImport(DllName64, EntryPoint = "mdSend", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdSend64(int path, byte[] buffer, int length);

      [DllImport(DllName32, EntryPoint = "mdSendEx", CallingConvention = CallingConvention.StdCall)]
      internal static extern int mdSendEx32(int path, int netno, int stno, int devtyp, int devno, ref int size, short[] data);

      [DllImport(DllName64, EntryPoint = "mdSendEx", CallingConvention = CallingConvention.StdCall)]
      internal static extern int mdSendEx64(int path, int netno, int stno, int devtyp, int devno, ref int size, short[] data);

      [DllImport(DllName32, EntryPoint = "mdSetTimeout", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdSetTimeout32(int path, int timeoutMs);

      [DllImport(DllName64, EntryPoint = "mdSetTimeout", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdSetTimeout64(int path, int timeoutMs);
   }

   public sealed class MelsecApiAdapter : IMelsecApiAdapter
   {
      #region Fields

      private readonly bool _isx64;

      #endregion

      #region Constructors

      public MelsecApiAdapter(bool isx64) => _isx64 = isx64;

      #endregion

      public short mdOpen(short chan, short mode, out int path) => _isx64 ? MelsecApi.mdOpen64(chan, mode, out path) : MelsecApi.mdOpen32(chan, mode, out path);
      public short mdClose(int path) => _isx64 ? MelsecApi.mdClose64(path) : MelsecApi.mdClose32(path);
      public short mdSetTimeout(int path, int timeoutMs) => _isx64 ? MelsecApi.mdSetTimeout64(path, timeoutMs) : MelsecApi.mdSetTimeout32(path, timeoutMs);
      public short mdSend(int path, byte[] buffer, int length) => _isx64 ? MelsecApi.mdSend64(path, buffer, length) : MelsecApi.mdSend32(path, buffer, length);

      public short mdReceive(int path, byte[] buffer, int length, out int received) =>
         _isx64 ? MelsecApi.mdReceive64(path, buffer, length, out received) : MelsecApi.mdReceive32(path, buffer, length, out received);

      public short mdDevRead(int path, int deviceCode, int startAddr, int count, short[] dest) => _isx64
         ? MelsecApi.mdDevRead64(path, deviceCode, startAddr, count, dest)
         : MelsecApi.mdDevRead32(path, deviceCode, startAddr, count, dest);

      public short mdDevWrite(int path, int deviceCode, int startAddr, int count, short[] src) => _isx64
         ? MelsecApi.mdDevWrite64(path, deviceCode, startAddr, count, src)
         : MelsecApi.mdDevWrite32(path, deviceCode, startAddr, count, src);

      public short mdGetStatus(int path, out int statusCode)
      {
         if (_isx64)
         {
            return MelsecApi.mdGetStatus64(path, out statusCode);
         }
         else
         {
            return MelsecApi.mdGetStatus32(path, out statusCode);
         }
      }

      public short mdGetLastError(int path, out int code)
      {
         if (_isx64)
         {
            return MelsecApi.mdGetLastError64(path, out code);
         }
         else
         {
            return MelsecApi.mdGetLastError32(path, out code);
         }
      }

      public int mdSendEx(int path, int netno, int stno, int devtyp, int devno, ref int size, short[] data) => _isx64
         ? MelsecApi.mdSendEx64(path, netno, stno, devtyp, devno, ref size, data)
         : MelsecApi.mdSendEx32(path, netno, stno, devtyp, devno, ref size, data);

      public int mdRandR(int path, int stno, int[] dev, short[] buf, int bufsize) =>
         _isx64 ? MelsecApi.mdRandREx64(path, stno, dev, buf, bufsize) : MelsecApi.mdRandREx32(path, stno, dev, buf, bufsize);

      public int mdRandW(int path, int stno, int[] dev, short[] buf, int bufsize) =>
         _isx64 ? MelsecApi.mdRandWEx64(path, stno, dev, buf, bufsize) : MelsecApi.mdRandWEx32(path, stno, dev, buf, bufsize);

      public short mdDevSet(int path, int stno, int devtyp, int devno) =>
         _isx64 ? MelsecApi.mdDevSet64(path, stno, devtyp, devno) : MelsecApi.mdDevSet32(path, stno, devtyp, devno);

      public short mdDevRst(int path, int stno, int devtyp, int devno) =>
         _isx64 ? MelsecApi.mdDevRst64(path, stno, devtyp, devno) : MelsecApi.mdDevRst32(path, stno, devtyp, devno);
   }
}