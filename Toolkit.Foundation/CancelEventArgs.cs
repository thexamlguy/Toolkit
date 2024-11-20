namespace Toolkit.Foundation;

public record CancelEventArgs<TSender>
{
    public TSender? Sender { get; }

    public CancelEventArgs(TSender sender)
    {
        Sender = sender;
    }

    public CancelEventArgs()
    {

    }
}