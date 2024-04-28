using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public class ComponentFactory(IServiceProvider provider,
    IProxyServiceCollection<IComponentBuilder> proxy,
    IComponentScopeCollection scopes) : 
    IComponentFactory
{
    public IComponentHost? Create<TComponent>(string name,
        ComponentConfiguration configuration, Action<IServiceCollection>? servicesDelegate = null) 
        where TComponent : IComponent
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
                    provider.GetRequiredService<INavigationContextCollection>());

                services.AddScoped(_ =>
                    provider.GetRequiredService<INavigationContextProvider>());

                services.AddScoped(_ =>
                    provider.GetRequiredService<IComponentScopeCollection>());

                services.AddTransient(_ =>
                    provider.GetRequiredService<IComponentScopeProvider>());

                services.AddRange(proxy.Services);
                services.AddSingleton(new ComponentScope(name));

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
