namespace Toolkit.Foundation;

public class Notify
{
    public static NotifyEventArgs<TSender> As<TSender>(TSender sender) => new(sender);

    public static NotifyEventArgs<TSender> As<TSender>() where TSender : new() => new(new TSender());
}