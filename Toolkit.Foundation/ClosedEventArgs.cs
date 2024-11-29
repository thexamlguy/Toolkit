namespace Toolkit.Foundation;

public record ClosedEventArgs<TValue>
{
    public TValue? Value { get; }

    public ClosedEventArgs(TValue value)
    {
        Value = value;
    }

    public ClosedEventArgs()
    {

    }
}