namespace Toolkit.Foundation;

public record ReplaceEventArgs<TSender>
{
    public TSender? Sender { get; }

    public int Index { get; }

    public ReplaceEventArgs(TSender sender, 
        int index)
    {
        Sender = sender;
        Index = index;
    }

    public ReplaceEventArgs(int index)
    {
        Index = index;
    }
}