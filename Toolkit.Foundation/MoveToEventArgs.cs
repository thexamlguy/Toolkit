namespace Toolkit.Foundation;

public record MoveToEventArgs<TValue>
{
    public TValue? Value { get; }

    public int OldIndex { get; }

    public int NewIndex { get; }

    public MoveToEventArgs(TValue value, 
        int oldIndex, 
        int newIndex)
    {
        Value = value;
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