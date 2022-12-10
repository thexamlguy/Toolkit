namespace Toolkit.Foundation
{
    public interface IServiceFactory
    {
        object? Create(Type type, params object?[] parameters);

        T? Create<T>(Type type, params object?[] parameters);
    }
}
