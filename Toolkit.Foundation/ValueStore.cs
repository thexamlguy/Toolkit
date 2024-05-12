namespace Toolkit.Foundation;

public class ValueStore<T> :
    IValueStore<T>
{
    public T? Value { get; private set; }

    public void Set(T value) => Value = value;
}
