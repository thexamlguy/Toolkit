namespace Toolkit.Foundation;

public record Create
{
    public static CreateEventArgs<TValue> As<TValue>(TValue value) =>
        new(value);

    public static CreateEventArgs<TValue> As<TValue>() where TValue : new() =>
        new(new TValue());
}