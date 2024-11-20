namespace Toolkit.Foundation;

public record ReadEventArgs<TSender>
{
    public TSender? Sender { get; }

    public ReadEventArgs(TSender sender)
    {
        Sender = sender;
    }

    public ReadEventArgs()
    {

    }
}