namespace WPFUI.DIControls.Interfaces;

public interface INavigable
{
    public void OnNavigationRequest(INavigation navigation, INavigationItem previousNavigationItem, ref object[]? ars);
}