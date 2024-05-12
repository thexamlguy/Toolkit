namespace Toolkit.Foundation;

public record Insert
{
    public static InsertEventArgs<TValue> As<TValue>(int index, TValue value) =>
        new(index, value);

    public static InsertEventArgs<TValue> As<TValue>(int index) where TValue : new() =>
        new(index, new TValue());
}