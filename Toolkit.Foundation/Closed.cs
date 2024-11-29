namespace Toolkit.Foundation;

public record Closed
{
    public static ClosedEventArgs<TValue> As<TValue>(TValue value) => new(value);

    public static ClosedEventArgs<TValue> As<TValue>() where TValue : new() => new(new TValue());
}

