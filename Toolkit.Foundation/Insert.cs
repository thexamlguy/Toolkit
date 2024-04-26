namespace Toolkit.Foundation;

public record Insert<TValue>(int Index, TValue Value);

public record Insert
{
    public static Insert<TValue> As<TValue>(int index, TValue value) =>
        new(index, value);

    public static Insert<TValue> As<TValue>(int index) where TValue : new() => 
        new(index, new TValue());
}
