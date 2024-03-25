using SpideyTools.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SpideyTools.Core.Helper
{
    public class Logger
    {
        public static ObservableCollection<string>? debugs = null;

        public static void Log(string message, [CallerMemberName] string memberName = "Unknown")
        {
            if(debugs != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    debugs.Add($"[{memberName}]: {message}");
                });
            }

            Trace.WriteLine($"[SpideyTools][{memberName}]: {message}");
        }
    }
}
