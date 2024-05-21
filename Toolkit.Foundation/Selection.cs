namespace Toolkit.Foundation;

public record Selection
{
    public static SelectionEventArgs<TValue> As<TValue>(TValue value) =>
        new(value);

    public static SelectionEventArgs<TValue> As<TValue>() where TValue : new() =>
        new(new TValue());
}