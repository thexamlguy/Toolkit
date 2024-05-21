namespace Toolkit.Foundation;

public record Created
{
    public static CreatedEventArgs<TValue> As<TValue>(TValue value) =>
        new(value);

    public static CreatedEventArgs<TValue> As<TValue>() where TValue : new() =>
        new(new TValue());
}
