namespace Toolkit.Foundation;

public record Validate
{
    public static ValidateEventArgs<TValue> As<TValue>(TValue value) =>
        new(value);

    public static ValidateEventArgs<TValue> As<TValue>() where TValue : new() =>
        new(new TValue());
}