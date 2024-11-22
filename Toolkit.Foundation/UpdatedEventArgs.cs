namespace Toolkit.Foundation;

public record UpdatedEventArgs<TSender>
{
    public TSender? Sender { get; }

    public UpdatedEventArgs(TSender sender)
    {
        Sender = sender;
    }

    public UpdatedEventArgs()
    {

    }
}