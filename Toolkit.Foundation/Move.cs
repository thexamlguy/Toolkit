namespace Toolkit.Foundation;

public record Move
{
    public static MoveEventArgs<TSender> As<TSender>(int index, TSender sender) => new(index, sender);

    public static MoveEventArgs<TSender> As<TSender>(int index) where TSender : new() => new(index, new TSender());
}