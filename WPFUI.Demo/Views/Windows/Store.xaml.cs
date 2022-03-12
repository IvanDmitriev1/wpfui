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
        public Store(StoreNavigation storeNavigation)
        {
            WPFUI.Background.Manager.Apply(this);
            InitializeComponent();

            _storeNavigation = storeNavigation;

            RootNavigation.Content = storeNavigation;
            Breadcrumb.Navigation = storeNavigation;

            storeNavigation.AddFrame(RootFrame);
        }

        private readonly StoreNavigation _storeNavigation;
    }
}
