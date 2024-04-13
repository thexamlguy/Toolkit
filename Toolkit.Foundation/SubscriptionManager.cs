using System.Reflection;

namespace Toolkit.Foundation;

public class SubscriptionManager(SubscriptionCollection subscriptions) :
    ISubscriptionManager
{
    public IEnumerable<object?> GetHandlers(Type notificationType, object key)
    {
        if (subscriptions.TryGetValue($"{(key is not null ? $"{key}:" : "")}{notificationType}", 
            out List<WeakReference>? subscribers))
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
                if (subscriptions.TryGetValue($"{(key is not null ? $"{key}:" : "")}{argumentType}", out List<WeakReference>? subscribers))
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
                subscriptions.AddOrUpdate($"{(key is not null ? $"{key}:" : "")}{argumentType}", _ => new List<WeakReference> { new(subscriber) }, (_, collection) =>
                {
                    collection.Add(new WeakReference(subscriber));
                    return collection;
                });
            }
        }
    }

    private static object? GetKeyFromHandler(object handler)
    {
        return handler.GetAttribute<NotificationAttribute>()
            is NotificationAttribute attribute
            ? handler.GetPropertyValue(() => attribute.Key) is { } value ? value : attribute.Key
            : null;
    }

    private static IEnumerable<Type> GetHandlerInterfaces(Type handlerType) => 
        handlerType.GetInterfaces().Where(interfaceType => interfaceType.IsGenericType &&
        interfaceType.GetGenericTypeDefinition() == typeof(INotificationHandler<>));
}
