namespace Toolkit.Foundation;

public record ActivatedEventArgs<TValue>
{
    public TValue? Value { get; }

    public ActivatedEventArgs(TValue value)
    {
        Value = value;
    }

    public ActivatedEventArgs()
    {

    }
}