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

        public PreferenceVM() 
        {
            // Keep
        }
    }
}
