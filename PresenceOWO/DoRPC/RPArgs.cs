using DiscordRPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresenceOWO.DoRPC
{
    public class RPArgs : EventArgs
    {
        public string ApplicationID { get; set; }
        public string Details { get; set; }
        public string State { get; set; }
        public string LargeImageKey { get; set; }
        public string LargeImageText { get; set; }
        public string SmallImageKey { get; set; }
        public string SmallImageText { get; set; }
        public long StartTimeStamp { get; set; }
        public long EndTimeStamp { get; set; }



        /// <summary>
        /// test
        /// </summary>
        void asd()
        {
            new RichPresence() { };
        }
    }
}
