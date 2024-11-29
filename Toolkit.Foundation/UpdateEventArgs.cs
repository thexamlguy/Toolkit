namespace Toolkit.Foundation;

public record UpdateEventArgs<TValue>
{
    public TValue? Value { get; }

    public UpdateEventArgs(TValue value)
    {
        Value = value;
    }

    public UpdateEventArgs()
    {

    }
}
