using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using PresenceOWO.ViewModels;
using PresenceOWO.DoRPC;
using PresenceOWO.OWOSystem;
using System.Windows.Forms;
using System.Drawing;

namespace PresenceOWO
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly WindowInteropHelper helper;
        public MainWindow()
        {
            InitializeComponent();
            helper = new WindowInteropHelper(this);
            ToolTipService.ShowDurationProperty.OverrideMetadata(
                typeof(DependencyObject), new FrameworkPropertyMetadata(int.MaxValue));
        }

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hwnd, int msg, int wParam, int lParam);

        private void ControlBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SendMessage(helper.Handle, 161, 2, 0);
        }

        private void ControlBar_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SettingsManager.Settings.HideOnClosed)
            {
                Hide();
                myNotifyIcon.Visibility = Visibility.Visible;
                return;
            }
            System.Windows.Application.Current.Shutdown();
        }

        private void MaximizeBtn_Click(object sender, RoutedEventArgs e)
        {
            if(WindowState == WindowState.Normal) 
                WindowState = WindowState.Maximized;
            else 
                WindowState = WindowState.Normal;
        }

        private void MinimizeBtn_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}
