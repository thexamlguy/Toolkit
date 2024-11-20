namespace Toolkit.Foundation;

public record SelectionEventArgs<TSender>
{
    public TSender? Sender { get; }

    public SelectionEventArgs(TSender sender)
    {
        Sender = sender;
    }

    public SelectionEventArgs()
    {

    }
}