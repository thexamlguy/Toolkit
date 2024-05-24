namespace Toolkit.Foundation;

public interface IValueStore<T>
{
    T? Value { get; }

    void Set(T value);
}