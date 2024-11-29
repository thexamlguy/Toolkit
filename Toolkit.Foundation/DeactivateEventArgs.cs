namespace Toolkit.Foundation;

public record DeactivateEventArgs<TValue>
{
    public TValue? Value { get; }

    public DeactivateEventArgs(TValue value)
    {
        Value = value;
    }

    public DeactivateEventArgs()
    {

    }
}