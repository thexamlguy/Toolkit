using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public class ComponentFactory(IServiceProvider provider,
    IProxyServiceCollection<IComponentBuilder> proxy,
    IComponentScopeCollection scopes) :
    IComponentFactory
{
    public IComponentHost? Create<TComponent, TConfiguration>(string key,
        Action<IConfigurationBuilder<TConfiguration>>? configurationDelegate = null,
        Action<IComponentBuilder>? componentDelegate = null,
        Action<IServiceCollection>? servicesDelegate = null)
        where TComponent : IComponent
        where TConfiguration : class, new()
    {
        if (provider.GetRequiredService<TComponent>() is TComponent component)
        {
            IComponentBuilder builder = component.Configure(configurationDelegate,
                componentDelegate);

            builder.AddServices(services =>
            {
                services.AddTransient(_ => 
                    provider.GetRequiredService<IProxyServiceCollection<IComponentBuilder>>());

                services.AddTransient(_ =>
                    provider.GetRequiredService<IComponentFactory>());

                services.AddTransient(_ =>
                    provider.GetRequiredService<IProxyService<IMessenger>>());

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
                services.AddSingleton(new NamedComponent(key));

                if (servicesDelegate is not null)
                {
                    servicesDelegate(services);
                }
            });

            IComponentHost host = builder.Build();

            scopes.Add(new ComponentScopeDescriptor(key,
                 host.Services.GetRequiredService<IServiceProvider>()));

            return host;
        }

        return default;
    }
}