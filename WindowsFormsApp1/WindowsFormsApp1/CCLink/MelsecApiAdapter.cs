using System.Runtime.InteropServices;

namespace WindowsFormsApp1.CCLink
{
   internal static class MelsecApi
   {
      #region Constant

      private const string DllName = "MDFUNC32.DLL";

      #endregion

      /// <summary>讀取介面卡的 LED 資訊</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="buf">讀取緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      [DllImport(DllName, EntryPoint = "mdBdLedRead", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdBdLedRead(int path, short[] buf);

      /// <summary>讀取介面卡模式</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="mode">輸出：模式代碼</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      [DllImport(DllName, EntryPoint = "mdBdModRead", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdBdModRead(int path, out short mode);

      /// <summary>設定介面卡模式</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="mode">模式代碼</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      [DllImport(DllName, EntryPoint = "mdBdModSet", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdBdModSet(int path, short mode);

      /// <summary>重置介面卡</summary>
      /// <param name="path">路徑指標</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      [DllImport(DllName, EntryPoint = "mdBdRst", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdBdRst(int path);

      /// <summary>讀取介面卡的開關狀態</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="buf">讀取緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      [DllImport(DllName, EntryPoint = "mdBdSwRead", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdBdSwRead(int path, short[] buf);

      /// <summary>讀取介面卡的版本資訊</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="buf">讀取緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      [DllImport(DllName, EntryPoint = "mdBdVerRead", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdBdVerRead(int path, short[] buf);

      /// <summary>關閉通訊線路</summary>
      /// <param name="path">路徑指標</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      [DllImport(DllName, EntryPoint = "mdClose", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdClose(int path);

      /// <summary>遠端控制 RUN / STOP / PAUSE</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="stno">站號</param>
      /// <param name="buf">控制代碼 (1:RUN, 2:STOP, 3:PAUSE)</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      [DllImport(DllName, EntryPoint = "mdControl", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdControl(int path, short stno, short buf);

      /// <summary>批量讀取裝置</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="deviceCode">裝置名稱代碼</param>
      /// <param name="startAddr">起始裝置號碼</param>
      /// <param name="count">讀取數量</param>
      /// <param name="dest">讀取緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      [DllImport(DllName, EntryPoint = "mdDevRead", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdDevRead(int path, int deviceCode, int startAddr, int count, short[] dest);

      /// <summary>位元裝置復位 (OFF)</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="stno">站號</param>
      /// <param name="devtyp">裝置代碼</param>
      /// <param name="devno">裝置號碼</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      [DllImport(DllName, EntryPoint = "mdDevRst", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdDevRst(int path, int stno, int devtyp, int devno);

      /// <summary>擴充位元裝置復位 (OFF)</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netno">網路編號</param>
      /// <param name="stno">站號</param>
      /// <param name="devtyp">裝置代碼</param>
      /// <param name="devno">指定裝置號碼</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      [DllImport(DllName, EntryPoint = "mddevrstex", CallingConvention = CallingConvention.StdCall)]
      internal static extern int mdDevRstEx(int path, int netno, int stno, int devtyp, int devno);

      /// <summary>位元裝置設置 (ON)</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="stno">站號</param>
      /// <param name="devtyp">裝置代碼</param>
      /// <param name="devno">裝置號碼</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      [DllImport(DllName, EntryPoint = "mdDevSet", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdDevSet(int path, int stno, int devtyp, int devno);

      /// <summary>擴充位元裝置設置 (ON)</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netno">網路編號</param>
      /// <param name="stno">站號</param>
      /// <param name="devtyp">裝置代碼</param>
      /// <param name="devno">指定裝置號碼</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      [DllImport(DllName, EntryPoint = "mddevsetex", CallingConvention = CallingConvention.StdCall)]
      internal static extern int mdDevSetEx(int path, int netno, int stno, int devtyp, int devno);

      /// <summary>批量寫入裝置</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="deviceCode">裝置名稱代碼</param>
      /// <param name="startAddr">起始裝置號碼</param>
      /// <param name="count">寫入數量</param>
      /// <param name="src">寫入緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      [DllImport(DllName, EntryPoint = "mdDevWrite", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdDevWrite(int path, int deviceCode, int startAddr, int count, short[] src);

      /// <summary>獲取最後一個錯誤碼</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="code">輸出：錯誤代碼</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      [DllImport(DllName, EntryPoint = "mdGetLastError", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdGetLastError(int path, out int code);

      /// <summary>讀取通訊狀態</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="statusCode">輸出：狀態代碼</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      [DllImport(DllName, EntryPoint = "mdGetStatus", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdGetStatus(int path, out int statusCode);

      /// <summary>初始化可程式控制器資訊表</summary>
      /// <param name="path">路徑指標</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      [DllImport(DllName, EntryPoint = "mdInit", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdInit(int path);

      /// <summary>開啟通訊線路</summary>
      /// <param name="chan">通訊線路頻道編號</param>
      /// <param name="mode">虛擬參數</param>
      /// <param name="path">輸出：開啟的路徑指標</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      [DllImport(DllName, EntryPoint = "mdOpen", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdOpen(short chan, short mode, out int path);

      /// <summary>擴充裝置隨機讀取</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netno">網路編號</param>
      /// <param name="stno">站號</param>
      /// <param name="dev">隨機指定的裝置代碼</param>
      /// <param name="buf">讀取緩衝區</param>
      /// <param name="bufsize">讀取資料的位元組數</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      [DllImport(DllName, EntryPoint = "mdrandrex", CallingConvention = CallingConvention.StdCall)]
      internal static extern int mdRandREx(int path, int netno, int stno, int[] dev, short[] buf, int bufsize);

      /// <summary>擴充裝置隨機寫入</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netno">網路編號</param>
      /// <param name="stno">站號</param>
      /// <param name="dev">隨機指定的裝置代碼</param>
      /// <param name="buf">寫入緩衝區</param>
      /// <param name="bufsize">虛擬參數</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      [DllImport(DllName, EntryPoint = "mdRandWEx", CallingConvention = CallingConvention.StdCall)]
      internal static extern int mdRandWEx(int path, int netno, int stno, int[] dev, short[] buf, int bufsize);

      /// <summary>接收資料</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="buffer">接收緩衝區</param>
      /// <param name="length">讀取長度</param>
      /// <param name="received">輸出：接收到的實際長度</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      [DllImport(DllName, EntryPoint = "mdReceive", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdReceive(int path, byte[] buffer, int length, out int received);

      /// <summary>擴充裝置批量讀取</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netno">網路編號</param>
      /// <param name="stno">站號</param>
      /// <param name="devtyp">裝置代號</param>
      /// <param name="devno">起始裝置號碼</param>
      /// <param name="size">輸入/輸出：讀取位元組數</param>
      /// <param name="data">讀取資料緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      [DllImport(DllName, EntryPoint = "mdReceiveEx", CallingConvention = CallingConvention.StdCall)]
      internal static extern int mdReceiveEx(int path, int netno, int stno, int devtyp, int devno, ref int size, short[] data);

      /// <summary>從遠端裝置站的緩衝記憶體讀取資料</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netno">網路編號</param>
      /// <param name="stno">站號</param>
      /// <param name="offset">位移量</param>
      /// <param name="size">輸入/輸出：讀取位元組數</param>
      /// <param name="data">讀取資料緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      [DllImport(DllName, EntryPoint = "mdrembufreadex", CallingConvention = CallingConvention.StdCall)]
      internal static extern int mdRemBufReadEx(int path, int netno, int stno, int offset, ref int size, short[] data);

      /// <summary>從指定 IP 位址的遠端裝置站緩衝記憶體讀取資料</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="ipaddress">IP 地址</param>
      /// <param name="offset">位移量</param>
      /// <param name="size">輸入/輸出：讀取位元組數</param>
      /// <param name="data">讀取資料緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      [DllImport(DllName, EntryPoint = "mdrembufreadipex", CallingConvention = CallingConvention.StdCall)]
      internal static extern int mdRemBufReadIPEx(int path, int ipaddress, int offset, ref int size, short[] data);

      /// <summary>寫入資料至遠端裝置站的緩衝記憶體</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netno">網路編號</param>
      /// <param name="stno">站號</param>
      /// <param name="offset">位移量</param>
      /// <param name="size">輸入/輸出：寫入位元組數</param>
      /// <param name="data">寫入資料緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      [DllImport(DllName, EntryPoint = "mdrembufwriteex", CallingConvention = CallingConvention.StdCall)]
      internal static extern int mdRemBufWriteEx(int path, int netno, int stno, int offset, ref int size, short[] data);

      /// <summary>寫入資料至指定 IP 位址的遠端裝置站緩衝記憶體</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="ipaddress">IP 地址</param>
      /// <param name="offset">位移量</param>
      /// <param name="size">輸入/輸出：寫入位元組數</param>
      /// <param name="data">寫入資料緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      [DllImport(DllName, EntryPoint = "mdrembufwriteipex", CallingConvention = CallingConvention.StdCall)]
      internal static extern int mdRemBufWriteIPEx(int path, int ipaddress, int offset, ref int size, short[] data);

      /// <summary>傳送資料</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="buffer">傳送緩衝區</param>
      /// <param name="length">傳送長度</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      [DllImport(DllName, EntryPoint = "mdSend", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdSend(int path, byte[] buffer, int length);

      /// <summary>擴充裝置批量寫入</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netno">網路編號</param>
      /// <param name="stno">站號</param>
      /// <param name="devtyp">裝置代碼</param>
      /// <param name="devno">起始裝置號碼</param>
      /// <param name="size">輸入/輸出：寫入位元組數</param>
      /// <param name="data">寫入資料緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      [DllImport(DllName, EntryPoint = "mdSendEx", CallingConvention = CallingConvention.StdCall)]
      internal static extern int mdSendEx(int path, int netno, int stno, int devtyp, int devno, ref int size, short[] data);

      /// <summary>設定通訊逾時時間</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="timeoutMs">逾時時間 (ms)</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      [DllImport(DllName, EntryPoint = "mdSetTimeout", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdSetTimeout(int path, int timeoutMs);

      /// <summary>讀取 CPU 型號名稱</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="stno">站號</param>
      /// <param name="buf">輸出：型號代碼</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      [DllImport(DllName, EntryPoint = "mdTypeRead", CallingConvention = CallingConvention.StdCall)]
      internal static extern short mdTypeRead(int path, short stno, out short buf);
   }

   public sealed class MelsecApiAdapter : IMelsecApiAdapter
   {
      #region Constructors

      public MelsecApiAdapter()
      {
      }

      #endregion

      /// <summary>開啟通訊線路</summary>
      /// <param name="chan">通訊線路頻道編號</param>
      /// <param name="mode">虛擬參數</param>
      /// <param name="path">輸出：開啟的路徑指標</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public short mdOpen(short chan, short mode, out int path) => MelsecApi.mdOpen(chan, mode, out path);

      /// <summary>關閉通訊線路</summary>
      /// <param name="path">路徑指標</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public short mdClose(int path) => MelsecApi.mdClose(path);

      /// <summary>設定通訊逾時時間</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="timeoutMs">逾時時間 (ms)</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public short mdSetTimeout(int path, int timeoutMs) => MelsecApi.mdSetTimeout(path, timeoutMs);

      /// <summary>傳送資料</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="buffer">傳送緩衝區</param>
      /// <param name="length">傳送長度</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public short mdSend(int path, byte[] buffer, int length) => MelsecApi.mdSend(path, buffer, length);

      /// <summary>接收資料</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="buffer">接收緩衝區</param>
      /// <param name="length">讀取長度</param>
      /// <param name="received">輸出：接收到的實際長度</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public short mdReceive(int path, byte[] buffer, int length, out int received) => MelsecApi.mdReceive(path, buffer, length, out received);

      /// <summary>批量讀取裝置</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="deviceCode">裝置名稱代碼</param>
      /// <param name="startAddr">起始裝置號碼</param>
      /// <param name="count">讀取數量</param>
      /// <param name="dest">讀取緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public short mdDevRead(int path, int deviceCode, int startAddr, int count, short[] dest) => MelsecApi.mdDevRead(path, deviceCode, startAddr, count, dest);

      /// <summary>批量寫入裝置</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="deviceCode">裝置名稱代碼</param>
      /// <param name="startAddr">起始裝置號碼</param>
      /// <param name="count">寫入數量</param>
      /// <param name="src">寫入緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public short mdDevWrite(int path, int deviceCode, int startAddr, int count, short[] src) => MelsecApi.mdDevWrite(path, deviceCode, startAddr, count, src);

      /// <summary>讀取通訊狀態</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="statusCode">輸出：狀態代碼</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public short mdGetStatus(int path, out int statusCode) => MelsecApi.mdGetStatus(path, out statusCode);

      /// <summary>獲取最後一個錯誤碼</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="code">輸出：錯誤代碼</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public short mdGetLastError(int path, out int code) => MelsecApi.mdGetLastError(path, out code);

      /// <summary>擴充裝置批量寫入</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netno">網路編號</param>
      /// <param name="stno">站號</param>
      /// <param name="devtyp">裝置代碼</param>
      /// <param name="devno">起始裝置號碼</param>
      /// <param name="size">輸入/輸出：寫入位元組數</param>
      /// <param name="data">寫入資料緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public int mdSendEx(int path, int netno, int stno, int devtyp, int devno, ref int size, short[] data) =>
         MelsecApi.mdSendEx(path, netno, stno, devtyp, devno, ref size, data);

      /// <summary>擴充裝置批量讀取</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netno">網路編號</param>
      /// <param name="stno">站號</param>
      /// <param name="devtyp">裝置代號</param>
      /// <param name="devno">起始裝置號碼</param>
      /// <param name="size">輸入/輸出：讀取位元組數</param>
      /// <param name="data">讀取資料緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public int mdReceiveEx(int path, int netno, int stno, int devtyp, int devno, ref int size, short[] data) =>
         MelsecApi.mdReceiveEx(path, netno, stno, devtyp, devno, ref size, data);

      /// <summary>擴充位元裝置設置 (ON)</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netno">網路編號</param>
      /// <param name="stno">站號</param>
      /// <param name="devtyp">裝置代碼</param>
      /// <param name="devno">指定裝置號碼</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public int mdDevSetEx(int path, int netno, int stno, int devtyp, int devno) => MelsecApi.mdDevSetEx(path, netno, stno, devtyp, devno);

      /// <summary>擴充位元裝置復位 (OFF)</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netno">網路編號</param>
      /// <param name="stno">站號</param>
      /// <param name="devtyp">裝置代碼</param>
      /// <param name="devno">指定裝置號碼</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public int mdDevRstEx(int path, int netno, int stno, int devtyp, int devno) => MelsecApi.mdDevRstEx(path, netno, stno, devtyp, devno);

      /// <summary>擴充裝置隨機寫入</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netno">網路編號</param>
      /// <param name="stno">站號</param>
      /// <param name="dev">隨機指定的裝置代碼</param>
      /// <param name="buf">寫入緩衝區</param>
      /// <param name="bufsize">虛擬參數</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public int mdRandWEx(int path, int netno, int stno, int[] dev, short[] buf, int bufsize) => MelsecApi.mdRandWEx(path, netno, stno, dev, buf, bufsize);

      /// <summary>擴充裝置隨機讀取</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netno">網路編號</param>
      /// <param name="stno">站號</param>
      /// <param name="dev">隨機指定的裝置代碼</param>
      /// <param name="buf">讀取緩衝區</param>
      /// <param name="bufsize">讀取資料的位元組數</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public int mdRandREx(int path, int netno, int stno, int[] dev, short[] buf, int bufsize) => MelsecApi.mdRandREx(path, netno, stno, dev, buf, bufsize);

      /// <summary>寫入資料至遠端裝置站的緩衝記憶體</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netno">網路編號</param>
      /// <param name="stno">站號</param>
      /// <param name="offset">位移量</param>
      /// <param name="size">輸入/輸出：寫入位元組數</param>
      /// <param name="data">寫入資料緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public int mdRemBufWriteEx(int path, int netno, int stno, int offset, ref int size, short[] data) =>
         MelsecApi.mdRemBufWriteEx(path, netno, stno, offset, ref size, data);

      /// <summary>從遠端裝置站的緩衝記憶體讀取資料</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netno">網路編號</param>
      /// <param name="stno">站號</param>
      /// <param name="offset">位移量</param>
      /// <param name="size">輸入/輸出：讀取位元組數</param>
      /// <param name="data">讀取資料緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public int mdRemBufReadEx(int path, int netno, int stno, int offset, ref int size, short[] data) =>
         MelsecApi.mdRemBufReadEx(path, netno, stno, offset, ref size, data);

      /// <summary>寫入資料至指定 IP 位址的遠端裝置站緩衝記憶體</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="ipaddress">IP 地址</param>
      /// <param name="offset">位移量</param>
      /// <param name="size">輸入/輸出：寫入位元組數</param>
      /// <param name="data">寫入資料緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public int mdRemBufWriteIPEx(int path, int ipaddress, int offset, ref int size, short[] data) =>
         MelsecApi.mdRemBufWriteIPEx(path, ipaddress, offset, ref size, data);

      /// <summary>從指定 IP 位址的遠端裝置站緩衝記憶體讀取資料</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="ipaddress">IP 地址</param>
      /// <param name="offset">位移量</param>
      /// <param name="size">輸入/輸出：讀取位元組數</param>
      /// <param name="data">讀取資料緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public int mdRemBufReadIPEx(int path, int ipaddress, int offset, ref int size, short[] data) =>
         MelsecApi.mdRemBufReadIPEx(path, ipaddress, offset, ref size, data);

      /// <summary>讀取 CPU 型號名稱</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="stno">站號</param>
      /// <param name="buf">輸出：型號代碼</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public short mdTypeRead(int path, short stno, out short buf) => MelsecApi.mdTypeRead(path, stno, out buf);

      /// <summary>遠端控制 RUN / STOP / PAUSE</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="stno">站號</param>
      /// <param name="buf">控制代碼 (1:RUN, 2:STOP, 3:PAUSE)</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public short mdControl(int path, short stno, short buf) => MelsecApi.mdControl(path, stno, buf);

      /// <summary>重置介面卡</summary>
      /// <param name="path">路徑指標</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public short mdBdRst(int path) => MelsecApi.mdBdRst(path);

      /// <summary>設定介面卡模式</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="mode">模式代碼</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public short mdBdModSet(int path, short mode) => MelsecApi.mdBdModSet(path, mode);

      /// <summary>讀取介面卡模式</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="mode">輸出：模式代碼</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public short mdBdModRead(int path, out short mode) => MelsecApi.mdBdModRead(path, out mode);

      /// <summary>讀取介面卡的 LED 資訊</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="buf">讀取緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public short mdBdLedRead(int path, short[] buf) => MelsecApi.mdBdLedRead(path, buf);

      /// <summary>讀取介面卡的開關狀態</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="buf">讀取緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public short mdBdSwRead(int path, short[] buf) => MelsecApi.mdBdSwRead(path, buf);

      /// <summary>讀取介面卡的版本資訊</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="buf">讀取緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public short mdBdVerRead(int path, short[] buf) => MelsecApi.mdBdVerRead(path, buf);

      /// <summary>初始化可程式控制器資訊表</summary>
      /// <param name="path">路徑指標</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public short mdInit(int path) => MelsecApi.mdInit(path);

      /// <summary>擴充裝置隨機讀取</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netno">網路編號</param>
      /// <param name="stno">站號</param>
      /// <param name="dev">隨機指定的裝置代碼</param>
      /// <param name="buf">讀取緩衝區</param>
      /// <param name="bufsize">讀取資料的位元組數</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public int mdRandR(int path, int netno, int stno, int[] dev, short[] buf, int bufsize) => MelsecApi.mdRandREx(path, netno, stno, dev, buf, bufsize);

      /// <summary>擴充裝置隨機寫入</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netno">網路編號</param>
      /// <param name="stno">站號</param>
      /// <param name="dev">隨機指定的裝置代碼</param>
      /// <param name="buf">寫入緩衝區</param>
      /// <param name="bufsize">虛擬參數</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public int mdRandW(int path, int netno, int stno, int[] dev, short[] buf, int bufsize) => MelsecApi.mdRandWEx(path, netno, stno, dev, buf, bufsize);

      /// <summary>位元裝置設置 (ON)</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="stno">站號</param>
      /// <param name="devtyp">裝置代碼</param>
      /// <param name="devno">裝置號碼</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public short mdDevSet(int path, int stno, int devtyp, int devno) => MelsecApi.mdDevSet(path, stno, devtyp, devno);

      /// <summary>位元裝置復位 (OFF)</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="stno">站號</param>
      /// <param name="devtyp">裝置代碼</param>
      /// <param name="devno">裝置號碼</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public short mdDevRst(int path, int stno, int devtyp, int devno) => MelsecApi.mdDevRst(path, stno, devtyp, devno);
   }
}