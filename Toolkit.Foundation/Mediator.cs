using System.Reflection;

namespace Toolkit.Foundation;

public class Mediator(IServiceProvider provider) :
    IMediator
{
    public Task<TResponse?> Handle<TRequest, TResponse>(TRequest request,
        CancellationToken cancellationToken = default)
        where TRequest : notnull
    {
        Type handlerType = typeof(HandlerWrapper<,>).MakeGenericType(request.GetType(),
            typeof(TResponse));

        if (provider.GetService(handlerType)
            is object handler)
        {
            if (handlerType.GetMethod("Handle") is MethodInfo handleMethod)
            {
                return (Task<TResponse?>)handleMethod.Invoke(handler, new object[] { request, cancellationToken })!;
            }
        }

        return Task.FromResult<TResponse?>(default);
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