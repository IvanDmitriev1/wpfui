﻿// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;

namespace Wpf.Ui.Controls;

/// <summary>
/// Modern navigation styled according to the principles of Fluent Design for Windows 11.
/// </summary>
[ToolboxItem(true)]
[ToolboxBitmap(typeof(NavigationFluent), "NavigationFluent.bmp")]
public class NavigationFluent : Wpf.Ui.Controls.Navigation.NavigationBase
{
    /// <summary>
    /// Property for <see cref="IsExpanded"/>.
    /// </summary>
    public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(
        nameof(IsExpanded),
        typeof(bool), typeof(NavigationFluent),
        new PropertyMetadata(true));

    /// <summary>
    /// Property for <see cref="IsExpandButtonVisible"/>.
    /// </summary>
    public static readonly DependencyProperty IsExpandButtonVisibleProperty = DependencyProperty.Register(
        nameof(IsExpandButtonVisible),
        typeof(bool), typeof(NavigationFluent),
        new PropertyMetadata(false));

    /// <summary>
    /// Property for <see cref="TemplateButtonCommand"/>.
    /// </summary>
    public static readonly DependencyProperty TemplateButtonCommandProperty =
        DependencyProperty.Register(nameof(TemplateButtonCommand),
            typeof(Common.IRelayCommand), typeof(NavigationFluent), new PropertyMetadata(null));

    /// <summary>
    /// Gets or sets a value indicating whether the menu is expanded.
    /// </summary>
    public bool IsExpanded
    {
        get => (bool)GetValue(IsExpandedProperty);
        set => SetValue(IsExpandedProperty, value);
    }

    /// <summary>
    /// TODO
    /// </summary>
    public bool IsExpandButtonVisible
    {
        get => (bool)GetValue(IsExpandButtonVisibleProperty);
        set => SetValue(IsExpandButtonVisibleProperty, value);
    }

    /// <summary>
    /// Command triggered after clicking the button.
    /// </summary>
    public Common.IRelayCommand TemplateButtonCommand => (Common.IRelayCommand)GetValue(TemplateButtonCommandProperty);

    /// <summary>
    /// Creates new instance and sets default <see cref="TemplateButtonCommandProperty"/>.
    /// </summary>
    public NavigationFluent() : base() =>
        SetValue(TemplateButtonCommandProperty, new Common.RelayCommand(o => Button_OnClick(this, o)));

    private void Button_OnClick(object sender, object parameter)
    {
        if (parameter == null)
            return;

        string param = parameter as string ?? String.Empty;

#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO: {typeof(NavigationFluent)} button clicked with param: {param}", "Wpf.Ui.NavigationFluent");
#endif
        if (param == "hamburger")
            IsExpanded = !IsExpanded;
    }
}
