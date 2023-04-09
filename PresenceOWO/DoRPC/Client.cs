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

        public static bool InPresence { get; set; }
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
                {
                    MessageBox.Show(@"Please input your application id");
                    return;
                }
                else
                {
                    app_id = e.ApplicationID;
                    Initialize();
                }

            }

            if (!string.IsNullOrEmpty(e.ApplicationID) && app_id != e.ApplicationID)
                UpdateAppID(e.ApplicationID);

            StartPresence(e);
        }

        private static void StartPresence(RPArgs e)
        {

            // oh yeah nice ass
            Assets ass = new Assets
            {
                SmallImageText = e.SmallImageText,
                LargeImageText = e.LargeImageText,
                LargeImageKey = e.LargeImageKey,
                SmallImageKey = e.SmallImageKey
            };

            RichPresence presence = new RichPresence()
                .WithDetails(e.Details)
                .WithState(e.State)
                .WithAssets(ass)
                ;

            if(e.TimestampModeNumber != 0)
            {
                Timestamps stamp = new Timestamps();
                if((TimestampMode)e.TimestampModeNumber == TimestampMode.UntilCustom)
                    stamp.End = Timestamps.FromUnixMilliseconds(e.Timestamp * 1000);
                else
                    stamp.Start = Timestamps.FromUnixMilliseconds(e.Timestamp * 1000);

                presence.WithTimestamps(stamp);
            }

            client.SkipIdenticalPresence = true;
            client.SetPresence(presence);
            InPresence = true;

            ArgDoing.LastUpdateTime = DateTime.Now;
            viewModel.UpdateTimestampText();
        }

        public static void StopPresence()
        {
            InPresence = false;
            client.ClearPresence();
            ArgDoing.LastUpdateTime = DateTime.Now;
        }
        private static void UpdateAppID(string newID)
        {
            app_id = newID;
            Initialized = false;
            Initialize();
        }

    }
}
