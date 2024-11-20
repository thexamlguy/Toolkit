namespace Toolkit.Foundation;

public record ChangedEventArgs<TSender>
{
    public TSender? Sender { get; }

    public ChangedEventArgs(TSender sender)
    {
        Sender = sender;
    }

    public ChangedEventArgs()
    {

    }
}