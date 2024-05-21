using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public class NavigateHandler(NamedComponent scope,
    IComponentScopeProvider componentScopeProvider) :
    INotificationHandler<NavigateEventArgs>
{
    public Task Handle(NavigateEventArgs args)
    {
        INavigationScope? navigationScope = null;
        if (args.Scope is "self" || args.Scope is "new")
        {
            if (args.Sender is IServiceProviderRequired requireServiceProvider)
            {
                if (args.Scope is "self")
                {
                    navigationScope = requireServiceProvider.Provider.GetRequiredService<INavigationScope>();
                }

                if (args.Scope is "new")
                {
                    IServiceScope serviceScope = requireServiceProvider.Provider.CreateScope();
                    navigationScope = serviceScope.ServiceProvider.GetRequiredService<INavigationScope>();
                }
            }
        }
        
        if (navigationScope is null) 
        {
            ComponentScopeDescriptor? descriptor = componentScopeProvider.Get(args.Scope ?? scope.Name);
            navigationScope = descriptor?.Services?.GetRequiredService<INavigationScope>();
        }

        navigationScope?.Navigate(args.Route, args.Sender,
                args.Region, args.Navigated, args.Parameters);

        return Task.CompletedTask;
    }
}