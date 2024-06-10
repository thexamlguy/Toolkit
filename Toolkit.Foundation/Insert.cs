namespace Toolkit.Foundation;

public record Insert
{
    public static InsertEventArgs<TSender> As<TSender>(int index, TSender sender) =>
        new(index, sender);

    public static InsertEventArgs<TSender> As<TSender>(int index) where TSender : new() =>
        new(index, new TSender());
}