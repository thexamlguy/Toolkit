namespace Toolkit.Foundation;

public interface IDecoratorService<T>
{
    T? Service { get; }

    void Set(T service);
}