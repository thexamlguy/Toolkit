namespace Toolkit.Foundation;

public record ConfirmEventArgs<TValue>
{
    public TValue? Value { get; }

    public ConfirmEventArgs(TValue value)
    {
        Value = value;
    }

    public ConfirmEventArgs()
    {

    }
}