﻿using Microsoft.Extensions.Configuration;
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
                    new ServiceFactory((type, parameters) => ActivatorUtilities.CreateInstance(provider, type, parameters!)));

                services.AddSingleton<IDisposer, Disposer>();

                services.AddScoped<SubscriptionCollection>();

                services.AddTransient<IHandlerProvider, HandlerProvider>();
                services.AddScoped<ISubscriber, Subscriber>();
                services.AddTransient<IPublisher, Publisher>();
                services.AddTransient<IMediator, Mediator>();

                services.AddTransient<IValidation, Validation>();
                services.AddTransient<IValidatorCollection, ValidatorCollection>();

                services.AddTransient<IContentFactory, ContentFactory>();
                services.AddTransient<INavigation, Navigation>();

                services.AddScoped<INavigationRegionCollection, NavigationRegionCollection>();
                services.AddTransient<INavigationRegionProvider, NavigationRegionProvider>();

                services.AddHandler<NavigateHandler>();
                services.AddHandler<NavigateBackHandler>();
            });
    }

    public static IComponentBuilder Create() =>
        new ComponentBuilder();

    public IComponentBuilder AddConfiguration<TConfiguration>(Action<TConfiguration> configurationDelegate)
        where TConfiguration : ComponentConfiguration, new()
    {
        TConfiguration configuration = new();

        if (configurationDelegate is not null)
        {
            configurationDelegate(configuration);
        }

        AddConfiguration(typeof(TConfiguration).Name, configuration);
        return this;
    }

    public IComponentBuilder AddConfiguration<TConfiguration>(string section,
        TConfiguration? configuration = null)
        where TConfiguration :
        ComponentConfiguration, new()
    {
        if (configurationRegistered)
        {
            return this;
        }

        configurationRegistered = true;

        hostBuilder.AddConfiguration(section: section,
            defaultConfiguration: configuration);

        return this;
    }

    public IComponentBuilder AddConfiguration<TConfiguration>(string section)
        where TConfiguration : ComponentConfiguration, new()
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