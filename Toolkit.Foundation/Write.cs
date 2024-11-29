namespace Toolkit.Foundation;

public class Write
{
    public static WriteEventArgs<TValue> As<TValue>(TValue value) =>
        new(value);

    public static WriteEventArgs<TValue> As<TValue>() where TValue : new() =>
        new(new TValue());
}