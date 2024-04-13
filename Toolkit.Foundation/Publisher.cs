using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Toolkit.Foundation;

public class Publisher(ISubscriptionManager subscriptionManager,
    IServiceProvider provider,
    IDispatcher dispatcher) : 
    IPublisher
{
    public Task Publish<TNotification>(object key,
        CancellationToken cancellationToken = default)
        where TNotification :
        INotification,
        new() => Publish(new TNotification(), async args => await args(),
                key, cancellationToken);

    public Task Publish<TNotification>(TNotification notification,
        CancellationToken cancellationToken = default)
        where TNotification :
        INotification => Publish(notification, async args => await args(),
                null, cancellationToken);

    public Task Publish<TNotification>(TNotification notification,
        object key,
        CancellationToken cancellationToken = default)
        where TNotification :
        INotification => Publish(notification, 
            async args => await args(), key, cancellationToken);

    public async Task Publish(object notification,
        Func<Func<Task>, Task> marshal,
        object? key = null,
        CancellationToken cancellationToken = default)
    {
        Type notificationType = notification.GetType();

        List<object?> handlers = provider.GetServices(typeof(NotificationHandlerWrapper<>)
            .MakeGenericType(notificationType)).ToList();

        foreach (object? handler in subscriptionManager
            .GetHandlers(notificationType, key!))
        {
            handlers.Add(handler);
        }

        foreach (object? handler in handlers)
        {
            if (handler is not null)
            {
                Type? handlerType = handler.GetType();
                MethodInfo? handleMethod = handlerType.GetMethod("Handle",
                    [notificationType, typeof(CancellationToken)]);

                if (handleMethod is not null)
                {
                    await marshal(() => (Task)handleMethod.Invoke(handler, new object[]
                        { notification, cancellationToken })!);
                }
            }
        }
    }

    public Task Publish(object notification,
        CancellationToken cancellationToken = default) => Publish(notification,
            async args => await args(),
                null, cancellationToken);

    public Task Publish<TNotification>(CancellationToken cancellationToken = default) 
        where TNotification : 
        INotification, new() => Publish(new TNotification(),
            async args => await args(),
                null, cancellationToken);

    public Task PublishUI<TNotification>(object key,
        CancellationToken cancellationToken = default)
        where TNotification :
        INotification, new() => Publish(new TNotification(), 
            args =>  dispatcher.InvokeAsync(async () => await args()),
                key, cancellationToken);

    public Task PublishUI<TNotification>(TNotification notification,
        CancellationToken cancellationToken = default)
        where TNotification :
        INotification => Publish(notification, 
            args => dispatcher.InvokeAsync(async () => await args()),
                null, cancellationToken);

    public Task PublishUI<TNotification>(TNotification notification,
        object key,
        CancellationToken cancellationToken = default)
        where TNotification :
        INotification => Publish(notification, 
            args => dispatcher.InvokeAsync(async () => await args()),
                key, cancellationToken);
    public Task PublishUIAsync<TNotification>(CancellationToken cancellationToken = default)
        where TNotification :
        INotification, new() => Publish(new TNotification(), 
            args => dispatcher.InvokeAsync(async () => await args()),
                null, cancellationToken);

    public Task PublishUI(object notification,
        CancellationToken cancellationToken = default) => Publish(notification, args =>
            dispatcher.InvokeAsync(async () => await args()),
                null, cancellationToken);
}
