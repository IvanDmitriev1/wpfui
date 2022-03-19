using Microsoft.Extensions.Options;
using WPFUI.DIControls;

namespace WPFUI.Demo
{
    public class StoreSnackbar : Snackbar
    {
        public StoreSnackbar(IOptions<SnackbarConfiguration> options) : base(options)
        {

        }
    }
}
