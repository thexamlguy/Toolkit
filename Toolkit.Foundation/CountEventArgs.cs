namespace Toolkit.Foundation;

public record CountEventArgs<TValue>
{
    public TValue? Value { get; }

    public CountEventArgs(TValue value)
    {
        Value = value;
    }

    public CountEventArgs()
    {

    }
}