namespace WindowsFormsApp1.CCLink.Interfaces
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
      short Open(short chan, short mode, out int path);

      /// <summary>關閉通訊線路</summary>
      /// <param name="path">路徑指標</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      short Close(int path);

      /// <summary>擴充裝置批量寫入</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netNo">網路編號</param>
      /// <param name="stNo">站號</param>
      /// <param name="devType">裝置代碼</param>
      /// <param name="devNo">起始裝置號碼</param>
      /// <param name="size">輸入/輸出：寫入位元組數</param>
      /// <param name="data">寫入資料緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      int SendEx(int path, int netNo, int stNo, int devType, int devNo, ref int size, short[] data);

      /// <summary>擴充裝置批量讀取</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netNo"></param>
      /// <param name="stNo"></param>
      /// <param name="devType"></param>
      /// <param name="devNo"></param>
      /// <param name="size">輸入/輸出：讀取位元組數</param>
      /// <param name="data">讀取資料緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      int ReceiveEx(int path, int netNo, int stNo, int devType, int devNo, ref int size, short[] data);

      /// <summary>擴充位元裝置設置 (ON)</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netNo"></param>
      /// <param name="stNo"></param>
      /// <param name="devType"></param>
      /// <param name="devNo"></param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      int DevSetEx(int path, int netNo, int stNo, int devType, int devNo);

      /// <summary>擴充位元裝置復位 (OFF)</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netNo">網路編號</param>
      /// <param name="stNo">站號</param>
      /// <param name="devType">裝置代碼</param>
      /// <param name="devNo">指定裝置號碼</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      int DevRstEx(int path, int netNo, int stNo, int devType, int devNo);

      /// <summary>擴充裝置隨機寫入</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netNo">網路編號</param>
      /// <param name="stNo">站號</param>
      /// <param name="dev">隨機指定的裝置代碼</param>
      /// <param name="buf">寫入緩衝區</param>
      /// <param name="bufSize">虛擬參數</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      int RandWEx(int path, int netNo, int stNo, int[] dev, short[] buf, int bufSize);

      /// <summary>擴充裝置隨機讀取</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netNo">網路編號</param>
      /// <param name="stNo">站號</param>
      /// <param name="dev">隨機指定的裝置代碼</param>
      /// <param name="buf">讀取緩衝區</param>
      /// <param name="bufSize">讀取資料的位元組數</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      int RandREx(int path, int netNo, int stNo, int[] dev, short[] buf, int bufSize);

      /// <summary>寫入資料至遠端裝置站的緩衝記憶體</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netNo">網路編號</param>
      /// <param name="stNo">站號</param>
      /// <param name="offset">位移量</param>
      /// <param name="size">輸入/輸出：寫入位元組數</param>
      /// <param name="data">寫入資料緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      int RemBufWriteEx(int path, int netNo, int stNo, int offset, ref int size, short[] data);

      /// <summary>從遠端裝置站的緩衝記憶體讀取資料</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="netNo"></param>
      /// <param name="stNo"></param>
      /// <param name="offset">位移量</param>
      /// <param name="size">輸入/輸出：讀取位元組數</param>
      /// <param name="data">讀取資料緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      int RemBufReadEx(int path, int netNo, int stNo, int offset, ref int size, short[] data);

      /// <summary>寫入資料至指定 IP 位址的遠端裝置站緩衝記憶體</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="ipAddress">IP 地址</param>
      /// <param name="offset">位移量</param>
      /// <param name="size">輸入/輸出：寫入位元組數</param>
      /// <param name="data">寫入資料緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      int RemBufWriteIPEx(int path, int ipAddress, int offset, ref int size, short[] data);

      /// <summary>從指定 IP 位址的遠端裝置站緩衝記憶體讀取資料</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="ipAddress">IP 地址</param>
      /// <param name="offset">位移量</param>
      /// <param name="size">輸入/輸出：讀取位元組數</param>
      /// <param name="data">讀取資料緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      int RemBufReadIPEx(int path, int ipAddress, int offset, ref int size, short[] data);

      /// <summary>讀取 CPU 型號名稱</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="stNo">站號</param>
      /// <param name="buf">輸出：型號代碼</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      short TypeRead(int path, short stNo, out short buf);

      /// <summary>遠端控制 RUN / STOP / PAUSE</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="stNo">站號</param>
      /// <param name="buf">控制代碼 (1:RUN, 2:STOP, 3:PAUSE)</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      short Control(int path, short stNo, short buf);

      /// <summary>重置介面卡</summary>
      /// <param name="path">路徑指標</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      short BoardRst(int path);

      /// <summary>設定介面卡模式</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="mode">模式代碼</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      short BoardModSet(int path, short mode);

      /// <summary>讀取介面卡模式</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="mode">輸出：模式代碼</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      short BoardModRead(int path, out short mode);

      /// <summary>讀取介面卡的 LED 資訊</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="buf">讀取緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      short BoardLedRead(int path, short[] buf);

      /// <summary>讀取介面卡的開關狀態</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="buf">讀取緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      short BoardSwRead(int path, short[] buf);

      /// <summary>讀取介面卡的版本資訊</summary>
      /// <param name="path">路徑指標</param>
      /// <param name="buf">讀取緩衝區</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      short BoardVerRead(int path, short[] buf);

      /// <summary>初始化可程式控制器資訊表</summary>
      /// <param name="path">路徑指標</param>
      /// <returns>0:成功, 其他:錯誤代碼</returns>
      short Init(int path);

      #endregion
   }
}