namespace Toolkit.Foundation;

public record Initialized<TValue>(TValue Value);

public record Initialized
{
    public static Initialized<TValue> As<TValue>(TValue value) => new(value);

    public static Initialized<TValue> As<TValue>() where TValue : new() => new(new TValue());
}