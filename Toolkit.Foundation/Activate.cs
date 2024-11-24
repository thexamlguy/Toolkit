namespace Toolkit.Foundation;

public record Activate
{
    public static ActivateEventArgs<TSender> As<TSender>(TSender sender) => new(sender);

    public static ActivateEventArgs<TSender> As<TSender>() where TSender : new() => new(new TSender());
}
