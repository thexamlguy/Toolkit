using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public class NavigateHandler(Scoped scoped,
    IScopedServiceProvider<Scoped> scopedServiceProvider) :
    IHandler<NavigateEventArgs>
{
    public void Handle(NavigateEventArgs args)
    {
        object? region = args.Region;
        string route = args.Route;
        IDictionary<string, object>? parameters = null;
        object? sender = args.Sender;

        string[] segments = route.Split('/');

        object? resolvedRegion = region is "self" ? "self" : null;
        IServiceProvider? serviceProvider = null;
        INavigationRegionProvider? navigationRegionProvider = null;
        IServiceFactory? serviceFactory = null;
        IMessenger? messenger = null;

        if (resolvedRegion is null && region is not null)
        {
            if (args.Sender is IServiceProviderRequired requireServiceProvider)
            {
                if (args.Scope is NavigateScope.Scoped)
                {
                    serviceProvider = requireServiceProvider.Provider;
                    serviceFactory = requireServiceProvider.Provider.GetRequiredService<IServiceFactory>();
                    navigationRegionProvider = requireServiceProvider.Provider.GetRequiredService<INavigationRegionProvider>();
                    messenger = requireServiceProvider.Provider.GetRequiredService<IMessenger>();
                }

                if (args.Scope is NavigateScope.CreateScope)
                {
                    IServiceScope serviceScope = requireServiceProvider.Provider.CreateScope();

                    serviceProvider = serviceScope.ServiceProvider;
                    serviceFactory = serviceScope.ServiceProvider.GetRequiredService<IServiceFactory>();
                    navigationRegionProvider = serviceScope.ServiceProvider.GetRequiredService<INavigationRegionProvider>();
                    messenger = requireServiceProvider.Provider.GetRequiredService<IMessenger>();
                }

                if (navigationRegionProvider is not null)
                {
                    if (navigationRegionProvider.TryGet(region, out object? value))
                    {
                        resolvedRegion = value;
                    }
                }
            }

            if (scopedServiceProvider.TryGet(scoped, out IServiceProvider? scopedProvider))
            {
                serviceProvider ??= scopedProvider;
                serviceFactory ??= scopedProvider?.GetRequiredService<IServiceFactory>();
                messenger ??= scopedProvider?.GetRequiredService<IMessenger>();

                if (resolvedRegion is null)
                {

                    navigationRegionProvider = scopedProvider?.GetRequiredService<INavigationRegionProvider>();
                    if (navigationRegionProvider is not null)
                    {
                        if (navigationRegionProvider.TryGet(region, out object? value))
                        {
                            resolvedRegion = value;
                        }
                    }
                }
            }
        }

        if (resolvedRegion is not null && serviceProvider is not null && serviceFactory is not null && messenger is not null)
        {
            Dictionary<string, object>? arguments = parameters?.ToDictionary(x => x.Key, x => x.Value, StringComparer.InvariantCultureIgnoreCase) ?? [];

            foreach (object segment in segments)
            {
                if (serviceProvider.GetRequiredKeyedService<IContentTemplateDescriptor>(segment) is IContentTemplateDescriptor descriptor)
                {
                    if (serviceProvider.GetRequiredKeyedService(descriptor.TemplateType, descriptor.Key) is object template)
                    {
                        object?[] resolvedArguments = parameters is not null
                        ? descriptor.ContentType
                            .GetConstructors()
                            .FirstOrDefault()?
                            .GetParameters()
                            .Select(x => x?.Name is not null && arguments is not null && arguments.TryGetValue(x.Name, out object? argument)
                                    ? argument
                                    : null)
                            .Where(argument => argument is not null)
                            .ToArray() ?? [] : [];

                        object? content = resolvedArguments is { Length: > 0 }
                            ? serviceFactory.Create(descriptor.ContentType, parameters)
                            : serviceProvider.GetRequiredKeyedService(descriptor.ContentType, descriptor.Key);

                        messenger.Send(new NavigateTemplateEventArgs(resolvedRegion, template, content, sender, parameters), 
                            resolvedRegion is string ? $"{resolvedRegion}" : resolvedRegion.GetType().Name);
                    }
                }
            }
        }
    }
}