using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using PresenceOWO.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PresenceOWO.OWOSystem
{
    public static class SettingsManager
    {
        public const string configPath = "config.json";

        /// <summary>
        /// The only true and real Settings class
        /// </summary>
        //=================================================//
        public static Settings Settings { get; private set; }
        //=================================================//

        public static bool Initialized { get; private set; } = false;
        private static Settings Default()
        {
            var settings = new Settings()
            {
                HideOnClosed = true,
                LaunchOnStartup = true,
            };

            RegisterStartup();

            return settings;
        }

        public static void InitSettings()
        {
            //TODO: implement user settings saver and loader
            Settings _settings = SavingManager.LoadFromJson<Settings>(configPath, false);
            Settings = _settings ?? Default();

            Initialized = true;
        }

        public static void SaveSettings()
        {
            SavingManager.SaveToJson(configPath, Settings);
        }

       

        public static void RegisterStartup()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            key.SetValue("PresenceOWO", $"\"{SavingManager.ExePath}\"");
            key.Close();
        }

        public static void UnregisterStartup()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            try {

                key.DeleteValue("PresenceOWO", true);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }



            key.Close();
        }
    }
}
