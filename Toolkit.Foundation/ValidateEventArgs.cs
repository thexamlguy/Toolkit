namespace Toolkit.Foundation;

public record ValidateEventArgs<TSender>
{
    public TSender? Sender { get; }

    public ValidateEventArgs(TSender sender)
    {
        Sender = sender;
    }

    public ValidateEventArgs()
    {

    }
}