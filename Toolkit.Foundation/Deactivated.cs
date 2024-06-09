namespace Toolkit.Foundation;

public record Deactivated
{
    public static DeactivatedEventArgs<TSender> As<TSender>(TSender sender) => new(sender);

    public static DeactivatedEventArgs<TSender> As<TSender>() where TSender : new() => new(new TSender());
}