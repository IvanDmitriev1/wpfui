#nullable enable
using WPFUI.Common;
using WPFUI.Controls.Interfaces;

namespace WPFUI.DIControls.Interfaces;

/// <summary>
/// 
/// </summary>
public interface ISnackbar : IIconControl
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="icon"></param>
    /// <param name="timeout"></param>
    /// <param name="title"></param>
    public void Expand(string message, Icon icon = Icon.Empty, int? timeout = null, string? title = null);
}