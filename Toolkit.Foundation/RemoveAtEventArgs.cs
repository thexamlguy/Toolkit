namespace Toolkit.Foundation;

public record RemoveAtEventArgs<TSender>
{
    public TSender? Sender { get; }

    public int Index { get; }

    public RemoveAtEventArgs(TSender sender,
        int index)
    {
        Sender = sender;
        Index = index;
    }

    public RemoveAtEventArgs(int index)
    {
        Index = index;
    }
}