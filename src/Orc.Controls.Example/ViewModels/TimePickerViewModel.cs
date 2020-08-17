namespace Orc.Controls.Example.ViewModels
{
    using System.ComponentModel;
    using System.Threading.Tasks;
    using System.Windows;
    using Catel.MVVM;
    using Orc.Controls;

    public partial class TimePickerViewModel : ViewModelBase
    {
        private AnalogueTime _time;


        public TimePickerViewModel()
        {
            _time = new AnalogueTime(0, 0, Meridiem.AM);
            SetAmPm = new Command(OnSetAmPm);
        }

#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

        public AnalogueTime Time
        {
            get { return _time; }
            set
            {
                if (!_time.Equals(value))
                {
                    _time = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Time)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DigitalTime)));
                }
            }
        }

        public Meridiem AmPm { get; set; }

        public DigitalTime MinTime { get { return new DigitalTime(9, 0); } }
        public DigitalTime MaxTime { get { return new DigitalTime(21, 0); } }

        public DigitalTime DigitalTime
        {
            get { return Time.ToDigitalTime(); }
            set
            {
                Time = value.ToAnalogueTime();
            }
        }

        public Command SetAmPm { get; }

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            
        }
        private void OnSetAmPm()
        {
            if (Time.Meridiem == Meridiem.AM)
            {
                Time = new AnalogueTime(Time.Hour, Time.Minute, Meridiem.PM);
            }
            else
            {
                Time = new AnalogueTime(Time.Hour, Time.Minute, Meridiem.AM);
            }

        }
    }
}
