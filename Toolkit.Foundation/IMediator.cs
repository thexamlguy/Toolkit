namespace Toolkit.Foundation;

public interface IMediator
{
    Task<TResponse?> SendAsync<TResponse>(IRequest<TResponse> request,
        CancellationToken cancellationToken = default);

    Task<object?> SendAsync(object message, CancellationToken
        cancellationToken = default);
}