namespace Toolkit.Foundation;

public interface ISubscriber
{
    void Subscribe(object subscriber);

    void Unsubscribe(object subscriber);
}