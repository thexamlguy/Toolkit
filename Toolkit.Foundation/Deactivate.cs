namespace Toolkit.Foundation;

public record Deactivate
{
    public static DeactivateEventArgs<TSender> As<TSender>(TSender sender) => new(sender);

    public static DeactivateEventArgs<TSender> As<TSender>() where TSender : new() => new(new TSender());
}