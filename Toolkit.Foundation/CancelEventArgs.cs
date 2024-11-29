namespace Toolkit.Foundation;

public record CancelEventArgs<TValue>
{
    public TValue? Value { get; }

    public CancelEventArgs(TValue value)
    {
        Value = value;
    }

    public CancelEventArgs()
    {

    }
}