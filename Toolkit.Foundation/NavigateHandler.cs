using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public class NavigateHandler(NamedComponent scope,
    IComponentScopeProvider componentScopeProvider,
    IServiceProvider provider) :
    INotificationHandler<NavigateEventArgs>
{
    public Task Handle(NavigateEventArgs args)
    {
        INavigationScope? navigationScope;
        if (args.Scope == "self")
        {
            navigationScope = provider.GetRequiredService<INavigationScope>();
        }
        else
        {
            ComponentScopeDescriptor? descriptor = componentScopeProvider.Get(args.Scope ?? scope.Name);
            navigationScope = descriptor?.Services?.GetRequiredService<INavigationScope>();
        }

        navigationScope?.Navigate(args.Route, args.Sender,
                args.Context, args.Navigated, args.Parameters);

        return Task.CompletedTask;
    }
}