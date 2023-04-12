using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresenceOWO.OWOSystem
{
    public static class SettingsManager
    {
        /// <summary>
        /// The only true and real Settings class
        /// </summary>
        //======================================================//
        public static readonly Settings Settings = new Settings();
        //======================================================//

        public static bool Initialized { get; private set; } = false;
        public static void SetDefault()
        {
            Settings.HideOnClosed = true;

            Initialized = true;
        }

        public static void InitSettings()
        {
            //TODO: implement user settings saver and loader
            SetDefault();
        }
    }
}
