namespace Toolkit.Foundation;

public record QueryEventArgs<TSender>
{
    public TSender? Sender { get; }

    public QueryEventArgs(TSender sender)
    {
        Sender  = sender;
    }

    public QueryEventArgs()
    {

    }
}