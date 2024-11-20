namespace Toolkit.Foundation;

public record ClosedEventArgs<TSender>
{
    public TSender? Sender { get; }

    public ClosedEventArgs(TSender sender)
    {
        Sender = sender;
    }

    public ClosedEventArgs()
    {

    }
}