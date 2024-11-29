namespace Toolkit.Foundation;

public record Cancel
{
    public static CancelEventArgs<TValue> As<TValue>(TValue value) => new(value);

    public static CancelEventArgs<TValue> As<TValue>() where TValue : new() => new(new TValue());
}