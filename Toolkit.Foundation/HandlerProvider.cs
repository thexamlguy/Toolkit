namespace Toolkit.Foundation;

public class HandlerProvider(SubscriptionCollection subscriptions) :
    IHandlerProvider
{
    public IEnumerable<object?> Get(object key)
    {
        var d = subscriptions;
        if (subscriptions.TryGetValue(key, out List<WeakReference>? subscribers))
        {
            foreach (WeakReference weakRef in subscribers.ToArray())
            {
                object? target = weakRef.Target;
                if (target != null)
                {
                    yield return target;
                }
                else
                {
                    subscribers.Remove(weakRef);
                }
            }
        }
    }
}