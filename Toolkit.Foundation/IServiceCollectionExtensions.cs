using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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

                    if (key is not null)
                    {
                        services.Add(new ServiceDescriptor(typeof(INotificationHandler<>)
                            .MakeGenericType(notificationType), key, typeof(THandler), lifetime));
                    }
                    else
                    {
                        services.Add(new ServiceDescriptor(typeof(INotificationHandler<>)
                            .MakeGenericType(notificationType), typeof(THandler), lifetime));
                    }


                    if (key is not null)
                    {
                        services.Add(new ServiceDescriptor(wrapperType, key, (provider, key) =>
                            provider.GetService<IServiceFactory>()?.Create(wrapperType,
                                provider.GetRequiredKeyedService(typeof(INotificationHandler<>).MakeGenericType(notificationType), key),
                                provider.GetServices(typeof(IPipelineBehaviour<>)
                                    .MakeGenericType(notificationType)))!, lifetime));
                    }
                    else
                    {
                        services.Add(new ServiceDescriptor(wrapperType, provider =>
                            provider.GetService<IServiceFactory>()?.Create(wrapperType,
                                provider.GetRequiredService(typeof(INotificationHandler<>).MakeGenericType(notificationType)),
                                provider.GetServices(typeof(IPipelineBehaviour<>)
                                    .MakeGenericType(notificationType)))!, lifetime));
                    }
                }

                if (contract.Name == typeof(IHandler<,>).Name &&
                    contract.GetGenericArguments() is { Length: 2 } handlerArguments)
                {
                    Type requestType = handlerArguments[0];
                    Type responseType = handlerArguments[1];

                    Type wrapperType = typeof(HandlerWrapper<,>)
                        .MakeGenericType(requestType, responseType);

                    if (key is not null)
                    {
                        services.Add(new ServiceDescriptor(typeof(THandler), key,
                              typeof(THandler), lifetime));
                    }
                    else
                    {
                        services.Add(new ServiceDescriptor(typeof(THandler),
                              typeof(THandler), lifetime));
                    }

                    if (key is not null)
                    {              
                        services.Add(new ServiceDescriptor(wrapperType, key, (provider, key) =>
                            provider.GetService<IServiceFactory>()?.Create(wrapperType,
                                    provider.GetRequiredKeyedService<THandler>(key),
                                    provider.GetServices(typeof(IPipelineBehaviour<,>)
                                        .MakeGenericType(requestType, responseType)))!, lifetime));
                    }
                    else
                    {
                        services.Add(new ServiceDescriptor(wrapperType, provider =>
                            provider.GetService<IServiceFactory>()?.Create(wrapperType,
                                    provider.GetRequiredService<THandler>(),
                                    provider.GetServices(typeof(IPipelineBehaviour<,>)
                                        .MakeGenericType(requestType, responseType)))!, lifetime));
                    }
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

        services.AddTransient<IContentTemplateDescriptor>(provider =>
            new ContentTemplateDescriptor(key, viewModelType, viewType, parameters));

        return services;
    }
}