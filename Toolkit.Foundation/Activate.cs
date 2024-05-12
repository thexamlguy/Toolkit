namespace Toolkit.Foundation;

public record Activate
{
    public static ActivateEventArgs<TValue> As<TValue>(TValue value) => new(value);

    public static ActivateEventArgs<TValue> As<TValue>() where TValue : new() => new(new TValue());
}
