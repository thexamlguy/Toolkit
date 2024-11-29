namespace Toolkit.Foundation;

public record Count
{
    public static CountEventArgs<TValue> As<TValue>(TValue value) =>
        new(value);

    public static CountEventArgs<TValue> As<TValue>() where TValue : new() =>
        new(new TValue());
}