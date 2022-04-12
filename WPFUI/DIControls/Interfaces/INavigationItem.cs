#nullable enable
using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WPFUI.Controls.Interfaces;

namespace WPFUI.DIControls.Interfaces;

/// <summary>
/// 
/// </summary>
public interface INavigationItem : IIconControl
{
    /// <summary>
    /// 
    /// </summary>
    public string Text { get; }
    /// <summary>
    /// 
    /// </summary>
    public Type PageType { get; }

    /// <summary>
    /// 
    /// </summary>
    public string PageTag { get; }

    /// <summary>
    /// 
    /// </summary>
    public BitmapSource? Image { get; init; }
    /// <summary>
    /// 
    /// </summary>
    public bool Footer { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public bool HiddenItem { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool IsActive { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public Page? Instance { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler? Clicked;

    /// <summary>
    /// 
    /// </summary>
    public ICommand OnClickCommand { get; }
}