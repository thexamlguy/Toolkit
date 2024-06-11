namespace Toolkit.Foundation;

public record Count
{
    public static CountEventArgs<TSender> As<TSender>(TSender sender) =>
        new(sender);

    public static CountEventArgs<TSender> As<TSender>() where TSender : new() =>
        new(new TSender());
}