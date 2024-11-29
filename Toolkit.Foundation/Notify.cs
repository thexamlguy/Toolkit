namespace Toolkit.Foundation;

public class Notify
{
    public static NotifyEventArgs<TValue> As<TValue>(TValue value) => new(value);

    public static NotifyEventArgs<TValue> As<TValue>() where TValue : new() => new(new TValue());
}