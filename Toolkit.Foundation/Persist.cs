namespace Toolkit.Foundation;

public class Persist
{
    public static PersistEventArgs<TSender> As<TSender>(TSender sender) => new(sender);

    public static PersistEventArgs<TSender> As<TSender>() where TSender : new() => new(new TSender());
}