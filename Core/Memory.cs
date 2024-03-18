using SpideyTools.Core.Helper;
using System.Text;

namespace SpideyTools.Core
{
    public class Memory
    {
        public static void patchInt(int address, int value)
        {
            if (ProcessHandler.processAlive())
            {
                byte[] buffer = BitConverter.GetBytes(value);

                Natives.WriteProcessMemory(ProcessHandler.gameProcess, new IntPtr(address), buffer, (uint)buffer.Length, out _);
            }
        }

        public static void patchString(int address, string value)
        {
            if (ProcessHandler.processAlive())
            {
                value += "\0";

                byte[] buffer = Encoding.UTF8.GetBytes(value);

                Natives.WriteProcessMemory(ProcessHandler.gameProcess, new IntPtr(address), buffer, (uint)buffer.Length, out _);
            }
        }

        public static void patchInstruction(int address, byte[] bytes)
        {
            if (ProcessHandler.processAlive())
            {
                Natives.WriteProcessMemory(ProcessHandler.gameProcess, new IntPtr(address), bytes, (uint)bytes.Length, out _);
            }
        }
    }
}
