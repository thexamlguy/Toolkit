namespace Toolkit.Framework.Foundation;

public class ServiceFactory : IServiceFactory
{
    private readonly Func<Type, object?> factory;
    private readonly Func<Type, object?[], object> creator;

    public ServiceFactory(Func<Type, object?> factory, Func<Type, object?[], object> creator)
    {
        this.factory = factory;
        this.creator = creator;
    }

    public object? Create(Type type, params object?[] parameters)
    {
        dynamic? lookup = factory(typeof(IServiceCreator<>).MakeGenericType(type));
        return lookup is not null ? lookup.Create(creator, parameters) : creator(type, parameters);
    }

    public T? Create<T>(Type type, params object?[] parameters)
    {
        dynamic? lookup = factory(typeof(IServiceCreator<>).MakeGenericType(type));
        return lookup is not null ? (T)lookup.Create(creator, parameters) : (T)creator(type, parameters);
    }
}