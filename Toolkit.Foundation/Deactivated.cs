namespace Toolkit.Foundation;

public record Deactivated
{
    public static DeactivatedEventArgs<TValue> As<TValue>(TValue value) => new(value);

    public static DeactivatedEventArgs<TValue> As<TValue>() where TValue : new() => new(new TValue());
}
