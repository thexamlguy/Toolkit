using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Toolkit.Foundation;

public class DefaultHostBuilder :
    HostBuilder
{
    public static IHostBuilder Create(Action<HostBuilder> builderDelegate)
    {
        HostBuilder hostBuilder = new();

        hostBuilder.ConfigureServices((context, services) =>
        {
            services.AddScoped<IServiceFactory>(provider =>
                new ServiceFactory((type, parameters) =>
                    ActivatorUtilities.CreateInstance(provider, type,
                        parameters?.Where(x => x is not null).ToArray()!)));

            services.AddSingleton<IComponentHostCollection,
                ComponentHostCollection>();

            services.AddSingleton<IDisposer, Disposer>();
            services.AddScoped<IMessenger, StrongReferenceMessenger>();

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

            services.AddTransient<IComparer<Scoped>>(provider => Comparer<Scoped>.Create((x, z) =>
                StringComparer.CurrentCultureIgnoreCase.Compare(x.Key, z.Key)));

            Scoped named = new("Root");
            services.AddSingleton<IScopedServiceDescriptor<Scoped>>(provider =>
            {
                ScopedServiceDescriptor<Scoped> descriptor = new();
                descriptor.Set(named);

                return descriptor;
            });

            services.AddSingleton(provider => provider.GetRequiredService<IScopedServiceDescriptor<Scoped>>().Value!);

            services.AddSingleton<ICache<Scoped, IServiceProvider>>(provider =>
            {
                Cache<Scoped, IServiceProvider> cache = new(provider.GetRequiredService<IComparer<Scoped>>())
                {
                    { named, provider }
                };

                return cache;
            });

            services.AddTransient(provider => provider.GetService<ICache<Scoped, IServiceProvider>>()!.Select(x => x.Value));

            services.AddTransient<IScopedServiceProvider<Scoped>, ScopedServiceProvider<Scoped>>();
            services.AddTransient<IScopedServiceFactory<Scoped>, ScopedServiceFactory<Scoped>>();

            //services.AddScoped<IComponentScopeCollection, ComponentScopeCollection>(provider =>
            //[
            //    new ComponentScopeDescriptor("Root", provider.GetRequiredService<IServiceProvider>())
            //]);

            //services.AddTransient<IComponentFactory, ComponentFactory>();
            //services.AddTransient<IComponentScopeProvider, ComponentScopeProvider>();

            services.AddScopedHandler<NavigateEventArgs, NavigateHandler>();
            services.AddScopedHandler<NavigateBackEventArgs, NavigateBackHandler>();

            services.AddTransient<IFileProvider, FileProvider>();

            //services.AddInitialization<ComponentInitializer>();
        });

        builderDelegate.Invoke(hostBuilder);

        hostBuilder.ConfigureServices((context, services) =>
        {
            services.AddHostedService<AppService>();
        });
        
        return hostBuilder;
    }
}