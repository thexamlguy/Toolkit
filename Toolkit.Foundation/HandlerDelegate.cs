namespace Toolkit.Foundation;

public delegate Task<TResponse> HandlerDelegate<TRequest, TResponse>(TRequest request,
    CancellationToken cancellationToken);