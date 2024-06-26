﻿using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Toolkit.Foundation;

public class Publisher(IHandlerProvider handlerProvider,
    IServiceFactory serviceFactory,
    IServiceProvider serviceProvider,
    IDispatcher dispatcher) :
    IPublisher
{
    public void Publish<TMessage>(object? key = null)
        where TMessage : new() =>
            Publish(serviceFactory.Create<TMessage>() ?? new TMessage(), async args => await args(), key);

    public void Publish<TMessage>(TMessage message)
        where TMessage : notnull =>
            Publish(message, async args => await args(), null);

    public void Publish<TMessage>(TMessage message,
        object? key = null)
        where TMessage : notnull =>
            Publish(message, async args => await args(), key);

    public void Publish(object message,
        Func<Func<Task>, Task> marshal,
        object? key = null)
    {
        Type notificationType = message.GetType();
        Type handlerType = typeof(NotificationHandlerWrapper<>)
            .MakeGenericType(notificationType);

        key = $"{(key is not null ? $"{key}:" : "")}{notificationType}";

        List<object?> handlers = [];
        foreach (object? handler in handlerProvider.Get(key))
        {
            handlers.Add(handler);
        }

        foreach (object? handler in serviceProvider.GetKeyedServices(handlerType, key))
        {
            handlers.Add(handler);
        }

        foreach (object? handler in handlers)
        {
            if (handler is not null)
            {
                MethodInfo? handleMethod = handler.GetType().GetMethod("Handle",
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

    public void PublishUI<TMessage>(object? key = null)
        where TMessage : new() =>
            Publish(new TMessage(), args => dispatcher.Invoke(async () => await args()), key);

    public void PublishUI<TMessage>(TMessage message)
        where TMessage : notnull =>
            Publish(message, args => dispatcher.Invoke(async () => await args()), null);

    public void PublishUI<TMessage>(TMessage message,
        object? key = null)
        where TMessage : notnull =>
            Publish(message, args => dispatcher.Invoke(async () => await args()), key);

    public void PublishUI<TMessage>()
        where TMessage : new() =>
            Publish(new TMessage(), args => dispatcher.Invoke(async () => await args()), null);

    public void PublishUI(object message) => Publish(message, args =>
            dispatcher.Invoke(async () => await args()), null);
}