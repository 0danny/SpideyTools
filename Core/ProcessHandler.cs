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
        public static string exeName = "SpiderMan";

        public static IntPtr gameProcess { get; private set; }
        public static Process? process { get; private set; }

        //Private
        private Thread? scanThread { get; set; }
        private int scanInterval = 2000;
        private bool scanning = true;
        private bool shouldRefresh = true;

        //Thread for scanning if Spiderman.exe is open.
        public void startScan()
        {
            scanThread = new Thread(new ThreadStart(scanMethod));

            scanThread.IsBackground = true;
            scanThread.Name = "Scanning Thread";
            scanThread.Start();
        }

        public void killProcess()
        {
            if(process != null && shouldRefresh == false)
            {
                process.Kill();
            }
        }

        public void stopScan()
        {
            scanning = false;
        }

        public Process? getProcessID()
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
            while (scanning)
            {
                process = getProcessID();

                Logger.Log($"Scanning for process....");

                if (process != null && shouldRefresh == true)
                {
                    //Get a handle to the process
                    gameProcess = Natives.OpenProcess(Natives.PROCESS_VM_READ | Natives.PROCESS_VM_WRITE | Natives.PROCESS_VM_OPERATION, false, process.Id);

                    shouldRefresh = false;

                    Logger.Log($"Found process -> {process.Id} -> {gameProcess}");

                    /*
                    MainWindow.instance.Dispatcher.Invoke(() =>
                    {
                    MainWindow.instance.main_processStatus.Content = $"Found({procID} - SpiderMan.exe)";
                        MainWindow.instance.main_processStatus.Foreground = Brushes.LightGreen;
                    });*/
                }
                else if (process == null)
                {
                    shouldRefresh = true;

                    /*
                    MainWindow.instance.Dispatcher.Invoke(() =>
                    {
                    MainWindow.instance.main_processStatus.Content = $"Not Found";
                    MainWindow.instance.main_processStatus.Foreground = Brushes.White;
                    }); */
                }

                Thread.Sleep(scanInterval);
            }
        }
    }
}
