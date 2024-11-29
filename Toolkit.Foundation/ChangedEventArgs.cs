namespace Toolkit.Foundation;

public record ChangedEventArgs<TValue>
{
    public TValue? Value { get; }

    public ChangedEventArgs(TValue value)
    {
        Value = value;
    }

    public ChangedEventArgs()
    {

    }
}