namespace Toolkit.Foundation;

public class Container<T> :
    IContainer<T>
{
    public T? Value { get; private set; }

    public void Set(T value) => Value = value;
}
