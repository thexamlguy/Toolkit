namespace Toolkit.Foundation;

public interface IDecoratorService<T>
{
    T? Value { get; }

    void Set(T? value);
}