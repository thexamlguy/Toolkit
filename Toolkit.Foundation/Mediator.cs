using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Toolkit.Foundation;

public class Mediator(IHandlerProvider handlerProvider,
    IServiceProvider provider) :
    IMediator
{
    public async Task<TResponse?> Handle<TMessage, TResponse>(TMessage message,
        CancellationToken cancellationToken = default)
        where TMessage : notnull
    {
        Type messageType = message.GetType();
        Type handlerWrapperType = typeof(HandlerWrapper<,>).MakeGenericType(messageType, typeof(TResponse));

        Dictionary<Type, List<object?>> handlers = [];

        foreach (object? service in provider.GetServices(handlerWrapperType))
        {
            if (service?.GetType() is Type serviceType)
            {
                if (!handlers.TryGetValue(serviceType, out List<object?>? handlerList))
                {
                    handlerList = [];
                    handlers.Add(serviceType, handlerList);
                }

                handlerList.Add(service);
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

    public Task<object?> Handle(object message,
        CancellationToken cancellationToken = default)
    {
        if (message.GetType().GetInterface(message.GetType().Name) is Type requestType &&
            requestType.GetGenericArguments().Length == 1)
        {
            Type responseType = requestType.GetGenericArguments()[0];
            Type handlerType = typeof(HandlerWrapper<,>).MakeGenericType(message.GetType(),
                responseType);

            if (provider.GetService(handlerType)
                is object handler)
            {
                if (handlerType.GetMethod("Handle") is MethodInfo handleMethod)
                {
                    return (Task<object?>)handleMethod.Invoke(handler, new object[] { message, cancellationToken })!;
                }
            }
        }

        return Task.FromResult<object?>(default);
    }
}