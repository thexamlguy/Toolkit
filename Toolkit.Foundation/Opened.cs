namespace Toolkit.Foundation;

public record Opened
{
    public static OpenedEventArgs<TValue> As<TValue>(TValue value) => new(value);

    public static OpenedEventArgs<TValue> As<TValue>() where TValue : new() => new(new TValue());
}