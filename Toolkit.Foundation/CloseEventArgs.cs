namespace Toolkit.Foundation;

public record CloseEventArgs<TSender>
{
    public TSender? Sender { get; }

    public CloseEventArgs(TSender sender)
    {
        Sender = sender;
    }

    public CloseEventArgs()
    {

    }
}