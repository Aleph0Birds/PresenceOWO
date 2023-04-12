using Hardcodet.Wpf.TaskbarNotification;
using PresenceOWO.DoRPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PresenceOWO.ViewModels
{
    public class NotifyIconVM : VMBase
    {
        //private Visibility iconVisible;

        //public Visibility IconVisible 
        //{ 
        //    get => iconVisible; 
        //    set
        //    {
        //        iconVisible = value;
        //        OnPropChanged(nameof(IconVisible));
        //    }
        //}

        public VMCommand ShowWindow { get; init; }
        public VMCommand ExitApp { get; init; }

        public NotifyIconVM()
        {
            ShowWindow = new VMCommand(showWindow);
            ExitApp = new VMCommand(handleExit);
            //IconVisible = Visibility.Hidden;
        }

        private void showWindow(object obj)
        {
            (Application.Current.MainWindow.FindName("myNotifyIcon") as TaskbarIcon)
                .Visibility = Visibility.Collapsed;
            Application.Current.MainWindow.Show();
        }

        private void handleExit(object _)
        {
            Application.Current.Shutdown();
        }

         
    }
}
