namespace Toolkit.Foundation;

public interface IInitialization
{
    Task Initialize();
}

public interface IInitialization<T>
{
    Task<T> Initialize();
}