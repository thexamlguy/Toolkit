namespace Toolkit.Foundation;

public record Create
{
    public static CreateEventArgs<TSender> As<TSender>(TSender sender) =>
        new(sender);

    public static CreateEventArgs<TSender> As<TSender>(params object[] parameters) where TSender : new() =>
        new(new TSender());
}