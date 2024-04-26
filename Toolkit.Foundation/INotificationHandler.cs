namespace Toolkit.Foundation;

public interface INotificationHandler<in TMessage> :
    IHandler
{
    Task Handle(TMessage args,
        CancellationToken cancellationToken = default);
}