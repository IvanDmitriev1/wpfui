#nullable enable
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WPFUI.DIControls.Interfaces;

namespace WPFUI.DIControls;

public class DefaultNavigationConfiguration
{
    public DefaultNavigationConfiguration()
    {
        VisableItems = new Dictionary<string, INavigationItem>();
        VisableFooterItems = new Dictionary<string, INavigationItem>();
        HiddenItemsItems = new Dictionary<string, INavigationItem>();

        StartupPageTag = string.Empty;
        PagesTypes = Type.EmptyTypes;
    }

    public string StartupPageTag { get; set; }
    public Dictionary<string,  INavigationItem> VisableItems { get; set; }
    public Dictionary<string, INavigationItem> VisableFooterItems { get; set; }
    public Dictionary<string, INavigationItem> HiddenItemsItems { get; set; }

    public IReadOnlyCollection<Type> PagesTypes { get; set; }
}

public class DefaultNavigation : INavigation, IDisposable
{
    public DefaultNavigation(IServiceProvider serviceProvider, IOptions<DefaultNavigationConfiguration> configureOptions)
    {
        var value = configureOptions.Value;
        _navigationStack = new ObservableCollection<INavigationItem>();
        _serviceProvider = serviceProvider;
        _history = new List<INavigationItem>();
        _startupPageTag = value.StartupPageTag;

        var dictionary = new Dictionary<string, INavigationItem>();

        foreach (var (key, navigationItem) in value.VisableItems)
        {
            navigationItem.Clicked += ItemOnClicked;

            dictionary.Add(key, navigationItem);
        }

        foreach (var (key, navigationItem) in value.VisableFooterItems)
        {
            navigationItem.Clicked += ItemOnClicked;
            navigationItem.Footer = true;

            dictionary.Add(key, navigationItem);
        }

        foreach (var (key, navigationItem) in value.HiddenItemsItems)
        {
            navigationItem.HiddenItem = true;
            navigationItem.Clicked += ItemOnClicked;

            dictionary.Add(key, navigationItem);
        }

        _allItems = new ReadOnlyDictionary<string, INavigationItem>(dictionary);


        if (value.PagesTypes.Count > 0)
            _pagesTypes = value.PagesTypes.ToDictionary<Type?, Type, Page?>(type => type, type => null);
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

    #region Public Properties

    public IEnumerable<INavigationItem> Items => _allItems.Values.Where(item => !item.Footer && !item.HiddenItem);

    public IEnumerable<INavigationItem> Footer => _allItems.Values.Where(item => item.Footer && !item.HiddenItem);

    public bool ReadyToNavigateBack => _history.Count > 1;

    #endregion

    #region Public methods 

    public void Dispose()
    {
        foreach (var item in _allItems.Values)
                item.Clicked -= ItemOnClicked;

        if (_frame != null)
            _frame.Navigating -= FrameOnNavigating;
    }

    public void AddFrame(Frame frame)
    {
        _frame = frame;
        _frame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
        _frame.Navigating += FrameOnNavigating;

        NavigateTo(_startupPageTag);
    }

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

    public void NavigateTo(string pageTag, object[]? args = null, bool refresh = false)
    {
        if (string.IsNullOrEmpty(pageTag) || _frame is null)
            return;

        if (pageTag == "..")
        {
            NavigateBack();
            return;
        }

        bool? addToNavigationStack = pageTag.Contains("//");
        if (addToNavigationStack == true)
            pageTag = pageTag.Replace("//", string.Empty).Trim();

        if (!_allItems.ContainsKey(pageTag))
            return;

        var item = _allItems[pageTag];
        if (item.HiddenItem && addToNavigationStack == false)
            addToNavigationStack = null;

        NavigateToItem(ref item, ref args, refresh, addToNavigationStack);
    }

    public void NavigateBack(object[]? args = null)
    {
        if (_history.Count <= 1) return;

        var item = _history[^2];
        NavigateToItem(ref item, ref args, addToNavigationStack:item.HiddenItem && _navigationStack.Count > 1, isBackNavigated:true);
    }

    #endregion

    #region PrivateMethods

    private void NavigateToItem(ref INavigationItem item, ref object[]? args, bool refresh = false, bool? addToNavigationStack = false, bool isBackNavigated = false)
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
        {
            item.Instance = GetPageInstance(item.PageType, refresh);
        }

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

        if (item.Instance!.DataContext is not null)
            InvokeINavigableMethod(previousNavigationItem, item.Instance.DataContext!, ref args);

        InvokeINavigableMethod(previousNavigationItem, item.Instance!, ref args);
        OnNavigated(previousNavigationItem);

        _frame!.Navigate(item.Instance);
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

    private void ItemOnClicked(object? sender, EventArgs e)
    {
        var item = (sender as INavigationItem)!;

        object[]? args = null;
        NavigateToItem(ref item, ref args, isBackNavigated: _navigationStack.Count > 1 && _navigationStack[^1].HiddenItem);
    }

    private void InvokeINavigableMethod(in INavigationItem previousNavigationItem, in object instance, ref object[]? args)
    {
        if (!instance.GetType().IsAssignableTo(typeof(INavigable))) return;

        var navigable = (INavigable)instance;
        navigable.OnNavigationRequest(this, previousNavigationItem, ref args);
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

    #endregion

    #region Events

    public event EventHandler<NavigatedEventArgs>? Navigated;
    private void OnNavigated(INavigationItem previousNavigationItem)
    {
        Navigated?.Invoke(this, new NavigatedEventArgs(previousNavigationItem, _navigationStack[^1], _navigationStack));
    }

    #endregion
}