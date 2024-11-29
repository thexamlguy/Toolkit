namespace Toolkit.Foundation;

public record ValidateEventArgs<TValue>
{
    public TValue? Value { get; }

    public ValidateEventArgs(TValue value)
    {
        Value = value;
    }

    public ValidateEventArgs()
    {

    }
}