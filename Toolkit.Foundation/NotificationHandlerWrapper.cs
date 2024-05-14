namespace Toolkit.Foundation;

public class NotificationHandlerWrapper<TMessage>(INotificationHandler<TMessage> handler,
    IEnumerable<IPipelineBehaviour<TMessage>> pipelineBehaviours)
{
    private readonly IEnumerable<IPipelineBehaviour<TMessage>> pipelineBehaviours =
        pipelineBehaviours.Reverse();

    public async Task Handle(TMessage message)
    {
        NotificationHandlerDelegate<TMessage> currentHandler = handler.Handle;
        foreach (IPipelineBehaviour<TMessage> behaviour in pipelineBehaviours)
        {
            NotificationHandlerDelegate<TMessage> previousHandler = currentHandler;
            currentHandler = async (args) =>
            {
                await behaviour.Handle(args, previousHandler);
            };
        }

        await currentHandler(message);
    }
}