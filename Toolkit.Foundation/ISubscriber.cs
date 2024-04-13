namespace Toolkit.Foundation;

public interface ISubscriber
{
    void Remove(object subscriber);

    void Add(object subscriber);
}