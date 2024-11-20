namespace Toolkit.Foundation;

public record NotifyEventArgs<TSender>
{
    public TSender? Sender { get; }

    public NotifyEventArgs(TSender sender)
    {
        Sender = sender;
    }

    public NotifyEventArgs()
    {

    }
}