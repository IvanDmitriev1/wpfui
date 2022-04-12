#nullable enable
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WPFUI.DIControls.Interfaces;

/// <summary>
/// 
/// </summary>
public interface INavigation
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="frame"></param>
    public void AddFrame(Frame frame);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tag"></param>
    public void Flush(string? tag = null);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pageTag"></param>
    /// <param name="args"></param>
    /// <param name="refresh"></param>
    /// <returns></returns>
    public Task NavigateTo(string pageTag, object[]? args = null, bool refresh = false);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public Task NavigateBack(object[]? args = null);

    /// <summary>
    /// 
    /// </summary>
    public bool ReadyToNavigateBack { get; }

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler<NavigatedEventArgs>? Navigated;
}

/// <summary>
/// 
/// </summary>
public class NavigatedEventArgs : EventArgs
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="previousePage"></param>
    /// <param name="currentPage"></param>
    /// <param name="navigationStack"></param>
    public NavigatedEventArgs(string previousePage, string currentPage, in IEnumerable<INavigationItem> navigationStack)
    {
        PreviousePage = previousePage;
        CurrentPage = currentPage;
        NavigationStack = navigationStack;
    }

    /// <summary>
    /// 
    /// </summary>
    public string PreviousePage { get; }

    /// <summary>
    /// 
    /// </summary>
    public string CurrentPage { get; }

    /// <summary>
    /// 
    /// </summary>
    public IEnumerable<INavigationItem> NavigationStack { get; }
}