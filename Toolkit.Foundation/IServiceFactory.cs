namespace Toolkit.Foundation;

public interface IServiceFactory
{
    object Create(Type type, params object?[]? parameters);

    TService Create<TService>(Action<TService> serviceDelegate,
        params object?[]? parameters);

    object Create(Type type, Action<object> serviceDelegate,
        params object?[]? parameters);

    TService Create<TService>(params object?[]? parameters);
}