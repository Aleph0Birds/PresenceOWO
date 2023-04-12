using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using PresenceOWO.DoRPC;

namespace PresenceOWO.Views
{
    /// <summary>
    /// RPView.xaml 的互動邏輯
    /// </summary>
    public partial class RPView : UserControl
    {
        private readonly Regex notNumberRegEx = new Regex(@"\D");
        public RPView()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = notNumberRegEx.IsMatch(e.Text);
        }
    }
}
