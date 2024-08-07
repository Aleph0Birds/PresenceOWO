﻿using DiscordRPC;
using Newtonsoft.Json;
using PresenceOWO.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresenceOWO.DoRPC
{
    [JsonObject]
    public class RPArgs : EventArgs
    {
        public string ApplicationID { get; set; }
        public string Details { get; set; }
        public string State { get; set; }
        public string LargeImageKey { get; set; }
        public string LargeImageText { get; set; }
        public string SmallImageKey { get; set; }
        public string SmallImageText { get; set; }
        public ulong Timestamp { get; set; }
        public int TimestampModeNumber { get; set; }

        // Buttons
        public string BtnText1 { get; set; }
        public string BtnUrl1 { get; set; }
        public string BtnText2 { get; set; }
        public string BtnUrl2 { get; set; }

        public byte EnabledButtons { get; set; }
    }

    public enum TimestampMode
    {
        None = 0,
        SinceStart = 1,
        SinceLastUpdate = 2,
        SinceCustom = 3,
        UntilCustom = 4
    }
}
