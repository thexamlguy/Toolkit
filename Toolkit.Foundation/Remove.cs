namespace Toolkit.Foundation;

public record Remove
{
    public static RemoveEventArgs<TValue> As<TValue>(TValue value) => new(value);

    public static RemoveEventArgs<TValue> As<TValue>() where TValue : new() => new(new TValue());
}