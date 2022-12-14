namespace Toolkit.Framework.Foundation;

public class NamedContentTemplateFactory : INamedContentTemplateFactory
{
    private readonly Dictionary<string, object> cache = new();

    private readonly IContentTemplateDescriptorProvider provider;
    private readonly IServiceFactory serviceFactory;

    public NamedContentTemplateFactory(IContentTemplateDescriptorProvider provider,
        IServiceFactory serviceFactory)
    {
        this.provider = provider;
        this.serviceFactory = serviceFactory;
    }

    public virtual object? Create(string name)
    {
        if (cache.TryGetValue(name, out object? view))
        {
            return view;
        }

        if (provider.Get(name) is IContentTemplateDescriptor descriptor)
        {
            view = serviceFactory.Create(descriptor.TemplateType);
            if (view is ICache cache)
            {
                this.cache[name] = cache;
            }

            if (descriptor.GetType().GenericTypeArguments is { Length: 2 })
            {
                (descriptor as dynamic).ViewInvoker?.Invoke(view);
            }
        }

        return view;
    }
}