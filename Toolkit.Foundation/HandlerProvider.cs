namespace Toolkit.Foundation;

public class HandlerProvider(SubscriptionCollection subscriptions) : 
    IHandlerProvider
{
    public IEnumerable<object?> Get(Type type,
        object key)
    {
        string subscriptionKey = $"{(key is not null ? $"{key}:" : "")}{type}";
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
