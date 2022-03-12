#nullable enable
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace WPFUI.DIControls.Interfaces;

public interface INavigation
{
    public void AddFrame(Frame frame);

    public void Flush(string? tag = null);

    public void NavigateTo(string pageTag, object[]? args = null, bool refresh = false);

    public void NavigateBack(object[]? args = null);

    public bool ReadyToNavigateBack { get; }

    public event EventHandler<NavigatedEventArgs>? Navigated;
}

public class NavigatedEventArgs : EventArgs
{
    public NavigatedEventArgs(INavigationItem previousePage, INavigationItem currentPage, in IEnumerable<INavigationItem> navigationStack)
    {
        PreviousePage = previousePage;
        CurrentPage = currentPage;
        NavigationStack = navigationStack;
    }

    public INavigationItem PreviousePage { get; }

    public INavigationItem CurrentPage { get; }

    public IEnumerable<INavigationItem> NavigationStack { get; }
}