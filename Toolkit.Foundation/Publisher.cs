using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Toolkit.Foundation;

public class Publisher(ISubscriptionManager subscriptionManager,
    IServiceProvider provider,
    IDispatcher dispatcher) :
    IPublisher
{
    public void Publish<TMessage>(object key)
        where TMessage : new() =>
            Publish(new TMessage(), async args => await args(), key);

    public void Publish<TMessage>(TMessage message)
        where TMessage : notnull =>
            Publish(message, async args => await args(), null);

    public void Publish<TMessage>(TMessage message, object key)
        where TMessage : notnull => 
            Publish(message, async args => await args(), key);

    public void Publish(object message,
        Func<Func<Task>, Task> marshal,
        object? key = null)
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
                    [notificationType]);

                if (handleMethod is not null)
                {
                    marshal(() => (Task)handleMethod.Invoke(handler, new object[]
                        { message })!);
                }
            }
        }
    }

    public void Publish(object message) => Publish(message,
            async args => await args(), null);

    public void Publish<TMessage>()
        where TMessage : new() =>
            Publish(new TMessage(), async args => await args(), null);

    public void PublishUI<TMessage>(object key)
        where TMessage : new() => 
            Publish(new TMessage(), args => dispatcher.Invoke(async () => await args()), key);

    public void PublishUI<TMessage>(TMessage message)
        where TMessage : notnull =>
            Publish(message, args => dispatcher.Invoke(async () => await args()), null);

    public void PublishUI<TMessage>(TMessage message,
        object key)
        where TMessage : notnull =>
            Publish(message, args => dispatcher.Invoke(async () => await args()), key);

    public void PublishUI<TMessage>()
        where TMessage : new() =>
            Publish(new TMessage(), args => dispatcher.Invoke(async () => await args()), null);

    public void PublishUI(object message) => Publish(message, args =>
            dispatcher.Invoke(async () => await args()), null);
}