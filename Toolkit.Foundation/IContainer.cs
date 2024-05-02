namespace Toolkit.Foundation;

public interface IContainer<T>
{
    T? Value { get; }

    void Set(T value);
}
