namespace WindowsFormsApp1.CCLink
{
   /// <summary>
   /// MELSEC API 介面包裝。簽章依手冊（p.53~55, p.113~117）定義。
   /// </summary>
   public interface IMelsecApiAdapter
   {
      #region Public Methods

      /// <summary>開啟通訊線路</summary>
      /// <param name="chan">通訊線路頻道編號</param>
      /// <param name="mode">虛擬參數</param>
      /// <param name="path">輸出：開啟的路徑指標</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      short mdOpen(short chan, short mode, out int path);

      /// <summary>關閉通訊線路</summary>
      /// <param name="path">路徑指標</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      short mdClose(int path);

      /// <summary>設定通訊逾時時間</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="timeoutMs">逾時時間 (ms)</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      short mdSetTimeout(int path, int timeoutMs);

      /// <summary>傳送資料</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="buffer">傳送緩衝區</param>
      /// <param name="length">傳送長度</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      short mdSend(int path, byte[] buffer, int length);

      /// <summary>接收資料</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="buffer">接收緩衝區</param>
      /// <param name="length">讀取長度</param>
      /// <param name="received">輸出：接收到的實際長度</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      short mdReceive(int path, byte[] buffer, int length, out int received);

      /// <summary>批量讀取裝置</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="deviceCode">裝置名稱代碼</param>
      /// <param name="startAddr">起始裝置號碼</param>
      /// <param name="count">讀取數量</param>
      /// <param name="dest">讀取緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      short mdDevRead(int path, int deviceCode, int startAddr, int count, short[] dest);

      /// <summary>批量寫入裝置</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="deviceCode">裝置名稱代碼</param>
      /// <param name="startAddr">起始裝置號碼</param>
      /// <param name="count">寫入數量</param>
      /// <param name="src">寫入緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      short mdDevWrite(int path, int deviceCode, int startAddr, int count, short[] src);

      /// <summary>讀取通訊狀態</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="statusCode">輸出：狀態代碼</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      short mdGetStatus(int path, out int statusCode);

      /// <summary>獲取最後一個錯誤碼</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="code">輸出：錯誤代碼</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      short mdGetLastError(int path, out int code);

      /// <summary>擴充裝置批量寫入</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netno">網路編號</param>
      /// <param name="stno">站號</param>
      /// <param name="devtyp">裝置代碼</param>
      /// <param name="devno">起始裝置號碼</param>
      /// <param name="size">輸入/輸出：寫入位元組數</param>
      /// <param name="data">寫入資料緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      int mdSendEx(int path, int netno, int stno, int devtyp, int devno, ref int size, short[] data);

      /// <summary>擴充裝置批量讀取</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netno">網路編號</param>
      /// <param name="stno">站號</param>
      /// <param name="devtyp">裝置代號</param>
      /// <param name="devno">起始裝置號碼</param>
      /// <param name="size">輸入/輸出：讀取位元組數</param>
      /// <param name="data">讀取資料緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      int mdReceiveEx(int path, int netno, int stno, int devtyp, int devno, ref int size, short[] data);

      /// <summary>擴充位元裝置設置 (ON)</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netno">網路編號</param>
      /// <param name="stno">站號</param>
      /// <param name="devtyp">裝置代碼</param>
      /// <param name="devno">指定裝置號碼</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      int mdDevSetEx(int path, int netno, int stno, int devtyp, int devno);

      /// <summary>擴充位元裝置復位 (OFF)</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netno">網路編號</param>
      /// <param name="stno">站號</param>
      /// <param name="devtyp">裝置代碼</param>
      /// <param name="devno">指定裝置號碼</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      int mdDevRstEx(int path, int netno, int stno, int devtyp, int devno);

      /// <summary>擴充裝置隨機寫入</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netno">網路編號</param>
      /// <param name="stno">站號</param>
      /// <param name="dev">隨機指定的裝置代碼</param>
      /// <param name="buf">寫入緩衝區</param>
      /// <param name="bufsize">虛擬參數</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      int mdRandWEx(int path, int netno, int stno, int[] dev, short[] buf, int bufsize);

      /// <summary>擴充裝置隨機讀取</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netno">網路編號</param>
      /// <param name="stno">站號</param>
      /// <param name="dev">隨機指定的裝置代碼</param>
      /// <param name="buf">讀取緩衝區</param>
      /// <param name="bufsize">讀取資料的位元組數</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      int mdRandREx(int path, int netno, int stno, int[] dev, short[] buf, int bufsize);

      /// <summary>寫入資料至遠端裝置站的緩衝記憶體</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netno">網路編號</param>
      /// <param name="stno">站號</param>
      /// <param name="offset">位移量</param>
      /// <param name="size">輸入/輸出：寫入位元組數</param>
      /// <param name="data">寫入資料緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      int mdRemBufWriteEx(int path, int netno, int stno, int offset, ref int size, short[] data);

      /// <summary>從遠端裝置站的緩衝記憶體讀取資料</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netno">網路編號</param>
      /// <param name="stno">站號</param>
      /// <param name="offset">位移量</param>
      /// <param name="size">輸入/輸出：讀取位元組數</param>
      /// <param name="data">讀取資料緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      int mdRemBufReadEx(int path, int netno, int stno, int offset, ref int size, short[] data);

      /// <summary>寫入資料至指定 IP 位址的遠端裝置站緩衝記憶體</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="ipaddress">IP 地址</param>
      /// <param name="offset">位移量</param>
      /// <param name="size">輸入/輸出：寫入位元組數</param>
      /// <param name="data">寫入資料緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      int mdRemBufWriteIPEx(int path, int ipaddress, int offset, ref int size, short[] data);

      /// <summary>從指定 IP 位址的遠端裝置站緩衝記憶體讀取資料</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="ipaddress">IP 地址</param>
      /// <param name="offset">位移量</param>
      /// <param name="size">輸入/輸出：讀取位元組數</param>
      /// <param name="data">讀取資料緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      int mdRemBufReadIPEx(int path, int ipaddress, int offset, ref int size, short[] data);

      /// <summary>讀取 CPU 型號名稱</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="stno">站號</param>
      /// <param name="buf">輸出：型號代碼</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      short mdTypeRead(int path, short stno, out short buf);

      /// <summary>遠端控制 RUN / STOP / PAUSE</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="stno">站號</param>
      /// <param name="buf">控制代碼 (1:RUN, 2:STOP, 3:PAUSE)</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      short mdControl(int path, short stno, short buf);

      /// <summary>重置介面卡</summary>
      /// <param name="path">路徑指標</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      short mdBdRst(int path);

      /// <summary>設定介面卡模式</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="mode">模式代碼</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      short mdBdModSet(int path, short mode);

      /// <summary>讀取介面卡模式</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="mode">輸出：模式代碼</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      short mdBdModRead(int path, out short mode);

      /// <summary>讀取介面卡的 LED 資訊</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="buf">讀取緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      short mdBdLedRead(int path, short[] buf);

      /// <summary>讀取介面卡的開關狀態</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="buf">讀取緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      short mdBdSwRead(int path, short[] buf);

      /// <summary>讀取介面卡的版本資訊</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="buf">讀取緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      short mdBdVerRead(int path, short[] buf);

      /// <summary>初始化可程式控制器資訊表</summary>
      /// <param name="path">路徑指標</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      short mdInit(int path);

      /// <summary>擴充裝置隨機讀取</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netno">網路編號</param>
      /// <param name="stno">站號</param>
      /// <param name="dev">隨機指定的裝置代碼</param>
      /// <param name="buf">讀取緩衝區</param>
      /// <param name="bufsize">讀取資料的位元組數</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      int mdRandR(int path, int netno, int stno, int[] dev, short[] buf, int bufsize);

      /// <summary>位元裝置設置 (ON)</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="stno">站號</param>
      /// <param name="devtyp">裝置代碼</param>
      /// <param name="devno">裝置號碼</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      short mdDevSet(int path, int stno, int devtyp, int devno);

      /// <summary>位元裝置復位 (OFF)</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="stno">站號</param>
      /// <param name="devtyp">裝置代碼</param>
      /// <param name="devno">裝置號碼</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      short mdDevRst(int path, int stno, int devtyp, int devno);

      /// <summary>擴充裝置隨機寫入</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netno">網路編號</param>
      /// <param name="stno">站號</param>
      /// <param name="dev">隨機指定的裝置代碼</param>
      /// <param name="buf">寫入緩衝區</param>
      /// <param name="bufsize">虛擬參數</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      int mdRandW(int path, int netno, int stno, int[] dev, short[] buf, int bufsize);

      #endregion
   }
}
