namespace Toolkit.Foundation;

public class ScopedServiceDescriptor<T> :
    IScopedServiceDescriptor<T>
{
    public T? Value { get; private set; }

    public void Set(T? value) => Value = value;
}