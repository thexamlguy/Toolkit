namespace Toolkit.Foundation;

public record Changed
{
    public static ChangedEventArgs<TValue> As<TValue>(TValue value) => new(value);

    public static ChangedEventArgs<TValue> As<TValue>() where TValue : new() => new(new TValue());
}