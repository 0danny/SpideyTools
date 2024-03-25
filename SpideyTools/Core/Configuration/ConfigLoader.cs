using Newtonsoft.Json;
using SpideyTools.Core.Helper;
using SpideyTools.Core.Models;
using System.IO;
using System.Windows.Forms;

namespace SpideyTools.Core.Configuration
{
    public class ConfigLoader
    {
        public static Config model { get; private set; } = new();

        private string configPath = "config.json";

        public void pickPath()
        {
            FolderBrowserDialog folderDialog = new();

            if (folderDialog.ShowDialog() == DialogResult.OK)
            {                
                model.gamePath = folderDialog.SelectedPath;
            }

            Logger.Log($"Set new game path -> {model.gamePath}");
        }

        public void load()
        {
            //Ensure it doesn't exist.
            if (!File.Exists(configPath))
            {
                update();
            }

            string configData = File.ReadAllText(configPath);

            if(!string.IsNullOrEmpty(configData))
            {
                //Load model
                try
                {
                    Config? loadedModel = JsonConvert.DeserializeObject<Config>(configData);

                    if(loadedModel != null )
                    {
                        model = loadedModel;

                        Logger.Log("Config loaded.");
                    }
                }
                catch (Exception ex) { Logger.Log($"Could not deserialize config -> {ex.Message}"); }
            }
            else
            {
                Logger.Log("Config is empty, delete it and relaunch.");
            }
        }

        public void update()
        {
            try
            {
                File.WriteAllText(configPath, JsonConvert.SerializeObject(model));
            }
            catch (Exception ex) { Logger.Log($"Cannot create config -> {ex.Message}"); }
        }
    }
}
