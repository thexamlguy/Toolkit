using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public class NavigateHandler(ComponentScope scope,
    IComponentScopeProvider provider) :
    INotificationHandler<Navigate>
{
    public async Task Handle(Navigate args, 
        CancellationToken cancellationToken)
    {
        if (provider.Get(args.Scope ?? scope.Name) 
            is ComponentScopeDescriptor descriptor)
        {
            if (descriptor?.Services?.GetService<INavigationScope>() is INavigationScope navigationScope)
            {
                await navigationScope.NavigateAsync(args.Route, args.Sender,
                    args.Context, args.Navigated, args.Parameters, cancellationToken);
            }
        }
    }
}
