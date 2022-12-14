namespace Toolkit.Framework.Foundation;

public class TypedContentFactory : ITypedContentFactory
{
    private readonly Dictionary<Type, object> cache = new();

    private readonly IReadOnlyCollection<IContentTemplateDescriptor> descriptors;
    private readonly IServiceFactory serviceFactory;

    public TypedContentFactory(IReadOnlyCollection<IContentTemplateDescriptor> descriptors,
        IServiceFactory serviceFactory)
    {
        this.descriptors = descriptors;
        this.serviceFactory = serviceFactory;
    }

    public virtual object? Create(Type type, params object[] parameters)
    {
        if (cache.TryGetValue(type, out object? data))
        {
            return data;
        }

        if (descriptors.FirstOrDefault(x => x.ContentType == type) is IContentTemplateDescriptor descriptor)
        {
            data = parameters is { Length: > 0 } ? serviceFactory.Create<object>(descriptor.ContentType, parameters) : serviceFactory.Create(descriptor.ContentType);
            if (data is ICache cache)
            {
                this.cache[type] = cache;
            }
        }

        return data;
    }
}