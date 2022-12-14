using System.Diagnostics.CodeAnalysis;

namespace Toolkit.Framework.Foundation;

public class ContentTemplateFactory : IContentTemplateFactory
{
    private readonly Dictionary<object, object> cache = new();

    private readonly IContentTemplateDescriptorProvider provider;
    private readonly IServiceFactory serviceFactory;

    public ContentTemplateFactory(IContentTemplateDescriptorProvider provider,
        IServiceFactory serviceFactory)
    {
        this.provider = provider;
        this.serviceFactory = serviceFactory;
    }

    public virtual object? Create([MaybeNull] object? data)
    {
        if (data is null)
        {
            return null;
        }

        if (cache.TryGetValue(data, out object? template))
        {
            return template;
        }

        if (provider.Get(data.GetType()) is IContentTemplateDescriptor descriptor)
        {
            template = serviceFactory.Create(descriptor.TemplateType);
            if (template is ICache cache)
            {
                this.cache[data] = cache;
            }
        }

        return template;
    }
}