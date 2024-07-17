namespace Toolkit.Foundation;

public record Close
{
    public static CloseEventArgs<TSender> As<TSender>(TSender sender) => new(sender);

    public static CloseEventArgs<TSender> As<TSender>() where TSender : new() => new(new TSender());
}