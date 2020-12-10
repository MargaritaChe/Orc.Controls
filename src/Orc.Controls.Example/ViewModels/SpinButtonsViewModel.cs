// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpinButtons.cs" company="">
// Clock-like TimePicker control https://github.com/roy-t/TimePicker
// </copyright>
//---------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Example.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using Catel.Data;
    using Catel.MVVM;
    using Enums;

    public class SpinButtonsViewModel : ViewModelBase
    {
        #region Constructors
        public SpinButtonsViewModel()
        {
            Val = 1.0;
            Max = 5.0;
            Min = 0.0;
            Step = 1.0;
            Orientation = SpinButtonsOrientation.Vertical;
        }
        #endregion

        #region Properties
        public double Val { get; set; }
        public double Max { get; set; }
        public double Min { get; set; }
        public double Step { get; set; }
        public SpinButtonsOrientation Orientation { get; set; }
        #endregion

        #region Methods
        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();
        }
        protected override void OnPropertyChanged(AdvancedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
        }
        #endregion
    }
}
