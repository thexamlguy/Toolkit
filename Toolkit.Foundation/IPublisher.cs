namespace Toolkit.Foundation;

public interface IPublisher
{
    void Publish<TMessage>(object key)
        where TMessage : new();

    void Publish<TMessage>(TMessage message)
        where TMessage : notnull;

    void Publish<TMessage>(TMessage message,
        object key)
        where TMessage : notnull;

    void Publish(object message,
        Func<Func<Task>, Task> marshal,
        object? key = null);

    void Publish<TMessage>()
        where TMessage : new();

    void Publish(object message);

    void PublishUI<TMessage>(TMessage message,
        object key) where TMessage : notnull;

    void PublishUI<TMessage>(object key)
        where TMessage : new();

    void PublishUI<TMessage>(TMessage message)
        where TMessage : notnull;

    void PublishUI(object message);

    void PublishUI<TMessage>()
        where TMessage : new();
}