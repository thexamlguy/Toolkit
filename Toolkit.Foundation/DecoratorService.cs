namespace Toolkit.Foundation;

public class DecoratorService<T> :
    IDecoratorService<T>
{
    public T? Value { get; private set; }

    public void Set(T? value) => Value = value;
}