namespace Toolkit.Foundation;

public record Open
{
    public static OpenEventArgs<TValue> As<TValue>(TValue value) => new(value);

    public static OpenEventArgs<TValue> As<TValue>() where TValue : new() => new(new TValue());
}