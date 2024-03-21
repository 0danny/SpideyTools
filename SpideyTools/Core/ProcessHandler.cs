using SpideyTools.Core.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;

namespace SpideyTools.Core
{
    public class ProcessHandler
    {
        public delegate void CallbackFunc(bool state);
        public CallbackFunc? callback;

        public static string exeName = "SpiderMan";

        public static IntPtr gameProcess { get; private set; }
        public static Process? process { get; set; }

        //Private
        private Thread? scanThread { get; set; }
        private int scanInterval = 2000;

        private bool scanning = true;
        private bool shouldRefresh = true;

        //Injector
        private Injector injector = new();

        public static bool processAlive()
        {
            return process != null;
        }

        public int getProcessID()
        {
            return process == null ? -1 : process.Id;
        }

        //Thread for scanning if Spiderman.exe is open.
        public void startScan()
        {
            //Enable debug privs
            Natives.EnableDebugPriv();

            injector.init();

            scanThread = new Thread(new ThreadStart(scanMethod));

            scanThread.IsBackground = true;
            scanThread.Name = "Scanning Thread";

            scanThread.Start();
        }

        public void killProcess()
        {
            if (process != null && shouldRefresh == false)
            {
                process.Kill();
            }
        }

        public void stopScan()
        {
            scanning = false;

            if (scanThread != null)
            {
                scanThread.Join();
            }
        }

        public Process? getProcess()
        {
            //Process list.
            Process[] process = Process.GetProcessesByName(exeName);

            if (process.Length > 0)
            {
                return process[0];
            }

            return null;
        }

        public void scanMethod()
        {
            Logger.Log($"Scanning for process....");

            while (scanning)
            {
                process = getProcess();

                if (process != null && shouldRefresh == true)
                {
                    //Get a handle to the process
                    gameProcess = Natives.OpenProcess((int)Natives.PROCESS_ALL_ACCESS, false, process.Id);

                    shouldRefresh = false;

                    injector.inject();

                    Logger.Log($"Found process -> {process.Id} -> {gameProcess}");

                    if (callback != null)
                    {
                        callback(true);
                    }
                }
                else if (process == null)
                {
                    shouldRefresh = true;

                    if (callback != null)
                    {
                        callback(false);
                    }
                }

                Thread.Sleep(scanInterval);
            }
        }
    }
}
