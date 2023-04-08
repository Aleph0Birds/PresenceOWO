using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using DiscordRPC;
using MaterialDesignThemes.Wpf;
using PresenceOWO.DoRPC;

namespace PresenceOWO.ViewModels
{
    public class RPVM : VMBase
    {

        private readonly object[] timestampElements;
        private DateTime selectedDate;
        private DateTime selectedTime;
        private bool showTimeContainer;

        public RPArgs Args { get; set; }

        public VMCommand UpdatePresence { get; init; }

        public VMCommand UpdateVisibility { get; init; }

        public VMCommand InitTimeElements { get; init; }

        public bool TimestampBoxEnabled { get; set; }
        public bool CustomTimeSelectEnabled { get; set; }

        /// <summary>
        /// Local Date
        /// </summary>
        public DateTime SelectedDate 
        { 
            get => selectedDate; 
            set {
                selectedDate = value;
                OnPropChanged(nameof(SelectedDate));
            }
        }

        /// <summary>
        /// Local Time
        /// </summary>
        public DateTime SelectedTime 
        { 
            get => selectedTime;
            set
            {
                selectedTime = value;
                OnPropChanged(nameof(SelectedTime));
            }
        }

        public RPVM()
        {

            UpdatePresence = new VMCommand(updateClient);
            UpdateVisibility = new VMCommand(UpdateElementsVisibilityOnChanged,
                (param) => param != null);
            InitTimeElements = new VMCommand(InitTimestampElements,
                (param) => !(param as object[]).Any(e => e == null));

            timestampElements = new object[4];

            RPArgs.ViewModel = this;
            
            // Arguments Initialization
            Args = new RPArgs()
            {
                Details = "OWO",
                State = "owo?",
                LargeImageText = "! OWO !",
                SmallImageText = ".w.",
                TimestampModeNumber = 1,
            };
            showTimeContainer = true;
            SelectedDate = DateTime.Today.ToLocalTime();
            SelectedTime = DateTime.Now;

            PropertyChanged += OnPropChangedHandle;
        }

        private void updateClient(object obj)
        {
            Client.HandleUpdate(this, Args);
        }

        private void UpdateElementsVisibilityOnChanged(object obj)
        {
            ComboBox comboBox = obj as ComboBox;
            if (!comboBox.IsLoaded || comboBox.SelectedIndex == -1) return;

            OnTimestampModeChanged(comboBox.SelectedIndex);
            UpdateTimeElementVisibility();
        }

        private void UpdateTimeElementVisibility()
        {
            TextBox timestampTextBox = timestampElements[0] as TextBox;
            Grid timeContainer = timestampElements[1] as Grid;

            timestampTextBox.IsEnabled = TimestampBoxEnabled;
            timestampTextBox.Text = Args.TimestampModeNumber == 0 ? "" : Args.Timestamp.ToString();
            timeContainer.Visibility = (Visibility)(1 - BoolTooInt(showTimeContainer));

            if (!showTimeContainer) return;

            DatePicker datePicker = timestampElements[2] as DatePicker;
            TimePicker timePicker = timestampElements[3] as TimePicker;

            datePicker.IsEnabled = CustomTimeSelectEnabled;
            timePicker.IsEnabled = CustomTimeSelectEnabled;

        }

        public void UpdateTimestampText(string newValue)
        {
            (timestampElements[0] as TextBox).Text = newValue;
        }

        private int BoolTooInt(bool b)
        {
            return b ? 1 : 0;
        }

        private void OnTimestampModeChanged(int modeNumber)
        {
            switch (modeNumber)
            {
                case 0: //None
                    Args.Timestamp = 0;
                    TimestampBoxEnabled = false;
                    CustomTimeSelectEnabled = false;
                    showTimeContainer = false;
                    break;
                //
                case 1:
                    Args.Timestamp = ArgDoing.StartTime;
                    TimestampBoxEnabled = true;
                    CustomTimeSelectEnabled = false;
                    showTimeContainer = true;
                    break;
                case 2:
                    Args.Timestamp = ArgDoing.LastUpdateTime ?? ArgDoing.StartTime;
                    TimestampBoxEnabled = true;
                    CustomTimeSelectEnabled = false;
                    showTimeContainer = true;
                    break;

                // Custom
                case 3:
                case 4:
                    Args.Timestamp = ArgDoing.DateTimeToTimestamp(SelectedDate, SelectedTime);
                    CustomTimeSelectEnabled = true;
                    TimestampBoxEnabled = true;
                    showTimeContainer = true;
                    break;

                default:
                    throw new Exception("Timestamp mode not supported.");
            }


        }

        private void InitTimestampElements(object obj)
        {
            var stackPanel = (obj as object[])[1] as StackPanel;

            timestampElements[0] = stackPanel.FindName("TimestampBox");
            timestampElements[1] = stackPanel.FindName("TimeContainer");
            timestampElements[2] = stackPanel.FindName("Calendar");
            timestampElements[3] = stackPanel.FindName("Clock");

            OnTimestampModeChanged(Args.TimestampModeNumber);
            UpdateTimeElementVisibility();
        }

        private void OnPropChangedHandle(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(SelectedDate) &&
                e.PropertyName != nameof(SelectedTime))
                return;


            Args.Timestamp = ArgDoing.DateTimeToTimestamp(SelectedDate, SelectedTime);
            UpdateTimestampText(Args.Timestamp.ToString());
        }

    }

}
