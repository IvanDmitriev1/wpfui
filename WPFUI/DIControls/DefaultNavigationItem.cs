#nullable enable
using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using WPFUI.Common;
using WPFUI.DIControls.Interfaces;

namespace WPFUI.DIControls;

/// <summary>
/// 
/// </summary>
public class DefaultNavigationItem : ObservableObject, INavigationItem
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pageType"></param>
    /// <param name="text"></param>
    /// <param name="pageTag"></param>
    /// <param name="icon"></param>
    /// <param name="iconFilled"></param>
    public DefaultNavigationItem(Type pageType, string pageTag, string text, Common.Icon icon, bool iconFilled = false)
    {
        PageType = pageType;
        Text = text;
        PageTag = pageTag;
        Icon = icon;
        IconFilled = iconFilled;

        OnClickCommand = new RelayCommand(o =>
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pageType"></param>
    /// <param name="text"></param>
    /// <param name="pageTag"></param>
    public DefaultNavigationItem(Type pageType, string pageTag, string text) : this(pageType, text, pageTag, Common.Icon.Empty)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pageType"></param>
    /// <param name="pageTag"></param>
    /// <param name="text"></param>
    /// <param name="imageUri"></param>
    public DefaultNavigationItem(Type pageType, string pageTag, string text, Uri imageUri) : this(pageType, text, pageTag, Common.Icon.Empty)
    {
        Image = new BitmapImage(imageUri);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pageType"></param>
    /// <param name="pageTag"></param>
    /// <param name="imageUri"></param>
    public DefaultNavigationItem(Type pageType, string pageTag, Uri imageUri) : this(pageType, pageType.Name, pageTag, imageUri)
    {
        Image = new BitmapImage(imageUri);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pageType"></param>
    /// <param name="pageTag"></param>
    public DefaultNavigationItem(Type pageType, string pageTag) : this(pageType, pageType.Name, pageTag)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pageType"></param>
    /// <param name="pageTag"></param>
    /// <param name="icon"></param>
    public DefaultNavigationItem(Type pageType, string pageTag, Icon icon) : this(pageType, pageTag, pageType.Name, icon)
    {
    }

    private bool _isActive;

    /// <summary>
    /// 
    /// </summary>
    public bool IsActive
    {
        get => _isActive;
        set => SetProperty(ref _isActive, value);
    }
    /// <summary>
    /// 
    /// </summary>
    public Page? Instance { get; set; }

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
    public string Text { get; }
    /// <summary>
    /// 
    /// </summary>
    public bool HiddenItem { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public bool Footer { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public Icon Icon { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public bool IconFilled { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public BitmapSource? Image { get; init; }
        
    /// <summary>
    /// 
    /// </summary>
    public event EventHandler? Clicked;
    /// <summary>
    /// 
    /// </summary>
    public ICommand OnClickCommand { get; }
}