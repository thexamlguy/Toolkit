namespace Toolkit.Foundation;

public class NotificationHandlerWrapper<TNotification>(INotificationHandler<TNotification> handler,
    IEnumerable<IPipelineBehavior<TNotification>> pipelineBehaviours)
    where TNotification : INotification
{
    private readonly IEnumerable<IPipelineBehavior<TNotification>> pipelineBehaviours =
        pipelineBehaviours.Reverse();

    public async Task Handle(TNotification notification,
        CancellationToken cancellationToken)
    {
        NotificationHandlerDelegate<TNotification> currentHandler = handler.Handle;
        foreach (IPipelineBehavior<TNotification> behavior in pipelineBehaviours)
        {
            NotificationHandlerDelegate<TNotification> previousHandler = currentHandler;
            currentHandler = async (args, token) =>
            {
                await behavior.Handle(args, previousHandler, token);
            };
        }
        
        await currentHandler(notification, cancellationToken);
    }
}
