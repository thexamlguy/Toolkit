namespace Toolkit.Foundation;

public record Cancel
{
    public static CancelEventArgs<TSender> As<TSender>(TSender sender) => new(sender);

    public static CancelEventArgs<TSender> As<TSender>() where TSender : new() => new(new TSender());
}