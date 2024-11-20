namespace Toolkit.Foundation;

public record CreateEventArgs<TSender>
{
    public TSender? Sender { get; }

    public CreateEventArgs(TSender sender)
    {
        Sender = sender;
    }

    public CreateEventArgs()
    {

    }
}