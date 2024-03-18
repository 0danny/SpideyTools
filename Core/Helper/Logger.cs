using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SpideyTools.Core.Helper
{
    public class Logger
    {
        public static void Log(string message)
        {
            Trace.WriteLine($"[SpideyTools]: {message}");
        }
    }
}
