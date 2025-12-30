using System.Runtime.InteropServices;

namespace WindowsFormsApp1.CCLink
{
   internal static class MelsecApi
   {
      #region Constant

      private const string DllName = "MDFUNC32.DLL";

      #endregion

      [DllImport(DllName, EntryPoint = "mdClose", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdClose(int path);

      [DllImport(DllName, EntryPoint = "mdDevRead", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdDevRead(int path, int deviceCode, int startAddr, int count, short[] dest);

      [DllImport(DllName, EntryPoint = "mdDevRst", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdDevRst(int path, int stno, int devtyp, int devno);

      [DllImport(DllName, EntryPoint = "mdDevSet", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdDevSet(int path, int stno, int devtyp, int devno);

      [DllImport(DllName, EntryPoint = "mdDevWrite", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdDevWrite(int path, int deviceCode, int startAddr, int count, short[] src);

      [DllImport(DllName, EntryPoint = "mdGetLastError", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdGetLastError(int path, out int code);

      [DllImport(DllName, EntryPoint = "mdGetStatus", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdGetStatus(int path, out int statusCode);

      [DllImport(DllName, EntryPoint = "mdOpen", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdOpen(short chan, short mode, out int path);

      [DllImport(DllName, EntryPoint = "mdRandREx", CallingConvention = CallingConvention.StdCall)]
      internal static extern int mdRandREx(int path, int stno, int[] dev, short[] buf, int bufsize);

      [DllImport(DllName, EntryPoint = "mdRandWEx", CallingConvention = CallingConvention.StdCall)]
      internal static extern int mdRandWEx(int path, int stno, int[] dev, short[] buf, int bufsize);

      [DllImport(DllName, EntryPoint = "mdReceive", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdReceive(int path, byte[] buffer, int length, out int received);

      [DllImport(DllName, EntryPoint = "mdReceiveEx", CallingConvention = CallingConvention.StdCall)]
      internal static extern int mdReceiveEx(int path, int netno, int stno, int devtyp, int devno, ref int size, short[] data);

      [DllImport(DllName, EntryPoint = "mdSend", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdSend(int path, byte[] buffer, int length);

      [DllImport(DllName, EntryPoint = "mdSendEx", CallingConvention = CallingConvention.StdCall)]
      internal static extern int mdSendEx(int path, int netno, int stno, int devtyp, int devno, ref int size, short[] data);

      [DllImport(DllName, EntryPoint = "mdSetTimeout", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdSetTimeout(int path, int timeoutMs);
   }

   public sealed class MelsecApiAdapter : IMelsecApiAdapter
   {
      #region Constructors

      public MelsecApiAdapter()
      {
      }

      #endregion

      public short mdOpen(short chan, short mode, out int path) => MelsecApi.mdOpen(chan, mode, out path);
      public short mdClose(int path) => MelsecApi.mdClose(path);
      public short mdSetTimeout(int path, int timeoutMs) => MelsecApi.mdSetTimeout(path, timeoutMs);
      public short mdSend(int path, byte[] buffer, int length) => MelsecApi.mdSend(path, buffer, length);

      public short mdReceive(int path, byte[] buffer, int length, out int received) =>
         MelsecApi.mdReceive(path, buffer, length, out received);

      public short mdDevRead(int path, int deviceCode, int startAddr, int count, short[] dest) =>
         MelsecApi.mdDevRead(path, deviceCode, startAddr, count, dest);

      public short mdDevWrite(int path, int deviceCode, int startAddr, int count, short[] src) =>
         MelsecApi.mdDevWrite(path, deviceCode, startAddr, count, src);

      public short mdGetStatus(int path, out int statusCode) => MelsecApi.mdGetStatus(path, out statusCode);

      public short mdGetLastError(int path, out int code) => MelsecApi.mdGetLastError(path, out code);

      public int mdSendEx(int path, int netno, int stno, int devtyp, int devno, ref int size, short[] data) =>
         MelsecApi.mdSendEx(path, netno, stno, devtyp, devno, ref size, data);

      public int mdRandR(int path, int stno, int[] dev, short[] buf, int bufsize) =>
         MelsecApi.mdRandREx(path, stno, dev, buf, bufsize);

      public int mdRandW(int path, int stno, int[] dev, short[] buf, int bufsize) =>
         MelsecApi.mdRandWEx(path, stno, dev, buf, bufsize);

      public short mdDevSet(int path, int stno, int devtyp, int devno) =>
         MelsecApi.mdDevSet(path, stno, devtyp, devno);

      public short mdDevRst(int path, int stno, int devtyp, int devno) =>
         MelsecApi.mdDevRst(path, stno, devtyp, devno);
   }
}