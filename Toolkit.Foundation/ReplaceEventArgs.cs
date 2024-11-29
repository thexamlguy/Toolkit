namespace Toolkit.Foundation;

public record ReplaceEventArgs<TValue>
{
    public TValue? Value { get; }

    public int Index { get; }

    public ReplaceEventArgs(TValue value, 
        int index)
    {
        Value = value;
        Index = index;
    }

    public ReplaceEventArgs(int index)
    {
        Index = index;
    }
}