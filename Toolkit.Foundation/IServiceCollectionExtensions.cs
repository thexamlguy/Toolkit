using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddAsyncHandler<TMessage, TResponse, THandler>(this IServiceCollection services,
        string key)
        where THandler : class, IAsyncHandler<TMessage, TResponse>
        where TMessage : class => AddAsyncHandler<TMessage, TResponse, THandler>(services, ServiceLifetime.Transient, key);

    public static IServiceCollection AddAsyncHandler<TMessage, TResponse, THandler>(this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Transient,
        string? key = null)
        where THandler : class, IAsyncHandler<TMessage, TResponse>
        where TMessage : class
    {
        if (key is { Length: > 0 })
        {
            services.Add(new ServiceDescriptor(typeof(IAsyncHandler<TMessage, TResponse>), key, typeof(THandler), lifetime));
            services.AddInitialization<AsyncHandlerKeyedInitialization<TMessage, TResponse, IAsyncHandler<TMessage, TResponse>>>(key);
        }
        else
        {
            services.Add(new ServiceDescriptor(typeof(IAsyncHandler<TMessage, TResponse>), typeof(THandler), lifetime));
            services.AddInitialization<AsyncHandlerInitialization<TMessage, TResponse, IAsyncHandler<TMessage, TResponse>>>();
        }

        return services;
    }

    public static IServiceCollection AddAsyncHandler<TMessage, THandler>(this IServiceCollection services,
        string key)
        where THandler : class, IAsyncHandler<TMessage>
        where TMessage : class => AddAsyncHandler<TMessage, THandler>(services, ServiceLifetime.Transient, key);

    public static IServiceCollection AddAsyncHandler<TMessage, THandler>(this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Transient,
        string? key = null)
        where THandler : class, IAsyncHandler<TMessage>
        where TMessage : class
    {
        if (key is { Length: > 0 })
        {
            services.Add(new ServiceDescriptor(typeof(IAsyncHandler<TMessage>), key, typeof(THandler), lifetime));
            services.AddInitialization<AsyncHandlerKeyedInitialization<TMessage, IAsyncHandler<TMessage>>>(key);
        }
        else
        {
            services.Add(new ServiceDescriptor(typeof(IAsyncHandler<TMessage>), typeof(THandler), lifetime));
            services.AddInitialization<AsyncHandlerInitialization<TMessage, IAsyncHandler<TMessage>>>();
        }

        return services;
    }

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
        string key)
        where THandler : class, IHandler<TMessage, TResponse>
        where TMessage : class => AddHandler<TMessage, TResponse, THandler>(services, ServiceLifetime.Transient, key);

    public static IServiceCollection AddHandler<TMessage, TResponse, THandler>(this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Transient,
        string? key = null)
        where THandler : class, IHandler<TMessage, TResponse>
        where TMessage : class
    {
        if (key is { Length: > 0 })
        {
            services.Add(new ServiceDescriptor(typeof(IHandler<TMessage, TResponse>), key, typeof(THandler), lifetime));
            services.AddInitialization<HandlerKeyedInitialization<TMessage, TResponse, IHandler<TMessage, TResponse>>>(key);
        }
        else
        {
            services.Add(new ServiceDescriptor(typeof(IHandler<TMessage, TResponse>), typeof(THandler), lifetime));
            services.AddInitialization<HandlerInitialization<TMessage, TResponse, IHandler<TMessage, TResponse>>>();
        }

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
        if (key is { Length: > 0 })
        {
            services.Add(new ServiceDescriptor(typeof(IHandler<TMessage>), key, typeof(THandler), lifetime));
            services.AddInitialization<HandlerKeyedInitialization<TMessage, IHandler<TMessage>>>(key);
        }
        else
        {
            services.Add(new ServiceDescriptor(typeof(IHandler<TMessage>), typeof(THandler), lifetime));
            services.AddInitialization<HandlerInitialization<TMessage, IHandler<TMessage>>>();
        }

        return services;
    }

    public static IServiceCollection AddInitialization<TInitialization, TInitializationImplementation>(this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Transient)
        where TInitialization : class,
        IInitialization
        where TInitializationImplementation : class,
        TInitialization
    {
        services.Add(new ServiceDescriptor(typeof(TInitialization), typeof(TInitializationImplementation), lifetime));
        services.AddTransient<IInitialization>(provider => provider.GetRequiredService<TInitialization>());
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

    public static IServiceCollection AddAsyncPipelineBehavior<TMessage, TResponse, TBehavior>(this IServiceCollection services)
        where TBehavior : class,
        IAsyncPipelineBehavior<TMessage, TResponse>
    {
        services.AddTransient<IAsyncPipelineBehavior<TMessage, TResponse>, TBehavior>();
        return services;
    }

    public static IServiceCollection AddPipelineBehavior<TMessage, TResponse, TBehavior>(this IServiceCollection services)
        where TBehavior : class,
        IPipelineBehavior<TMessage, TResponse>
    {
        services.AddTransient<IPipelineBehavior<TMessage, TResponse>, TBehavior>();
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
        params object?[]? parameters)
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
        params object?[]? parameters)
    {
        parameters = [readDelegate, writeDelegate, .. parameters ?? Enumerable.Empty<object?>()];
        return AddTemplate<TViewModel, TViewModel, TView>(services, key, serviceLifetime, parameters);
    }

    public static IServiceCollection AddValueTemplate<TConfiguration, TValue, TViewModel, TViewModelImplementation, TView>(this IServiceCollection services,
        Func<TConfiguration, TValue> readDelegate,
        Action<TValue?, TConfiguration> writeDelegate,
        object? key = null,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient,
        params object?[]? parameters)
    {
        parameters = [readDelegate, writeDelegate, .. parameters ?? Enumerable.Empty<object?>()];
        return AddTemplate<TViewModel, TViewModelImplementation, TView>(services, key, serviceLifetime, parameters);
    }
}