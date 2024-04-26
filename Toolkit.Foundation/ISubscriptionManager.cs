namespace Toolkit.Foundation;

public interface ISubscriptionManager
{
    IEnumerable<object?> GetHandlers(Type notificationType, object key);

    void Remove(object subscriber);

    void Add(object subscriber);
}