﻿// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WPFUI.DIControls;

namespace WPFUI.Demo.Views
{
    /// <summary>
    /// Interaction logic for Container.xaml
    /// </summary>
    public partial class Container : Window
    {
        public Container(Dialog dialog, Snackbar snackbar, DefaultNavigation navigation)
        {
            WPFUI.Background.Manager.Apply(this);

            InitializeComponent();

            //RootTitleBar.CloseActionOverride = CloseActionOverride;

            RootDialog.Content =  dialog;
            RootSnackbar.Content = snackbar;

            RootNavigation.Content = navigation;
            Breadcrumb.Navigation = navigation;

            navigation.AddFrame(RootFrame);

            Task.Run(async () =>
            {
                await Task.Delay(4000);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    RootWelcomeGrid.Visibility = Visibility.Hidden;
                    RootGrid.Visibility = Visibility.Visible;
                });
            });

            //RootTitleBar.NotifyIconMenu = new ContextMenu();
            //RootTitleBar.NotifyIconMenu.Items.Add(new MenuItem() { Header = "Test #1" });
        }

        private void CloseActionOverride(WPFUI.Controls.TitleBar titleBar, Window window)
        {
            Application.Current.Shutdown();
        }

        private void TitleBar_OnMinimizeClicked(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Minimize button clicked");
        }

        private void TrayMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is not MenuItem menuItem) return;

            string tag = menuItem.Tag as string ?? String.Empty;

            System.Diagnostics.Debug.WriteLine("Menu item clicked: " + tag);
        }

        private void RootTitleBar_OnNotifyIconClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Notify Icon clicked");
        }

        private void RootNavigation_OnNavigatedForward(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Navigated forward");
        }

        private void RootNavigation_OnNavigatedBackward(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Navigated backward");
        }
    }
}
