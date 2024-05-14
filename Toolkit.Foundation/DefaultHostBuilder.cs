using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Toolkit.Foundation;

public class DefaultHostBuilder :
    HostBuilder
{
    public static IHostBuilder Create()
    {
        return new HostBuilder()
            .UseContentRoot("Local", true)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("Settings.json", true, true);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddScoped<IServiceFactory>(provider =>
                    new ServiceFactory((type, parameters) => ActivatorUtilities.CreateInstance(provider, type, parameters!)));

                services.AddSingleton<IComponentHostCollection,
                    ComponentHostCollection>();

                services.AddSingleton<IDisposer, Disposer>();

                services.AddScoped<SubscriptionCollection>();
                services.AddTransient<ISubscriptionManager, SubscriptionManager>();

                services.AddTransient<ISubscriber, Subscriber>();

                services.AddTransient<IPublisher, Publisher>();
                services.AddTransient<IMediator, Mediator>();

                services.AddScoped<IProxyService<IPublisher>>(provider =>
                    new ProxyService<IPublisher>(provider.GetRequiredService<IPublisher>()));

                services.AddScoped<IProxyService<INavigationRegionProvider>>(provider =>
                    new ProxyService<INavigationRegionProvider>(provider.GetRequiredService<INavigationRegionProvider>()));

                services.AddScoped<IProxyService<IComponentHostCollection>>(provider =>
                    new ProxyService<IComponentHostCollection>(provider.GetRequiredService<IComponentHostCollection>()));

                services.AddTransient<IContentTemplateDescriptorProvider, ContentTemplateDescriptorProvider>();

                services.AddTransient<INavigationProvider, NavigationProvider>();

                services.AddScoped<INavigationRegionCollection, NavigationRegionCollection>();
                services.AddTransient<INavigationRegionProvider, NavigationRegionProvider>();

                services.AddScoped<INavigationScope, NavigationScope>();

                services.AddSingleton(new NamedComponent("Root"));
                services.AddScoped<IComponentScopeCollection, ComponentScopeCollection>(provider => new ComponentScopeCollection
                {
                    new ComponentScopeDescriptor("Root", provider.GetRequiredService<IServiceProvider>())
                });

                services.AddTransient<IComponentFactory, ComponentFactory>();
                services.AddTransient<IComponentScopeProvider, ComponentScopeProvider>();

                services.AddHandler<NavigateHandler>();
                services.AddHandler<NavigateBackHandler>();

                services.AddInitializer<ComponentInitializer>();
                services.AddHostedService<AppService>();
            });
    }
}