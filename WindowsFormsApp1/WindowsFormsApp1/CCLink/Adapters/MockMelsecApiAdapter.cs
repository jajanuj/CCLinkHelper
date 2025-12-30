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

      private readonly Dictionary<int, int> _autoRespondMap = new Dictionary<int, int>();
      private readonly Dictionary<int, short> _bits = new Dictionary<int, short>();
      private readonly object _lock = new object();
      private readonly Dictionary<int, short> _words = new Dictionary<int, short>();

      #endregion

      #region Constructors

      public MockMelsecApiAdapter()
      {
         _autoRespondMap[CCLinkConstants.DefaultRequestFlagAddress] = CCLinkConstants.DefaultResponseFlagAddress;
      }

      #endregion

      #region Public Methods

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
            int count = size / 2;
            for (int i = 0; i < count; i++)
            {
               int key = devNo + i;
               if (devType == CCLinkConstants.DEV_LB)
               {
                  data[i] = _bits.ContainsKey(key) ? _bits[key] : (short)0;
               }
               else
               {
                  data[i] = _words.ContainsKey(key) ? _words[key] : (short)0;
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
            if (devType == CCLinkConstants.DEV_LB && _autoRespondMap.TryGetValue(devNo, out var resp))
            {
               _bits[resp] = 1;
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

      public int DevRstEx(int path, int netNo, int stNo, int devType, int devNo)
      {
         lock (_lock)
         {
            _bits[devNo] = 0;
            if (devType == CCLinkConstants.DEV_LB && _autoRespondMap.TryGetValue(devNo, out var resp))
            {
               _bits[resp] = 0;
            }
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

      #endregion
   }
}