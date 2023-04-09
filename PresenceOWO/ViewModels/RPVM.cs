using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
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
        private readonly DispatcherTimer updateTimer;
        private DateTime? selectedDateTime;
        private bool customTimeSelectEnabled;
        private bool timestampBoxEnabled;
        private string timerString;

        public RPArgs Args { get; set; }

        public VMCommand UpdatePresence { get; init; }

        public VMCommand UpdateVisibility { get; init; }

        public VMCommand InitTimeElements { get; init; }

        public bool TimestampBoxEnabled
        {
            get => timestampBoxEnabled;
            set
            {
                timestampBoxEnabled = value;
                OnPropChanged(nameof(TimestampBoxEnabled));
            }
        }
        public bool CustomTimeSelectEnabled
        {
            get => customTimeSelectEnabled;
            set
            {
                customTimeSelectEnabled = value;
                OnPropChanged(nameof(CustomTimeSelectEnabled));
            }
        }

        public string TimerString 
        {
            get => timerString;
            set
            {
                timerString = value;
                OnPropChanged(nameof(TimerString));
            }
        }

        /// <summary>
        /// Local Date
        /// </summary>
        public DateTime SelectedDate
        {
            get => selectedDate;
            set
            {
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

            updateTimer = new DispatcherTimer();
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Interval = new TimeSpan(0, 0, 1);

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

            updateTimer.IsEnabled = true;
            updateTimer.Start();
            PropertyChanged += OnPropChangedHandle;
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            UpdateTimerText();
        }

        private void UpdateTimerText()
        {
            if (Args.TimestampModeNumber == 0 || !selectedDateTime.HasValue)
                return;

            TimeSpan span;
            if (Args.TimestampModeNumber == 4) // until custom
            {
                if (selectedDateTime < DateTime.Now) 
                    span = TimeSpan.Zero;
                else
                    span = (TimeSpan)(selectedDateTime - DateTime.Now);
                
                TimerString = $"{span:hh\\:mm\\:ss} left";
            }
            else
            {

                if (selectedDateTime > DateTime.Now)
                    span = TimeSpan.Zero;
                else
                    span = (TimeSpan)(DateTime.Now - selectedDateTime);
                TimerString = $"{span:hh\\:mm\\:ss} elapsed";
            }
        }

        private void updateClient(object obj)
        {
            Client.HandleUpdate(this, Args);
        }

        private void UpdateElementsVisibilityOnChanged(object obj)
        {
            ComboBox comboBox = obj as ComboBox;
            if (!comboBox.IsLoaded || comboBox.SelectedIndex == -1)
            {
                updateTimer.Stop();
                updateTimer.IsEnabled = false;
                return;
            }
            updateTimer.IsEnabled = true;

            OnTimestampModeChanged(comboBox.SelectedIndex);
            UpdateTimeElementVisibility();
        }

        private void UpdateTimeElementVisibility()
        {
            TextBox timestampTextBox = timestampElements[0] as TextBox;
            Grid timeContainer = timestampElements[1] as Grid;

            timestampTextBox.Text = Args.TimestampModeNumber == 0 ? "" : Args.Timestamp.ToString();
            timeContainer.Visibility = (Visibility)(1 - BoolTooInt(showTimeContainer));
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
                    updateTimer.Stop();
                    TimerString = string.Empty;
                    selectedDateTime = null;
                    break;
                //
                case 1:
                    Args.Timestamp = ArgDoing.DateTimeToTimestamp(ArgDoing.StartTime);
                    selectedDateTime = ArgDoing.StartTime;
                    goto case 420;

                case 2:
                    var t = ArgDoing.LastUpdateTime ?? ArgDoing.StartTime;
                    Args.Timestamp = ArgDoing.DateTimeToTimestamp(t);
                    selectedDateTime = t;
                    goto case 420;

                case 420:
                    TimestampBoxEnabled = true;
                    CustomTimeSelectEnabled = false;
                    showTimeContainer = true;
                    goto case 69420;

                // Custom
                case 3:
                case 4:
                    Args.Timestamp = ArgDoing.DateTimeToTimestamp(SelectedDate, SelectedTime);
                    CustomTimeSelectEnabled = true;
                    TimestampBoxEnabled = true;
                    showTimeContainer = true;
                    selectedDateTime = ArgDoing.CombineDateTime(SelectedDate, SelectedTime);
                    goto case 69420;

                case 69420:
                    if(!updateTimer.IsEnabled) updateTimer.Start();
                    UpdateTimerText();
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

            selectedDateTime = ArgDoing.CombineDateTime(SelectedDate, SelectedTime);
            Args.Timestamp = ArgDoing.DateTimeToTimestamp(SelectedDate, SelectedTime);
            UpdateTimestampText(Args.Timestamp.ToString());
        }

    }

}
