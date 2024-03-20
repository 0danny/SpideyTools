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

                Logger.Log($"Load library handle -> {loadLibraryHndl}, -> {Marshal.GetLastWin32Error()}");
            }
        }

        //Small load library injector to get the internal DLL loaded.
        public void inject()
        {
            if(ProcessHandler.processAlive() && loadLibraryHndl != IntPtr.Zero && kernelHndl != IntPtr.Zero)
            {
                Logger.Log($"Injecting {dllPath} now.");

                IntPtr allocMemAddress = Natives.VirtualAllocEx(ProcessHandler.gameProcess, IntPtr.Zero, (uint)(dllPath.Length + 1), Natives.MEM_COMMIT, Natives.PAGE_READWRITE);

                Logger.Log($"Virtual Alloc Ex Error -> {Marshal.GetLastWin32Error()}");

                Natives.WriteProcessMemory(ProcessHandler.gameProcess, allocMemAddress, Encoding.ASCII.GetBytes(dllPath), (uint)(dllPath.Length + 1), out _);

                Logger.Log($"Written DLL path to memory at -> {allocMemAddress}, length: {(uint)(dllPath.Length + 1)}");

                IntPtr remoteThread = Natives.CreateRemoteThread(ProcessHandler.gameProcess, IntPtr.Zero, 0, loadLibraryHndl, allocMemAddress, 0, IntPtr.Zero);

                Logger.Log($"Remote Thread Error -> {Marshal.GetLastWin32Error()}");

                Natives.WaitForSingleObject(remoteThread, 10000); // Wait for the thread to complete
                Natives.GetExitCodeThread(remoteThread, out IntPtr loadLibraryResult);

                Logger.Log($"Load Library Result -> Last Error: {Marshal.GetLastWin32Error()}, -> GetExitCodeThread -> {loadLibraryResult}");

                if (remoteThread == IntPtr.Zero)
                {
                    Logger.Log("Failed to create remote thread.");
                }
                else
                {
                    Logger.Log("DLL injected successfully.");
                }

                //Natives.VirtualFreeEx(ProcessHandler.gameProcess, allocMemAddress, 0, (int)Natives.MEM_RELEASE);
            }
            else
            {
                Logger.Log("The process is not alive || load library or kernel handle is null.");
            }
        }
    }
}
