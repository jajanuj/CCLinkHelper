namespace WindowsFormsApp1.Models
{
   /// <summary>
   /// 排出資料：包含追蹤資料與排出理由代碼
   /// </summary>
   public class MoveOutData
   {
      /// <summary>
      /// 追蹤資料 (Offset 0-9, 10 words)
      /// </summary>
      public TrackingData TrackingData { get; set; }

      /// <summary>
      /// 排出理由代碼 (Offset 10, 1 word)
      /// 0: 廢棄
      /// 1: 再投入
      /// 2: MP廢棄
      /// </summary>
      public ushort ReasonCode { get; set; }

      /// <summary>
      /// 建構函式
      /// </summary>
      public MoveOutData()
      {
         TrackingData = new TrackingData();
         ReasonCode = 0;
      }

      /// <summary>
      /// 轉換為 PLC 寫入格式 (11 words: TrackingData 10 words + ReasonCode 1 word)
      /// </summary>
      public short[] ToRawData()
      {
         var trackingRaw = TrackingData.ToRawData();
         var result = new short[11];
         
         // 複製 TrackingData (10 words)
         System.Array.Copy(trackingRaw, 0, result, 0, 10);
         
         // 加入 ReasonCode (1 word)
         result[10] = (short)ReasonCode;
         
         return result;
      }

      /// <summary>
      /// 從 PLC 原始資料建立 MoveOutData
      /// </summary>
      public static MoveOutData FromRawData(short[] data)
      {
         if (data == null || data.Length < 11)
         {
            return new MoveOutData();
         }

         var trackingRaw = new short[10];
         System.Array.Copy(data, 0, trackingRaw, 0, 10);

         return new MoveOutData
         {
            TrackingData = TrackingData.FromRawData(trackingRaw),
            ReasonCode = (ushort)data[10]
         };
      }
   }
}
