namespace Toolkit.Foundation;

public interface ISubscription
{
    void Add(object subscriber);

    void Remove(object subscriber);
}