using System;
using System.Threading.Tasks;

using WindowsFormsApp1.CCLink.Interfaces;
using WindowsFormsApp1.CCLink.Models;

namespace WindowsFormsApp1.CCLink.Services
{
   public class BackgroundMonitor
   {
      #region Constant

      // address constants - these may be adjusted to match real mapping
      private const int AddrCheckClockReq = 0x00A0;
      private const int AddrCheckClockResp = 0x00A1;

      private const int AddrTimeSyncReq = 0x00B0;
      private const int AddrTimeSyncResp = 0x00B1;
      private const int AddrTimeSyncData = 0x4000;

      private const int AddrStatusReport = 0x5000;

      #endregion

      #region Fields

      private readonly IPlcDriver _plc;
      private bool _isRunning;

      #endregion

      #region Constructors

      public BackgroundMonitor(IPlcDriver plc)
      {
         _plc = plc ?? throw new ArgumentNullException(nameof(plc));
      }

      #endregion

      #region Public Methods

      public async Task RunLoopAsync()
      {
         _isRunning = true;
         while (_isRunning)
         {
            try
            {
               // 1. Check Clock (echo request to response)
               bool clockSignal = _plc.ReadBit(new LinkDeviceAddress("LB", AddrCheckClockReq, 1));
               _plc.WriteBit(new LinkDeviceAddress("LB", AddrCheckClockResp, 1), clockSignal);

               // 2. Time sync
               if (_plc.ReadBit(new LinkDeviceAddress("LB", AddrTimeSyncReq, 1)))
               {
                  short[] timeData = _plc.ReadWords(new LinkDeviceAddress("LW", AddrTimeSyncData, 7));
                  // Optional: set system time - omitted for safety

                  _plc.WriteBit(new LinkDeviceAddress("LB", AddrTimeSyncResp, 1), true);

                  int waitCount = 0;
                  while (_plc.ReadBit(new LinkDeviceAddress("LB", AddrTimeSyncReq, 1)) && waitCount < 20)
                  {
                     await Task.Delay(100);
                     waitCount++;
                  }

                  _plc.WriteBit(new LinkDeviceAddress("LB", AddrTimeSyncResp, 1), false);
               }

               // 3. Status report
               short[] currentStatus = GetMachineStatus();
               _plc.WriteWords(new LinkDeviceAddress("LW", AddrStatusReport, currentStatus.Length), currentStatus);
            }
            catch (Exception ex)
            {
               // swallow and continue
               System.Diagnostics.Debug.WriteLine($"BackgroundMonitor error: {ex.Message}");
            }

            await Task.Delay(200);
         }
      }

      public void Stop()
      {
         _isRunning = false;
      }

      #endregion

      #region Private Methods

      private short[] GetMachineStatus()
      {
         return new short[10];
      }

      #endregion
   }
}
