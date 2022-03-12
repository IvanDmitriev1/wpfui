// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WPFUI.Common;
using WPFUI.Demo.Views;
using WPFUI.Demo.Views.Pages;
using WPFUI.Demo.Views.Windows;
using WPFUI.DIControls;
using WPFUI.DIControls.Interfaces;

namespace WPFUI.Demo;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        _host = Host.CreateDefaultBuilder().ConfigureServices(ConfigureServices).Build();
    }

    private void ConfigureServices(IServiceCollection collection)
    {
        collection.Configure<DefaultNavigationConfiguration>(configuration =>
        {
            configuration.StartupPageTag = nameof(Dashboard);

            configuration.VisableItems = new Dictionary<string, INavigationItem>()
            {
                {nameof(Dashboard), new DefaultNavigationItem(typeof(Dashboard), new Uri("pack://application:,,,/Assets/microsoft-shell-desktop.ico"))},
                {nameof(Forms), new DefaultNavigationItem(typeof(Forms), new Uri("pack://application:,,,/Assets/microsoft-shell-accessibility.ico"))},
                {nameof(Views.Pages.Controls), new DefaultNavigationItem(typeof(Views.Pages.Controls), new Uri("pack://application:,,,/Assets/microsoft-shell-settings.ico"))},
                {nameof(Actions), new DefaultNavigationItem(typeof(Actions), new Uri("pack://application:,,,/Assets/microsoft-shell-workspace.ico"))},
                {nameof(Colors), new DefaultNavigationItem(typeof(Colors), new Uri("pack://application:,,,/Assets/microsoft-shell-colors.ico"))},
                {nameof(Icons), new DefaultNavigationItem(typeof(Icons), new Uri("pack://application:,,,/Assets/microsoft-shell-gallery.ico"))},
                {nameof(WindowsPage), new DefaultNavigationItem(typeof(WindowsPage), new Uri("pack://application:,,,/Assets/microsoft-shell-monitor.ico"))}
            };

            /*configuration.HiddenItemsItems = new Dictionary<string, INavigationItem>()
            {
                {nameof(Page1), new DefaultNavigationItem(typeof(Page1))},
                {nameof(Page2), new DefaultNavigationItem(typeof(Page2))},
                {nameof(Page3), new DefaultNavigationItem(typeof(Page3))},
            };*/
        });

        collection.Configure<StoreNavigationItemConfiguration>(configuration =>
        {
            configuration.StartupPageTag = nameof(DashboardStore);

            configuration.VisableItems = new Dictionary<string, INavigationItem>()
            {
                {nameof(DashboardStore), new StoreNavigationItem(typeof(DashboardStore), "Dashboard", Icon.Home24)},
                {"Apps", new StoreNavigationItem(typeof(Forms), "Apps", Icon.Apps24)},
                {nameof(Views.Pages.Controls), new StoreNavigationItem(typeof(Views.Pages.Controls), Icon.ResizeLarge24)},
                {"Games", new StoreNavigationItem(typeof(Actions), Icon.Games24)},
                {nameof(Colors), new StoreNavigationItem(typeof(Colors), Icon.Color24)},
            };

            configuration.VisableFooterItems = new Dictionary<string, INavigationItem>()
            {
                {"Library", new StoreNavigationItem(typeof(DashboardStore), Icon.Library24)},
                {"Help", new StoreNavigationItem(typeof(DashboardStore),Icon.QuestionCircle24 )},
            };
        });

        collection.AddSingleton<Container>();
        collection.AddTransient<Store>();
        collection.AddTransient<Editor>();
        collection.AddTransient<Xbox>();
        collection.AddTransient<Backdrop>();

        collection.AddScoped<Dialog>();
        collection.AddScoped<Snackbar>();
        collection.AddScoped<DefaultNavigation>();
        collection.AddScoped<StoreNavigation>();

        collection.AddTransient<Colors>();
        collection.AddTransient<Dashboard>();
        collection.AddTransient<Forms>();
        collection.AddTransient<Views.Pages.Controls>();
        collection.AddTransient<Actions>();
        collection.AddTransient<Icons>();
        collection.AddTransient<WindowsPage>();
        collection.AddTransient<DashboardStore>();
    }

    private readonly IHost _host;

    protected override void OnStartup(StartupEventArgs e)
    {
        Theme.Watcher.Start(true, true);

        _host.StartAsync();

        var window = _host.Services.GetRequiredService<Container>();
        window.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _host.StopAsync();
        _host.Dispose();
    }
}