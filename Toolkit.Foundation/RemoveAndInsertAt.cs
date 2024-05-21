namespace Toolkit.Foundation;

public record RemoveAndInsertAt
{
    public static RemoveAndInsertAtEventArgs<TValue> As<TValue>(int oldIndex, int newIndex, TValue value) =>
        new(oldIndex, newIndex, value);
}