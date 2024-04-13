namespace Toolkit.Foundation;

public interface INavigationProvider
{
    INavigation? Get(Type type);
}
