using Newtonsoft.Json.Linq;
using PresenceOWO.DoRPC;
using PresenceOWO.IO;
using PresenceOWO.OWOSystem;
using System;

namespace PresenceOWO.ViewModels
{
    public class MainVM : VMBase
    {
        private VMBase _currentChildView;

        private readonly RPVM rpVM;
        private readonly PreferenceVM preferenceVM;

        public VMBase CurrentChildView
        {
            get => _currentChildView; 
            set
            {
                _currentChildView = value;
                OnPropChanged(nameof(CurrentChildView));
            }
        }

        public VMCommand ShowRPView { get; init; }
        public VMCommand ShowPreferenceView { get; init; }
        public VMCommand OnClosing { get; init; }

        public MainVM()
        {
            rpVM = new RPVM();
            preferenceVM = new PreferenceVM();

            ShowRPView = new VMCommand(_ShowRPView);
            ShowPreferenceView = new VMCommand(_ShowPrefernceView);
            OnClosing = new VMCommand(HandleClosing);

            Initialize();
        }

        private void Initialize()
        {
            ArgDoing.StartTime = DateTime.Now;
            ArgDoing.LastUpdateTime = null;

            SavingManager.Initialize();
            SettingsManager.InitSettings();

            rpVM.Initialize();

            _ShowRPView("OWO");
        }

        private void HandleClosing (object _)
        {
            rpVM.OnWindowClosing();
            SettingsManager.SaveSettings();
        }


        public void ShowView(VMBase baseVM, object obj = null)
        {
            CurrentChildView = baseVM;
        }

        public void _ShowRPView(object _) => ShowView(rpVM);
        public void _ShowPrefernceView(object _) => ShowView(preferenceVM);
    }
}
