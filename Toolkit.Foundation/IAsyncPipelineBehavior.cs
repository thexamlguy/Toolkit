namespace Toolkit.Foundation;

public interface IAsyncPipelineBehavior<TMessage, 
    TResponse>
{
    Task<TResponse> Handle(TMessage message, 
        Func<Task<TResponse>> next);
}

public interface IAsyncPipelineBehavior<TMessage>
{
    Task Handle(TMessage message, 
        Func<Task> next);
}