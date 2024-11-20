namespace Toolkit.Foundation;

public record RequestEventArgs<TSender>
{
    public TSender? Sender { get; }

    public RequestEventArgs(TSender sender)
    {
        Sender = sender;
    }

    public RequestEventArgs()
    {

    }
}