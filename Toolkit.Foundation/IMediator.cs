namespace Toolkit.Foundation;

public interface IMediator
{
    Task<TResponse?> Handle<TRequest, TResponse>(TRequest request,
        CancellationToken cancellationToken = default)
        where TRequest : notnull;

    Task<object?> Handle(object request, CancellationToken
        cancellationToken = default);
}