using DiscordRPC;
using DiscordRPC.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PresenceOWO.DoRPC
{
    public static class Client
    {
        private static DiscordRpcClient client;
        private static string app_id;
        private static bool Initialized;
        public static void Initialize()
        {
            client = new (app_id)
            {
                Logger = new ConsoleLogger { Level = LogLevel.Warning }
            };

            client.Initialize();
            Initialized = true;
        }

        public static void HandleUpdate(object sender, RPArgs e)
        {
            if(!Initialized)
            {
                if (string.IsNullOrEmpty(e.ApplicationID))
                    MessageBox.Show(@"Please input your application id");
                else
                {
                    app_id = e.ApplicationID;
                    Initialize();
                }

            }

            if (!string.IsNullOrEmpty(e.ApplicationID) && app_id != e.ApplicationID)
                UpdateAppID(e.ApplicationID);
        }

        private static void UpdateAppID(string newID)
        {
            app_id = newID;
            Initialized = false;
            Initialize();
        }
    }
}
