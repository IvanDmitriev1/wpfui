using WPFUI.Common;
using WPFUI.Controls.Interfaces;

namespace WPFUI.DIControls.Interfaces;

public interface ISnackbar : IIconControl
{
    public void Expand(string message, Icon icon = Icon.Empty, int? timeout = null, string? title = null);
}