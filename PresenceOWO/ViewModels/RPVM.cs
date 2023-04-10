using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;
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
        private bool stopPresenceButtonEnabled;
        private Stack<object> buttonStack;
        private bool addBtnBtnEnabled;
        private bool button1Enabled;
        private bool button2Enabled;

        public RPArgs Args { get; set; }

        public VMCommand UpdatePresence { get; init; }

        public VMCommand UpdateVisibility { get; init; }

        public VMCommand InitTimeElements { get; init; }
        public VMCommand StopPresence { get; init; }
        public VMCommand OpenURL { get; init; }
        public VMCommand CreateButton { get; init; }

        public VMCommand RemoveButton { get; init; }

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

        public bool StopPresenceButtonEnabled
        {
            get => stopPresenceButtonEnabled;
            set
            {
                stopPresenceButtonEnabled = value;
                OnPropChanged(nameof(StopPresenceButtonEnabled));
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

        public bool AddBtnBtnEnabled
        {
            get => addBtnBtnEnabled;
            set
            {
                addBtnBtnEnabled = value;
                OnPropChanged(nameof(AddBtnBtnEnabled));
            }
        }

        public bool Button1Enabled
        {
            get => button1Enabled;
            set
            {
                button1Enabled = value;
                OnPropChanged(nameof(Button1Enabled));
            }
        }
        public bool Button2Enabled
        {
            get => button2Enabled;
            set
            {
                button2Enabled = value;
                OnPropChanged(nameof(Button2Enabled));
            }
        }

        public RPVM()
        {
            #region Initialize VMCommands
            UpdatePresence = new VMCommand(updateClient);
            UpdateVisibility = new VMCommand(UpdateElementsVisibilityOnChanged,
                ArgDoing.NotNull);
            InitTimeElements = new VMCommand(InitTimestampElements,
                param => !(param as object[]).Any(e => e == null));
            StopPresence = new VMCommand(stopPresence);
            OpenURL = new VMCommand(openURL, ArgDoing.NotNullAs<string>);
            CreateButton = new VMCommand(createButton, ArgDoing.NotNullAs<StackPanel>);
            RemoveButton = new VMCommand(removedButton, ArgDoing.NotNullAs<StackPanel>);
            #endregion

            timestampElements = new object[2];
            buttonStack = new Stack<object>(2);

            updateTimer = new DispatcherTimer();
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Interval = new TimeSpan(0, 0, 1);

            Client.InPresence = false;

            // Arguments Initialization
            Args = new RPArgs()
            {
                Details = "OWO",
                State = "owo?",
                LargeImageText = "! OWO !",
                SmallImageText = ".w.",
                TimestampModeNumber = 1,
                BtnText1 = "Click to see pussy",
                BtnUrl1 = @"https://www.youtube.com/watch?v=uwmeH6Rnj2E",
            };

            showTimeContainer = true;
            SelectedDate = DateTime.Today.ToLocalTime();
            SelectedTime = DateTime.Now;
            StopPresenceButtonEnabled = false;
            AddBtnBtnEnabled = true;

            updateTimer.IsEnabled = true;
            updateTimer.Start();
            PropertyChanged += OnPropChangedHandle;
        }

        private void removedButton(object obj)
        {
            if (buttonStack.Count <= 0) return;
            StackPanel buttonStackPanel = obj as StackPanel;

            if (buttonStack.Count == 2)
            {
                Button2Enabled = false;
                AddBtnBtnEnabled = true;
            }
            else Button1Enabled = false;

            buttonStackPanel.Children.Remove(buttonStack.Pop() as Button);

        }

        private void createButton(object obj)
        {
            if (buttonStack.Count >= 2) return; // Max 2 buttons

            StackPanel buttonStackPanel = obj as StackPanel;
            // ah yes nice butt
            Button butt = new Button();
            butt.Style = buttonStackPanel.FindResource("PresenceButton") as Style;
            string contentBindPath;
            string propName;

            if (buttonStack.Count == 1)
            {
                contentBindPath = "Args.BtnText2";
                propName = "Args.BtnUrl2";
                Button2Enabled = true;
                AddBtnBtnEnabled = false;
            }
            else // empty
            {
                contentBindPath = "Args.BtnText1";
                propName = "Args.BtnUrl1";
                Button1Enabled = true;
            }

            butt.SetBinding(Button.ContentProperty, new Binding(contentBindPath));
            butt.SetBinding(Button.CommandProperty, new Binding(nameof(OpenURL)));
            butt.SetBinding(Button.CommandParameterProperty, new Binding(propName));
            butt.ToolTip = new ToolTip() { Content = "Right click to remove a button" };

            buttonStackPanel.Children.Insert(buttonStack.Count, butt);
            buttonStack.Push(butt);

        }

        public void openURL(object obj)
        {
            if (string.IsNullOrEmpty(obj as string)) return;
            Process.Start(obj as string);
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
            Args.EnabledButtons = (byte)buttonStack.Count;
            Client.HandleUpdate(this, Args);
            StopPresenceButtonEnabled = Client.InPresence;
        }

        private void stopPresence(object obj)
        {
            Client.StopPresence();
            StopPresenceButtonEnabled = Client.InPresence;
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
            timeContainer.Visibility = (Visibility)(1 - BoolToByte(showTimeContainer));
        }

        public void UpdateTimestampText(string newValue)
        {
            (timestampElements[0] as TextBox).Text = newValue;
        }

        public void UpdateTimestampText()
        {
            UpdateTimestampText(Args.Timestamp.ToString());
        }

        private unsafe byte BoolToByte(bool b)
        {
            return *((byte*)&b);
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
                    if (!updateTimer.IsEnabled) updateTimer.Start();
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
