namespace Toolkit.Foundation;

public class Request
{
    public static RequestEventArgs<TSender> As<TSender>(TSender sender) =>
        new(sender);

    public static RequestEventArgs<TSender> As<TSender>() where TSender : new() =>
        new(new TSender());
}