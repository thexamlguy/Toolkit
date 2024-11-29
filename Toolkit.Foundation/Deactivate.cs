namespace Toolkit.Foundation;

public record Deactivate
{
    public static DeactivateEventArgs<TValue> As<TValue>(TValue value) => new(value);

    public static DeactivateEventArgs<TValue> As<TValue>() where TValue : new() => new(new TValue());
}