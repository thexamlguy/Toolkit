namespace Toolkit.Foundation;

public record Opened
{
    public static OpenedEventArgs<TSender> As<TSender>(TSender sender) => new(sender);

    public static OpenedEventArgs<TSender> As<TSender>() where TSender : new() => new(new TSender());
}