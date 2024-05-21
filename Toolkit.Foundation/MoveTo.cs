namespace Toolkit.Foundation;

public record MoveTo
{
    public static MoveToEventArgs<TValue> As<TValue>(int oldIndex, int newIndex) =>
        new(oldIndex, newIndex);
}