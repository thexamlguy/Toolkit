namespace Toolkit.Foundation;

public record ActivateEventArgs<TSender>
{
    public TSender? Sender { get; }

    public ActivateEventArgs(TSender sender)
    {
        Sender = sender;
    }

    public ActivateEventArgs()
    {

    }
}