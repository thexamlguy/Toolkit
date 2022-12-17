using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace Toolkit.Framework.Foundation;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddConfiguration<TConfiguration>(this IServiceCollection serviceCollection, IConfiguration configuration) where TConfiguration : class, new()
    {
        serviceCollection.Configure<TConfiguration>(configuration);
        serviceCollection.AddTransient(provider => provider.GetService<IOptionsMonitor<TConfiguration>>()!.CurrentValue);
        serviceCollection.AddTransient<ConfigurationInitializer<TConfiguration>>();

        return serviceCollection;
    }

    public static IServiceCollection AddFoundation(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddSingleton<IMediator, Mediator>()
            .AddHandler<InitializeHandler>()
            .AddSingleton<IServiceFactory>(provider => new ServiceFactory(provider.GetService, (instanceType, parameters) => ActivatorUtilities.CreateInstance(provider, instanceType, parameters!)))
            .AddHandler<ServiceFactoryHandler>()
            .AddSingleton<IInitialization, Initialization>(provider => new Initialization(() =>
            {
                return serviceCollection.Where(x => x.ServiceType.GetInterfaces()
                    .Contains(typeof(IInitializable)) || x.ServiceType == typeof(IInitializable))
                    .GroupBy(x => x.ServiceType)
                    .Select(x => x.First())
                    .SelectMany(x => provider.GetServices(x.ServiceType)
                    .Select(x => (IInitializable?)x)).ToList();
            }));

        return serviceCollection;
    }

    public static IServiceCollection AddHandler<THandler>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Transient) where THandler : notnull
    {
        if (typeof(THandler).GetInterface(typeof(INotificationHandler<>).Name) is { } notificationContract)
        {
            if (notificationContract.GetGenericArguments() is { Length: 1 } arguments)
            {
                Type notificationType = arguments[0];

                services.TryAdd(new ServiceDescriptor(typeof(THandler), typeof(THandler), ServiceLifetime.Singleton));
                services.Add(new ServiceDescriptor(typeof(INotificationHandler<>).MakeGenericType(notificationType), sp => sp.GetRequiredService<THandler>(), ServiceLifetime.Singleton));
            }
        }
  
        if (typeof(THandler).GetInterface(typeof(IRequestHandler<,>).Name) is { } requestContract)
        {
            if (requestContract.GetGenericArguments() is { Length: 2 } arguments)
            {
                Type requestType = arguments[0];
                Type responseType = arguments[1];

                Type wrapperType = typeof(RequestClassHandlerWrapper<,>).MakeGenericType(requestType, responseType);

                services.TryAdd(new ServiceDescriptor(typeof(THandler), typeof(THandler), lifetime));
                services.Add(new ServiceDescriptor(wrapperType,
                    sp =>
                    {
                        return sp.GetService<IServiceFactory>()?.Create(wrapperType, sp.GetRequiredService<THandler>(), sp.GetServices(typeof(IPipelineBehavior<,>).MakeGenericType(requestType, responseType)))!;
                    },
                    lifetime
                ));

            }
        }

        if (typeof(THandler).GetInterface(typeof(ICommandHandler<,>).Name) is { } commandContract)
        {
            if (commandContract.GetGenericArguments() is { Length: 2 } arguments)
            {
                Type requestType = arguments[0];
                Type responseType = arguments[1];

                Type wrapperType = typeof(CommandClassHandlerWrapper<,>).MakeGenericType(requestType, responseType);

                services.TryAdd(new ServiceDescriptor(typeof(THandler), typeof(THandler), lifetime));
                services.Add(new ServiceDescriptor(wrapperType,
                    sp =>
                    {
                        return sp.GetService<IServiceFactory>()?.Create(wrapperType, sp.GetRequiredService<THandler>(), sp.GetServices(typeof(IPipelineBehavior<,>).MakeGenericType(requestType, responseType)))!;
                    },
                    lifetime
                ));
            }
        }

        if (typeof(THandler).GetInterface(typeof(IQueryHandler<,>).Name) is { } queryContract)
        {
            if (queryContract.GetGenericArguments() is { Length: 2 } arguments)
            {
                Type requestType = arguments[0];
                Type responseType = arguments[1];

                Type wrapperType = typeof(QueryClassHandlerWrapper<,>).MakeGenericType(requestType, responseType);

                services.TryAdd(new ServiceDescriptor(typeof(THandler), typeof(THandler), lifetime));
                services.Add(new ServiceDescriptor(wrapperType,
                    sp =>
                    {
                        return sp.GetService<IServiceFactory>()?.Create(wrapperType, sp.GetRequiredService<THandler>(), sp.GetServices(typeof(IPipelineBehavior<,>).MakeGenericType(requestType, responseType)))!;
                    },
                    lifetime
                ));
            }
        }
        return services;
    }

    public static IServiceCollection AddWritableConfiguration<TConfiguration>(this IServiceCollection serviceCollection, IConfiguration configuration) where TConfiguration : class, new()
    {
        serviceCollection.Configure<TConfiguration>(configuration);
        serviceCollection.AddTransient<IConfigurationWriter<TConfiguration>>(provider => provider.GetService<IServiceFactory>()?.Create< ConfigurationWriter<TConfiguration>>(configuration is IConfigurationSection section ? section.Path : "")!);
        serviceCollection.AddTransient(provider => provider.GetService<IOptionsMonitor<TConfiguration>>()!.CurrentValue);
        serviceCollection.AddHandler<WriteHandler<TConfiguration>>();
        serviceCollection.AddTransient<ConfigurationInitializer<TConfiguration>>();
        
        return serviceCollection;
    }
}