using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SpideyTools.Core.Helper;
using SpideyTools.Core.Models;
using SpideyTools.Core.Mods;
using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SpideyTools.Core.ViewModels
{
    public partial class WindowVM : ObservableObject
    {
        //Class Variables
        private readonly string version = "0.1";
        private readonly ProcessHandler procHandler = new();

        //Mod Instances
        private readonly CharacterSwap characterSwap = new();
        private readonly WindowSize windowSize = new();

        //Window
        [ObservableProperty]
        private string windowTitle = "SpideyTools";

        [ObservableProperty]
        private string processStatus = "Not Found";

        [ObservableProperty]
        private SolidColorBrush processStatusColor = Brushes.Red;

        //Debug Window
        [ObservableProperty]
        public ObservableCollection<string> debugs = new();

        /* Modifications */
        [ObservableProperty]
        private ObservableCollection<CharacterMod> characterMods;

        [ObservableProperty]
        private CharacterMod? selectedCharacter;

        //Size
        [ObservableProperty]
        private int windowHeight = 720;

        [ObservableProperty]
        private int windowWidth = 1280;

        public WindowVM()
        {
            //This is bad
            Logger.debugs = Debugs;

            Logger.Log($"SpideyTools started -> v{version}");
            windowTitle = $"SpideyTools - v{version} - dan";

            characterMods = characterSwap.characterMods;

            procHandler.callback = processHandlerCallback;
            procHandler.startScan();
        }

        public void windowClosing()
        {
            procHandler.stopScan();
        }

        public void processHandlerCallback(bool state)
        {
            if (state)
            {
                ProcessStatus = $"Found({procHandler.getProcessID()} - SpiderMan.exe)";
                ProcessStatusColor = Brushes.LightGreen;

                windowSize.changeSize(1280, 720);
            }
            else
            {
                ProcessStatus = $"Not Found";
                ProcessStatusColor = Brushes.White;
            }
        }

        //Buttons
        [RelayCommand]
        public void killGame()
        {
            Logger.Log("Killing the game.");

            procHandler.killProcess();
        }

        [RelayCommand]
        public void setResolution()
        {
            Logger.Log($"Setting resolution to {WindowWidth} by {WindowHeight}");

            windowSize.changeSize(WindowWidth, WindowHeight);
        }

        [RelayCommand]
        public void useCharacter()
        {
            if (SelectedCharacter != null)
            {
                Logger.Log($"Changing to character -> {SelectedCharacter.InternalName}");

                characterSwap.swap(SelectedCharacter.InternalName);

                MessageBox.Show("Successfully patched character.", "SpideyTools");
            }
        }
    }
}
