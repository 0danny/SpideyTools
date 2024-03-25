using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SpideyTools.Core.Configuration;
using SpideyTools.Core.Helper;
using SpideyTools.Core.Models;
using SpideyTools.Core.Mods;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace SpideyTools.Core.ViewModels
{
    public partial class WindowVM : ObservableObject
    {
        //Class Variables
        private readonly string version = "0.1";
        private readonly ProcessHandler procHandler = new();
        private readonly ConfigLoader configLoader = new();

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
        private int windowHeight = 600;

        [ObservableProperty]
        private int windowWidth = 800;

        [ObservableProperty]
        private string gamePathBox = "Unknown.";

        public WindowVM()
        {
            //This is bad
            Logger.debugs = Debugs;
            Logger.Log($"SpideyTools started -> v{version}");
            windowTitle = $"SpideyTools - v{version} - dan";

            configLoader.load();
            populateSettings();

            characterMods = characterSwap.characterMods;

            procHandler.callback = processHandlerCallback;
            procHandler.init();
        }

        public void populateSettings()
        {
            GamePathBox = ConfigLoader.model.gamePath;
        }

        public void windowClosing()
        {
            configLoader.update();
        }

        public void processHandlerCallback(bool state)
        {
            if (state)
            {
                ProcessStatus = $"Locked({procHandler.getProcessID()})";
                ProcessStatusColor = Brushes.LightGreen;

                windowSize.changeSize(800, 600);
            }
            else
            {
                ProcessStatus = $"Not Started";
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
        public void startGame()
        {
            Logger.Log("Starting the game.");

            procHandler.startProcess();
        }

        [RelayCommand]
        public void setPath()
        {
            configLoader.pickPath();

            GamePathBox = ConfigLoader.model.gamePath;
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

                Dialogs.Show("Successfully patched character.");
            }
        }
    }
}
