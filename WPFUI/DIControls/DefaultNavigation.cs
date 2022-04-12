#nullable enable
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WPFUI.DIControls.Interfaces;

namespace WPFUI.DIControls;

/// <summary>
/// 
/// </summary>
public class DefaultNavigationConfiguration
{
    /// <summary>
    /// 
    /// </summary>
    public DefaultNavigationConfiguration()
    {
        VisibleItems = Array.Empty<INavigationItem>();
        VisibleFooterItems = Array.Empty<INavigationItem>();
        HiddenItemsItems = Array.Empty<INavigationItem>();

        StartupPageTag = string.Empty;
        PagesTypes = Type.EmptyTypes;
    }

    /// <summary>
    /// 
    /// </summary>
    public string StartupPageTag { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public INavigationItem[] VisibleItems { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public INavigationItem[] VisibleFooterItems { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public INavigationItem[] HiddenItemsItems { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public IReadOnlyCollection<Type> PagesTypes { get; set; }
}

/// <summary>
/// 
/// </summary>
public class DefaultNavigation : INavigation, IDisposable
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="configureOptions"></param>
    public DefaultNavigation(IServiceProvider serviceProvider, IOptions<DefaultNavigationConfiguration> configureOptions)
    {
        var value = configureOptions.Value;
        _navigationStack = new ObservableCollection<INavigationItem>();
        _serviceProvider = serviceProvider;
        _history = new List<INavigationItem>();
        _startupPageTag = value.StartupPageTag;

        var dictionary = new Dictionary<string, INavigationItem>();

        AddToDictionary(value.VisibleItems, ref dictionary, false, false);
        AddToDictionary(value.VisibleFooterItems, ref dictionary, false, true);
        AddToDictionary(value.HiddenItemsItems, ref dictionary, true, false);

        _allItems = new ReadOnlyDictionary<string, INavigationItem>(dictionary);


        if (value.PagesTypes.Count > 0)
            _pagesTypes = value.PagesTypes.ToDictionary<Type?, Type, Page?>(type => type!, type => null);
    }

    #region Variables

    private readonly string _startupPageTag;
    private readonly IServiceProvider _serviceProvider;
    private Frame? _frame;
    private readonly IReadOnlyDictionary<string, INavigationItem> _allItems;
    private readonly List<INavigationItem> _history;
    private readonly ObservableCollection<INavigationItem> _navigationStack;

    private static Dictionary<Type, Page?> _pagesTypes = new();

    #endregion

    #region Properties

    /// <summary>
    /// 
    /// </summary>
    public IEnumerable<INavigationItem> Items => _allItems.Values.Where(item => !item.Footer && !item.HiddenItem);

    /// <summary>
    /// 
    /// </summary>
    public IEnumerable<INavigationItem> Footer => _allItems.Values.Where(item => item.Footer && !item.HiddenItem);

    /// <summary>
    /// 
    /// </summary>
    public bool ReadyToNavigateBack => _history.Count > 1;

    #endregion

    #region Public methods 

    /// <summary>
    /// 
    /// </summary>
    public void Dispose()
    {
        foreach (var item in _allItems.Values)
                item.Clicked -= ItemOnClicked;

        if (_frame != null)
            _frame.Navigating -= FrameOnNavigating;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="frame"></param>
    public void AddFrame(Frame frame)
    {
        _frame = frame;
        _frame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
        _frame.Navigating += FrameOnNavigating;

        NavigateTo(_startupPageTag);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tag"></param>
    public void Flush(string? tag = null)
    {
        if (tag is null)
        {
            foreach (var item in _allItems.Values)
                item.Instance = null;

            return;
        }

        if (!_allItems.ContainsKey(tag))
            return;

        _allItems[tag].Instance = null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pageTag"></param>
    /// <param name="args"></param>
    /// <param name="refresh"></param>
    /// <returns></returns>
    public Task NavigateTo(string pageTag, object[]? args = null, bool refresh = false)
    {
        if (string.IsNullOrEmpty(pageTag) || _frame is null)
            return Task.CompletedTask;

        if (pageTag == "..")
            return NavigateBack();

        bool? addToNavigationStack = pageTag.Contains("//");
        if (addToNavigationStack == true)
            pageTag = pageTag.Replace("//", string.Empty).Trim();

        if (!_allItems.ContainsKey(pageTag))
            return Task.CompletedTask;

        var item = _allItems[pageTag];
        if (item.HiddenItem && addToNavigationStack == false)
            addToNavigationStack = null;

        return NavigateToItem(item, args, refresh, addToNavigationStack);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public Task NavigateBack(object[]? args = null)
    {
        if (_history.Count <= 1) return Task.CompletedTask;

        var item = _history[^2];
        return NavigateToItem(item, args, addToNavigationStack:item.HiddenItem && _navigationStack.Count > 1, isBackNavigated:true);
    }

    #endregion

    #region PrivateMethods

    private async Task NavigateToItem(INavigationItem item, object[]? args, bool refresh = false, bool? addToNavigationStack = false, bool isBackNavigated = false)
    {
        switch (_navigationStack.Count)
        {
            case > 0 when _navigationStack[^1] == item:
                return;
            case 0:
                _navigationStack.Add(item);
                break;
        }

        if (addToNavigationStack == true && !_navigationStack.Contains(item))
            _navigationStack.Add(item);

        if (!item.HiddenItem || addToNavigationStack == null)
        {
            _navigationStack[0].IsActive = false;
            _navigationStack[0] = item;

            ClearNavigationStack(1);
        }

        if (item.Instance is null || refresh)
            item.Instance = GetPageInstance(item.PageType, refresh);

        var navigationStackCount = _navigationStack.Count;
        if (navigationStackCount > 1)
        {
            var navItem = _navigationStack[^2];
            if (navItem.HiddenItem)
                navItem.IsActive = false;

            var index = _navigationStack.IndexOf(item);
            if (index < navigationStackCount - 1)
                ClearNavigationStack(++index);
        }

        item.IsActive = true;

        var previousNavigationItem = isBackNavigated ? _history[^2] : _navigationStack[^1];
        if (isBackNavigated)
        {
            _history.RemoveAt(_history.LastIndexOf(previousNavigationItem));
            _history.RemoveAt(_history.LastIndexOf(_history[^1]));
        }

        _history.Add(item);
        OnNavigated(previousNavigationItem);
        _frame!.Navigate(item.Instance);

        if (item.Instance!.DataContext is not null)
            await InvokeINavigableMethod(previousNavigationItem, item.Instance.DataContext!, args);

        await InvokeINavigableMethod(previousNavigationItem, item.Instance!, args);
    }

    private void ClearNavigationStack(int itemIndex)
    {
        var navigationStackCount = _navigationStack.Count;
        List<INavigationItem> buffer = new(navigationStackCount - itemIndex);

        for (int i = itemIndex; i <= navigationStackCount - 1; i++)
            buffer.Add(_navigationStack[i]);

        foreach (var item in buffer)
            _navigationStack.Remove(item);
    }

    private async void ItemOnClicked(object? sender, EventArgs e)
    {
        var item = (sender as INavigationItem)!;

        object[]? args = null;
        await NavigateToItem(item, args, isBackNavigated: _navigationStack.Count > 1 && _navigationStack[^1].HiddenItem);
    }

    private Task InvokeINavigableMethod(in INavigationItem previousNavigationItem, object instance, object[]? args)
    {
        if (!instance.GetType().IsAssignableTo(typeof(INavigable))) return Task.CompletedTask;

        var navigable = (INavigable)instance;
        return navigable.OnNavigationRequest(this, previousNavigationItem.PageTag, args);
    }

    private static void FrameOnNavigating(object sender, NavigatingCancelEventArgs e)
    {
        var frame = (Frame)sender;
        frame.NavigationService.RemoveBackEntry();
    }

    private Page GetPageInstance(in Type pageType, bool refresh)
    {
        if (_pagesTypes.Count == 0)
            return (Page) _serviceProvider.GetRequiredService(pageType);

        if (refresh)
            _pagesTypes[pageType] = null;

        if (_pagesTypes[pageType] is null)
            return _pagesTypes[pageType] = (Page) _serviceProvider.GetRequiredService(pageType);

        return _pagesTypes[pageType]!;
    }

    private void AddToDictionary(in INavigationItem[] items, ref Dictionary<string, INavigationItem> dictionary, bool hiddenItem, bool footer)
    {
        foreach (var item in items)
        {
            if (hiddenItem)
                item.HiddenItem = true;

            if (footer)
                item.Footer = true;

            item.Clicked += ItemOnClicked;
            dictionary.Add(item.PageTag, item);
        }
    }

    #endregion

    #region Events

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler<NavigatedEventArgs>? Navigated;
    private void OnNavigated(INavigationItem previousNavigationItem)
    {
        Navigated?.Invoke(this, new NavigatedEventArgs(previousNavigationItem.PageTag, _navigationStack[^1].PageTag, _navigationStack));
    }

    #endregion
}