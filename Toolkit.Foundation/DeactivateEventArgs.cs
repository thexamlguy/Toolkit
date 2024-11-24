namespace Toolkit.Foundation;

public record DeactivateEventArgs<TSender>
{
    public TSender? Sender { get; }

    public DeactivateEventArgs(TSender sender)
    {
        Sender = sender;
    }

    public DeactivateEventArgs()
    {

    }
}