namespace Toolkit.Foundation;

public class ServiceFactory(Func<Type, object?[]?, object> factory) :
    IServiceFactory
{
    public TService Create<TService>(params object?[]? parameters) =>
        (TService)factory(typeof(TService), parameters);

    public TService Create<TService>(Action<TService> serviceDelegate,
        params object?[]? parameters)
    {
        TService service = (TService)factory(typeof(TService), parameters);
        serviceDelegate.Invoke(service);

        return service;
    }

    public object Create(Type type, params object?[]? parameters) =>
        factory(type, parameters);
}