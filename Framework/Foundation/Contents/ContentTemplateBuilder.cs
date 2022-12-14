using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace Toolkit.Framework.Foundation;

public class ContentTemplateBuilder : IContentTemplateBuilder
{
    private readonly List<IContentTemplateDescriptor> descriptors = new();

    public IReadOnlyCollection<IContentTemplateDescriptor> Descriptors => new ReadOnlyCollection<IContentTemplateDescriptor>(descriptors);

    public IContentTemplateBuilder Add<TViewModel, TView>(string name, ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        descriptors.Add(new ContentTemplateDescriptor(typeof(TViewModel), typeof(TView), name, lifetime));
        return this;
    }

    public IContentTemplateBuilder Add<TViewModel, TView>(ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        descriptors.Add(new ContentTemplateDescriptor(typeof(TViewModel), typeof(TView), null, lifetime));
        return this;
    }
}