#nullable enable
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Options;
using WPFUI.Common;
using WPFUI.DIControls.Interfaces;

namespace WPFUI.DIControls;

public class SnackbarConfiguration
{
    public SnackbarConfiguration()
    {
        Timeout = 2000;
        Title = string.Empty;
        SlideTransform = new TranslateTransform();
    }

    public int Timeout { get; set; }
    public string Title { get; set; }
    public TranslateTransform SlideTransform { get; set; }
}

public partial class Snackbar : ObservableObject, ISnackbar
{
    public Snackbar(IOptions<SnackbarConfiguration> options)
    {
        _identifier = new EventIdentifier();
        var configuration = options.Value;

        _defaultTitle = configuration.Title;
        _timeout = configuration.Timeout;
        _slideTransform = configuration.SlideTransform;
        _icon = Icon.Empty;
    }

    private readonly int _timeout;
    private readonly string _defaultTitle;
    private readonly EventIdentifier _identifier;

    private Icon _icon;
    private bool _iconFilled;

    #region ObservableProperties

    [ObservableProperty]
    private bool _show;

    [ObservableProperty]
    private string _title;

    [ObservableProperty]
    private string _message;

    [ObservableProperty]
    private TranslateTransform _slideTransform;

    public Thickness IconMargin => Icon == Icon.Empty ? new Thickness(0) : new Thickness(0, 0, 12, 0);

    public Icon Icon
    {
        get => _icon;
        set
        {
            SetProperty(ref _icon, value);
            OnPropertyChanged(nameof(IconMargin));
        }
    }

    public bool IconFilled
    {
        get => _iconFilled;
        set => SetProperty(ref _iconFilled, value);
    }

    #endregion

    [ICommand]
    private void ButtonClose()
    {
        HideComponent();
    }

    private void ShowComponent(int? timeout = null)
    {
        if (Show)
            return;

        Show = true;

        timeout ??= _timeout;

        if (timeout > 0)
            HideComponent((int) timeout);
    }

    private async void HideComponent(int timeout = 0)
    {
        if (!Show) return;

        if (timeout < 1)
            Show = false;

        uint currentEvent = _identifier.GetNext();

        await Task.Delay(timeout);

        if (Application.Current == null)
            return;

        if (_identifier.IsEqual(currentEvent))
            Show = false;
    }

    private void SetValues(in string message, Icon icon = Icon.Empty, in string? title = null)
    {
        Title = title ?? _defaultTitle;
        Message = message;
        Icon = icon;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="icon"></param>
    /// <param name="title"></param>
    /// <param name="timeout"></param>
    public async void Expand(string message, Icon icon = Icon.Empty, int? timeout = null, string? title = null)
    {
        if (Show)
        {
            HideComponent();

            await Task.Delay(300);

            if (Application.Current == null)
                return;

            SetValues(message, icon, title);
            ShowComponent(timeout);

            return;
        }

        SetValues(message, icon, title);
        ShowComponent(timeout);
    }
}