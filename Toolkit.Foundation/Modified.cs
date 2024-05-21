namespace Toolkit.Foundation;

public record Modified
{
    public static ModifiedEventArgs<TValue> As<TValue>(TValue oldValue, TValue newValue) =>
        new(oldValue, newValue);
}
