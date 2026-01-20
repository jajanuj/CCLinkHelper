using System.Collections.Generic;
using WindowsFormsApp1.CCLink.Interfaces;
using WindowsFormsApp1.CCLink.Models;

namespace WindowsFormsApp1.CCLink.Adapters
{
   /// <summary>
   /// Simple in-memory mock of IMelsecApiAdapter for local simulation/testing.
   /// Stores LB bits and LW words in dictionaries and simulates mdOpen/mdClose.
   /// Supports auto-respond mapping: when a request LB is set, the mock can automatically set a response LB.
   /// </summary>
   public sealed class MockMelsecApiAdapter : IMelsecApiAdapter
   {
      #region Fields

      private readonly Dictionary<int, short> _bits = new Dictionary<int, short>();
      private readonly object _lock = new object();
      private readonly System.Action<string> _logger;
      private readonly Dictionary<int, short> _words = new Dictionary<int, short>();

      #endregion

      #region Constructors

      public MockMelsecApiAdapter(System.Action<string> logger = null)
      {
         _logger = logger;
      }

      #endregion

      public short Open(short chan, short mode, out int path)
      {
         lock (_lock)
         {
            path = 1;
            return 0;
         }
      }

      public short Close(int path)
      {
         lock (_lock)
         {
            return 0;
         }
      }

      public int SendEx(int path, int netNo, int stNo, int devType, int devNo, ref int size, short[] data)
      {
         lock (_lock)
         {
            bool isBitDevice = devType == CCLinkConstants.DEV_LB || devType == CCLinkConstants.DEV_LX || devType == CCLinkConstants.DEV_LY;
            
            // 對於 bit 設備，size 是 bytes，但實際 API 將 16 bits 打包成一個 short
            // 所以實際 bit 數量 = (size / 2) * 16 = size * 8
            // 但我們的 _bits 字典每個 key 對應一個 bit
            int count;
            if (isBitDevice)
            {
               // 每個 short 包含 16 bits，size / 2 = short 數量，* 16 = bit 數量
               count = (size / 2) * 16;
            }
            else
            {
               count = size / 2;  // Word 設備：每個地址一個 word
            }

            for (int i = 0; i < count; i++)
            {
               int addr = devNo + i;
               if (isBitDevice)
               {
                  // 從 data 中提取 bit 值
                  int shortIndex = i / 16;
                  int bitOffset = i % 16;
                  if (shortIndex < data.Length)
                  {
                     short bitValue = (short)((data[shortIndex] >> bitOffset) & 1);
                     _bits[addr] = bitValue;
                     if (bitValue != 0)
                     {
                        _logger?.Invoke($"[MockAdapter] SendEx: Bit LB{addr:X4} = {bitValue}");
                     }
                  }
               }
               else
               {
                  if (i < data.Length)
                  {
                     _words[addr] = data[i];
                     _logger?.Invoke($"[MockAdapter] SendEx: Word LW{addr:X4} = {data[i]}");
                  }
               }
            }

            return 0;
         }
      }

      public int ReceiveEx(int path, int netNo, int stNo, int devType, int devNo, ref int size, short[] data)
      {
         lock (_lock)
         {
            bool isBitDevice = devType == CCLinkConstants.DEV_LB || devType == CCLinkConstants.DEV_LX || devType == CCLinkConstants.DEV_LY;
            
            int count;
            if (isBitDevice)
            {
               // 每個 short 包含 16 bits
               count = (size / 2) * 16;
            }
            else
            {
               count = size / 2;
            }

            // Initialize data array for bit devices to ensure correct packing
            if (isBitDevice)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = 0;
                }
            }

            for (int i = 0; i < count; i++)
            {
               int addr = devNo + i;
               if (isBitDevice)
               {
                  // 將 bit 值打包到 data 中
                  int shortIndex = i / 16;
                  int bitOffset = i % 16;
                  if (shortIndex < data.Length)
                  {
                     short bitValue = _bits.ContainsKey(addr) ? _bits[addr] : (short)0;
                     if (bitValue != 0)
                     {
                        data[shortIndex] |= (short)(1 << bitOffset);
                     }
                  }
               }
               else
               {
                  if (i < data.Length)
                  {
                     data[i] = _words.ContainsKey(addr) ? _words[addr] : (short)0;
                  }
               }
            }

            return 0;
         }
      }

      public int DevSetEx(int path, int netNo, int stNo, int devType, int devNo)
      {
         lock (_lock)
         {
            _bits[devNo] = 1;
            _logger?.Invoke($"[MockAdapter] DevSetEx: LB{devNo:X4} = 1");

            return 0;
         }
      }

      public int DevRstEx(int path, int netNo, int stNo, int devType, int devNo)
      {
         lock (_lock)
         {
            _bits[devNo] = 0;
            _logger?.Invoke($"[MockAdapter] DevRstEx: LB{devNo:X4} = 0");

            return 0;
         }
      }

      public int RandWEx(int path, int netNo, int stNo, int[] dev, short[] buf, int bufSize) => 0;

      public int RandREx(int path, int netNo, int stNo, int[] dev, short[] buf, int bufSize) => 0;

      public int RemBufWriteEx(int path, int netNo, int stNo, int offset, ref int size, short[] data) => 0;

      public int RemBufReadEx(int path, int netNo, int stNo, int offset, ref int size, short[] data) => 0;

      public int RemBufWriteIPEx(int path, int ipAddress, int offset, ref int size, short[] data) => 0;

      public int RemBufReadIPEx(int path, int ipAddress, int offset, ref int size, short[] data) => 0;

      public short TypeRead(int path, short stNo, out short buf)
      {
         buf = 0;
         return 0;
      }

      public short Control(int path, short stNo, short buf) => 0;

      public short BoardRst(int path) => 0;

      public short BoardModSet(int path, short mode) => 0;

      public short BoardModRead(int path, out short mode)
      {
         mode = 0;
         return 0;
      }

      public short BoardLedRead(int path, short[] buf) => 0;

      public short BoardSwRead(int path, short[] buf) => 0;

      public short BoardVerRead(int path, short[] buf) => 0;

      public short Init(int path) => 0;
   }
}