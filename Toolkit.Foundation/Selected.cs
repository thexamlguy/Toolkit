namespace Toolkit.Foundation;

public record Selected<TValue>(TValue Value);

public record Selected
{
    public static Selected<TValue> As<TValue>(TValue value) =>
        new(value);

    public static Selected<TValue> As<TValue>() where TValue : new() =>
        new(new TValue());
}