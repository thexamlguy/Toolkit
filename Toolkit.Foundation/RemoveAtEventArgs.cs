namespace Toolkit.Foundation;

public record RemoveAtEventArgs<TValue>
{
    public TValue? Value { get; }

    public int Index { get; }

    public RemoveAtEventArgs(TValue value,
        int index)
    {
        Value = value;
        Index = index;
    }

    public RemoveAtEventArgs(int index)
    {
        Index = index;
    }
}