// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpinButtons.cs" company="WildGums">
//   Copyright (c) 2008 - 2020 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.Controls
{
    using System;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using Catel.Logging;

    [TemplatePart(Name = "PART_Grid", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_ValueTextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_StackPanel", Type = typeof(StackPanel))]
    [TemplatePart(Name = "PART_ButtonUp", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "PART_UpArrow", Type = typeof(Path))]
    [TemplatePart(Name = "PART_UpArrowBorder", Type = typeof(Border))]
    [TemplatePart(Name = "PART_ButtonDown", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "PART_DownArrow", Type = typeof(Path))]
    [TemplatePart(Name = "PART_DownArrowBorder", Type = typeof(Border))]

    public class SpinButtons : Control
    {
        #region Fields

        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private RepeatButton _buttonUp;
        private RepeatButton _buttonDown;

        #endregion

        #region Constructors

        static SpinButtons()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SpinButtons),
                new FrameworkPropertyMetadata(typeof(SpinButtons)));
        }

        #endregion

        #region Dependency properties

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(double), typeof(SpinButtons), new PropertyMetadata(0.0));

        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register(nameof(MaxValue), typeof(double), typeof(SpinButtons), new PropertyMetadata(0.0, (sender, e) => ((SpinButtons)sender).OnMaxValueChanged()));

        public double MinValue
        {
            get { return (double)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register(nameof(MinValue), typeof(double), typeof(SpinButtons), new PropertyMetadata(0.0, (sender, e) => ((SpinButtons)sender).OnMinValueChanged()));

        public double Step
        {
            get { return (double)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }

        public static readonly DependencyProperty StepProperty =
            DependencyProperty.Register(nameof(Step), typeof(double), typeof(SpinButtons), new PropertyMetadata(0.0));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(SpinButtons), new PropertyMetadata(Orientation.Vertical, (sender, e) => ((SpinButtons)sender).OnOrientationChanged()));

        public DropdownArrowLocation UpArrowLocation
        {
            get { return (DropdownArrowLocation)GetValue(UpArrowLocationProperty); }
            set { SetValue(UpArrowLocationProperty, value); }
        }

        public static readonly DependencyProperty UpArrowLocationProperty = 
            DependencyProperty.Register(nameof(UpArrowLocation), typeof(DropdownArrowLocation), typeof(SpinButtons), new PropertyMetadata(DropdownArrowLocation.Top));

        public DropdownArrowLocation DownArrowLocation
        {
            get { return (DropdownArrowLocation)GetValue(DownArrowLocationProperty); }
            set { SetValue(DownArrowLocationProperty, value); }
        }

        public static readonly DependencyProperty DownArrowLocationProperty =
            DependencyProperty.Register(nameof(DownArrowLocation), typeof(DropdownArrowLocation), typeof(SpinButtons), new PropertyMetadata(DropdownArrowLocation.Bottom));

        #endregion

        #region Methods

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _buttonUp = GetTemplateChild("PART_ButtonUp") as RepeatButton;
            if (_buttonUp is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_ButtonUp'");
            }
            _buttonUp.Click += OnUpButtonClick;

            _buttonDown = GetTemplateChild("PART_ButtonDown") as RepeatButton;
            if (_buttonDown is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_ButtonDown'");
            }
            _buttonDown.Click += OnDownButtonClick;
        }

            private void OnUpButtonClick(object sender, RoutedEventArgs e)
            {
                if (Value + Step <= MaxValue)
                {
                    Value += Step;
                }
            }

            private void OnDownButtonClick(object sender, RoutedEventArgs e)
            {
                if (Value - Step >= MinValue)
                {
                    Value -= Step;
                }
            }

        private void OnOrientationChanged()
        {
            if (Orientation == Orientation.Horizontal)
            {
                SetCurrentValue(UpArrowLocationProperty, DropdownArrowLocation.Top);
                SetCurrentValue(DownArrowLocationProperty, DropdownArrowLocation.Bottom);
            }
            if (Orientation == Orientation.Vertical)
            {
                SetCurrentValue(UpArrowLocationProperty, DropdownArrowLocation.Right);
                SetCurrentValue(DownArrowLocationProperty, DropdownArrowLocation.Left);
            }
        }

        private void OnMaxValueChanged()
        {
            if (Value > MaxValue)
            {
                SetCurrentValue(ValueProperty, MaxValue);
            }
        }

        private void OnMinValueChanged()
        {
            if (Value < MinValue)
            {
                SetCurrentValue(ValueProperty, MinValue);
            }
        }
        #endregion
    }
}
