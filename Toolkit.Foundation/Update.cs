namespace Toolkit.Foundation;

public record Update
{
    public static UpdateEventArgs<TValue> As<TValue>(TValue value) =>
        new(value);

    public static UpdateEventArgs<TValue> As<TValue>() where TValue : new() =>
        new(new TValue());
}