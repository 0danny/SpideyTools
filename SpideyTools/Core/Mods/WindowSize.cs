using SpideyTools.Core.Helper;
using System.Runtime.InteropServices;

namespace SpideyTools.Core.Mods
{
    public class WindowSize
    {
        public void changeSize(int width, int height)
        {
            IntPtr hWnd = Natives.FindWindow("Spider-Man", "Spider-Man");
            if (hWnd == IntPtr.Zero)
            {
                Logger.Log("Window not found");
                return;
            }

            int newStyle = 0x10C00000 | Natives.WS_SYSMENU | Natives.WS_MINIMIZEBOX;

            // Set the new window style
            Natives.SetWindowLong(hWnd, Natives.GWL_STYLE, newStyle);

            bool result = Natives.SetWindowPos(hWnd, IntPtr.Zero, 0, 0, width, height, Natives.SetWindowPosFlags.SWP_NOZORDER);
            if (!result)
            {
                Logger.Log($"Failed to set window position -> {Marshal.GetLastWin32Error()}");
            }
        }
    }
}
