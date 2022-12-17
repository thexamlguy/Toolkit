namespace Toolkit.Framework.Foundation;

public class NamedContentFactory : INamedContentFactory
{
    private readonly Dictionary<string, object> cache = new();

    private readonly IReadOnlyCollection<IContentTemplateDescriptor> descriptors;
    private readonly IServiceFactory serviceFactory;

    public NamedContentFactory(IReadOnlyCollection<IContentTemplateDescriptor> descriptors,
        IServiceFactory serviceFactory)
    {
        this.descriptors = descriptors;
        this.serviceFactory = serviceFactory;
    }

    public virtual object? Create(string name, params object?[] parameters)
    {
        if (cache.TryGetValue(name, out object? data))
        {
            return data;
        }

        if (descriptors.FirstOrDefault(x => x.Name == name) is IContentTemplateDescriptor descriptor)
        {
            data = parameters is { Length: > 0 } ? serviceFactory.Create<object>(descriptor.ContentType, parameters) : serviceFactory.Create(descriptor.ContentType);
            if (data is ICache cache)
            {
                this.cache[name] = cache;
            }
        }

        return data;
    }
}