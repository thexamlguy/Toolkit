namespace Toolkit.Foundation;

public record RequestEventArgs
{
    public object? Sender { get; init; }

    public RequestEventArgs(object? sender = null)
    {
        Sender = sender;
    }
}

public record RequestEventArgs<TSender> : RequestEventArgs
{
    public RequestEventArgs(TSender sender) : base(sender)
    {
    }
}
