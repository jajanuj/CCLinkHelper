using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ActUtlType64Lib;
using WindowsFormsApp1.CCLink.Interfaces;
using WindowsFormsApp1.CCLink.Models;

namespace WindowsFormsApp1.CCLink.Adapters
{
   public class MxComponentAdapter : ICCLinkController, IDisposable
   {
      #region Fields

      private readonly object _cacheLock = new object();

      // Cache
      private readonly Dictionary<string, short[]> _deviceMemory = new Dictionary<string, short[]>(StringComparer.OrdinalIgnoreCase);
      private readonly object _lock = new object();
      private readonly int _logicalStationNumber;
      private bool _isConnected;
      private ActUtlType64Class _plc;
      private TimeSpan _pollInterval = TimeSpan.FromMilliseconds(100);
      private CancellationTokenSource _scanCts;

      // Polling
      private List<ScanRange> _scanRanges = new List<ScanRange>();
      private Task _scanTask;

      #endregion

      #region Constructors

      public MxComponentAdapter(int logicalStationNumber)
      {
         _logicalStationNumber = logicalStationNumber;
      }

      #endregion

      #region Private Methods

      private short GetCachedValue(string type, int address)
      {
         lock (_cacheLock)
         {
            if (_deviceMemory.TryGetValue(type, out var memory))
            {
               if (address >= 0 && address < memory.Length)
               {
                  return memory[address];
               }
            }
         }

         return 0;
      }

      // --- Internal Helpers ---
      private void StartPolling()
      {
         if (_scanTask != null)
         {
            return;
         }

         _scanCts = new CancellationTokenSource();
         _scanTask = Task.Run(() => PollingLoop(_scanCts.Token));
      }

      private void StopPolling()
      {
         if (_scanCts != null)
         {
            _scanCts.Cancel();
            try
            {
               _scanTask?.Wait(1000);
            }
            catch
            {
            }

            _scanCts.Dispose();
            _scanCts = null;
         }

         _scanTask = null;
      }

      private async Task PollingLoop(CancellationToken ct)
      {
         while (!ct.IsCancellationRequested)
         {
            try
            {
               if (_isConnected)
               {
                  await PollAsync();
               }
            }
            catch
            {
               // ignore poll errors
            }

            await Task.Delay(_pollInterval, ct);
         }
      }

      private Task PollAsync()
      {
         return Task.Run(() =>
         {
            List<ScanRange> ranges;
            lock (_cacheLock)
            {
               ranges = _scanRanges.ToList();
            }

            lock (_lock)
            {
               foreach (var range in ranges)
               {
                  PollRange(range);
               }
            }
         });
      }

      private void PollRange(ScanRange range)
      {
         // Differentiate Bit vs Word reading
         if (IsBitDevice(range.Kind))
         {
            PollBitRange(range);
         }
         else
         {
            PollWordRange(range);
         }
      }

      private bool IsBitDevice(string kind)
      {
         string k = kind.ToUpper();
         return k == "X" || k == "Y" || k == "B" || k == "M" || k == "L" || k == "F" || k == "LB" || k == "SB" || k == "LX" || k == "LY";
      }

      private void PollWordRange(ScanRange range)
      {
         // Read words simply.
         // range.Start, range.End are integer addresses.
         // Size = End - Start + 1
         int size = range.End - range.Start + 1;
         string startAddr = FormatMxAddress(range.Kind, range.Start);

         // Using loop for safety as Block2 array passing is tricky in safe context without helper
         // In real optimized scenario, we'd use int[] buffer and Block read
         // Here, loop is safer and acceptable for small ranges.
         // If range is large, this is slow.
         // Compromise: Use GetDevice2 one by one.

         short[] data = new short[size];
         for (int i = 0; i < size; i++)
         {
            string addr = FormatMxAddress(range.Kind, range.Start + i);
            _plc.GetDevice2(addr, out data[i]);
         }

         UpdateCache(range.Kind, range.Start, data);
      }

      private void PollBitRange(ScanRange range)
      {
         // For bits, we need to store them as 0/1 in short[] cache
         int size = range.End - range.Start + 1;
         short[] data = new short[size];

         for (int i = 0; i < size; i++)
         {
            string addr = FormatMxAddress(range.Kind, range.Start + i);
            int val = 0;
            _plc.GetDevice(addr, out val); // GetDevice returns bit value in int
            data[i] = (short)(val != 0 ? 1 : 0);
         }

         UpdateCache(range.Kind, range.Start, data);
      }

      private void UpdateCache(string kind, int start, short[] data)
      {
         lock (_cacheLock)
         {
            if (!_deviceMemory.ContainsKey(kind))
            {
               // Allocate
               int max = Math.Max(start + data.Length, 1024); // Minimal size
               _deviceMemory[kind] = new short[max];
            }

            var memory = _deviceMemory[kind];
            if (start + data.Length > memory.Length)
            {
               // Reallocate
               int newSize = Math.Max(memory.Length * 2, start + data.Length);
               var newMem = new short[newSize];
               Array.Copy(memory, newMem, memory.Length);
               _deviceMemory[kind] = newMem;
               memory = newMem;
            }

            Array.Copy(data, 0, memory, start, data.Length);
         }
      }

      #endregion

      public Task OpenAsync(CancellationToken ct = default)
      {
         return Task.Run(() =>
         {
            lock (_lock)
            {
               if (_isConnected)
               {
                  return;
               }

               try
               {
                  _plc = new ActUtlType64Class();
                  _plc.ActLogicalStationNumber = _logicalStationNumber;
                  int ret = _plc.Open();
                  if (ret != 0)
                  {
                     throw new MelsecException($"Failed to open Mx Component. Error Code: 0x{ret:X8}");
                  }

                  _isConnected = true;
                  StartPolling();
               }
               catch (Exception ex)
               {
                  throw new MelsecException("Error opening Mx Component connection.", ex);
               }
            }
         }, ct);
      }

      public Task CloseAsync(CancellationToken ct = default)
      {
         return Task.Run(() =>
         {
            StopPolling();
            lock (_lock)
            {
               if (!_isConnected)
               {
                  return;
               }

               try
               {
                  _plc?.Close();
               }
               finally
               {
                  _isConnected = false;
               }
            }
         }, ct);
      }

      public Task<bool> ReadBitsAsync(string address, CancellationToken ct = default)
      {
         // Direct Read
         return Task.Run(() =>
         {
            lock (_lock)
            {
               int value = 0;
               string mxAddress = ConvertToMxAddress(address);
               int ret = _plc.GetDevice(mxAddress, out value); // GetDevice reads single bit/word as int
               if (ret != 0)
               {
                  throw new MelsecException($"Error reading bit {mxAddress}. Code: 0x{ret:X8}");
               }

               return value != 0;
            }
         }, ct);
      }

      public Task WriteBitsAsync(string address, IEnumerable<bool> values, CancellationToken ct = default)
      {
         return Task.Run(() =>
         {
            lock (_lock)
            {
               var valArray = values.ToArray();
               var (type, offset) = ParseAddress(address);
               for (int i = 0; i < valArray.Length; i++)
               {
                  string currentAddr = FormatMxAddress(type, offset + i);
                  int ret = _plc.SetDevice(currentAddr, valArray[i] ? 1 : 0);
                  if (ret != 0)
                  {
                     throw new MelsecException($"Error writing bit {currentAddr}. Code: 0x{ret:X8}");
                  }

                  // Update Cache Optimistically?
                  // UpdateCacheSingle(type.ToUpper(), offset + i, (short)(valArray[i] ? 1 : 0));
               }
            }
         }, ct);
      }

      public Task<IReadOnlyList<short>> ReadWordsAsync(string address, int count, CancellationToken ct = default)
      {
         return Task.Run(() =>
         {
            lock (_lock)
            {
               var list = new List<short>();
               var (type, offset) = ParseAddress(address);

               // Optimization: Try ReadDeviceBlock if count > 1
               // Mx Component ReadDeviceBlock2 reads WORDS.
               if (count > 0)
               {
                  string startAddr = FormatMxAddress(type, offset);
                  short[] buffer = new short[count];
                  // Note: using 'out buffer[0]' pattern requires unsafe or interop fix.
                  // C# 'out' only accepts a variable.
                  // But if the interop definition allows array, we can pass it.
                  // Let's rely on loop for safety as implemented before, unless we are sure.
                  // Or use 'ReadDeviceBlock' (int array) which is safer in COM Interop usually.
                  // Interop.ActUtlType64Lib definitions:
                  // int ReadDeviceBlock2(string szDevice, int lSize, out short lplData);
                  // If we cannot pass array, loop is the only safe managed way without unsafe code.

                  for (int i = 0; i < count; i++)
                  {
                     string currParams = FormatMxAddress(type, offset + i);
                     short valShort = 0;
                     int ret = _plc.GetDevice2(currParams, out valShort);
                     if (ret != 0)
                     {
                        throw new MelsecException($"Error reading word {currParams}. Code: 0x{ret:X8}");
                     }

                     list.Add(valShort);
                  }
               }

               return (IReadOnlyList<short>)list;
            }
         }, ct);
      }

      public Task WriteWordsAsync(string address, IEnumerable<short> values, CancellationToken ct = default)
      {
         return Task.Run(() =>
         {
            lock (_lock)
            {
               var arr = values.ToArray();
               var (type, offset) = ParseAddress(address);
               for (int i = 0; i < arr.Length; i++)
               {
                  string currParams = FormatMxAddress(type, offset + i);
                  int ret = _plc.SetDevice2(currParams, arr[i]);
                  if (ret != 0)
                  {
                     throw new MelsecException($"Error writing word {currParams}. Code: 0x{ret:X8}");
                  }
               }
            }
         }, ct);
      }

      public Task<ControllerStatus> GetStatusAsync(CancellationToken ct = default)
      {
         return Task.FromResult(new ControllerStatus
         {
            IsConnected = _isConnected,
            LastErrorCode = 0
         });
      }

      public void SetScanRanges(IEnumerable<ScanRange> ranges)
      {
         lock (_cacheLock)
         {
            _scanRanges = ranges.ToList();
         }
         // If connected, might trigger immediate poll or let loop handle it
      }

      public bool GetBit(string address)
      {
         var (type, val) = ParseAddress(address);
         return GetCachedValue(type, val) != 0;
      }

      public short GetWord(string address)
      {
         var (type, val) = ParseAddress(address);
         return GetCachedValue(type, val);
      }

      public void Dispose()
      {
         _plc = null;
         StopPolling();
      }

      #region Address Helper

      private string ConvertToMxAddress(string address)
      {
         var (type, val) = ParseAddress(address);
         return FormatMxAddress(type, val);
      }

      private (string type, int value) ParseAddress(string address)
      {
         // Remove digits to get type
         string type = new string(address.TakeWhile(c => !char.IsDigit(c)).ToArray());
         string numPart = address.Substring(type.Length);

         // Parse numPart as HEX always
         int val = Convert.ToInt32(numPart, 16);
         return (type, val);
      }

      private string FormatMxAddress(string type, int value)
      {
         type = type.ToUpper();
         if (type == "M" || type == "D" || type == "L" || type == "R") // Dec types
         {
            return $"{type}{value}";
         }
         else // Hex types
         {
            return $"{type}{value:X}";
         }
      }

      #endregion
   }
}