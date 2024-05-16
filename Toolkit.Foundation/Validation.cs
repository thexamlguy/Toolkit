namespace Toolkit.Foundation;

public record Validation
{
    public static ValidationEventArgs<TValue> As<TValue>(TValue value) =>
        new(value);

    public static ValidationEventArgs<TValue> As<TValue>() where TValue : new() =>
        new(new TValue());
}