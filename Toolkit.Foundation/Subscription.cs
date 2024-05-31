using System.Reactive.Disposables;

namespace Toolkit.Foundation;

public class Subscription(SubscriptionCollection subscriptions,
    IDisposer disposer) :
    ISubscription
{
    public void Add(object subscriber)
    {
        Type handlerType = subscriber.GetType();

        IDictionary<Type, object> keys = GetKeysFromHandler(subscriber);
        foreach (Type interfaceType in GetHandlerInterfaces(handlerType))
        {
            if (interfaceType.GetGenericArguments().FirstOrDefault() is Type argumentType)
            {
                keys.TryGetValue(argumentType, out object? key);

                string subscriptionKey = $"{(key is not null ? $"{key}:" : "")}{argumentType}";
                subscriptions.AddOrUpdate(subscriptionKey, _ => new List<WeakReference> { new(subscriber) }, (_, collection) =>
                {
                    collection.Add(new WeakReference(subscriber));
                    return collection;
                });

                disposer.Add(subscriber, Disposable.Create(() => RemoveSubscriber(subscriber, subscriptionKey)));
            }
        }
    }

    public void Remove(object subscriber)
    {
        Type handlerType = subscriber.GetType();
        IDictionary<Type, object> keys = GetKeysFromHandler(subscriber);
        foreach (Type interfaceType in GetHandlerInterfaces(handlerType))
        {
            if (interfaceType.GetGenericArguments().FirstOrDefault() is Type argumentType)
            {
                keys.TryGetValue(argumentType, out object? key);

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

    private void RemoveSubscriber(object subscriber,
        string key)
    {
        if (subscriptions.TryGetValue(key, out List<WeakReference>? subscribers))
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
                subscriptions.TryRemove(key, out _);
            }
        }
    }

    //private object? GetKeyFromHandler(object handler) =>
    //    handler.GetAttribute<NotificationAttribute>() is NotificationAttribute attribute
    //        ? handler.GetPropertyValue(() => attribute.Key) is { } value ? value : attribute.Key : null;

    private IDictionary<Type, object> GetKeysFromHandler(object handler)
    {
        Dictionary<Type, object> keys = [];

        foreach (NotificationAttribute attribute in handler.GetAttributes<NotificationAttribute>())
        {
            keys.Add(attribute.Type, attribute.Key);
        }

        return keys;
    }


    private IEnumerable<Type> GetHandlerInterfaces(Type handlerType) =>
        handlerType.GetInterfaces().Where(interfaceType =>
        {
            Type? definition = interfaceType.IsGenericType ? interfaceType.GetGenericTypeDefinition() : null;
            return definition == typeof(INotificationHandler<>) ||
                   definition == typeof(IHandler<>) ||
                   definition == typeof(IHandler<,>);
        });
}