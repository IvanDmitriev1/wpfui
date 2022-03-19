#nullable enable
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using WPFUI.Common;

namespace WPFUI.DIControls.Interfaces;

public class DialogConfiguration
{
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

    public string Title { get; set; }

    public string LeftButtonText { get; set; }
    public string RightButtonText { get; set; }

    public Common.Appearance LeftButtonAppearance { get; set; }
    public Common.Appearance RightButtonAppearance { get; set; }

    public double Width { get; set; }
    public double Height { get; set; }
}

public interface IDialog
{
    public Task<ButtonPressed> ShowDialog(string message, DialogConfiguration? configuration = null);
}