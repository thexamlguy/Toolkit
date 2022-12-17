namespace Toolkit.Framework.Foundation;

public interface IPipelineBehavior<TMessage, TResponse> where TMessage : notnull, IMessage
{
    ValueTask<TResponse> Handle(
        TMessage message,
        CancellationToken cancellationToken,
        MessageHandlerDelegate<TMessage, TResponse> next
    );
}
