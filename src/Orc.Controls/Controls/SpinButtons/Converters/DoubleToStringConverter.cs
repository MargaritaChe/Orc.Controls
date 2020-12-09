// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpinButtonsOrientationConverter.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using Catel;
    using Catel.MVVM.Converters;

    /// <summary>
    /// The  converter.
    /// </summary>
    [System.Windows.Data.ValueConversion(typeof(double), typeof(string))]
    public class DoubleToStringConverter : ValueConverterBase
    {
        #region Methods
        /// <summary>
        /// Converts value <see cref="double"/> values into <see cref="string"/>.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The parementer</param>
        /// <returns>
        /// A converted value.
        /// </returns>
        protected override object Convert(object value, Type targetType, object parameter)
        {
            return value == null ? null : value.ToString();
        }
        #endregion
    }
}
