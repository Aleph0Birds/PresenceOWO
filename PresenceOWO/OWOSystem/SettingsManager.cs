using Newtonsoft.Json.Linq;
using PresenceOWO.IO;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private static void SetDefault()
        {
            Settings = new Settings();

            Settings.HideOnClosed = true;

            Initialized = true;
        }

        public static void InitSettings()
        {
            //TODO: implement user settings saver and loader
            Settings _settings = SavingManager.LoadFromJson<Settings>(configPath, false);
            if(_settings != null) 
                Settings = _settings;
            else
                SetDefault();
        }

        public static void SaveSettings()
        {
            SavingManager.SaveToJson(configPath, Settings);
        }
    }
}
