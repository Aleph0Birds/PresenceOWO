using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public MainVM()
        {
            rpVM = new RPVM();
            preferenceVM = new PreferenceVM();

            ShowRPView = new VMCommand(_ShowRPView);
            ShowPreferenceView = new VMCommand(_ShowPrefernceView);

            _ShowRPView("OWO");
        }


        public void ShowView(VMBase baseVM, object obj = null)
        {
            CurrentChildView = baseVM;
        }

        public void _ShowRPView(object _) => ShowView(rpVM);
        public void _ShowPrefernceView(object _) => ShowView(preferenceVM);
    }
}
