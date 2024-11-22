namespace Toolkit.Foundation;

public record Updated
{
    public static UpdatedEventArgs<TValue> As<TValue>(TValue value) =>
        new(value);

    public static UpdatedEventArgs<TValue> As<TValue>() where TValue : new() =>
        new(new TValue());
}