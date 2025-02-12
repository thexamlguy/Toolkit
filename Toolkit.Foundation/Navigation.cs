using CommunityToolkit.Mvvm.Messaging;

namespace Toolkit.Foundation;

public class Navigation(INavigationRegionProvider navigationRegionProvider,
    IMessenger messenger) :
    INavigation
{
    public void Back(object? region)
    {
        if (region is not null)
        {
            navigationRegionProvider.TryGet(region, out region);
        }

        if (region is not null)
        {
            Type navigationType = region is Type type ? type : region.GetType();
            Type navigateType = typeof(NavigateBackEventArgs<>).MakeGenericType(navigationType);
            if (Activator.CreateInstance(navigateType, [region]) is object navigate)
            {
                messenger.Send(navigate, navigationType.Name);
            }
        }
    }
}