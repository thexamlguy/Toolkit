namespace Toolkit.Foundation;

public record Confirm
{
    public static ConfirmEventArgs<TValue> As<TValue>(TValue value) =>
        new(value);

    public static ConfirmEventArgs<TValue> As<TValue>() where TValue : new() =>
        new(new TValue());
}