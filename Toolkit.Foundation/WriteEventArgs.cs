namespace Toolkit.Foundation;

public record WriteEventArgs<TValue>
{
    public TValue? Value { get; }

    public WriteEventArgs(TValue value)
    {
        Value = value;
    }

    public WriteEventArgs()
    {

    }
}