namespace Toolkit.Foundation;

public record Changed<TValue>(TValue? Value = default);

public record Changed
{
    public static Changed<TValue> As<TValue>(TValue value) => new(value);

    public static Changed<TValue> As<TValue>() where TValue : new() => new(new TValue());
}