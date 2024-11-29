namespace Toolkit.Foundation;

public record UpdatedEventArgs<TValue>
{
    public TValue? Value { get; }

    public UpdatedEventArgs(TValue value)
    {
        Value = value;
    }

    public UpdatedEventArgs()
    {

    }
}