namespace Toolkit.Foundation;

public record Move<TValue>(int Index, TValue Value);

public record Move
{
    public static Move<TValue> As<TValue>(int index, TValue value) =>
        new(index, value);

    public static Insert<TValue> As<TValue>(int index) where TValue : new() =>
        new(index, new TValue());
}