using Microsoft.Extensions.Options;
using WPFUI.DIControls;
using WPFUI.DIControls.Interfaces;

namespace WPFUI.Demo;

public class StoreDialog : Dialog
{
    public StoreDialog(IOptions<DialogConfiguration> configuration) : base(configuration)
    {
    }
}