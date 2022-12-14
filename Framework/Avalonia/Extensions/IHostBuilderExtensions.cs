using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Toolkit.Framework.Foundation;

namespace Toolkit.Framework.Avalonia;

public static class IHostBuilderExtensions
{
    public static IHostBuilder ConfigureTemplates(this IHostBuilder hostBuilder, Action<IContentTemplateBuilder> builderDelegate)
    {
        hostBuilder.ConfigureServices((hostBuilderContext, serviceCollection) =>
        {
            ContentTemplateBuilder? builder = new();
            builderDelegate?.Invoke(builder);

            serviceCollection.TryAddSingleton(builder.Descriptors);
            serviceCollection.TryAddSingleton<IContentTemplateDescriptorProvider, ContentTemplateDescriptorProvider>();
            serviceCollection.TryAddSingleton<IContentTemplateFactory, ContentTemplateFactory>();
            serviceCollection.TryAddSingleton<INamedContentFactory, NamedContentFactory>();
            serviceCollection.TryAddSingleton<ITypedContentFactory, TypedContentFactory>();
            serviceCollection.TryAddSingleton<INamedContentTemplateFactory, NamedContentTemplateFactory>();
            serviceCollection.TryAddSingleton<IContentTemplateSelector, ContentTemplateSelector>();

            foreach (IContentTemplateDescriptor? descriptor in builder.Descriptors)
            {
                serviceCollection.Add(new ServiceDescriptor(descriptor.TemplateType, descriptor.TemplateType, descriptor.Lifetime));
                serviceCollection.Add(new ServiceDescriptor(descriptor.ContentType, descriptor.ContentType, descriptor.Lifetime));
            }
        });

        return hostBuilder;
    }
}