using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SpideyTools.Core.Helper;
using SpideyTools.Core.Models;
using SpideyTools.Core.Mods;
using System;
using System.Collections.ObjectModel;
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

        //Window
        [ObservableProperty]
        private string windowTitle = "SpideyTools";

        [ObservableProperty]
        private string processStatus = "Not Found";

        [ObservableProperty]
        private SolidColorBrush processStatusColor = Brushes.Red;

        //Modifications
        [ObservableProperty]
        private ObservableCollection<CharacterMod> characterMods;

        [ObservableProperty]
        private CharacterMod? selectedCharacter;

        public WindowVM()
        {
            Logger.Log($"SpideyTools started -> v{version}");
            windowTitle = $"SpideyTools - v{version} - dan";

            characterMods = characterSwap.characterMods;

            procHandler.callback = processHandlerCallback;
            procHandler.startScan();
        }

        public void processHandlerCallback(bool state)
        {
            Logger.Log($"Callback {state}");

            if (state)
            {
                ProcessStatus = $"Found({procHandler.getProcessID()} - SpiderMan.exe)";
                ProcessStatusColor = Brushes.LightGreen;
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
        public void useCharacter()
        {
            if(SelectedCharacter != null)
            {
                Logger.Log($"Changing to character -> {SelectedCharacter.InternalName}");

                characterSwap.swap(SelectedCharacter.InternalName);

                MessageBox.Show("Successfully patched character.", "SpideyTools");
            }
        }
    }
}
