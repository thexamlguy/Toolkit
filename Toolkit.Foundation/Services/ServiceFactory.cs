namespace Toolkit.Foundation
{
    public class ServiceFactory : IServiceFactory
    {
        private readonly Func<Type, object?> factory;
        private readonly Func<Type, object?[], object> creator;

        public ServiceFactory(Func<Type, object?> factory, Func<Type, object?[], object> creator)
        {
            this.factory = factory;
            this.creator = creator;
        }

        public T? Get<T>(Type type)
        {
            T? value = (T?)factory(type);
            return value;
        }

        public T Create<T>(Type type, params object?[] parameters)
        {
            dynamic? lookup = factory(typeof(IServiceCreator<>).MakeGenericType(type));
            return lookup is not null ? (T)lookup.Create(creator, parameters) : (T)creator(type, parameters);
        }
    }
}
