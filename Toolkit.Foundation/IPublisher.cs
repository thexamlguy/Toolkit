namespace Toolkit.Foundation;

public interface IPublisher
{
    public Task Publish<TNotification>(object key,
        CancellationToken cancellationToken = default)
        where TNotification :
        INotification,
        new();

    public Task Publish<TNotification>(TNotification notification,
        CancellationToken cancellationToken = default)
        where TNotification :
        INotification;

    public Task Publish<TNotification>(TNotification notification,
        object key,
        CancellationToken cancellationToken = default)
        where TNotification :
        INotification;

    Task PublishUI<TNotification>(TNotification notification,
        object key,
        CancellationToken cancellationToken = default)
        where TNotification :
        INotification;

    Task PublishUI<TNotification>(object key,
        CancellationToken cancellationToken = default)
        where TNotification :
        INotification,
        new();

    Task PublishUI<TNotification>(TNotification notification,
        CancellationToken cancellationToken = default)
        where TNotification :
        INotification;

    Task PublishUI(object notification,
        CancellationToken cancellationToken = default);

    Task Publish(object notification,
        Func<Func<Task>, Task> marshal,
        object? key = null,
        CancellationToken cancellationToken = default);

    Task PublishUIAsync<TNotification>(CancellationToken cancellationToken = default)
        where TNotification :
        INotification,
        new();

    Task Publish<TNotification>(CancellationToken cancellationToken = default)
        where TNotification :
        INotification,
        new();

    public Task Publish(object notification, CancellationToken cancellationToken = default);
}
