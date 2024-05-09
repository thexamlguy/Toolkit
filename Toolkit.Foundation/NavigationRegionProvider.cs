namespace Toolkit.Foundation;

public class NavigationRegionProvider(INavigationRegionCollection contexts) :
    INavigationRegionProvider
{
    public object? Get(object key) =>
        contexts.TryGetValue(key, out object? target) ? target : default;

    public bool TryGet(object name,
        out object? value)
    {
        if (contexts.TryGetValue(name,
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