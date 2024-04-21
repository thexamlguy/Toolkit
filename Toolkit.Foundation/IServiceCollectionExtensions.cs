using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;
using Microsoft.Extensions.Hosting;
using System;
using System.Text.Json;
using Toolkit.Foundation;

namespace Toolkit.Foundation;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddCache<TKey, TValue>(this IServiceCollection services)
        where TKey : notnull
        where TValue : notnull
    {
        services.AddScoped<ICache<TKey, TValue>, Cache<TKey, TValue>>();
        services.AddTransient(provider => provider.GetService<ICache<TKey, TValue>>()!.Select(x => x.Value));

        return services;
    }

    public static IServiceCollection AddCache<TValue>(this IServiceCollection services)
    {
        services.AddSingleton<ICache<TValue>, Cache<TValue>>();
        services.AddTransient(provider => provider.GetService<ICache<TValue>>()!.Select(x => x));

        return services;
    }

    public static IServiceCollection AddComponent<TComponent>(this IServiceCollection services)
        where TComponent : class, 
        IComponent
    {
        services.AddTransient<IComponent, TComponent>();
        return services;
    }

    public static IServiceCollection AddConfiguration<TConfiguration,
        TValue>(this IServiceCollection services,
        Func<TConfiguration, Action<TValue>> changed)
        where TConfiguration : class
        where TValue : class, new()
    {
        services.AddSingleton(new ConfigurationValue<TConfiguration, TValue>(changed));
        services.AddHandler<ConfigurationChangedHandler<TConfiguration, TValue>>();

        return services;
    }

    public static IServiceCollection AddConfiguration<TConfiguration>(this IServiceCollection services, 
        string section)
        where TConfiguration : class, new() =>
            services.AddConfiguration<TConfiguration>(section, "Settings.json", null);

    public static IServiceCollection AddConfiguration<TConfiguration>(this IServiceCollection services)
        where TConfiguration : class, new() => 
            services.AddConfiguration<TConfiguration>(typeof(TConfiguration).Name, "Settings.json", null);

    public static IServiceCollection AddConfiguration<TConfiguration>(this IServiceCollection services,
        Action<TConfiguration> configurationDelegate)
        where TConfiguration : class, new()
    {
        TConfiguration configuration = new();
        configurationDelegate.Invoke(configuration);

        return services.AddConfiguration(typeof(TConfiguration).Name, "Settings.json", configuration);
    }

    public static IServiceCollection AddConfiguration<TConfiguration>(this IServiceCollection services,
        Action<TConfiguration> configurationDelegate,
        string section)
        where TConfiguration : class, new()
    {
        TConfiguration configuration = new();
        configurationDelegate.Invoke(configuration);

        return services.AddConfiguration(section, "Settings.json", configuration);
    }

    public static IServiceCollection AddConfiguration<TConfiguration>(this IServiceCollection services,
        TConfiguration configuration)
        where TConfiguration : class, new() =>
            services.AddConfiguration(configuration.GetType().Name, "Settings.json", configuration);

    public static IServiceCollection AddConfiguration<TConfiguration>(this IServiceCollection services,
        object configuration)
        where TConfiguration : class, new() => 
            services.AddConfiguration(configuration.GetType().Name,
                "Settings.json", (TConfiguration?)configuration);

    public static IServiceCollection AddConfiguration<TConfiguration>(this IServiceCollection services,
        string section,
        string path = "Settings.json",
        TConfiguration? defaultConfiguration = null,
        Action<JsonSerializerOptions>? serializerDelegate = null)
        where TConfiguration : class, new()
    {
        services.TryAddSingleton<IConfigurationFile<TConfiguration>>(provider =>
        {
            IFileInfo? fileInfo = null;
            if (provider.GetService<IHostEnvironment>() is IHostEnvironment hostEnvironment)
            {
                IFileProvider fileProvider = hostEnvironment.ContentRootFileProvider;
                fileInfo = fileProvider.GetFileInfo(path);
            }

            fileInfo ??= new PhysicalFileInfo(new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path)));
            return new ConfigurationFile<TConfiguration>(fileInfo);
        });

        services.TryAddKeyedSingleton<IConfigurationSource<TConfiguration>>(section, (provider, KeyAccelerator) =>
        {
            JsonSerializerOptions? defaultSerializer = null;
            if (serializerDelegate is not null)
            {
                defaultSerializer = new JsonSerializerOptions();
                serializerDelegate.Invoke(defaultSerializer);
            }

            return new ConfigurationSource<TConfiguration>(provider.GetRequiredService<IConfigurationFile<TConfiguration>>(),
                section, defaultSerializer);
        });

        //services.AddHostedService<ConfigurationMonitor<TConfiguration>>();
        services.TryAddKeyedTransient<IConfigurationReader<TConfiguration>>(section, (provider, key) =>
            new ConfigurationReader<TConfiguration>(provider.GetRequiredKeyedService<IConfigurationSource<TConfiguration>>(key),
                provider.GetRequiredKeyedService<IConfigurationFactory<TConfiguration>>(key)));

        services.TryAddKeyedTransient<IConfigurationWriter<TConfiguration>>(section, (provider, key) =>
            new ConfigurationWriter<TConfiguration>(provider.GetRequiredKeyedService<IConfigurationSource<TConfiguration>>(key)));

        services.TryAddKeyedTransient<IConfigurationFactory<TConfiguration>>(section, (provider, key) => 
            new ConfigurationFactory<TConfiguration>(() => defaultConfiguration ?? new TConfiguration()));

        services.AddTransient<IInitializer, ConfigurationInitializer<TConfiguration>>(provider =>
            new ConfigurationInitializer<TConfiguration>(provider.GetRequiredKeyedService<IConfigurationReader<TConfiguration>>(section),
                provider.GetRequiredKeyedService<IConfigurationWriter<TConfiguration>>(section),
                provider.GetRequiredKeyedService<IConfigurationFactory<TConfiguration>>(section),
                provider.GetRequiredService<IPublisher>()));

        services.AddTransient<IConfigurationInitializer<TConfiguration>, ConfigurationInitializer<TConfiguration>>(provider =>
            provider.GetRequiredService<IServiceFactory>().Create<ConfigurationInitializer<TConfiguration>>(section));

        services.AddTransient<IWritableConfiguration<TConfiguration>, WritableConfiguration<TConfiguration>>();

        services.TryAddKeyedTransient<IConfiguration<TConfiguration>>(section, (provider, key) =>
            new Configuration<TConfiguration>(section, provider.GetRequiredKeyedService<IConfigurationReader<TConfiguration>>(key)));

        services.AddTransient(provider => provider.GetRequiredKeyedService<IConfiguration<TConfiguration>>(section));
        services.AddTransient(provider => provider.GetRequiredKeyedService<IConfiguration<TConfiguration>>(section).Value);

        return services;
    }

    public static IServiceCollection AddHandler<THandler>(this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Transient)
        where THandler : IHandler
    {
        if (typeof(THandler).GetInterfaces() is Type[] contracts)
        {
            foreach (Type contract in contracts)
            {
                if (contract.Name == typeof(INotificationHandler<>).Name &&
                    contract.GetGenericArguments() is { Length: 1 } notificationHandlerArguments)
                {
                    Type notificationType = notificationHandlerArguments[0];

                    Type wrapperType = typeof(NotificationHandlerWrapper<>)
                        .MakeGenericType(notificationType);

                    services.TryAdd(new ServiceDescriptor(typeof(INotificationHandler<>)
                        .MakeGenericType(notificationType), typeof(THandler), lifetime));

                    services.Add(new ServiceDescriptor(wrapperType, provider =>
                        provider.GetService<IServiceFactory>()?.Create(wrapperType,
                            provider.GetRequiredService(typeof(INotificationHandler<>).MakeGenericType(notificationType)),
                            provider.GetServices(typeof(IPipelineBehavior<>)
                                .MakeGenericType(notificationType)))!, lifetime));
                }

                if (contract.Name == typeof(IHandler<,>).Name &&
                    contract.GetGenericArguments() is { Length: 2 } handlerArguments)
                {
                    Type requestType = handlerArguments[0];
                    Type responseType = handlerArguments[1];

                    Type wrapperType = typeof(HandlerWrapper<,>)
                        .MakeGenericType(requestType, responseType);

                    services.TryAdd(new ServiceDescriptor(typeof(THandler),
                        typeof(THandler), lifetime));

                    services.Add(new ServiceDescriptor(wrapperType, provider =>
                        provider.GetService<IServiceFactory>()?.Create(wrapperType,
                                provider.GetRequiredService<THandler>(),
                                provider.GetServices(typeof(IPipelineBehavior<,>)
                                    .MakeGenericType(requestType, responseType)))!, lifetime));
                }
            }

            return services;
        }

        return services;
    }

    public static IServiceCollection AddInitializer<TInitializer>(this IServiceCollection services)
        where TInitializer : class,
        IInitializer
    {
        services.AddTransient<IInitializer, TInitializer>();
        return services;
    }

    public static IServiceCollection AddNavigateHandler<THandler>(this IServiceCollection services)
        where THandler : INavigateHandler,
        IHandler
    {
        IEnumerable<Type> contracts = typeof(THandler).GetInterfaces()
            .Where(x => x.Name == typeof(INavigateHandler<>).Name || x.Name == typeof(INavigateBackHandler<>).Name);

        foreach (Type contract in contracts)
        {
            if (contract.GetGenericArguments() is { Length: 1 } arguments)
            {
                services.AddTransient<INavigation>(provider => new Navigation
                {
                    Type = arguments[0]
                });
            }
        }

        services.AddHandler<THandler>();
        return services;
    }

    public static IServiceCollection AddRange(this IServiceCollection services,
        IServiceCollection fromServices)
    {
        foreach (ServiceDescriptor service in fromServices)
        {
            services.Add(service);
        }

        return services;
    }

    public static IServiceCollection AddTemplate<TViewModel, TView>(this IServiceCollection services,
                        object? key = null,
        params object[]? parameters)
    {
        Type viewModelType = typeof(TViewModel);
        Type viewType = typeof(TView);

        key ??= viewModelType.Name.Replace("ViewModel", "");

        services.AddTransient(viewModelType, provider => 
            provider.GetRequiredService<IServiceFactory>().Create<TViewModel>(parameters)!);

        services.AddTransient(viewType);

        services.AddKeyedTransient(viewModelType, key, (provider, key) =>
            provider.GetRequiredService<IServiceFactory>().Create<TViewModel>(parameters)!);

        services.AddKeyedTransient(viewType, key);

        services.AddTransient<IContentTemplateDescriptor>(provider => 
            new ContentTemplateDescriptor(key, viewModelType, viewType, parameters));

        return services;
    }
}