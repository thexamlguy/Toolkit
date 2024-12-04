namespace Toolkit.Foundation;

public interface IScopedServiceDescriptor<T>
{
    T? Value { get; }

    void Set(T? value);
}