namespace Toolkit.Foundation;

public record Delete
{
    public static DeleteEventArgs<TSender> As<TSender>(TSender sender) =>
        new(sender);

    public static DeleteEventArgs<TSender> As<TSender>() where TSender : new() =>
        new(new TSender());
}
