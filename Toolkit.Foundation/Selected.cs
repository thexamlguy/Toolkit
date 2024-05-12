namespace Toolkit.Foundation;

public record Selected
{
    public static SelectedEventArgs<TValue> As<TValue>(TValue value) =>
        new(value);

    public static SelectedEventArgs<TValue> As<TValue>() where TValue : new() =>
        new(new TValue());
}