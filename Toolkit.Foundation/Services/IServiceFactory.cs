namespace Toolkit.Foundation
{
    public interface IServiceFactory
    {
        T? Get<T>(Type type);

        T Create<T>(Type type, params object?[] parameters);
    }
}
