namespace Toolkit.Foundation;

public class Query
{
    public static QueryEventArgs<TValue> As<TValue>(TValue value) =>
        new(value);

    public static QueryEventArgs<TValue> As<TValue>() where TValue : new() =>
        new(new TValue());
}