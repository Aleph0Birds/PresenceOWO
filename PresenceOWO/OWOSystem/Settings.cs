using Newtonsoft.Json;

namespace PresenceOWO.OWOSystem
{
    [JsonObject]
    public class Settings : ViewModels.VMBase
    {

        public bool HideOnClosed { get; set; }
        
    }
}
