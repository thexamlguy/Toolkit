namespace Toolkit.Foundation;

public interface IInitializer
{
    Task Initialize();
}

public interface IInitializer<T>
{
    Task<T> Initialize();
}