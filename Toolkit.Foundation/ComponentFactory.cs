using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public class ComponentFactory(IServiceProvider provider,
    IProxyServiceCollection<IComponentBuilder> proxy,
    IComponentScopeCollection scopes) : 
    IComponentFactory
{
    public IComponentHost? Create<TComponent, TConfiguration>(string name,
        TConfiguration configuration, 
        Action<IServiceCollection>? servicesDelegate = null)
        where TComponent : IComponent
        where TConfiguration : ComponentConfiguration, new()
    {
        if (provider.GetRequiredService<TComponent>() is TComponent component)
        {
            IComponentBuilder builder = component.Create();
            builder.AddServices(services =>
            {
                services.AddTransient(_ =>
                    provider.GetRequiredService<IProxyService<IPublisher>>());

                services.AddTransient(_ =>
                    provider.GetRequiredService<IProxyService<IComponentHostCollection>>());

                services.AddScoped(_ =>
                    provider.GetRequiredService<INavigationRegionCollection>());

                services.AddScoped(_ =>
                    provider.GetRequiredService<INavigationRegionProvider>());

                services.AddScoped(_ =>
                    provider.GetRequiredService<IComponentScopeCollection>());

                services.AddTransient(_ =>
                    provider.GetRequiredService<IComponentScopeProvider>());

                services.AddRange(proxy.Services);
                services.AddSingleton(new NamedComponent(name));

                if (servicesDelegate is not null)
                {
                    servicesDelegate(services);
                }
            });

            builder.AddConfiguration(name, configuration);
            IComponentHost host = builder.Build();

            scopes.Add(new ComponentScopeDescriptor(name,
                 host.Services.GetRequiredService<IServiceProvider>()));

            return host;
        }

        return default;
    }
}
