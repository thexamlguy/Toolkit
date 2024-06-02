namespace Toolkit.Foundation
{
    public interface IMediator
    {
        Task<object?> Handle(object message,
            object? key = null,
            CancellationToken cancellationToken = default);

        Task<TResponse?> Handle<TMessage, TResponse>(TMessage message,
            object? key = null,
            CancellationToken cancellationToken = default)
            where TMessage : notnull;
    }
}