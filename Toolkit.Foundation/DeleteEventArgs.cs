namespace Toolkit.Foundation;

public record DeleteEventArgs<TSender>
{
    public TSender? Sender { get; }

    public DeleteEventArgs(TSender sender)
    {
        Sender = sender;
    }

    public DeleteEventArgs()
    {

    }
}