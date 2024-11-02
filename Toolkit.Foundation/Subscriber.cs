using System.Reactive.Disposables;

namespace Toolkit.Foundation;

public class Subscriber(SubscriptionCollection subscriptions,
    IDisposer disposer) :
    ISubscriber
{
    public void Subscribe(object subscriber)
    {
        IDictionary<Type, List<object>> subscribers = GetSubscriptionKeys(subscriber);

        foreach (Type handlerType in GetHandlerInterfaces(subscriber.GetType()))
        {
            if (handlerType.Name == typeof(INotificationHandler<>).Name &&
                handlerType.GetGenericArguments() is { Length: 1 } notificationHandlerArguments)
            {
                Type notificationType = notificationHandlerArguments[0];
                AddSubscriptions(subscriber, subscribers, notificationType);
            }

            if (handlerType.Name == typeof(IHandler<,>).Name &&
                handlerType.GetGenericArguments() is { Length: 2 } handlerArguments)
            {
                Type requestType = handlerArguments[0];
                Type responseType = handlerArguments[1];
                Type wrapperType = typeof(HandlerWrapper<,>).MakeGenericType(requestType, responseType);

                AddSubscriptions(subscriber, subscribers, wrapperType);
            }
        }
    }

    public void Unsubscribe(object subscriber)
    {
        IDictionary<Type, List<object>> subscribers = GetSubscriptionKeys(subscriber);

        foreach (Type handlerType in GetHandlerInterfaces(subscriber.GetType()))
        {
            if (handlerType.Name == typeof(INotificationHandler<>).Name &&
                handlerType.GetGenericArguments() is { Length: 1 } notificationHandlerArguments)
            {
                Type notificationType = notificationHandlerArguments[0];
                RemoveSubscriptions(subscriber, subscribers, notificationType);
            }

            if (handlerType.Name == typeof(IHandler<,>).Name &&
                handlerType.GetGenericArguments() is { Length: 2 } handlerArguments)
            {
                Type requestType = handlerArguments[0];
                Type responseType = handlerArguments[1];
                Type wrapperType = typeof(HandlerWrapper<,>).MakeGenericType(requestType, responseType);

                RemoveSubscriptions(subscriber, subscribers, wrapperType);
            }
        }
    }

    private void AddOrUpdateSubscription(object subscriber, 
        string preferredKey)
    {
        subscriptions.AddOrUpdate(preferredKey, _ => new List<WeakReference> { new(subscriber) }, (_, collection) =>
        {
            collection.Add(new WeakReference(subscriber));
            return collection;
        });

        disposer.Add(subscriber, Disposable.Create(() => { 
            
            RemoveSubscription(subscriber, preferredKey);
        
        }));
    }

    private void AddSubscriptions(object subscriber, 
        IDictionary<Type, List<object>> subscribers, 
        Type handlerType)
    {
        if (subscribers.TryGetValue(handlerType, out List<object>? keys))
        {
            foreach (object key in keys)
            {
                string preferredKey = $"{(key is not null ? $"{key}:" : "")}{handlerType}";
                AddOrUpdateSubscription(subscriber, preferredKey);
            }
        }
        else
        {
            string preferredKey = $"{handlerType}";
            AddOrUpdateSubscription(subscriber, preferredKey);
        }
    }

    private IEnumerable<Type> GetHandlerInterfaces(Type handlerType) =>
        handlerType.GetInterfaces().Where(interfaceType =>
        {
            Type? definition = interfaceType.IsGenericType ? interfaceType.GetGenericTypeDefinition() : null;
            return definition == typeof(INotificationHandler<>) ||
                   definition == typeof(IHandler<>) ||
                   definition == typeof(IHandler<,>);
        });

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

            if (subscriber.GetPropertyValue(() => attribute.Key) is object key)
            {
                value.Add(key);
            }
            else
            {
                value.Add(attribute.Key);
            }
        }

        return keys;
    }

    private void RemoveSubscription(object subscriber,
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

    private void RemoveSubscriptions(object subscriber,
        IDictionary<Type, List<object>> subscribers, 
        Type handlerType)
    {
        if (subscribers.TryGetValue(handlerType, out List<object>? keys))
        {
            foreach (object key in keys)
            {
                string subscriptionKey = $"{(key is not null ? $"{key}:" : "")}{handlerType}";
                RemoveSubscription(subscriber, subscriptionKey);
            }
        }
        else
        {
            string subscriptionKey = $"{handlerType}";
            RemoveSubscription(subscriber, subscriptionKey);
        }
    }
}