namespace Toolkit.Foundation;

public record Delete
{
    public static DeleteEventArgs<TValue> As<TValue>(TValue value) =>
        new(value);

    public static DeleteEventArgs<TValue> As<TValue>() where TValue : new() =>
        new(new TValue());
}