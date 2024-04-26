namespace Toolkit.Foundation;

public class Subscriber(ISubscriptionManager subscriptionManager) :
    ISubscriber
{
    public void Remove(object subscriber) =>
        subscriptionManager.Remove(subscriber);

    public void Add(object subscriber) =>
        subscriptionManager.Add(subscriber);
}