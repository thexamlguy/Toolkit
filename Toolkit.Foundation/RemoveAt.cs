namespace Toolkit.Foundation;

public record RemoveAt
{
    public static RemoveAtEventArgs<TSender> As<TSender>(int index) =>
        new(index);
}