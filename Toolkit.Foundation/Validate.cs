namespace Toolkit.Foundation;

public record Validate
{
    public static ValidateEventArgs<TSender> As<TSender>(TSender sender) =>
        new(sender);

    public static ValidateEventArgs<TSender> As<TSender>() where TSender : new() =>
        new(new TSender());
}