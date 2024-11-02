namespace Toolkit.Foundation;

public record Changed
{
    public static ChangedEventArgs<TSender> As<TSender>(TSender sender) => new(sender);

    public static ChangedEventArgs<TSender> As<TSender>() where TSender : new() => new(new TSender());
}