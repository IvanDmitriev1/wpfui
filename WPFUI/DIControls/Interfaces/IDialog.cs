#nullable enable
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using WPFUI.Common;

namespace WPFUI.DIControls.Interfaces;

/// <summary>
/// 
/// </summary>
public class DialogConfiguration
{
    /// <summary>
    /// 
    /// </summary>
    public DialogConfiguration()
    {
        Title = string.Empty;
        LeftButtonText = "Ok";
        RightButtonText = "Cancel";

        LeftButtonAppearance = Common.Appearance.Primary;
        RightButtonAppearance = Common.Appearance.Secondary;

        Width = 420;
        Height = 200;
    }

    /// <summary>
    /// 
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string LeftButtonText { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string RightButtonText { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public Common.Appearance LeftButtonAppearance { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public Common.Appearance RightButtonAppearance { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public double Width { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public double Height { get; set; }
}

/// <summary>
/// 
/// </summary>
public interface IDialog
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public Task<ButtonPressed> ShowDialog(string message, DialogConfiguration? configuration = null);
}