namespace Toolkit.Foundation;

public interface IMediator
{
    Task<object?> Handle(Type responseType,
        object message,
        object? key = null,
        CancellationToken cancellationToken = default);

    Task<TResponse?> Handle<TMessage, TResponse>(TMessage message,
        object? key = null,
        CancellationToken cancellationToken = default)
        where TMessage : notnull;

    IAsyncEnumerable<object?> HandleAsyncMany(Type responseType,
        object message,
        object? key = null,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<TResponse?> HandleAsyncMany<TMessage, TResponse>(TMessage message,
        object? key = null,
        CancellationToken cancellationToken = default)
        where TMessage : notnull;

    Task<IList<TResponse?>> HandleMany<TMessage, TResponse>(TMessage message,
            object? key = null,
            CancellationToken cancellationToken = default)
            where TMessage : notnull;
}