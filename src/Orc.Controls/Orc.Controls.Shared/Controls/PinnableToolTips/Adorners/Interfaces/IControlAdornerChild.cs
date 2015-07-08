﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IControlAdornerChild.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Windows;

    /// <summary>
    /// The ControlAdornerChild interface.
    /// </summary>
    internal interface IControlAdornerChild
    {
        /// <summary>
        /// The get position.
        /// </summary>
        /// <returns>The <see cref="Point" />.</returns>
        Point GetPosition();
    }
}