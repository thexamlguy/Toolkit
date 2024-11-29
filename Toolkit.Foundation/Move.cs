namespace Toolkit.Foundation;

public record Move
{
    public static MoveEventArgs<TValue> As<TValue>(int index, TValue value) => new(index, value);

    public static MoveEventArgs<TValue> As<TValue>(int index) where TValue : new() => new(index, new TValue());
}