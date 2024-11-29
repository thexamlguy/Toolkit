namespace Toolkit.Foundation;

public record QueryEventArgs<TValue>
{
    public TValue? Value { get; }

    public QueryEventArgs(TValue value)
    {
        Value  = value;
    }

    public QueryEventArgs()
    {

    }
}