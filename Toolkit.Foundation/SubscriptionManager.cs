using System.Reactive.Disposables;

namespace Toolkit.Foundation;

public class SubscriptionManager(SubscriptionCollection subscriptions, 
    IDisposer disposer) : 
    ISubscriptionManager
{
    public IEnumerable<object?> GetHandlers(Type notificationType, 
        object key)
    {
        string subscriptionKey = $"{(key is not null ? $"{key}:" : "")}{notificationType}";
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

    public void Remove(object subscriber)
    {
        Type handlerType = subscriber.GetType();
        object? key = GetKeyFromHandler(subscriber);
        foreach (Type interfaceType in GetHandlerInterfaces(handlerType))
        {
            if (interfaceType.GetGenericArguments().FirstOrDefault() is Type argumentType)
            {
                string subscriptionKey = $"{(key is not null ? $"{key}:" : "")}{argumentType}";
                if (subscriptions.TryGetValue(subscriptionKey, out List<WeakReference>? subscribers))
                {
                    for (int i = subscribers.Count - 1; i >= 0; i--)
                    {
                        if (!subscribers[i].IsAlive || subscribers[i].Target == subscriber)
                        {
                            subscribers.RemoveAt(i);
                        }
                    }
                }
            }
        }
    }

    public void Add(object subscriber)
    {
        Type handlerType = subscriber.GetType();
        object? key = GetKeyFromHandler(subscriber);
        foreach (Type interfaceType in GetHandlerInterfaces(handlerType))
        {
            if (interfaceType.GetGenericArguments().FirstOrDefault() is Type argumentType)
            {
                string subscriptionKey = $"{(key is not null ? $"{key}:" : "")}{argumentType}";
                subscriptions.AddOrUpdate(subscriptionKey, _ => new List<WeakReference> { new(subscriber) }, (_, collection) =>
                {
                    collection.Add(new WeakReference(subscriber));
                    return collection;
                });

                disposer.Add(subscriber, Disposable.Create(() => RemoveSubscriber(subscriber, argumentType)));
            }
        }
    }

    private void RemoveSubscriber(object subscriber, 
        Type argumentType)
    {
        string subscriptionKey = $"{argumentType}";
        if (subscriptions.TryGetValue(subscriptionKey, out List<WeakReference>? subscribers))
        {
            for (int i = subscribers.Count - 1; i >= 0; i--)
            {
                if (subscribers[i].Target == subscriber)
                {
                    subscribers.RemoveAt(i);
                }
            }

            if (subscribers.Count == 0)
            {
                subscriptions.TryRemove(subscriptionKey, out _);
            }
        }
    }

    private object? GetKeyFromHandler(object handler) => 
        handler.GetAttribute<NotificationAttribute>() is NotificationAttribute attribute
            ? handler.GetPropertyValue(() => attribute.Key) is { } value ? value : attribute.Key : null;

    private IEnumerable<Type> GetHandlerInterfaces(Type handlerType) =>
        handlerType.GetInterfaces().Where(interfaceType => interfaceType.IsGenericType &&
            interfaceType.GetGenericTypeDefinition() == typeof(INotificationHandler<>));
}
