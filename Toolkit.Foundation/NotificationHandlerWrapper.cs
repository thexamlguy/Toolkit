namespace Toolkit.Foundation;

public class NotificationHandlerWrapper<TMessage>(INotificationHandler<TMessage> handler,
    IEnumerable<IPipelineBehaviour<TMessage>> pipelineBehaviours)
{
    private readonly IEnumerable<IPipelineBehaviour<TMessage>> pipelineBehaviours =
        pipelineBehaviours.Reverse();

    public async Task Handle(TMessage message,
        CancellationToken cancellationToken)
    {
        NotificationHandlerDelegate<TMessage> currentHandler = handler.Handle;
        foreach (IPipelineBehaviour<TMessage> behaviour in pipelineBehaviours)
        {
            NotificationHandlerDelegate<TMessage> previousHandler = currentHandler;
            currentHandler = async (args, token) =>
            {
                await behaviour.Handle(args, previousHandler, token);
            };
        }

        await currentHandler(message, cancellationToken);
    }
}