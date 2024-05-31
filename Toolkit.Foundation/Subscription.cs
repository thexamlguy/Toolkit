using System.Reactive.Disposables;

namespace Toolkit.Foundation;

public class Subscription(SubscriptionCollection subscriptions,
    IDisposer disposer) :
    ISubscription
{
    public void Add(object subscriber)
    {
        Type handlerType = subscriber.GetType();

        IDictionary<Type, List<object>> subscribers = GetSubscriptionKeys(subscriber);
        foreach (Type interfaceType in GetHandlerInterfaces(handlerType))
        {
            if (interfaceType.GetGenericArguments().FirstOrDefault() is Type argumentType)
            {
                subscribers.TryGetValue(argumentType, out List<object>? keys);
                if (keys is not null)
                {
                    foreach (object key in keys)
                    {
                        string subscriptionKey = $"{(key is not null ? $"{key}:" : "")}{argumentType}";
                        subscriptions.AddOrUpdate(subscriptionKey, _ => new List<WeakReference> { new(subscriber) }, (_, collection) =>
                        {
                            collection.Add(new WeakReference(subscriber));
                            return collection;
                        });

                        disposer.Add(subscriber, Disposable.Create(() => RemoveSubscriber(subscriber, subscriptionKey)));
                    }
                }
                else
                {
                    string subscriptionKey = $"{argumentType}";
                    subscriptions.AddOrUpdate(subscriptionKey, _ => new List<WeakReference> { new(subscriber) }, (_, collection) =>
                    {
                        collection.Add(new WeakReference(subscriber));
                        return collection;
                    });

                    disposer.Add(subscriber, Disposable.Create(() => RemoveSubscriber(subscriber, subscriptionKey)));
                }
            }
        }
    }

    public void Remove(object subscriber)
    {
        Type handlerType = subscriber.GetType();
        IDictionary<Type, List<object>> subscribers = GetSubscriptionKeys(subscriber);
        foreach (Type interfaceType in GetHandlerInterfaces(handlerType))
        {
            if (interfaceType.GetGenericArguments().FirstOrDefault() is Type argumentType)
            {
                subscribers.TryGetValue(argumentType, out List<object>? keys);
                if (keys is not null)
                {
                    foreach (object key in keys)
                    {
                        string subscriptionKey = $"{(key is not null ? $"{key}:" : "")}{argumentType}";
                        if (subscriptions.TryGetValue(subscriptionKey, out List<WeakReference>? existing))
                        {
                            for (int i = existing.Count - 1; i >= 0; i--)
                            {
                                if (!existing[i].IsAlive || existing[i].Target == subscriber)
                                {
                                    existing.RemoveAt(i);
                                }
                            }
                        }
                    }
                }
                else
                {
                    string subscriptionKey = $"{argumentType}";
                    if (subscriptions.TryGetValue(subscriptionKey, out List<WeakReference>? existing))
                    {
                        for (int i = existing.Count - 1; i >= 0; i--)
                        {
                            if (!existing[i].IsAlive || existing[i].Target == subscriber)
                            {
                                existing.RemoveAt(i);
                            }
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

    private IDictionary<Type, List<object>> GetSubscriptionKeys(object subscriber)
    {
        Dictionary<Type, List<object>> keys = [];
        foreach (NotificationAttribute attribute in subscriber.GetAttributes<NotificationAttribute>())
        {
            if (!keys.TryGetValue(attribute.Type, out List<object>? value))
            {
                value = ([]);
                keys[attribute.Type] = value;
            }

            value.Add(attribute.Key);
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