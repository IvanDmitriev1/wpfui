#nullable enable
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Options;
using WPFUI.Common;

namespace WPFUI.DIControls;

public class DialogConfiguration
{
    public string DefaultTitle { get; set; } = string.Empty;
    public string DefaultLeftButtonText { get; set; } = "Ok";
    public string DefaultRightButtonText { get; set; } = "Cancel";

    public Appearance ButtonLeftAppearance { get; set; } = Appearance.Primary;
    public Appearance ButtonRightAppearance { get; set; } = Appearance.Secondary;

    public double Width { get; set; } = 420;
    public double Height { get; set; } = 200;
}

public partial class Dialog : ObservableObject
{
    public Dialog(IOptions<DialogConfiguration> configuration)
    {
        Configuration = configuration.Value;
    }

    public DialogConfiguration Configuration { get; }

    private TaskCompletionSource<ButtonPressed> _tcs;

    [ObservableProperty]
    private string _text;

    [ObservableProperty]
    private bool _show;

    [ObservableProperty]
    private string _title;

    [ObservableProperty]
    private string _leftButtonText;

    [ObservableProperty]
    private string _rightButtonText;

    [ICommand]
    private void LeftButtonClick()
    {
        _tcs.SetResult(ButtonPressed.Left);
        SetValues();
    }

    [ICommand]
    private void RightButtonClick()
    {
        _tcs.SetResult(ButtonPressed.Right);
        SetValues();
    }

    [ICommand]
    private void SetValues()
    {
        Title = Configuration.DefaultTitle;
        LeftButtonText = Configuration.DefaultLeftButtonText;
        RightButtonText = Configuration.DefaultRightButtonText;

        Show = false;
    }

    /// <summary>
    /// Show dialog
    /// </summary>
    /// <param name="message"></param>
    /// <param name="title"></param>
    /// <param name="leftButtonText"></param>
    /// <param name="rightButtonText"></param>
    /// <returns></returns>
    public Task<ButtonPressed> ShowDialog(string message, string? title = null, string? leftButtonText = null, string? rightButtonText = null)
    {
        _tcs = new TaskCompletionSource<ButtonPressed>();

        Show = true;
        Text = message;

        Title = title ?? Configuration.DefaultTitle;
        LeftButtonText = leftButtonText ?? Configuration.DefaultLeftButtonText;
        RightButtonText = rightButtonText ?? Configuration.DefaultRightButtonText;

        return _tcs.Task;
    }
}