#nullable enable
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Options;
using WPFUI.Common;
using WPFUI.DIControls.Interfaces;

namespace WPFUI.DIControls;

/// <summary>
/// 
/// </summary>
[INotifyPropertyChanged]
public partial class DialogObservableConfiguration : DialogConfiguration
{
    private string _title;
    private string _leftButtonText;
    private string _rightButtonText;
    private Common.Appearance _leftButtonAppearance;
    private Common.Appearance _rightButtonAppearance;
    private double _width;
    private double _height;


    /// <summary>
    /// 
    /// </summary>
    public new string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public new string LeftButtonText
    {
        get => _leftButtonText;
        set => SetProperty(ref _leftButtonText, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public new string RightButtonText
    {
        get => _rightButtonText;
        set => SetProperty(ref _rightButtonText, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public new Common.Appearance LeftButtonAppearance
    {
        get => _leftButtonAppearance;
        set => SetProperty(ref _leftButtonAppearance, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public new Common.Appearance RightButtonAppearance
    {
        get => _rightButtonAppearance;
        set => SetProperty(ref _rightButtonAppearance, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public new double Width
    {
        get => _width;
        set => SetProperty(ref _width, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public new double Height
    {
        get => _height;
        set => SetProperty(ref _height, value);
    }
}


/// <summary>
/// 
/// </summary>
public partial class Dialog : ObservableObject, IDialog
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="configuration"></param>
    public Dialog(IOptions<DialogConfiguration> configuration)
    {
        _defaultConfiguration = configuration.Value;
        Configuration = new DialogObservableConfiguration();

        SetValues(_defaultConfiguration);
    }

    private TaskCompletionSource<ButtonPressed> _tcs;
    private readonly DialogConfiguration _defaultConfiguration;

    /// <summary>
    /// 
    /// </summary>
    public DialogObservableConfiguration Configuration { get; }

    [ObservableProperty]
    private string _text;

    [ObservableProperty]
    private bool _show;

    [ICommand]
    private void LeftButtonClick()
    {
        _tcs.SetResult(ButtonPressed.Left);
        SetDefaultValues();
    }

    [ICommand]
    private void RightButtonClick()
    {
        _tcs.SetResult(ButtonPressed.Right);
        SetDefaultValues();
    }

    private void SetDefaultValues()
    {
        Show = false;
        SetValues(_defaultConfiguration);
    }

    private void SetValues(in DialogConfiguration configuration)
    {
        Configuration.Title = configuration.Title;
        Configuration.LeftButtonText = configuration.LeftButtonText;
        Configuration.RightButtonText = configuration.RightButtonText;
        Configuration.LeftButtonAppearance = configuration.LeftButtonAppearance;
        Configuration.RightButtonAppearance = configuration.RightButtonAppearance;
        Configuration.Height = configuration.Height;
        Configuration.Width = configuration.Width;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public Task<ButtonPressed> ShowDialog(string message, DialogConfiguration? configuration = null)
    {
        _tcs = new TaskCompletionSource<ButtonPressed>();

        Show = true;
        Text = message;

        if (configuration is not null)
            SetValues(configuration);

        return _tcs.Task;
    }
}