namespace Toolkit.Foundation;

public record DeactivatedEventArgs<TValue>
{
    public TValue? Value { get; }

    public DeactivatedEventArgs(TValue value)
    {
        Value = value;
    }

    public DeactivatedEventArgs()
    {

    }
}
