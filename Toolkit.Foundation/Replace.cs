namespace Toolkit.Foundation;

public record Replace
{
    public static ReplaceEventArgs<TSender> As<TSender>(int index, TSender sender) => new(index, sender);

    public static ReplaceEventArgs<TSender> As<TSender>(int index) where TSender : new() => new(index, new TSender());
}