using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SpideyTools.Core.Helper
{
    public class Dialogs
    {
        public static void Show(string text)
        {
            MessageBox.Show(text, "SpideyTools", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.None);
        }
    }
}
