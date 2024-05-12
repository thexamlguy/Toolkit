using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public class NavigateHandler(NamedComponent scope,
    IComponentScopeProvider componentScopeProvider,
    IServiceProvider provider) :
    INotificationHandler<Navigate>
{
    public async Task Handle(Navigate args,
      CancellationToken cancellationToken)
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

        if (navigationScope is not null)
        {
            await navigationScope.NavigateAsync(args.Route, args.Sender,
                args.Context, args.Navigated, args.Parameters, cancellationToken);
        }
    }
}