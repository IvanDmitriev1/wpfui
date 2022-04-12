#nullable enable
using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using WPFUI.Common;
using WPFUI.DIControls.Interfaces;

namespace WPFUI.DIControls;

public class DefaultNavigationItem : ObservableObject, INavigationItem
{
    public DefaultNavigationItem(Type pageType, string text, Common.Icon icon, bool iconFilled = false)
    {
        PageType = pageType;
        Text = text;
        Icon = icon;
        IconFilled = iconFilled;

        OnClickCommand = new RelayCommand(o =>
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        });
    }

    public DefaultNavigationItem(Type pageType, Common.Icon icon, bool iconFilled = false) : this(pageType, pageType.Name, icon, iconFilled)
    {
    }

    public DefaultNavigationItem(Type pageType, string text) : this(pageType, text, Common.Icon.Empty)
    {
    }

    public DefaultNavigationItem(Type pageType) : this(pageType, pageType.Name)
    {
    }

    public DefaultNavigationItem(Type pageType, string text, Uri imageUri) : this(pageType, text)
    {
        Image = new BitmapImage(imageUri);
    }

    public DefaultNavigationItem(Type pageType, Uri imageUri) : this(pageType, pageType.Name, imageUri)
    {
    }

    private bool _isActive;

    public bool IsActive
    {
        get => _isActive;
        set => SetProperty(ref _isActive, value);
    }
    public Page? Instance { get; set; }

    public Type PageType { get; }
    public string Text { get; }
    public bool HiddenItem { get; set; }
    public bool Footer { get; set; }

    public Icon Icon { get; set; }
    public bool IconFilled { get; set; }
    public BitmapSource? Image { get; init; }
        

    public event EventHandler? Clicked;
    public ICommand OnClickCommand { get; }
}