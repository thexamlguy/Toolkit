namespace Toolkit.Foundation;

public record Confirm<TValue>(TValue Value);

public record Confirm
{
    public static Confirm<TValue> As<TValue>(TValue value) =>
        new(value);

    public static Confirm<TValue> As<TValue>() where TValue : new() =>
        new(new TValue());
}