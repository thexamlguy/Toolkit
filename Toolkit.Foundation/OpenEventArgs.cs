namespace Toolkit.Foundation;

public record OpenEventArgs<TSender>
{
    public TSender? Sender { get; }

    public OpenEventArgs(TSender sender)
    {
        Sender = sender;
    }

    public OpenEventArgs()
    {

    }
}