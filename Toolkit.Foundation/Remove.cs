namespace Toolkit.Foundation;

public record Remove<TValue>(TValue Value);

public record Remove
{
    public static Remove<TValue> As<TValue>(TValue value) =>
        new(value);

    public static Remove<TValue> As<TValue>() where TValue : new() =>
        new(new TValue());
}