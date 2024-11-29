namespace Toolkit.Foundation;

public record ReadEventArgs<TValue>
{
    public TValue? Value { get; }

    public ReadEventArgs(TValue value)
    {
        Value = value;
    }

    public ReadEventArgs()
    {

    }
}