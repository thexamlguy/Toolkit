namespace Toolkit.Foundation;

public class Write
{
    public static WriteEventArgs<TSender> As<TSender>(TSender sender) =>
        new(sender);

    public static WriteEventArgs<TSender> As<TSender>() where TSender : new() =>
        new(new TSender());
}