namespace Toolkit.Foundation;

public record UpdateEventArgs<TSender>
{
    public TSender? Sender { get; }

    public UpdateEventArgs(TSender sender)
    {
        Sender = sender;
    }

    public UpdateEventArgs()
    {

    }
}