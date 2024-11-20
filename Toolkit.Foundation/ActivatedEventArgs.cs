namespace Toolkit.Foundation;

public record ActivatedEventArgs<TSender>
{
    public TSender? Sender { get; }

    public ActivatedEventArgs(TSender sender)
    {
        Sender = sender;
    }

    public ActivatedEventArgs()
    {

    }
}