namespace Toolkit.Foundation;

public record Edit
{
    public static EditEventArgs<TValue> As<TValue>(TValue value) =>
        new(value);

    public static EditEventArgs<TValue> As<TValue>() where TValue : new() =>
        new(new TValue());
}
