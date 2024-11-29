namespace Toolkit.Foundation;

public record NotifyEventArgs<TValue>
{
    public TValue? Value { get; }

    public NotifyEventArgs(TValue value)
    {
        Value = value;
    }

    public NotifyEventArgs()
    {

    }
}