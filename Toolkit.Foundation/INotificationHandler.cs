namespace Toolkit.Foundation;

public interface INotificationHandler<in TNotification> :
    IHandler
    where TNotification :
    INotification
{
    Task Handle(TNotification args,
        CancellationToken cancellationToken = default);
}