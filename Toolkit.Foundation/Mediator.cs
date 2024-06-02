using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Runtime.CompilerServices;

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
        List<object?> handlers = GetHandlers<TMessage, TResponse>(message, key);

        foreach (object? handler in handlers)
        {
            MethodInfo? handleMethod = handler?.GetType().GetMethod("Handle", [message.GetType(), typeof(CancellationToken)]);
            if (handleMethod != null)
            {
                return await (Task<TResponse?>)handleMethod.Invoke(handler, new object[] { message, cancellationToken })!;
            }
        }

        return default;
    }

    public async IAsyncEnumerable<TResponse?> HandleMany<TMessage, TResponse>(TMessage message,
        object? key = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
        where TMessage : notnull
    {
        List<object?> handlers = GetHandlers<TMessage, TResponse>(message, key);

        foreach (object? handler in handlers)
        {
            MethodInfo? handleMethod = handler?.GetType().GetMethod("Handle", [message.GetType(), typeof(CancellationToken)]);
            if (handleMethod != null)
            {
                yield return await (Task<TResponse?>)handleMethod.Invoke(handler, new object[] { message, cancellationToken })!;
            }
        }
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
            Type handlerWrapperType = typeof(HandlerWrapper<,>).MakeGenericType(message.GetType(), responseType);

            List<object?> handlers = GetHandlers(message, handlerWrapperType, key);

            foreach (object? handler in handlers)
            {
                MethodInfo? handleMethod = handler?.GetType().GetMethod("Handle", [messageType, typeof(CancellationToken)]);
                if (handleMethod != null)
                {
                    return await (Task<object?>)handleMethod.Invoke(handler, new object[] { message, cancellationToken })!;
                }
            }
        }

        return default;
    }

    private List<object?> GetHandlers<TMessage, TResponse>(TMessage message, object? key)
        where TMessage : notnull
    {
        Type messageType = message.GetType();
        Type handlerWrapperType = typeof(HandlerWrapper<,>).MakeGenericType(messageType, typeof(TResponse));

        return GetHandlers(message, handlerWrapperType, key);
    }

    private List<object?> GetHandlers(object message, Type handlerWrapperType, object? key)
    {
        Type messageType = message.GetType();
        Dictionary<Type, List<object?>> handlers = [];

        void AddHandlers(IEnumerable<object?> newHandlers)
        {
            foreach (object? handler in newHandlers)
            {
                if (handler == null) continue;

                Type serviceType = handler.GetType();
                if (!handlers.TryGetValue(serviceType, out List<object?>? handlerList))
                {
                    handlerList = [];
                    handlers.Add(serviceType, handlerList);
                }

                handlerList.Add(handler);
            }
        }

        IEnumerable<object?> keyedServices = key != null ? provider.GetKeyedServices(handlerWrapperType, key) : 
            provider.GetServices(handlerWrapperType);
        AddHandlers(keyedServices);

        IEnumerable<object?> additionalHandlers = handlerProvider.Get(messageType, key);
        AddHandlers(additionalHandlers);

        return handlers.SelectMany(entry => entry.Value).ToList();
    }

}