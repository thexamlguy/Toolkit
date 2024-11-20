namespace Toolkit.Foundation;

public record RemoveEventArgs<TSender>
{
    public TSender? Sender { get; }

    public RemoveEventArgs(TSender sender)
    {
        Sender = sender;
    }

    public RemoveEventArgs()
    {

    }
}