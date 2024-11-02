namespace Toolkit.Foundation;

public interface INavigationRegionProvider
{
    object? Get(object key);

    bool TryGet(object key,
        out object? value);
}