namespace Toolkit.Foundation;

public record CloseEventArgs<TValue>
{
    public TValue? Value { get; }

    public CloseEventArgs(TValue value)
    {
        Value = value;
    }

    public CloseEventArgs()
    {

    }
}