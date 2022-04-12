#nullable enable
using System.Threading.Tasks;

namespace WPFUI.DIControls.Interfaces;

/// <summary>
/// 
/// </summary>
public interface INavigable
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="navigation"></param>
    /// <param name="previousPageTag"></param>
    /// <param name="ars"></param>
    /// <returns></returns>
    public Task OnNavigationRequest(INavigation navigation, string previousPageTag, object[]? ars);
}