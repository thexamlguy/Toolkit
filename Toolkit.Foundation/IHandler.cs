namespace Toolkit.Foundation;

public interface IHandler;

public interface IHandler<in TRequest, TResponse> :
    IHandler
    where TRequest :
    IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest args,
        CancellationToken cancellationToken);
}

public interface IHandler<in TRequest> :
    IHandler<TRequest, Unit>
    where TRequest :
    IRequest<Unit>;