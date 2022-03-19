#nullable enable
using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WPFUI.Controls.Interfaces;

namespace WPFUI.DIControls.Interfaces;

public interface INavigationItem : IIconControl
{
    public string Text { get; }
    public Type PageType { get; }

    public BitmapSource? Image { get; init; }
    public bool Footer { get; set; }
    public bool HiddenItem { get; set; }

    public bool IsActive { get; set; }
    public Page? Instance { get; set; }

    public event EventHandler? Clicked;

    public ICommand OnClickCommand { get; }
}