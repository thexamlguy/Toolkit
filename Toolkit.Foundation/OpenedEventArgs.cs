namespace Toolkit.Foundation;

public record OpenedEventArgs<TSender>
{
    public TSender? Sender { get; }

    public OpenedEventArgs(TSender sender)
    {
        Sender = sender;
    }

    public OpenedEventArgs()
    {

    }
}