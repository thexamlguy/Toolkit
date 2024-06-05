namespace Toolkit.Foundation;

public record Create
{
    public static CreateEventArgs<TValue> As<TValue>(TValue value, params object[] Parameters) =>
        new(value);

    public static CreateEventArgs<TValue> As<TValue>(params object[] Parameters) where TValue : new() =>
        new(new TValue());
}