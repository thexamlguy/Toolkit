namespace Toolkit.Framework.Foundation;

public class NamedTemplateFactory : INamedTemplateFactory
{
    private readonly Dictionary<string, object> cache = new();

    private readonly ITemplateDescriptorProvider provider;
    private readonly IServiceFactory serviceFactory;

    public NamedTemplateFactory(ITemplateDescriptorProvider provider,
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

        if (provider.Get(name) is ITemplateDescriptor descriptor)
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