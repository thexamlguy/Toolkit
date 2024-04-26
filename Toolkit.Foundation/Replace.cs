
namespace Toolkit.Foundation;

public record Replace<TValue>(int Index, TValue Value);

public record Replace
{
    public static Replace<TValue> As<TValue>(int index, TValue value) =>
        new(index, value);

    public static Replace<TValue> As<TValue>(int index) where TValue : new() =>
        new(index, new TValue());
}