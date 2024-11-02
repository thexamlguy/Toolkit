namespace Toolkit.Foundation;

public record Created
{
    public static CreatedEventArgs<TSender> As<TSender>(TSender sender) =>
        new(sender);

    public static CreatedEventArgs<TSender> As<TSender>() where TSender : new() =>
        new(new TSender());
}