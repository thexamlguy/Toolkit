namespace Toolkit.Foundation;

public record Activated
{
    public static ActivatedEventArgs<TValue> As<TValue>(TValue value) => new(value);

    public static ActivatedEventArgs<TValue> As<TValue>() where TValue : new() => new(new TValue());
}