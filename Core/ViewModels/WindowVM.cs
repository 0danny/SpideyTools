using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SpideyTools.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SpideyTools.Core.ViewModels
{
    public partial class WindowVM : ObservableObject
    {
        //Class Variables
        private readonly string version = "0.1";
        private readonly ProcessHandler procHandler = new();

        //Observables
        [ObservableProperty]
        public string windowTitle = "SpideyTools";

        [ObservableProperty]
        public string processStatus = "Not Found";

        [ObservableProperty]
        public SolidColorBrush processStatusColor = Brushes.Red;

        public WindowVM()
        {
            Logger.Log($"SpideyTools started -> v{version}");
            windowTitle = $"SpideyTools - v{version} - dan";

            procHandler.startScan();
        }

        //Buttons
        [RelayCommand]
        public void killGame()
        {
            Logger.Log("Killing the game.");

            procHandler.stopScan();
            procHandler.killProcess();
        }
    }
}
