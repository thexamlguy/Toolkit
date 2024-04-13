using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Toolkit.Foundation;

namespace Toolkit.Foundation;

public class DefaultBuilder : 
    HostBuilder
{
    public static IHostBuilder Create()
    {
        return new HostBuilder()
            .UseContentRoot("Local", true)
            .ConfigureAppConfiguration(config =>
            {
                config.AddJsonFile("Settings.json", true, true);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddScoped<IServiceFactory>(provider =>
                    new ServiceFactory((type, parameters) => ActivatorUtilities.CreateInstance(provider, type, parameters!)));

                services.AddSingleton<IComponentHostCollection, 
                    ComponentHostCollection>();

                services.AddScoped<SubscriptionCollection>();
                services.AddScoped<ISubscriptionManager, SubscriptionManager>();
                services.AddTransient<ISubscriber, Subscriber>();
                services.AddTransient<IPublisher, Publisher>();

                services.AddScoped<IMediator, Mediator>();

                services.AddScoped<IProxyService<IPublisher>>(provider =>
                    new ProxyService<IPublisher>(provider.GetRequiredService<IPublisher>()));

                services.AddScoped<IProxyService<INavigationContextProvider>>(provider =>
                    new ProxyService<INavigationContextProvider>(provider.GetRequiredService<INavigationContextProvider>()));

                services.AddScoped<IProxyService<IComponentHostCollection>>(provider =>
                    new ProxyService<IComponentHostCollection>(provider.GetRequiredService<IComponentHostCollection>()));

                services.AddScoped<IDisposer, Disposer>();

                services.AddTransient<IContentTemplateDescriptorProvider, ContentTemplateDescriptorProvider>();

                services.AddTransient<INavigationProvider, NavigationProvider>();

                services.AddScoped<INavigationContextCollection, NavigationContextCollection>();
                services.AddTransient<INavigationContextProvider, NavigationContextProvider>();

                services.AddTransient<INavigationScope, NavigationScope>();
         
                services.AddScoped<IComponentScopeCollection, ComponentScopeCollection>(provider => new ComponentScopeCollection
                {
                    { "Default", provider.GetRequiredService<IServiceProvider>() }
                });

                services.AddTransient<IComponentScopeProvider, ComponentScopeProvider>();

                services.AddHandler<NavigateHandler>();
                services.AddHandler<NavigateBackHandler>();

                services.AddInitializer<ComponentInitializer>();
                services.AddHostedService<AppService>();
            });
    }
}
