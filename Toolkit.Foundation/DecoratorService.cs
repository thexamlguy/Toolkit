namespace Toolkit.Foundation;

public class DecoratorService<T> :
    IDecoratorService<T>
{
    public T? Service { get; private set; }

    public void Set(T value) => Service = value;
}