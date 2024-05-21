namespace Toolkit.Foundation;

public record RemoveAt
{
    public static RemoveAtEventArgs<TValue> As<TValue>(int index) =>
        new(index);
}
