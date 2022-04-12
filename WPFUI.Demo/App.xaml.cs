// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

#nullable enable
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
using Debug = System.Diagnostics.Debug;

namespace WPFUI.Demo;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        _windowsDependencies = new Dictionary<string, Type[]>()
        {
            {"MainWindow", new []
            {
                typeof(DefaultNavigation),
                typeof(Snackbar),
                typeof(Dialog),
            }},

            {"StoreWindow", new []
            {
                typeof(StoreNavigation),
                typeof(StoreSnackbar),
                typeof(StoreDialog)
            }}
        };
        _pages = new[]
        {
            typeof(Colors),
            typeof(Dashboard),
            typeof(Forms),
            typeof(Views.Pages.Controls),
            typeof(Actions),
            typeof(Icons),
            typeof(WindowsPage),
            typeof(DashboardStore),
        };

        _host = Host.CreateDefaultBuilder().ConfigureServices(ConfigureServices).Build();
    }

    public delegate INavigation NavigationResolver();
    public delegate ISnackbar SnackBarResolver();
    public delegate IDialog DialogResolver();

    private readonly IReadOnlyDictionary<string, Type[]> _windowsDependencies;
    private readonly IReadOnlyCollection<Type> _pages;
    private readonly IHost _host;

    private void ConfigureServices(IServiceCollection collection)
    {
        collection.Configure<DefaultNavigationConfiguration>(configuration =>
        {
            configuration.PagesTypes = _pages;

            configuration.StartupPageTag = nameof(Dashboard);

            configuration.VisibleItems = new INavigationItem[]
            {
                new DefaultNavigationItem(typeof(Dashboard), nameof(Dashboard), new Uri("pack://application:,,,/Assets/microsoft-shell-desktop.ico")),
                new DefaultNavigationItem(typeof(Forms), nameof(Forms), new Uri("pack://application:,,,/Assets/microsoft-shell-accessibility.ico")),
                new DefaultNavigationItem(typeof(Views.Pages.Controls), nameof(Views.Pages.Controls), new Uri("pack://application:,,,/Assets/microsoft-shell-settings.ico")),
                new DefaultNavigationItem(typeof(Actions), nameof(Actions), new Uri("pack://application:,,,/Assets/microsoft-shell-workspace.ico")),
                new DefaultNavigationItem(typeof(Colors), nameof(Colors), new Uri("pack://application:,,,/Assets/microsoft-shell-colors.ico")),
                new DefaultNavigationItem(typeof(Icons), nameof(Icons), new Uri("pack://application:,,,/Assets/microsoft-shell-gallery.ico")),
                new DefaultNavigationItem(typeof(WindowsPage), nameof(WindowsPage), new Uri("pack://application:,,,/Assets/microsoft-shell-monitor.ico")),
            };
        });


        collection.Configure<StoreNavigationItemConfiguration>(configuration =>
        {
            configuration.StartupPageTag = nameof(DashboardStore);

            configuration.VisibleItems = new INavigationItem[]
            {
                new DefaultNavigationItem(typeof(DashboardStore), nameof(DashboardStore), "Dashboard", Icon.Home24),
                new DefaultNavigationItem(typeof(Forms), "Apps", "Apps", Icon.Home24),
                new DefaultNavigationItem(typeof(Views.Pages.Controls), nameof(Views.Pages.Controls), Icon.ResizeLarge24),
                new DefaultNavigationItem(typeof(Actions), "Games", Icon.Games24),
                new DefaultNavigationItem(typeof(Colors), nameof(Colors), Icon.Color24),
            };

            configuration.VisibleFooterItems = new INavigationItem[]
            {
                new DefaultNavigationItem(typeof(DashboardStore), "Library", Icon.Library24),
                new DefaultNavigationItem(typeof(DashboardStore), "Help", Icon.QuestionCircle24),
            };
        });

        collection.Configure<DialogConfiguration>(configuration =>
        {
            configuration.Title = "WPFUI";
        });

        collection.Configure<SnackbarConfiguration>(configuration =>
        {
            configuration.Title = "WPFUI";
        });

        collection.AddSingleton<Container>();
        collection.AddTransient<Store>();
        collection.AddTransient<Editor>();
        collection.AddTransient<Xbox>();
        collection.AddTransient<Backdrop>();
        collection.AddTransient<TaskManager>();

        RegisterWindowsDependencies(collection);
        RegisterPages(collection);

        collection.AddScoped<NavigationResolver>(provider => () => DetermenDependency<INavigation>(provider));
        collection.AddScoped<SnackBarResolver>(provider => () => DetermenDependency<ISnackbar>(provider));
        collection.AddScoped<DialogResolver>(provider => () => DetermenDependency<IDialog>(provider));
    }


    private void RegisterWindowsDependencies(IServiceCollection collection)
    {
        foreach (var types in _windowsDependencies.Values)
            foreach (var type in types)
                collection.AddScoped(type);
    }

    private void RegisterPages(IServiceCollection collection)
    {
        foreach (var type in _pages)
            collection.AddTransient(type);
    }

    private T DetermenDependency<T>(IServiceProvider provider)
    {
        return IteratingThrowWindows(windowName =>
        {
            if (!_windowsDependencies.ContainsKey(windowName))
                return default;

            foreach (var type in _windowsDependencies[windowName])
            {
                if (type.IsAssignableTo(typeof(T)))
                    return (T?) provider.GetRequiredService(type);
            }

            return default;
        });
    }

    private static T IteratingThrowWindows<T>(Func<string, T?> func)
    {
        T? item = default; 

        foreach (Window window in App.Current.Windows)
        {
            if (!window.IsActive) continue;

            item = func.Invoke(window.Name);
        }

        Debug.Assert(item != null, nameof(item) + " != null");
        return item;
    }

    protected override void OnStartup(StartupEventArgs e)
    {
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