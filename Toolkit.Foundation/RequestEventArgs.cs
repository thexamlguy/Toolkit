namespace Toolkit.Foundation;

public record RequestEventArgs
{
    public object? Value { get; init; }

    public RequestEventArgs(object? value = null)
    {
        Value = value;
    }
}

public record RequestEventArgs<TValue> : RequestEventArgs
{
    public RequestEventArgs(TValue value) : base(value)
    {
    }
}
