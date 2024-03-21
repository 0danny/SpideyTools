using SpideyTools.Core.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SpideyTools.Core
{
    public class Injector
    {
        private string dllPath = "";

        private IntPtr kernelHndl = IntPtr.Zero;
        private IntPtr loadLibraryHndl = IntPtr.Zero;

        public void init()
        {
            Logger.Log("Initializing LoadLibrary injector.");

            dllPath = $"{Directory.GetCurrentDirectory()}\\SpideyPipe.dll";

            Logger.Log($"Working directory -> {dllPath}");

            kernelHndl = Natives.GetModuleHandle("kernel32.dll");

            Logger.Log($"Kernel handle -> {kernelHndl}");

            if (kernelHndl != IntPtr.Zero)
            {
                loadLibraryHndl = Natives.GetProcAddress(kernelHndl, "LoadLibraryA");

                Logger.Log($"Load library handle -> {loadLibraryHndl}");
            }
        }

        //Small load library injector to get the internal DLL loaded.
        public void inject()
        {
            if(!File.Exists(dllPath))
            {
                Logger.Log("The DLL cannot be found.");
                return;
            }

            if(ProcessHandler.processAlive() && loadLibraryHndl != IntPtr.Zero && kernelHndl != IntPtr.Zero)
            {
                Logger.Log($"Injecting {dllPath} now.");

                IntPtr allocMemAddress = Natives.VirtualAllocEx(ProcessHandler.gameProcess, IntPtr.Zero, (uint)(dllPath.Length + 1), Natives.MEM_COMMIT, Natives.PAGE_READWRITE);

                Natives.WriteProcessMemory(ProcessHandler.gameProcess, allocMemAddress, Encoding.ASCII.GetBytes(dllPath), (uint)(dllPath.Length + 1), out _);

                Logger.Log($"Written DLL path to memory at -> {allocMemAddress}, length: {(uint)(dllPath.Length + 1)}");

                IntPtr remoteThread = Natives.CreateRemoteThread(ProcessHandler.gameProcess, IntPtr.Zero, 0, loadLibraryHndl, allocMemAddress, 0, IntPtr.Zero);

                if (remoteThread == IntPtr.Zero)
                {
                    Logger.Log("Failed to create remote thread.");
                }
                else
                {
                    Natives.WaitForSingleObject(remoteThread, 10000); // Wait for the thread to complete
                    Natives.GetExitCodeThread(remoteThread, out IntPtr loadLibResult);

                    if (loadLibResult == IntPtr.Zero)
                    {
                        Logger.Log($"Load Library Result -> Last Error: {Natives.GetLastError()}, -> GetExitCodeThread -> {loadLibResult}");

                        Logger.Log("DLL injection failed.");
                    }
                    else
                    {
                        Logger.Log($"DLL injected successfully -> {loadLibResult}");
                    }
                }

                //Free the memory we used to write dll path.
                Natives.VirtualFreeEx(ProcessHandler.gameProcess, allocMemAddress, 0, (int)Natives.MEM_RELEASE);
            }
            else
            {
                Logger.Log("The process is not alive || load library or kernel handle is null.");
            }
        }
    }
}
