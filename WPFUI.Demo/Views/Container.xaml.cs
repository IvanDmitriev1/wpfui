// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WPFUI.Controls;
using WPFUI.DIControls;
using Dialog = WPFUI.DIControls.Dialog;
using Snackbar = WPFUI.DIControls.Snackbar;


namespace WPFUI.Demo.Views
{
    /// <summary>
    /// Interaction logic for Container.xaml
    /// </summary>
    public partial class Container : Window
    {
        public Container(Dialog dialog, Snackbar snackbar, DefaultNavigation navigation)
        {
            //WPFUI.Appearance.Background.Apply(this, WPFUI.Appearance.BackgroundType.Mica);

            InitializeComponent();

            RootDialog.Content =  dialog;
            RootSnackbar.Content = snackbar;

            RootTitleBar.Navigation = navigation;
            RootNavigation.Content = navigation;
            Breadcrumb.Navigation = navigation;

            navigation.AddFrame(RootFrame);
            Loaded += (sender, args) =>
            {
                WPFUI.Appearance.Watcher.Watch(this, Appearance.BackgroundType.Mica, true);
            };

            //RootTitleBar.CloseActionOverride = CloseActionOverride;

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

        private void ThemeOnChanged(WPFUI.Appearance.ThemeType currentTheme, Color systemAccent)
        {
            System.Diagnostics.Debug.WriteLine($"DEBUG | {typeof(Container)} was informed that the theme has been changed to {currentTheme}", "WPFUI.Demo");
        }

        private void CloseActionOverride(TitleBar titleBar, Window window)
        {
            Application.Current.Shutdown();
        }

        private void RootDialog_LeftButtonClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("DEBUG | Root dialog action button was clicked!", "WPFUI.Demo");
        }

        private void RootDialog_RightButtonClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("DEBUG | Root dialog custom right button was clicked!", "WPFUI.Demo");
        }

        private void TitleBar_OnMinimizeClicked(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("DEBUG | Minimize button clicked", "WPFUI.Demo");
        }

        private void TrayMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is not MenuItem menuItem) return;

            string tag = menuItem.Tag as string ?? String.Empty;

            System.Diagnostics.Debug.WriteLine("DEBUG | Menu item clicked: " + tag, "WPFUI.Demo");
        }

        private void RootTitleBar_OnNotifyIconClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("DEBUG | Notify Icon clicked", "WPFUI.Demo");
        }

        private void RootNavigation_OnNavigatedForward(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("DEBUG | Navigated forward", "WPFUI.Demo");
        }

        private void RootNavigation_OnNavigatedBackward(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("DEBUG | Navigated backward", "WPFUI.Demo");
        }
    }
}
