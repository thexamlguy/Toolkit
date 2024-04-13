namespace Toolkit.Foundation;

public interface IPipelineBehavior<TMessage, TResponse>
    where TMessage : IMessage
{
    Task<TResponse> Handle(TMessage message,
        HandlerDelegate<TMessage, TResponse> next,
        CancellationToken cancellationToken = default);
}

public interface IPipelineBehavior<TNotification>
    where TNotification : INotification
{
    Task Handle(TNotification notification,
        NotificationHandlerDelegate<TNotification> next,
        CancellationToken cancellationToken = default);
}