namespace Toolkit.Foundation;

public record ConfirmEventArgs<TSender>
{
    public TSender? Sender { get; }

    public ConfirmEventArgs(TSender sender)
    {
        Sender = sender;
    }

    public ConfirmEventArgs()
    {

    }
}