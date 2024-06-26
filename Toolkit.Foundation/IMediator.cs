﻿namespace Toolkit.Foundation;

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

    IAsyncEnumerable<object?> HandleManyAsync(Type responseType,
        object message,
        object? key = null,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<TResponse?> HandleManyAsync<TMessage, TResponse>(TMessage message,
        object? key = null,
        CancellationToken cancellationToken = default)
        where TMessage : notnull;

    Task<IList<TResponse?>> HandleMany<TMessage, TResponse>(TMessage message,
            object? key = null,
            CancellationToken cancellationToken = default)
            where TMessage : notnull;
}