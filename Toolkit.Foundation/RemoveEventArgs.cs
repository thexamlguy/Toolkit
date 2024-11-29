namespace Toolkit.Foundation;

public record RemoveEventArgs<TValue>
{
    public TValue? Value { get; }

    public RemoveEventArgs(TValue value)
    {
        Value = value;
    }

    public RemoveEventArgs()
    {

    }
}