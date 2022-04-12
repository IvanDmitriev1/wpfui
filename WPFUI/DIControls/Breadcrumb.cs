using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using WPFUI.DIControls.Interfaces;

namespace WPFUI.DIControls;

/// <summary>
/// 
/// </summary>
public class Breadcrumb : System.Windows.Controls.Control
{
    /// <summary>
    /// Property for <see cref="Navigation"/>.
    /// </summary>
    public static readonly DependencyProperty NavigationProperty = DependencyProperty.Register(nameof(Navigation),
        typeof(INavigation), typeof(Breadcrumb),
        new PropertyMetadata(null));

    /// <summary>
    /// 
    /// </summary>
    public static readonly DependencyProperty NavigationItemsProperty = DependencyProperty.Register(nameof(NavigationItems),
        typeof(ObservableCollection<INavigationItem>), typeof(Breadcrumb), new PropertyMetadata(new ObservableCollection<INavigationItem>()));

    /// <summary>
    /// 
    /// </summary>
    public static readonly DependencyProperty FirstNavigationItemProperty = DependencyProperty.Register(nameof(FirstNavigationItem),
        typeof(INavigationItem), typeof(Breadcrumb), new PropertyMetadata(null));

    /// <summary>
    /// 
    /// </summary>
    public static readonly DependencyProperty IsFirstElementActiveProperty = DependencyProperty.Register(nameof(IsFirstElementActive),
        typeof(bool), typeof(Breadcrumb), new PropertyMetadata(null));

    /// <summary>
    /// <see cref="INavigation"/> based on which <see cref="Breadcrumb"/> displays the titles.
    /// </summary>
    public INavigation Navigation
    {
        get => (INavigation) GetValue(NavigationProperty);
        set
        {
            SetValue(NavigationProperty, value);
            value.Navigated += (sender, args) => OnNavigated(args);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public bool IsFirstElementActive
    {
        get => (bool)GetValue(IsFirstElementActiveProperty);
        set => SetValue(IsFirstElementActiveProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public INavigationItem FirstNavigationItem
    {
        get => (INavigationItem)GetValue(FirstNavigationItemProperty);
        set => SetValue(FirstNavigationItemProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public ObservableCollection<INavigationItem> NavigationItems
    {
        get => (ObservableCollection<INavigationItem>)GetValue(NavigationItemsProperty);
        set => SetValue(NavigationItemsProperty, value);
    }

    private void OnNavigated(NavigatedEventArgs eventArgs)
    {
        var collection = eventArgs.NavigationStack.ToArray();
        if (collection.Length == 0) return;

        FirstNavigationItem = collection[0];
        IsFirstElementActive = collection.Length <= 1;

        NavigationItems.Clear();
        for (int i = 1; i < collection.Length; i++)
        {
            NavigationItems.Add(collection[i]);
        }
    }
}