namespace Toolkit.Foundation;

public record Activated<TValue>(TValue? Value = default);

public record Activated
{
    public static Activated<TValue> As<TValue>(TValue value) => new(value);

    public static Activated<TValue> As<TValue>() where TValue : new() => new(new TValue());
}
