namespace Toolkit.Foundation;

public record Selection
{
    public static SelectionEventArgs<TSender?> As<TSender>(TSender? sender) =>
        new(sender);

    public static SelectionEventArgs<TSender?> As<TSender>() where TSender : new() =>
        new(new TSender());
}