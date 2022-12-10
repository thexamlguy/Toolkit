namespace Toolkit.Framework.Foundation;

public class ServiceCreator<I, T> : IServiceCreator<I>
{
    public virtual object Create(Func<Type, object[], object> creator, params object[] parameters)
    {
        return creator(typeof(T), parameters);
    }
}