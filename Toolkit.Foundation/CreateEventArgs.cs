namespace Toolkit.Foundation;

public record CreateEventArgs<TValue>
{
    public TValue? Value { get; }

    public CreateEventArgs(TValue value)
    {
        Value = value;
    }

    public CreateEventArgs()
    {

    }
}