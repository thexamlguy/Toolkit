using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Toolkit.Foundation;

public class Publisher(ISubscriptionManager subscriptionManager,
    IServiceProvider provider,
    IDispatcher dispatcher) : 
    IPublisher
{
    public Task Publish<TMessage>(object key,
        CancellationToken cancellationToken = default)
        where TMessage : new() => 
            Publish(new TMessage(), async args => await args(),
                key, cancellationToken);

    public Task Publish<TMessage>(TMessage message,
        CancellationToken cancellationToken = default)
        where TMessage : notnull => 
            Publish(message, async args => await args(),
                null, cancellationToken);

    public Task Publish<TMessage>(TMessage message,
        object key,
        CancellationToken cancellationToken = default)
        where TMessage : notnull =>
            Publish(message, async args => await args(), 
                key, cancellationToken);

    public async Task Publish(object message,
        Func<Func<Task>, Task> marshal,
        object? key = null,
        CancellationToken cancellationToken = default)
    {
        Type notificationType = message.GetType();

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
                        { message, cancellationToken })!);
                }
            }
        }
    }

    public Task Publish(object message,
        CancellationToken cancellationToken = default) => Publish(message,
            async args => await args(),
                null, cancellationToken);

    public Task Publish<TMessage>(CancellationToken cancellationToken = default) 
        where TMessage :  new() => 
            Publish(new TMessage(), async args => await args(),
                null, cancellationToken);

    public Task PublishUI<TMessage>(object key,
        CancellationToken cancellationToken = default)
        where TMessage : new() => 
            Publish(new TMessage(), args =>  dispatcher.InvokeAsync(async () => await args()),
                key, cancellationToken);

    public Task PublishUI<TMessage>(TMessage message,
        CancellationToken cancellationToken = default)
        where TMessage : notnull => 
            Publish(message, args => dispatcher.InvokeAsync(async () => await args()),
                null, cancellationToken);

    public Task PublishUI<TMessage>(TMessage message,
        object key,
        CancellationToken cancellationToken = default)
        where TMessage : notnull => 
            Publish(message, args => dispatcher.InvokeAsync(async () => await args()),
                key, cancellationToken);
    public Task PublishUI<TMessage>(CancellationToken cancellationToken = default)
        where TMessage : new() => 
            Publish(new TMessage(), args => dispatcher.InvokeAsync(async () => await args()),
                null, cancellationToken);

    public Task PublishUI(object message,
        CancellationToken cancellationToken = default) => Publish(message, args =>
            dispatcher.InvokeAsync(async () => await args()),
                null, cancellationToken);
}
