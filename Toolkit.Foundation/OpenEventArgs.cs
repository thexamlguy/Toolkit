namespace Toolkit.Foundation;

public record OpenEventArgs<TValue>
{
    public TValue? Value { get; }

    public OpenEventArgs(TValue value)
    {
        Value = value;
    }

    public OpenEventArgs()
    {

    }
}