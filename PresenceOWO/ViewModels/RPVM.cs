using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DiscordRPC;
using MaterialDesignThemes.Wpf;
using PresenceOWO.DoRPC;

namespace PresenceOWO.ViewModels
{
    public class RPVM : VMBase
    {

        public RPArgs Args { get; set; }

        public VMCommand UpdatePresence { get; init; }


        public RPVM()
        {
            UpdatePresence = new VMCommand(Update);
            Args = new RPArgs() {
                Details = "OWO",
                State = "owo?"
            };
        }
        private void Update(object obj)
        {
            Client.HandleUpdate(this, Args);
        }
    }

}
