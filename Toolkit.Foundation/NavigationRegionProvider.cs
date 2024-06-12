namespace Toolkit.Foundation;

public class NavigationRegionProvider(INavigationRegionCollection collection) :
    INavigationRegionProvider
{
    public object? Get(object key) =>
        collection.TryGetValue(key, out object? target) ? target : default;

    public bool TryGet(object name,
        out object? value)
    {
        if (collection.TryGetValue(name,
            out object? target))
        {
            value = target;
            return true;
        }
        else
        {
            value = null;
            return false;
        }
    }
}