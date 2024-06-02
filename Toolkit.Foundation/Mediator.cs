using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Toolkit.Foundation;

public class Mediator(IHandlerProvider handlerProvider,
    IServiceProvider provider) :
    IMediator
{
    public async Task<TResponse?> Handle<TMessage, TResponse>(TMessage message,
        object? key = null,
        CancellationToken cancellationToken = default)
        where TMessage : notnull
    {
        Type messageType = message.GetType();
        Type handlerWrapperType = typeof(HandlerWrapper<,>).MakeGenericType(messageType, 
            typeof(TResponse));

        Dictionary<Type, List<object?>> handlers = [];
        foreach (object? handler in key is not null ? provider.GetKeyedServices(handlerWrapperType, key) :
            provider.GetServices(handlerWrapperType))
        {
            if (handler?.GetType() is Type serviceType)
            {
                if (!handlers.TryGetValue(serviceType, out List<object?>? handlerList))
                {
                    handlerList = [];
                    handlers.Add(serviceType, handlerList);
                }

                handlerList.Add(handler);
            }
        }

        foreach (object? handler in handlerProvider.Get(messageType))
        {
            if (handler is not null)
            {
                Type handlerType = handler.GetType();
                if (!handlers.TryGetValue(handlerType, out List<object?>? handlerList))
                {
                    handlerList = [];
                    handlers.Add(handlerType, handlerList);
                }

                handlerList.Add(handler);
            }
        }

        foreach (KeyValuePair<Type, List<object?>> handlerEntry in handlers)
        {
            foreach (object? handler in handlerEntry.Value)
            {
                if (handler?.GetType().GetMethod("Handle", [messageType, typeof(CancellationToken)]) is MethodInfo handleMethod)
                {
                    return await (Task<TResponse?>)handleMethod.Invoke(handler,
                        new object[] { message, cancellationToken })!;
                }
            }
        }

        return default;
    }

    public async Task<object?> Handle(object message,
        object? key = null,
        CancellationToken cancellationToken = default)
    {
        Type messageType = message.GetType();

        if (messageType.GetInterface(message.GetType().Name) is Type requestType &&
            requestType.GetGenericArguments().Length == 1)
        {
            Type responseType = requestType.GetGenericArguments()[0];
            Type handlerWrapperType = typeof(HandlerWrapper<,>).MakeGenericType(message.GetType(),
                responseType);

            Dictionary<Type, List<object?>> handlers = [];

            foreach (object? handler in handlerProvider.Get(messageType))
            {
                if (handler is not null)
                {
                    Type handlerType = handler.GetType();
                    if (!handlers.TryGetValue(handlerType, out List<object?>? handlerList))
                    {
                        handlerList = [];
                        handlers.Add(handlerType, handlerList);
                    }

                    handlerList.Add(handler);
                }
            }

            foreach (object? handler in key is not null ? provider.GetKeyedServices(handlerWrapperType, key) :
                provider.GetServices(handlerWrapperType))
            {
                if (handler?.GetType() is Type serviceType)
                {
                    if (!handlers.TryGetValue(serviceType, out List<object?>? handlerList))
                    {
                        handlerList = [];
                        handlers.Add(serviceType, handlerList);
                    }

                    handlerList.Add(handler);
                }
            }

            foreach (KeyValuePair<Type, List<object?>> handlerEntry in handlers)
            {
                foreach (object? handler in handlerEntry.Value)
                {
                    if (handler?.GetType().GetMethod("Handle", [messageType, typeof(CancellationToken)]) is MethodInfo handleMethod)
                    {
                        return await(Task<object?>)handleMethod.Invoke(handler,
                            new object[] { message, cancellationToken })!;
                    }
                }
            }
        }

        return default;
    }
}