using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddAsyncInitialization<TInitialization>(this IServiceCollection services)
        where TInitialization : class,
        IAsyncInitialization
    {
        services.AddTransient<IAsyncInitialization, TInitialization>();
        return services;
    }

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

    public static IServiceCollection AddHandler<TMessage, TResponse, THandler>(this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Transient)
        where THandler : class, IHandler<TMessage, TResponse>
        where TMessage : class
    {
        services.Add(new ServiceDescriptor(typeof(THandler), typeof(THandler), lifetime));
        services.AddInitialization<HandlerInitialization<TMessage, TResponse, THandler>>();

        return services;
    }

    public static IServiceCollection AddAsyncHandler<TMessage, TResponse, THandler>(this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Transient)
        where THandler : class, IAsyncHandler<TMessage, TResponse>
        where TMessage : class
    {
        services.Add(new ServiceDescriptor(typeof(THandler), typeof(THandler), lifetime));
        services.AddInitialization<AsyncHandlerInitialization<TMessage, TResponse, THandler>>();

        return services;
    }

    public static IServiceCollection AddAsyncHandler<TMessage, THandler>(this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Transient)
        where THandler : class, IAsyncHandler<TMessage>
        where TMessage : class
    {
        services.Add(new ServiceDescriptor(typeof(THandler), typeof(THandler), lifetime));
        services.AddInitialization<AsyncHandlerInitialization<TMessage, THandler>>();

        return services;
    }

    public static IServiceCollection AddHandler<TMessage, THandler>(this IServiceCollection services,
        string key)
        where THandler : class, IHandler<TMessage>
        where TMessage : class => AddHandler<TMessage, THandler>(services, ServiceLifetime.Transient, key);

    public static IServiceCollection AddHandler<TMessage, THandler>(this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Transient,
        string? key = null)
        where THandler : class, IHandler<TMessage>
        where TMessage : class
    {
        if (key is { Length: > 0})
        {
            services.Add(new ServiceDescriptor(typeof(THandler), key, typeof(THandler), lifetime));
            services.AddInitialization<HandlerInitialization<TMessage, THandler>>(key);
        }
        else
        {
            services.Add(new ServiceDescriptor(typeof(THandler), typeof(THandler), lifetime));
            services.AddInitialization<HandlerInitialization<TMessage, THandler>>();
        }

        return services;
    }

    public static IServiceCollection AddInitialization<TInitialization>(this IServiceCollection services)
        where TInitialization : class,
        IInitialization
    {
        services.AddTransient<IInitialization, TInitialization>();
        return services;
    }

    public static IServiceCollection AddInitialization<TInitialization>(this IServiceCollection services, 
        params object[] parameters)
        where TInitialization : class,
        IInitialization
    {
        services.AddTransient<IInitialization>(provider => provider.GetRequiredService<IServiceFactory>().Create<TInitialization>(parameters));
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
        return AddTemplate<TViewModel, TViewModel, TView>(services, key, serviceLifetime, parameters);
    }

    public static IServiceCollection AddTemplate<TViewModel, TViewModelImplementation, TView>(this IServiceCollection services,
        object? key = null,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient,
        params object[]? parameters)
    {
        Type viewModelType = typeof(TViewModel);
        Type viewModelImplementationType = typeof(TViewModelImplementation);
        Type viewType = typeof(TView);

        key ??= viewModelImplementationType.Name.Replace("ViewModel", "");

        if (parameters is { Length: 0 })
        {
            services.Add(new ServiceDescriptor(viewModelType, viewModelImplementationType, serviceLifetime));
        }
        else
        {
            services.Add(new ServiceDescriptor(viewModelType, provider =>
                provider.GetRequiredService<IServiceFactory>().Create<TViewModelImplementation>(parameters)!, serviceLifetime));
        }

        services.Add(new ServiceDescriptor(viewType, viewType, serviceLifetime));

        if (parameters is { Length: 0 })
        {
            services.Add(new ServiceDescriptor(viewModelType, key, viewModelImplementationType, serviceLifetime));
        }
        else
        {
            services.Add(new ServiceDescriptor(viewModelType, key, (provider, key) =>
                provider.GetRequiredService<IServiceFactory>().Create<TViewModelImplementation>(parameters)!, serviceLifetime));
        }

        services.Add(new ServiceDescriptor(viewType, key, viewType, serviceLifetime));

        services.AddKeyedTransient<IContentTemplateDescriptor>(key, (provider, _) =>
            new ContentTemplateDescriptor(key, viewModelImplementationType, viewType, parameters));

        return services;
    }

    public static IServiceCollection AddValueTemplate<TConfiguration, TValue, TViewModel, TView>(this IServiceCollection services,
        Func<TConfiguration, TValue> readDelegate,
        Action<TValue?, TConfiguration> writeDelegate,
        object? key = null,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient,
        params object[]? parameters)
    {
        parameters = [readDelegate, writeDelegate, .. parameters ?? Enumerable.Empty<object?>()];
        return AddTemplate<TViewModel, TViewModel, TView>(services, key, serviceLifetime, parameters);
    }

    public static IServiceCollection AddValueTemplate<TConfiguration, TValue, TViewModel, TViewModelImplementation, TView>(this IServiceCollection services,
        Func<TConfiguration, TValue> readDelegate,
        Action<TValue?, TConfiguration> writeDelegate,
        object? key = null,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient,
        params object[]? parameters)
    {
        parameters = [readDelegate, writeDelegate, .. parameters ?? Enumerable.Empty<object?>()];
        return AddTemplate<TViewModel, TViewModelImplementation, TView>(services, key, serviceLifetime, parameters);
    }
}