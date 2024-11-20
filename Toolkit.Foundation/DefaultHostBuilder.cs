using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Toolkit.Foundation;

public class DefaultHostBuilder :
    HostBuilder
{
    public static IHostBuilder Create()
    {
        return new HostBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddScoped<IServiceFactory>(provider =>
                    new ServiceFactory((type, parameters) => 
                        ActivatorUtilities.CreateInstance(provider, type,
                            parameters?.Where(x => x is not null).ToArray()!)));

                services.AddSingleton<IComponentHostCollection,
                    ComponentHostCollection>();

                services.AddSingleton<IDisposer, Disposer>();
                services.AddSingleton<IMessenger, StrongReferenceMessenger>(_ => StrongReferenceMessenger.Default);


                services.AddTransient<IValidation, Validation>();
                services.AddTransient<IValidatorCollection, ValidatorCollection>();

                services.AddScoped<IProxyService<IMessenger>>(provider =>
                    new ProxyService<IMessenger>(provider.GetRequiredService<IMessenger>()));

                services.AddScoped<IProxyService<INavigationRegionProvider>>(provider =>
                    new ProxyService<INavigationRegionProvider>(provider.GetRequiredService<INavigationRegionProvider>()));

                services.AddScoped<IProxyService<IComponentHostCollection>>(provider =>
                    new ProxyService<IComponentHostCollection>(provider.GetRequiredService<IComponentHostCollection>()));

                services.AddTransient<IContentFactory, ContentFactory>();

                services.AddScoped<INavigationRegionCollection, NavigationRegionCollection>();
                services.AddTransient<INavigationRegionProvider, NavigationRegionProvider>();

                services.AddScoped<INavigation, Navigation>();

                services.AddSingleton(new NamedComponent("Root"));
                services.AddScoped<IComponentScopeCollection, ComponentScopeCollection>(provider =>
                [
                    new ComponentScopeDescriptor("Root", provider.GetRequiredService<IServiceProvider>())
                ]);

                services.AddTransient<IComponentFactory, ComponentFactory>();
                services.AddTransient<IComponentScopeProvider, ComponentScopeProvider>();

                services.AddHandler<NavigateEventArgs, NavigateHandler>();
                services.AddHandler<NavigateBackEventArgs, NavigateBackHandler>();

                services.AddInitialization<ComponentInitializer>();
                services.AddHostedService<AppService>();
            });
    }
}