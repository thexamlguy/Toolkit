namespace Toolkit.Foundation
{
    public class NamedDataTemplateFactory : INamedDataTemplateFactory
    {
        private readonly Dictionary<string, object> cache = new();

        private readonly IReadOnlyCollection<ITemplateDescriptor> descriptors;
        private readonly IServiceFactory serviceFactory;

        public NamedDataTemplateFactory(IReadOnlyCollection<ITemplateDescriptor> descriptors,
            IServiceFactory serviceFactory)
        {
            this.descriptors = descriptors;
            this.serviceFactory = serviceFactory;
        }

        public virtual object? Create(string name, params object[] parameters)
        {
            if (cache.TryGetValue(name, out object? data))
            {
                return data;
            }

            if (descriptors.FirstOrDefault(x => x.Name == name) is ITemplateDescriptor descriptor)
            {
                data = parameters is { Length: > 0 } ? serviceFactory.Create<object>(descriptor.DataType, parameters) : serviceFactory.Create(descriptor.DataType);
                if (data is ICache cache)
                {
                    this.cache[name] = cache;
                }
            }

            return data;
        }
    }
}
