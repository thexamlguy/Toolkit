namespace Toolkit.Foundation;

public interface IPipelineBehaviour<TMessage, TResponse>
{
    Task<TResponse> Handle(TMessage message,
        HandlerDelegate<TMessage, TResponse> next,
        CancellationToken cancellationToken = default);
}

public interface IPipelineBehaviour<TMessage>
{
    Task Handle(TMessage message,
        NotificationHandlerDelegate<TMessage> next,
        CancellationToken cancellationToken = default);
}