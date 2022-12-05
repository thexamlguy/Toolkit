namespace Toolkit.Foundation
{
    public class ServiceCreator<T> : IServiceCreator<T>
    {
        public virtual object Create(Func<Type, object[], object> creator, params object[] parameters)
        {
            return creator(typeof(T), parameters);
        }
    }
}
