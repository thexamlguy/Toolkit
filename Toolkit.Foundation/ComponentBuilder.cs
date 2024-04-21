using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Toolkit.Foundation;

public class ComponentBuilder : 
    IComponentBuilder
{
    private readonly IHostBuilder hostBuilder;

    private bool configurationRegistered;

    private ComponentBuilder()
    {
        hostBuilder = new HostBuilder()
            .UseContentRoot("Local", true)
            .ConfigureAppConfiguration(config =>
            {
                config.AddJsonFile("Settings.json", true, true);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddScoped<IComponentHost, ComponentHost>();

                services.AddScoped<IServiceFactory>(provider =>
                    new ServiceFactory((type, parameters) =>
                        ActivatorUtilities.CreateInstance(provider, type, parameters!)));

                services.AddScoped<SubscriptionCollection>();
                services.AddScoped<ISubscriptionManager, SubscriptionManager>();

                services.AddTransient<ISubscriber, Subscriber>();
                services.AddTransient<IPublisher, Publisher>();

                services.AddTransient<IMediator, Mediator>();
                services.AddScoped<IDisposer, Disposer>();

                services.AddTransient<INavigationScope, NavigationScope>();

                services.AddTransient<INavigationProvider, NavigationProvider>();
                services.AddScoped<INavigationContextCollection, NavigationContextCollection>();
                services.AddTransient<INavigationContextProvider, NavigationContextProvider>();

                services.AddHandler<NavigateHandler>();
                services.AddHandler<NavigateBackHandler>();
            });
    }

    public static IComponentBuilder Create() =>
        new ComponentBuilder();

    public IComponentBuilder AddConfiguration<TConfiguration>(Action<TConfiguration> configurationDelegate)
        where TConfiguration : ComponentConfiguration, new()
    {
        AddConfiguration(typeof(TConfiguration).Name, configurationDelegate);
        return this;
    }

    public IComponentBuilder AddConfiguration<TConfiguration>(string section,
        Action<TConfiguration>? configurationDelegate = null) 
        where TConfiguration :
        ComponentConfiguration, new()
    {
        if (configurationRegistered)
        {
            return this;
        }

        configurationRegistered = true;
        TConfiguration configuration = new();

        if (configurationDelegate is not null)
        {
            configurationDelegate(configuration);
        }

        hostBuilder.ConfigureServices(services =>
        {
            services.AddConfiguration<ComponentConfiguration>(section: section,
                defaultConfiguration: configuration);

            services.AddConfiguration(configuration);
        });

        return this;
    }

    public IComponentBuilder AddConfiguration<TConfiguration>(string section) 
        where TConfiguration : 
        ComponentConfiguration, new()
    {
        AddConfiguration<TConfiguration>(section, null);
        return this;
    }

    public IComponentBuilder AddServices(Action<IServiceCollection> configureDelegate)
    {
        hostBuilder.ConfigureServices(configureDelegate);
        return this;
    }

    public IComponentHost Build()
    {
        IHost host = hostBuilder.Build();
        return host.Services.GetRequiredService<IComponentHost>();
    }
}