using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Options;
using WPFUI.Common;
using WPFUI.Controls.Interfaces;

namespace WPFUI.DIControls;

public class SnackbarConfiguration
{
    public int Timeout { get; set; } = 2000;
    public TranslateTransform SlideTransform { get; set; } = new();
}

public partial class Snackbar : ObservableObject, IIconControl
{
    public Snackbar(IOptions<SnackbarConfiguration> options)
    {
        _identifier = new EventIdentifier();
        var value = options.Value;

        _timeout = value.Timeout;
        _slideTransform = value.SlideTransform;

        _icon = Icon.Empty;
    }

    #region ObservableProperties

    [ObservableProperty]
    private bool _show;

    [ObservableProperty]
    private int _timeout;

    [ObservableProperty]
    private string _title;

    [ObservableProperty]
    private string _message;

    [ObservableProperty]
    private TranslateTransform _slideTransform;

    private Icon _icon;
    private bool _iconFilled;

    private readonly EventIdentifier _identifier;

    public Thickness IconMargin => Icon == Common.Icon.Empty ? new Thickness(0) : new Thickness(0, 0, 12, 0);

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

        timeout ??= Timeout;

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

    /// <summary>
    /// Sets <see cref="Title"/> and <see cref="Message"/>, then shows the snackbar for the amount of time specified in <see cref="Timeout"/>.
    /// </summary>
    public async void Expand(string message, string title, Common.Icon icon = Common.Icon.Empty, int? timeout = null)
    {
        if (Show)
        {
            HideComponent();

            await Task.Delay(300);

            if (Application.Current == null)
                return;

            Title = title;
            Message = message;
            Icon = icon;

            ShowComponent(timeout);

            return;
        }

        Title = title;
        Message = message;
        Icon = icon;

        ShowComponent(timeout);
    }
}