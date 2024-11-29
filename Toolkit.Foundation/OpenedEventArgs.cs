namespace Toolkit.Foundation;

public record OpenedEventArgs<TValue>
{
    public TValue? Value { get; }

    public OpenedEventArgs(TValue value)
    {
        Value = value;
    }

    public OpenedEventArgs()
    {

    }
}