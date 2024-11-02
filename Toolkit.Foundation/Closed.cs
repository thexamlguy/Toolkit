namespace Toolkit.Foundation;

public record Closed
{
    public static ClosedEventArgs<TSender> As<TSender>(TSender sender) => new(sender);

    public static ClosedEventArgs<TSender> As<TSender>() where TSender : new() => new(new TSender());
}

