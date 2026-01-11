using System.Collections.Generic;
using System.Threading.Tasks;
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
      private readonly Dictionary<int, short> _words = new Dictionary<int, short>();

      #endregion

      #region Constructors

      public MockMelsecApiAdapter()
      {
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
            int count = size / 2;
            for (int i = 0; i < count; i++)
            {
               if (devType == CCLinkConstants.DEV_LB)
               {
                  _bits[devNo + i] = data[i];
               }
               else
               {
                  _words[devNo + i] = data[i];
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
            
            // netNo == 1 表示 bit 模式讀取（8-bit 對齊打包）
            if (isBitDevice && netNo == 1)
            {
               // Bit mode: pack 8 bits per byte, 2 bytes per short
               int bitsToRead = size * 8; // size 是要讀取的 byte 數
               for (int i = 0; i < data.Length; i++)
               {
                  int lowByte = 0;
                  int highByte = 0;
                  
                  // 每個 short 包含 2 個 byte = 16 個 bit
                  for (int bit = 0; bit < 8; bit++)
                  {
                     int bitAddr = devNo + i * 16 + bit;
                     if (_bits.ContainsKey(bitAddr) && _bits[bitAddr] != 0)
                     {
                        lowByte |= (1 << bit);
                     }
                  }
                  for (int bit = 0; bit < 8; bit++)
                  {
                     int bitAddr = devNo + i * 16 + 8 + bit;
                     if (_bits.ContainsKey(bitAddr) && _bits[bitAddr] != 0)
                     {
                        highByte |= (1 << bit);
                     }
                  }
                  
                  data[i] = (short)(lowByte | (highByte << 8));
               }
            }
            else
            {
               // Word mode or non-bit device: read individual values
               int count = size / 2;
               for (int i = 0; i < count; i++)
               {
                  int key = devNo + i;
                  if (isBitDevice)
                  {
                     data[i] = _bits.ContainsKey(key) ? _bits[key] : (short)0;
                  }
                  else
                  {
                     data[i] = _words.ContainsKey(key) ? _words[key] : (short)0;
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

            return 0;
         }
      }

      public int DevRstEx(int path, int netNo, int stNo, int devType, int devNo)
      {
         lock (_lock)
         {
            _bits[devNo] = 0;

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