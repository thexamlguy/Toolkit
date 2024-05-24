namespace Toolkit.Foundation;

public class HandlerProvider(SubscriptionCollection subscriptions) :
    IHandlerProvider
{
    public IEnumerable<object?> Get(Type type,
        object? key = null)
    {
        string subscriptionKey = $"{(key is not null ? $"{key}:" : "")}{type}";
        var d = subscriptions;
        if (subscriptions.TryGetValue(subscriptionKey, out List<WeakReference>? subscribers))
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