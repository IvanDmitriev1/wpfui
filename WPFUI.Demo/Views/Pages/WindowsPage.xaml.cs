// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using WPFUI.Demo.Views.Windows;
using WPFUI.DIControls.Interfaces;

namespace WPFUI.Demo.Views.Pages;

/// <summary>
/// Interaction logic for WindowsPage.xaml
/// </summary>
public partial class WindowsPage : Page, INavigable
{
    private readonly IServiceProvider _serviceProvider;

    public WindowsPage(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        _serviceProvider = serviceProvider;
    }

    private void CardStore_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        _serviceProvider.GetRequiredService<Store>().Show();
    }

    private void CardXbox_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        _serviceProvider.GetRequiredService<Xbox>().Show();
    }

    private void CardEditor_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        _serviceProvider.GetRequiredService<Editor>().Show();
    }

    private void CardBackdrop_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        _serviceProvider.GetRequiredService<Backdrop>().Show();
    }

    public void OnNavigationRequest(INavigation navigation, INavigationItem previousNavigationItem, ref object[] ars)
    {
        System.Diagnostics.Debug.WriteLine("Page with window selectors loaded.");
    }
}