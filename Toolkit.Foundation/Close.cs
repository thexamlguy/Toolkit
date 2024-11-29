namespace Toolkit.Foundation;

public record Close
{
    public static CloseEventArgs<TValue> As<TValue>(TValue value) => new(value);

    public static CloseEventArgs<TValue> As<TValue>() where TValue : new() => new(new TValue());
}