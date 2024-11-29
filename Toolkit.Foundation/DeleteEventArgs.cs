namespace Toolkit.Foundation;

public record DeleteEventArgs<TValue>
{
    public TValue? Value { get; }

    public DeleteEventArgs(TValue value)
    {
        Value = value;
    }

    public DeleteEventArgs()
    {

    }
}