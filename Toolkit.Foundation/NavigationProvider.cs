namespace Toolkit.Foundation;

public class NavigationProvider(IEnumerable<INavigation> navigations) :
    INavigationProvider
{
    public INavigation? Get(Type type)
    {
        if (navigations.FirstOrDefault(x => type == x.Type ||
            type.BaseType == x.Type) is INavigation navigation)
        {
            return navigation;
        }

        return default;
    }
}