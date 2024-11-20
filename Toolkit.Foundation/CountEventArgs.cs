namespace Toolkit.Foundation;

public record CountEventArgs<TSender>
{
    public TSender? Sender { get; }

    public CountEventArgs(TSender sender)
    {
        Sender = sender;
    }

    public CountEventArgs()
    {

    }
}