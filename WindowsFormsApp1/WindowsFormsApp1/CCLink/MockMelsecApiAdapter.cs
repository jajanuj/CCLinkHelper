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

      public short mdOpen(short chan, short mode, out int path)
      {
         lock (_lock)
         {
            _open = true;
            path = 1;
            return 0;
         }
      }

      public short mdClose(int path)
      {
         lock (_lock)
         {
            _open = false;
            return 0;
         }
      }

      public short mdSetTimeout(int path, int timeoutMs) => 0;

      public short mdSend(int path, byte[] buffer, int length) => 0;

      public short mdReceive(int path, byte[] buffer, int length, out int received)
      {
         received = 0;
         return 0;
      }

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

      public short mdGetStatus(int path, out int statusCode)
      {
         statusCode = _open ? 0 : -1;
         return 0;
      }

      public short mdGetLastError(int path, out int code)
      {
         code = 0;
         return 0;
      }

      public int mdSendEx(int path, int netno, int stno, int devtyp, int devno, ref int size, short[] data) => 0;

      public int mdRandR(int path, int stno, int[] dev, short[] buf, int bufsize) => 0;

      public int mdRandW(int path, int stno, int[] dev, short[] buf, int bufsize) => 0;

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