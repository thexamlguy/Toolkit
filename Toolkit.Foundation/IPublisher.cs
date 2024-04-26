namespace Toolkit.Foundation;

public interface IPublisher
{
    public Task Publish<TMessage>(object key,
        CancellationToken cancellationToken = default)
        where TMessage : new();

    public Task Publish<TMessage>(TMessage message,
        CancellationToken cancellationToken = default)
        where TMessage : notnull;

    public Task Publish<TMessage>(TMessage message,
        object key,
        CancellationToken cancellationToken = default)
        where TMessage : notnull;

    Task PublishUI<TMessage>(TMessage message,
        object key,
        CancellationToken cancellationToken = default)
        where TMessage : notnull;
    Task PublishUI<TMessage>(object key,
        CancellationToken cancellationToken = default)
        where TMessage : new();

    Task PublishUI<TMessage>(TMessage message,
        CancellationToken cancellationToken = default)
        where TMessage : notnull;

    Task PublishUI(object message,
        CancellationToken cancellationToken = default);

    Task Publish(object message,
        Func<Func<Task>, Task> marshal,
        object? key = null,
        CancellationToken cancellationToken = default);

    Task PublishUI<TMessage>(CancellationToken cancellationToken = default)
        where TMessage : new();

    Task Publish<TMessage>(CancellationToken cancellationToken = default)
        where TMessage : new();

    public Task Publish(object message, CancellationToken cancellationToken = default);
}
