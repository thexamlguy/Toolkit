namespace Toolkit.Foundation;

public static class ICollectionSynchronizationExtensions
{
    public static int IndexOf<TItem>(this ICollectionSynchronization<TItem> synchronization,
        TItem item) => synchronization is IList<TItem> collection ? collection.IndexOf(item) : -1;
}