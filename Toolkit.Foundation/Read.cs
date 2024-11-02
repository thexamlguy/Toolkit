namespace Toolkit.Foundation;

public class Read
{
    public static ReadEventArgs<TSender> As<TSender>(TSender sender) =>
        new(sender);

    public static ReadEventArgs<TSender> As<TSender>() where TSender : new() =>
        new(new TSender());
}