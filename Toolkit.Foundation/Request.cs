namespace Toolkit.Foundation;

public class Request
{
    public static RequestEventArgs<TValue> As<TValue>(TValue value) =>
        new(value);

    public static RequestEventArgs<TValue> As<TValue>() where TValue : new() =>
        new(new TValue());
}