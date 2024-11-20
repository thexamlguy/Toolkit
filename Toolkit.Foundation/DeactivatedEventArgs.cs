namespace Toolkit.Foundation;

public record DeactivatedEventArgs<TSender>
{
    public TSender? Sender { get; }

    public DeactivatedEventArgs(TSender sender)
    {
        Sender = sender;
    }

    public DeactivatedEventArgs()
    {

    }
}