using SpideyTools.Core.Configuration;
using SpideyTools.Core.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;
using static SpideyTools.Core.Helper.Natives;

namespace SpideyTools.Core
{
    public class ProcessHandler
    {
        public delegate void CallbackFunc(bool state);
        public CallbackFunc? callback;

        public static string exeName = "SpiderMan";

        public static IntPtr gameProcess { get; private set; }
        public static STARTUPINFO startInfo = new();
        public static PROCESS_INFORMATION processInfo = new();

        //Injector
        private Injector injector = new();

        public static bool processAlive()
        {
            return gameProcess != IntPtr.Zero;
        }

        public int getProcessID()
        {
            return processInfo.dwProcessId == 0 ? -1 : (int)processInfo.dwProcessId;
        }

        //Thread for scanning if Spiderman.exe is open.
        public void init()
        {
            //Enable debug privs
            EnableDebugPriv();

            injector.init();
        }

        public void killProcess()
        {
            if (gameProcess != IntPtr.Zero)
            {
                //Kill Process
                Process? spiderProc = getProcess();

                if (spiderProc != null)
                    spiderProc.Kill();
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

        public void startProcess()
        {
            string gamePath = @$"{ConfigLoader.model.gamePath}\{exeName}.exe";

            Logger.Log($"Launching game path -> {gamePath}");

            bool result = CreateProcessA(gamePath, null, IntPtr.Zero, IntPtr.Zero, false, ProcessCreationFlags.CREATE_SUSPENDED, IntPtr.Zero, ConfigLoader.model.gamePath, ref startInfo, out processInfo);

            if (result)
            {
                Logger.Log("Process created successfully in suspended state.");

                gameProcess = OpenProcess(PROCESS_ALL_ACCESS, false, (int)processInfo.dwProcessId);

                Logger.Log($"Aquired game handle -> {gameProcess}");

                injector.inject();

                ResumeThread(processInfo.hThread);

                if(callback != null)
                    callback(true);
            }
            else
            {
                Logger.Log("Failed to create process.");

                if (callback != null)
                    callback(false);
            }

        }
    }
}
