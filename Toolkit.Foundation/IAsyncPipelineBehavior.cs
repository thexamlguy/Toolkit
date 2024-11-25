namespace Toolkit.Foundation;

public interface IAsyncPipelineBehavior<TMessage,
    TResponse>
{
    Task<TResponse> Handle(TMessage message, 
        AsyncHandlerDelegate<TResponse> next);
}