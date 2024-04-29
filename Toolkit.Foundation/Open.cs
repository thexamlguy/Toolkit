namespace Toolkit.Foundation;

public record Open<TValue>(TValue Value);

public record Open
{
    public static Open<TValue> As<TValue>(TValue value) => new(value);

    public static Open<TValue> As<TValue>() where TValue : new() => new(new TValue());
}