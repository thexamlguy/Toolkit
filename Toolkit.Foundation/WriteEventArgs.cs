namespace Toolkit.Foundation;

public record WriteEventArgs<TSender>
{
    public TSender? Sender { get; }

    public WriteEventArgs(TSender sender)
    {
        Sender = sender;
    }

    public WriteEventArgs()
    {

    }
}