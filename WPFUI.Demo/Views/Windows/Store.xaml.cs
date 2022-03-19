// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;

namespace WPFUI.Demo.Views.Windows
{
    /// <summary>
    /// Interaction logic for Bubble.xaml
    /// </summary>
    public partial class Store : Window
    {
        public Store(StoreNavigation storeNavigation, StoreSnackbar snackbar, StoreDialog storeDialog)
        {
            WPFUI.Appearance.Background.Apply(this, WPFUI.Appearance.BackgroundType.Mica);

            InitializeComponent();

            _storeNavigation = storeNavigation;
            RootSnackbar.Content = snackbar;
            RootDialog.Content = storeDialog;

            RootNavigation.Content = storeNavigation;
            Breadcrumb.Navigation = storeNavigation;

            storeNavigation.AddFrame(RootFrame);
        }

        private readonly StoreNavigation _storeNavigation;
    }
}
