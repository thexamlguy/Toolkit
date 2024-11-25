namespace Toolkit.Foundation;

public interface IAsyncHandler<TMessage, TResponse> :
    IHandler 
{
    Task<TResponse> Handle(TMessage args,
        CancellationToken cancellationToken = default);
}

public interface IAsyncHandler<TMessage> :
    IHandler
{
    Task Handle(TMessage args,
        CancellationToken cancellationToken = default);
}
