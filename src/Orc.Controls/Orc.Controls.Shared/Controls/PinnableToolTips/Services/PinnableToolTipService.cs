﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PinnableToolTipService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Interop;

    /// <summary>
    /// The pinnable toolTip service.
    /// </summary>
    public static class PinnableToolTipService
    {
        #region Constants
        /// <summary>
        /// The default initial show delay.
        /// </summary>
        private const int DefaultInitialShowDelay = 500;

        /// <summary>
        /// The default show duration.
        /// </summary>
        private const int DefaultShowDuration = 5000;

        /// <summary>
        /// The locker.
        /// </summary>
        private static readonly object Locker = new object();

        /// <summary>
        /// The mouse position.
        /// </summary>
        private static Point _mousePosition;

        /// <summary>
        /// The root visual.
        /// </summary>
        private static FrameworkElement _rootVisual;
        #endregion

        #region Static Fields
        /// <summary>
        /// The initial show delay property.
        /// </summary>
        public static readonly DependencyProperty InitialShowDelayProperty = DependencyProperty.RegisterAttached("InitialShowDelay",
            typeof (int), typeof (PinnableToolTipService), new PropertyMetadata(DefaultInitialShowDelay));

        /// <summary>
        /// The placement property.
        /// </summary>
        public static readonly DependencyProperty PlacementProperty = DependencyProperty.RegisterAttached("Placement",
            typeof (PlacementMode), typeof (PinnableToolTipService), new PropertyMetadata(PlacementMode.Mouse, OnPlacementPropertyChanged));

        /// <summary>
        /// The placement target property.
        /// </summary>
        public static readonly DependencyProperty PlacementTargetProperty = DependencyProperty.RegisterAttached("PlacementTarget",
            typeof (UIElement), typeof (PinnableToolTipService), new PropertyMetadata(OnPlacementTargetPropertyChanged));

        /// <summary>
        /// The show duration property.
        /// </summary>
        public static readonly DependencyProperty ShowDurationProperty = DependencyProperty.RegisterAttached("ShowDurartion",
            typeof (int), typeof (PinnableToolTipService), new PropertyMetadata(DefaultShowDuration));

        /// <summary>
        /// The toolTip property.
        /// </summary>
        public static readonly DependencyProperty ToolTipProperty = DependencyProperty.RegisterAttached("ToolTip",
            typeof (object), typeof (PinnableToolTipService), new PropertyMetadata(OnToolTipPropertyChanged));

        /// <summary>
        /// The toolTip owner property.
        /// </summary>
        public static readonly DependencyProperty IsToolTipOwnerProperty = DependencyProperty.RegisterAttached("IsToolTipOwner",
            typeof (bool), typeof (PinnableToolTipService), new PropertyMetadata(OnIsToolTipOwnerPropertyChanged));

        /// <summary>
        /// The elements and tool tips.
        /// </summary>
        private static readonly Dictionary<UIElement, PinnableToolTip> ElementsAndToolTips =
            new Dictionary<UIElement, PinnableToolTip>();
        #endregion

        #region Properties
        /// <summary>
        ///     Gets the mouse position.
        /// </summary>
        internal static Point MousePosition
        {
            get
            {
                lock (Locker)
                {
                    return _mousePosition;
                }
            }

            private set
            {
                lock (Locker)
                {
                    _mousePosition = value;
                }
            }
        }

        /// <summary>
        ///     Gets the root visual.
        /// </summary>
        internal static FrameworkElement RootVisual
        {
            get
            {
                lock (Locker)
                {
                    return _rootVisual;
                }
            }

            private set
            {
                lock (Locker)
                {
                    _rootVisual = value;
                }
            }
        }
        #endregion

        #region Public Methods and Operators
        /// <summary>
        /// The get initial show delay.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The <see cref="int" />.</returns>
        public static int GetInitialShowDelay(DependencyObject element)
        {
            return (int) element.GetValue(InitialShowDelayProperty);
        }

        /// <summary>
        /// The get placement.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The <see cref="PlacementMode" />.</returns>
        public static PlacementMode GetPlacement(DependencyObject element)
        {
            return (PlacementMode) element.GetValue(PlacementProperty);
        }

        /// <summary>
        /// The get placement target.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The <see cref="UIElement" />.</returns>
        public static UIElement GetPlacementTarget(DependencyObject element)
        {
            return (UIElement) element.GetValue(PlacementTargetProperty);
        }

        /// <summary>
        /// The get show duration.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The <see cref="int" />.</returns>
        public static int GetShowDuration(DependencyObject element)
        {
            return (int) element.GetValue(ShowDurationProperty);
        }

        /// <summary>
        /// The get toolTip.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The <see cref="object" />.</returns>
        public static object GetToolTip(DependencyObject element)
        {
            return element.GetValue(ToolTipProperty);
        }

        /// <summary>
        /// The get is toolTip owner.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The <see cref="object" />.</returns>
        public static bool GetIsToolTipOwner(DependencyObject element)
        {
            return (bool) element.GetValue(IsToolTipOwnerProperty);
        }

        /// <summary>
        /// The set initial show delay.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetInitialShowDelay(DependencyObject element, int value)
        {
            element.SetValue(InitialShowDelayProperty, value);
        }

        /// <summary>
        /// The set placement.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetPlacement(DependencyObject element, PlacementMode value)
        {
            element.SetValue(PlacementProperty, value);
        }

        /// <summary>
        /// The set placement target.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetPlacementTarget(DependencyObject element, UIElement value)
        {
            element.SetValue(PlacementTargetProperty, value);
        }

        /// <summary>
        /// The set show duration.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetShowDuration(DependencyObject element, int value)
        {
            element.SetValue(ShowDurationProperty, value);
        }

        /// <summary>
        /// The set toolTip.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetToolTip(DependencyObject element, object value)
        {
            element.SetValue(ToolTipProperty, value);
        }

        /// <summary>
        /// The set is toolTip owner.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetIsToolTipOwner(DependencyObject element, bool value)
        {
            element.SetValue(IsToolTipOwnerProperty, value);
        }
        #endregion

        #region Methods
        /// <summary>
        /// The convert to toolTip.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>The <see cref="PinnableToolTip" />.</returns>
        private static PinnableToolTip ConvertToToolTip(object p)
        {
            return p as PinnableToolTip ?? new PinnableToolTip {Content = p};
        }

        /// <summary>
        /// The framework element unloaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private static void FrameworkElementUnloaded(object sender, RoutedEventArgs e)
        {
            var element = (UIElement) sender;
            PinnableToolTip toolTip = null;
            lock (Locker)
            {
                if (ElementsAndToolTips.ContainsKey(element))
                {
                    toolTip = ElementsAndToolTips[element];
                }
            }

            if (toolTip != null)
            {
                toolTip.Hide();
            }
        }

        /// <summary>
        /// The on control enabled changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private static void OnControlEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool) e.NewValue)
            {
                return;
            }

            var element = (UIElement) sender;
            PinnableToolTip toolTip = null;
            lock (Locker)
            {
                if (ElementsAndToolTips.ContainsKey(element))
                {
                    toolTip = ElementsAndToolTips[element];
                }
            }

            if (toolTip != null)
            {
                toolTip.StopTimer();
            }
        }

        /// <summary>
        /// The on element mouse enter.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private static void OnElementMouseEnter(object sender, MouseEventArgs e)
        {
            UIElement currentElement;
            PinnableToolTip toolTip = null;

            lock (Locker)
            {
                currentElement = sender as UIElement;
                if (currentElement != null)
                {
                    if (ElementsAndToolTips.ContainsKey(currentElement))
                    {
                        toolTip = ElementsAndToolTips[currentElement];
                    }
                    else
                    {
                        return;
                    }
                }

                MousePosition = e.GetPosition(null);
                SetRootVisual();
            }

            if (toolTip == null || toolTip.Content == null || toolTip.IsTimerEnabled || toolTip.IsOpen)
            {
                return;
            }

            var initialShowDelay = GetInitialShowDelay(currentElement);
            var showDuration = GetShowDuration(currentElement);

            if (initialShowDelay == 0)
            {
                toolTip.Show();
            }
            else
            {
                toolTip.SetupTimer(initialShowDelay, showDuration);
                toolTip.StartTimer();
            }
        }

        /// <summary>
        /// The on element mouse leave.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private static void OnElementMouseLeave(object sender, MouseEventArgs e)
        {
            PinnableToolTip toolTip = null;

            lock (Locker)
            {
                var currentElement = (UIElement) sender;
                if (ElementsAndToolTips.ContainsKey(currentElement))
                {
                    toolTip = ElementsAndToolTips[currentElement];
                }
                else
                {
                    return;
                }
            }

            if (toolTip == null)
            {
                return;
            }

            if (!toolTip.IsOpen)
            {
                toolTip.StopTimer();
            }
        }

        /// <summary>
        /// The on placement property changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The e.</param>
        private static void OnPlacementPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (UIElement) d;

            PinnableToolTip toolTip = null;
            lock (Locker)
            {
                if (ElementsAndToolTips.ContainsKey(element))
                {
                    toolTip = ElementsAndToolTips[element];
                }
            }

            if (toolTip != null && toolTip.IsOpen)
            {
                toolTip.PerformPlacement();
            }
        }

        /// <summary>
        /// The on placement target property changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The e.</param>
        private static void OnPlacementTargetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (UIElement) d;

            PinnableToolTip toolTip = null;
            lock (Locker)
            {
                if (ElementsAndToolTips.ContainsKey(element))
                {
                    toolTip = ElementsAndToolTips[element];
                }
            }

            if (toolTip != null && toolTip.IsOpen)
            {
                toolTip.PerformPlacement();
            }
        }

        /// <summary>
        /// The on root visual mouse left button down.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private static void OnRootVisualMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            lock (Locker)
            {
                foreach (var toolTip in ElementsAndToolTips.Values.Where(toolTip => toolTip != null && !toolTip.IsPinned))
                {
                    if (ScreenHelper.IsParentOf(toolTip, e.OriginalSource as DependencyObject))
                    {
                        continue;
                    }

                    toolTip.Hide();
                }
            }
        }

        /// <summary>
        /// The on root visual mouse move.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private static void OnRootVisualMouseMove(object sender, MouseEventArgs e)
        {
            MousePosition = e.GetPosition(null);
        }

        /// <summary>
        /// The on is toolTip owner property changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The e.</param>
        private static void OnIsToolTipOwnerPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
            {
                UnregisterToolTip(d as UIElement);
            }

            if ((e.NewValue != null) && ((bool) e.NewValue))
            {
                RegisterToolTip(d as UIElement, null);
            }
        }

        /// <summary>
        /// The on toolTip property changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The e.</param>
        private static void OnToolTipPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
            {
                UnregisterToolTip(d as UIElement);
            }

            if (e.NewValue != null)
            {
                RegisterToolTip(d as UIElement, e.NewValue);
            }
        }

        /// <summary>
        /// The register toolTip.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="p">The p.</param>
        private static void RegisterToolTip(UIElement owner, object p)
        {
            if (owner == null)
            {
                return;
            }

            lock (Locker)
            {
                if (ElementsAndToolTips.ContainsKey(owner))
                {
                    return;
                }
            }

            var element = owner as FrameworkElement;
            PinnableToolTip toolTip = null;

            if (p != null)
            {
                toolTip = p as PinnableToolTip ?? ConvertToToolTip(p);
                toolTip.Owner = owner;
            }

            if (element != null)
            {
                element.Unloaded += FrameworkElementUnloaded;
                if (toolTip != null)
                {
                    toolTip.DataContext = element.DataContext;
                }
            }

            owner.MouseEnter += OnElementMouseEnter;
            owner.MouseLeave += OnElementMouseLeave;

            var control = owner as Control;
            if (control != null)
            {
                control.IsEnabledChanged += OnControlEnabledChanged;
            }

            lock (Locker)
            {
                ElementsAndToolTips[owner] = toolTip;
            }
        }

        /// <summary>
        ///     The set root visual.
        /// </summary>
        private static void SetRootVisual()
        {
            lock (Locker)
            {
                if ((RootVisual != null) || (Application.Current == null))
                {
                    return;
                }

                RootVisual = BrowserInteropHelper.IsBrowserHosted ? null :
                    (Application.Current.MainWindow.Content as FrameworkElement) != null ?
                        Application.Current.MainWindow.Content as FrameworkElement : Application.Current.MainWindow;

                if (RootVisual == null)
                {
                    return;
                }

                RootVisual.MouseMove += OnRootVisualMouseMove;
                RootVisual.AddHandler(
                    UIElement.MouseLeftButtonDownEvent,
                    new MouseButtonEventHandler(OnRootVisualMouseLeftButtonDown),
                    true);
            }
        }

        /// <summary>
        /// The unregister toolTip.
        /// </summary>
        /// <param name="owner">The owner.</param>
        private static void UnregisterToolTip(UIElement owner)
        {
            try
            {
                if (owner == null)
                {
                    return;
                }

                lock (Locker)
                {
                    PinnableToolTip toolTip = null;
                    if (ElementsAndToolTips.ContainsKey(owner))
                    {
                        toolTip = ElementsAndToolTips[owner];
                    }
                    else
                    {
                        return;
                    }

                    var element = owner as FrameworkElement;
                    if (element != null)
                    {
                        element.Unloaded -= FrameworkElementUnloaded;
                        if (toolTip != null)
                        {
                            toolTip.DataContext = null;
                        }
                    }

                    owner.MouseEnter -= OnElementMouseEnter;
                    owner.MouseLeave -= OnElementMouseLeave;

                    var control = owner as Control;
                    if (control != null)
                    {
                        control.IsEnabledChanged -= OnControlEnabledChanged;
                    }

                    if (toolTip != null)
                    {
                        toolTip.Hide();
                        toolTip.Owner = null;
                    }

                    ElementsAndToolTips.Remove(owner);
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion
    }
}