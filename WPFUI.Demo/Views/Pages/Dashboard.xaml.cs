// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WPFUI.DIControls;
using WPFUI.DIControls.Interfaces;

namespace WPFUI.Demo.Views.Pages
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page, INavigable
    {
        public Dashboard(DefaultNavigation navigation)
        {
            InitializeComponent();

            _navigation = navigation;
        }

        private readonly INavigation _navigation;

        private async void ActionCardIcons_Click(object sender, RoutedEventArgs e)
        {        
            await _navigation.NavigateTo(nameof(Icons));
        }

        private async void ActionCardColors_Click(object sender, RoutedEventArgs e)
        {
            await _navigation.NavigateTo(nameof(Colors));
        }

        private async void ActionCardControls_Click(object sender, RoutedEventArgs e)
        {
            await _navigation.NavigateTo(nameof(Controls));
        }

        public Task OnNavigationRequest(INavigation navigation, string previousPageTag, object[] ars)
        {
            System.Diagnostics.Debug.WriteLine("DEBUG | Navigated to dashboard", "WPFUI.Demo");

            return Task.CompletedTask;
        }
    }
}
