namespace Toolkit.Foundation;

public class Read
{
    public static ReadEventArgs<TValue> As<TValue>(TValue value) =>
        new(value);

    public static ReadEventArgs<TValue> As<TValue>() where TValue : new() =>
        new(new TValue());
}