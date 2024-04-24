
namespace Toolkit.Foundation;

public record Create<TValue>(TValue Value) :
    INotification;

public record Create
{
    public static Create<TValue> As<TValue>(TValue value) => new(value);

    public static Create<TValue> As<TValue>() where TValue : new() => new(new TValue());
}