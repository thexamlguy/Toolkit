using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Toolkit.Foundation.Avalonia
{
    public static class IHostBuilderExtensions
    {
        public static IHostBuilder ConfigureTemplates(this IHostBuilder hostBuilder, Action<ITemplateBuilder> builderDelegate)
        {
            hostBuilder.ConfigureServices((hostBuilderContext, serviceCollection) =>
            {
                TemplateBuilder? builder = new();
                builderDelegate?.Invoke(builder);

                serviceCollection.TryAddSingleton(builder.Descriptors);
                serviceCollection.TryAddSingleton<ITemplateDescriptorProvider, TemplateDescriptorProvider>();
                serviceCollection.TryAddSingleton<ITemplateFactory, TemplateFactory>();
                serviceCollection.TryAddSingleton<INamedTemplateFactory, NamedTemplateFactory>();
                serviceCollection.TryAddSingleton<ITypedDataTemplateFactory, TypedDataTemplateFactory>();
                serviceCollection.TryAddSingleton<INamedDataTemplateFactory, NamedDataTemplateFactory>();
                serviceCollection.TryAddSingleton<ITemplateSelector, TemplateSelector>();

                foreach (ITemplateDescriptor? descriptor in builder.Descriptors)
                {
                    serviceCollection.Add(new ServiceDescriptor(descriptor.TemplateType, descriptor.TemplateType, descriptor.Lifetime));
                    serviceCollection.Add(new ServiceDescriptor(descriptor.DataType, descriptor.DataType, descriptor.Lifetime));
                }
            });

            return hostBuilder;
        }
    }
}
