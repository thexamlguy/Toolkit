namespace Toolkit.Foundation;

public record SelectionEventArgs<TValue>
{
    public TValue? Value { get; }

    public SelectionEventArgs(TValue value)
    {
        Value = value;
    }

    public SelectionEventArgs()
    {

    }
}