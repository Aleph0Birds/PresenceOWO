using static PresenceOWO.OWOSystem.SettingsManager;

namespace PresenceOWO.ViewModels
{
    public class PreferenceVM : VMBase
    {
        public bool _HideOnClosed
        {
            get => Settings.HideOnClosed;
            set
            {
                Settings.HideOnClosed = value;
                OnPropChanged(nameof(_HideOnClosed));
            }
        }

        public bool _LaunchOnStartup
        {
            get => Settings.LaunchOnStartup;
            set
            {
                Settings.LaunchOnStartup = value;
                if(value)
                    RegisterStartup();
                else
                    UnregisterStartup();
                OnPropChanged(nameof(_LaunchOnStartup));
            }
        }

        public PreferenceVM() 
        {
            // Keep
        }
    }
}
