using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public class NavigateBackHandler(IComponentScopeProvider provider) :
    INotificationHandler<NavigateBack>
{
    public async Task Handle(NavigateBack args,
        CancellationToken cancellationToken)
    {
        if (provider.Get(args.Scope ?? "Default")
            is IServiceProvider scope)
        {
            if (scope.GetService<INavigationScope>() is INavigationScope navigationScope)
            {
                await navigationScope.NavigateBackAsync(args.Context, cancellationToken);
            }
        }
    }
}


