using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Toolkit.Foundation;

public class ComponentInitializer(IEnumerable<IComponent> components,
    IProxyServiceCollection<IComponentBuilder> typedServices,
    IComponentHostCollection hosts,
    IComponentScopeCollection scopes,
    IServiceProvider provider) :
    IInitialization
{
    public void Initialize()
    {
        foreach (IComponent component in components)
        {
            IComponentBuilder builder = component.Configure("");
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

                services.AddRange(typedServices.Services);

                services.AddSingleton(new NamedComponent(component.GetType().Name));
            });

            IComponentHost host = builder.Build();

            scopes.Add(new ComponentScopeDescriptor(component.GetType().Name,
                provider.GetRequiredService<IServiceProvider>()));

            hosts.Add(host);
            host.Start();
        }
    }
}