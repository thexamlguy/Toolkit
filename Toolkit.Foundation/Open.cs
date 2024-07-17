namespace Toolkit.Foundation;

public record Open
{
    public static OpenEventArgs<TSender> As<TSender>(TSender sender) => new(sender);

    public static OpenEventArgs<TSender> As<TSender>() where TSender : new() => new(new TSender());
}