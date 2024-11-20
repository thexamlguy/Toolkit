namespace Toolkit.Foundation;

public record MoveToEventArgs<TSender>
{
    public TSender? Sender { get; }

    public int OldIndex { get; }

    public int NewIndex { get; }

    public MoveToEventArgs(TSender sender, 
        int oldIndex, 
        int newIndex)
    {
        Sender = sender;
        OldIndex = oldIndex;
        NewIndex = newIndex;
    }

    public MoveToEventArgs(int oldIndex,
        int newIndex)
    {
        OldIndex = oldIndex;
        NewIndex = newIndex;
    }
}