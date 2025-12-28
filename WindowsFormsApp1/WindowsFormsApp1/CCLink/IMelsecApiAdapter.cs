namespace WindowsFormsApp1.CCLink
{
   /// <summary>
   /// MELSEC API 介面包裝。簽章依手冊（p.53~55, p.113~117）定義。
   /// </summary>
   public interface IMelsecApiAdapter
   {
      #region Public Methods

      short mdOpen(short chan, short mode, out int path);
      short mdClose(int path);
      short mdSetTimeout(int path, int timeoutMs);
      short mdSend(int path, byte[] buffer, int length);
      short mdReceive(int path, byte[] buffer, int length, out int received);
      short mdDevRead(int path, int deviceCode, int startAddr, int count, short[] dest);
      short mdDevWrite(int path, int deviceCode, int startAddr, int count, short[] src);
      short mdGetStatus(int path, out int statusCode);
      short mdGetLastError(int path, out int code);

      /// <summary>
      /// 擴展裝元件批量寫入（mdSendEx）。
      /// </summary>
      int mdSendEx(int path, int netno, int stno, int devtyp, int devno, ref int size, short[] data);

      /// <summary>
      /// 隨機讀取（mdRandR）：指定不同裝置/位址集合的讀取。
      /// dev[] 為裝置代碼陣列，buf[] 為回傳資料，bufsize 為返回元素數。
      /// </summary>
      int mdRandR(int path, int stno, int[] dev, short[] buf, int bufsize);

      /// <summary>
      /// 位元件設置（mdDevSet）：將指定位元件置為 ON。
      /// </summary>
      short mdDevSet(int path, int stno, int devtyp, int devno);

      /// <summary>
      /// 位元件復位（mdDevRst）：將指定位元件置為 OFF。
      /// </summary>
      short mdDevRst(int path, int stno, int devtyp, int devno);

      /// <summary>
      /// 位元件隨機寫入（mdRandW）：將資料寫入到隨機指定的裝元件集合。
      /// dev[] 為目標裝置代碼陣列，buf[] 為要寫入的資料，bufsize 為元素數。
      /// </summary>
      int mdRandW(int path, int stno, int[] dev, short[] buf, int bufsize);

      #endregion
   }
}
