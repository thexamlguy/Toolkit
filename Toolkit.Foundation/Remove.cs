namespace Toolkit.Foundation;

public record Remove
{
    public static RemoveEventArgs<TSender> As<TSender>(TSender sender) => new(sender);

    public static RemoveEventArgs<TSender> As<TSender>() where TSender : new() => new(new TSender());
}