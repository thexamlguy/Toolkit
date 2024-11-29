namespace Toolkit.Foundation;

public record ActivateEventArgs<TValue>
{
    public TValue? Value { get; }

    public ActivateEventArgs(TValue value)
    {
        Value = value;
    }

    public ActivateEventArgs()
    {

    }
}

public record ActivateEventArgs<TKey, TValue>
{
    public TValue? Value { get; }

    public ActivateEventArgs(TValue value)
    {
        Value = value;
    }

    public ActivateEventArgs()
    {

    }
}