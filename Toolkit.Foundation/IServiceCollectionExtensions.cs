using Microsoft.Extensions.DependencyInjection;

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

    public static IServiceCollection AddHandler<THandler>(this IServiceCollection services,
        string key)
        where THandler : IHandler
    {
        return AddHandler<THandler>(services, ServiceLifetime.Transient, key);
    }

    public static IServiceCollection AddHandler<THandler>(this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Transient,
        string? key = null)
        where THandler : IHandler
    {
        if (typeof(THandler).GetInterfaces() is Type[] handlerTypes)
        {
            foreach (Type handlerType in handlerTypes)
            {
                if (handlerType.Name == typeof(INotificationHandler<>).Name &&
                    handlerType.GetGenericArguments() is { Length: 1 } notificationHandlerArguments)
                {
                    Type notificationType = notificationHandlerArguments[0];
                    Type wrapperType = typeof(NotificationHandlerWrapper<>).MakeGenericType(notificationType);

                    string preferredKey = $"{(key is not null ? $"{key}:" : "")}{notificationType}";

                    services.Add(new ServiceDescriptor(typeof(INotificationHandler<>)
                        .MakeGenericType(notificationType), preferredKey, typeof(THandler), lifetime));

                    services.Add(new ServiceDescriptor(wrapperType, preferredKey, (provider, registeredKey) =>
                            provider.GetService<IServiceFactory>()?.Create(wrapperType,
                                provider.GetRequiredKeyedService(typeof(INotificationHandler<>).MakeGenericType(notificationType), registeredKey),
                                provider.GetServices(typeof(IPipelineBehaviour<>)
                                    .MakeGenericType(notificationType)))!, lifetime));
                }

                if (handlerType.Name == typeof(IHandler<,>).Name &&
                    handlerType.GetGenericArguments() is { Length: 2 } handlerArguments)
                {
                    Type requestType = handlerArguments[0];
                    Type responseType = handlerArguments[1];

                    Type wrapperType = typeof(HandlerWrapper<,>).MakeGenericType(requestType, responseType);
                    string preferredKey = $"{(key is not null ? $"{key}:" : "")}{wrapperType}";

                    services.Add(new ServiceDescriptor(typeof(THandler), preferredKey,
                        typeof(THandler), lifetime));

                    services.Add(new ServiceDescriptor(wrapperType, preferredKey, (provider, actualKey) =>
                        provider.GetService<IServiceFactory>()?.Create(wrapperType,
                                provider.GetRequiredKeyedService<THandler>(preferredKey),
                                provider.GetServices(typeof(IPipelineBehaviour<,>)
                                    .MakeGenericType(requestType, responseType)))!, lifetime));
                }
            }

            return services;
        }

        return services;
    }

    public static IServiceCollection AddInitializer<TInitializer>(this IServiceCollection services)
        where TInitializer : class,
        IInitialization
    {
        services.AddTransient<IInitialization, TInitializer>();
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
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient,
        params object[]? parameters)
    {
        Type viewModelType = typeof(TViewModel);
        Type viewType = typeof(TView);

        key ??= viewModelType.Name.Replace("ViewModel", "");

        if (parameters is { Length: 0 })
        {
            services.Add(new ServiceDescriptor(viewModelType, viewModelType, serviceLifetime));
        }
        else
        {
            services.Add(new ServiceDescriptor(viewModelType, provider =>
                provider.GetRequiredService<IServiceFactory>().Create<TViewModel>(parameters)!, serviceLifetime));
        }

        services.Add(new ServiceDescriptor(viewType, viewType, serviceLifetime));

        if (parameters is { Length: 0 })
        {
            services.Add(new ServiceDescriptor(viewModelType, key, viewModelType, serviceLifetime));
        }
        else
        {
            services.Add(new ServiceDescriptor(viewModelType, key, (provider, key) =>
                provider.GetRequiredService<IServiceFactory>().Create<TViewModel>(parameters)!, serviceLifetime));
        }

        services.Add(new ServiceDescriptor(viewType, key, viewType, serviceLifetime));

        services.AddKeyedTransient<IContentTemplateDescriptor>(key, (provider, _) =>
            new ContentTemplateDescriptor(key, viewModelType, viewType, parameters));

        return services;
    }
}