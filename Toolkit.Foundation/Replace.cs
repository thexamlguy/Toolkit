namespace Toolkit.Foundation;

public record Replace
{
    public static ReplaceEventArgs<TValue> As<TValue>(int index, TValue value) => new(value, index);

    public static ReplaceEventArgs<TValue> As<TValue>(int index) where TValue : new() => new(index);
}