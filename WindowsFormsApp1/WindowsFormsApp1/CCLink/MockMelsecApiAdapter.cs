using System.Collections.Generic;
using System.Threading.Tasks;

namespace WindowsFormsApp1.CCLink
{
   /// <summary>
   /// Simple in-memory mock of IMelsecApiAdapter for local simulation/testing.
   /// Stores LB bits and LW words in dictionaries and simulates mdOpen/mdClose.
   /// Supports auto-respond mapping: when a request LB is set, the mock can automatically set a response LB.
   /// </summary>
   public sealed class MockMelsecApiAdapter : IMelsecApiAdapter
   {
      #region Fields

      // 當收到某個 request bit 被 set 時，自動將它對應的 response bit 設為 1
      private readonly Dictionary<int, int> _autoRespondMap = new Dictionary<int, int>();
      private readonly Dictionary<int, short> _bits = new Dictionary<int, short>();
      private readonly object _lock = new object();
      private readonly Dictionary<int, short> _words = new Dictionary<int, short>();
      private bool _open;

      #endregion

      #region Constructors

      public MockMelsecApiAdapter()
      {
         // 預設：當 LB0300 被設置時，回應 LB0100
         _autoRespondMap[CCLinkConstants.DefaultRequestFlagAddress] = CCLinkConstants.DefaultResponseFlagAddress;
      }

      #endregion

      /// <summary>開啟通訊線路</summary>
      /// <param name="chan">通訊線路頻道編號</param>
      /// <param name="mode">虛擬參數</param>
      /// <param name="path">輸出：開啟的路徑指標</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public short mdOpen(short chan, short mode, out int path)
      {
         lock (_lock)
         {
            _open = true;
            path = 1;
            return 0;
         }
      }

      /// <summary>關閉通訊線路</summary>
      /// <param name="path">路徑指標</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public short mdClose(int path)
      {
         lock (_lock)
         {
            _open = false;
            return 0;
         }
      }

      /// <summary>設定通訊逾時時間</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="timeoutMs">逾時時間 (ms)</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public short mdSetTimeout(int path, int timeoutMs) => 0;

      /// <summary>傳送資料</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="buffer">傳送緩衝區</param>
      /// <param name="length">傳送長度</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public short mdSend(int path, byte[] buffer, int length) => 0;

      /// <summary>接收資料</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="buffer">接收緩衝區</param>
      /// <param name="length">讀取長度</param>
      /// <param name="received">輸出：接收到的實際長度</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public short mdReceive(int path, byte[] buffer, int length, out int received)
      {
         received = 0;
         return 0;
      }

      /// <summary>批量讀取裝置</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="deviceCode">裝置名稱代碼</param>
      /// <param name="startAddr">起始裝置號碼</param>
      /// <param name="count">讀取數量</param>
      /// <param name="dest">讀取緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public short mdDevRead(int path, int deviceCode, int startAddr, int count, short[] dest)
      {
         lock (_lock)
         {
            for (int i = 0; i < count; i++)
            {
               if (deviceCode == CCLinkConstants.DEV_LB) // LB bits
               {
                  var key = startAddr + i;
                  dest[i] = _bits.ContainsKey(key) ? _bits[key] : (short)0;
               }
               else // LW words or others
               {
                  var key = startAddr + i;
                  dest[i] = _words.ContainsKey(key) ? _words[key] : (short)0;
               }
            }

            return 0;
         }
      }

      /// <summary>批量寫入裝置</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="deviceCode">裝置名稱代碼</param>
      /// <param name="startAddr">起始裝置號碼</param>
      /// <param name="count">寫入數量</param>
      /// <param name="src">寫入緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public short mdDevWrite(int path, int deviceCode, int startAddr, int count, short[] src)
      {
         lock (_lock)
         {
            for (int i = 0; i < count; i++)
            {
               if (deviceCode == CCLinkConstants.DEV_LB) // LB bits (store 0/1 in short)
               {
                  _bits[startAddr + i] = src[i];
               }
               else // LW words
               {
                  _words[startAddr + i] = src[i];
               }
            }

            return 0;
         }
      }

      /// <summary>讀取通訊狀態</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="statusCode">輸出：狀態代碼</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public short mdGetStatus(int path, out int statusCode)
      {
         statusCode = _open ? 0 : -1;
         return 0;
      }

      /// <summary>獲取最後一個錯誤碼</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="code">輸出：錯誤代碼</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public short mdGetLastError(int path, out int code)
      {
         code = 0;
         return 0;
      }

      /// <summary>擴充裝置批量寫入</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netno">網路編號</param>
      /// <param name="stno">站號</param>
      /// <param name="devtyp">裝置代碼</param>
      /// <param name="devno">起始裝置號碼</param>
      /// <param name="size">輸入/輸出：寫入位元組數</param>
      /// <param name="data">寫入資料緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public int mdSendEx(int path, int netno, int stno, int devtyp, int devno, ref int size, short[] data) => 0;

      /// <summary>擴充裝置批量讀取</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netno">網路編號</param>
      /// <param name="stno">站號</param>
      /// <param name="devtyp">裝置代號</param>
      /// <param name="devno">起始裝置號碼</param>
      /// <param name="size">輸入/輸出：讀取位元組數</param>
      /// <param name="data">讀取資料緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public int mdReceiveEx(int path, int netno, int stno, int devtyp, int devno, ref int size, short[] data) => 0;

      /// <summary>擴充位元裝置設置 (ON)</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netno">網路編號</param>
      /// <param name="stno">站號</param>
      /// <param name="devtyp">裝置代碼</param>
      /// <param name="devno">指定裝置號碼</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public int mdDevSetEx(int path, int netno, int stno, int devtyp, int devno) => 0;

      /// <summary>擴充位元裝置復位 (OFF)</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netno">網路編號</param>
      /// <param name="stno">站號</param>
      /// <param name="devtyp">裝置代碼</param>
      /// <param name="devno">指定裝置號碼</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public int mdDevRstEx(int path, int netno, int stno, int devtyp, int devno) => 0;

      /// <summary>擴充裝置隨機寫入</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netno">網路編號</param>
      /// <param name="stno">站號</param>
      /// <param name="dev">隨機指定的裝置代碼</param>
      /// <param name="buf">寫入緩衝區</param>
      /// <param name="bufsize">虛擬參數</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public int mdRandWEx(int path, int netno, int stno, int[] dev, short[] buf, int bufsize) => 0;

      /// <summary>擴充裝置隨機讀取</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netno">網路編號</param>
      /// <param name="stno">站號</param>
      /// <param name="dev">隨機指定的裝置代碼</param>
      /// <param name="buf">讀取緩衝區</param>
      /// <param name="bufsize">讀取資料的位元組數</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public int mdRandREx(int path, int netno, int stno, int[] dev, short[] buf, int bufsize) => 0;

      /// <summary>寫入資料至遠端裝置站的緩衝記憶體</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netno">網路編號</param>
      /// <param name="stno">站號</param>
      /// <param name="offset">位移量</param>
      /// <param name="size">輸入/輸出：寫入位元組數</param>
      /// <param name="data">寫入資料緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public int mdRemBufWriteEx(int path, int netno, int stno, int offset, ref int size, short[] data) => 0;

      /// <summary>從遠端裝置站的緩衝記憶體讀取資料</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netno">網路編號</param>
      /// <param name="stno">站號</param>
      /// <param name="offset">位移量</param>
      /// <param name="size">輸入/輸出：讀取位元組數</param>
      /// <param name="data">讀取資料緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public int mdRemBufReadEx(int path, int netno, int stno, int offset, ref int size, short[] data) => 0;

      /// <summary>寫入資料至指定 IP 位址的遠端裝置站緩衝記憶體</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="ipaddress">IP 地址</param>
      /// <param name="offset">位移量</param>
      /// <param name="size">輸入/輸出：寫入位元組數</param>
      /// <param name="data">寫入資料緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public int mdRemBufWriteIPEx(int path, int ipaddress, int offset, ref int size, short[] data) => 0;

      /// <summary>從指定 IP 位址的遠端裝置站緩衝記憶體讀取資料</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="ipaddress">IP 地址</param>
      /// <param name="offset">位移量</param>
      /// <param name="size">輸入/輸出：讀取位元組數</param>
      /// <param name="data">讀取資料緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public int mdRemBufReadIPEx(int path, int ipaddress, int offset, ref int size, short[] data) => 0;

      /// <summary>讀取 CPU 型號名稱</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="stno">站號</param>
      /// <param name="buf">輸出：型號代碼</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public short mdTypeRead(int path, short stno, out short buf) { buf = 0; return 0; }

      /// <summary>遠端控制 RUN / STOP / PAUSE</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="stno">站號</param>
      /// <param name="buf">控制代碼 (1:RUN, 2:STOP, 3:PAUSE)</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public short mdControl(int path, short stno, short buf) => 0;

      /// <summary>重置介面卡</summary>
      /// <param name="path">路徑指標</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public short mdBdRst(int path) => 0;

      /// <summary>設定介面卡模式</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="mode">模式代碼</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public short mdBdModSet(int path, short mode) => 0;

      /// <summary>讀取介面卡模式</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="mode">輸出：模式代碼</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public short mdBdModRead(int path, out short mode) { mode = 0; return 0; }

      /// <summary>讀取介面卡的 LED 資訊</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="buf">讀取緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public short mdBdLedRead(int path, short[] buf) => 0;

      /// <summary>讀取介面卡的開關狀態</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="buf">讀取緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public short mdBdSwRead(int path, short[] buf) => 0;

      /// <summary>讀取介面卡的版本資訊</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="buf">讀取緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public short mdBdVerRead(int path, short[] buf) => 0;

      /// <summary>初始化可程式控制器資訊表</summary>
      /// <param name="path">路徑指標</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public short mdInit(int path) => 0;

      /// <summary>擴充裝置隨機讀取</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netno">網路編號</param>
      /// <param name="stno">站號</param>
      /// <param name="dev">隨機指定的裝置代碼</param>
      /// <param name="buf">讀取緩衝區</param>
      /// <param name="bufsize">讀取資料的位元組數</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public int mdRandR(int path, int netno, int stno, int[] dev, short[] buf, int bufsize) => 0;

      /// <summary>擴充裝置隨機寫入</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netno">網路編號</param>
      /// <param name="stno">站號</param>
      /// <param name="dev">隨機指定的裝置代碼</param>
      /// <param name="buf">寫入緩衝區</param>
      /// <param name="bufsize">虛擬參數</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public int mdRandW(int path, int netno, int stno, int[] dev, short[] buf, int bufsize) => 0;

      /// <summary>位元裝置設置 (ON)</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="stno">站號</param>
      /// <param name="devtyp">裝置代碼</param>
      /// <param name="devno">裝置號碼</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public short mdDevSet(int path, int stno, int devtyp, int devno)
      {
         // set LB bit to 1 and trigger auto-respond if configured
         lock (_lock)
         {
            _bits[devno] = 1;
            if (devtyp == CCLinkConstants.DEV_LB && _autoRespondMap.TryGetValue(devno, out var resp))
            {
               // set response immediately
               _bits[resp] = 1;

               // schedule clearing response after short delay to simulate device behavior
               Task.Run(async () =>
               {
                  await Task.Delay(500).ConfigureAwait(false);
                  lock (_lock)
                  {
                     _bits[resp] = 0;
                  }
               });
            }

            return 0;
         }
      }

      /// <summary>位元裝置復位 (OFF)</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="stno">站號</param>
      /// <param name="devtyp">裝置代碼</param>
      /// <param name="devno">裝置號碼</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      public short mdDevRst(int path, int stno, int devtyp, int devno)
      {
         lock (_lock)
         {
            _bits[devno] = 0;
            // 若 request cleared，則清除對應 response（模擬行為）
            if (devtyp == CCLinkConstants.DEV_LB && _autoRespondMap.TryGetValue(devno, out var resp))
            {
               _bits[resp] = 0;
            }

            return 0;
         }
      }
   }
}
