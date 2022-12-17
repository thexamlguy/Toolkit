namespace Toolkit.Framework.Foundation;

public class Mediator : IMediator
{
    private readonly IServiceFactory factory;

    public Mediator(IServiceFactory factory)
    {
        this.factory = factory;
    }

    public ValueTask<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        dynamic? handler = factory.Create(typeof(RequestClassHandlerWrapper<,>).MakeGenericType(request.GetType(), typeof(TResponse)));  
        if (handler is not null)
        {
            return handler.Handle((dynamic)request, cancellationToken);
        }

        return default;
    }

    public ValueTask<TResponse> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
    {
        dynamic? handler = factory.Create(typeof(CommandClassHandlerWrapper<,>).MakeGenericType(command.GetType(), typeof(TResponse)));
        if (handler is not null)
        {
            return handler.Handle((dynamic)command, cancellationToken);
        }

        return default;
    }

    public ValueTask<TResponse> Send<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
    {
        dynamic? handler = factory.Create(typeof(QueryClassHandlerWrapper<,>).MakeGenericType(query.GetType(), typeof(TResponse)));
        if (handler is not null)
        {
            return handler.Handle((dynamic)query, cancellationToken);
        }

        return default;
    }

    public ValueTask<object?> Send(object message, CancellationToken cancellationToken = default)
    {
        if (message.GetType().GetInterface(typeof(IRequest<>).Name) is { } requestInterface)
        {
            if (requestInterface.GetGenericArguments() is { Length: 1 } arguments)
            {
                Type responseType = arguments[0];

                dynamic? handler = factory.Create(typeof(RequestClassHandlerWrapper<,>).MakeGenericType(message.GetType(), responseType));
                if (handler is not null)
                {
                    return handler.Handle((dynamic)message, cancellationToken);
                }
            }
        }

        if (message.GetType().GetInterface(typeof(ICommand<>).Name) is { } commandInterface)
        {
            if (commandInterface.GetGenericArguments() is { Length: 1 } arguments)
            {
                Type responseType = arguments[0];

                dynamic? handler = factory.Create(typeof(CommandClassHandlerWrapper<,>).MakeGenericType(message.GetType(), responseType));
                if (handler is not null)
                {
                    return handler.Handle((dynamic)message, cancellationToken);
                }
            }
        }

        if (message.GetType().GetInterface(typeof(IQuery<>).Name) is { } queryInterface)
        {
            if (queryInterface.GetGenericArguments() is { Length: 1 } arguments)
            {
                Type responseType = arguments[0];

                dynamic? handler = factory.Create(typeof(QueryClassHandlerWrapper<,>).MakeGenericType(message.GetType(), responseType));
                if (handler is not null)
                {
                    return handler.Handle((dynamic)message, cancellationToken);
                }
            }
        }

        return default;
    }
}
