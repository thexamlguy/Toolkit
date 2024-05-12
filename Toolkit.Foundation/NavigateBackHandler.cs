using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public class NavigateBackHandler(IComponentScopeProvider provider) :
    INotificationHandler<NavigateBackEventArgs>
{
    public async Task Handle(NavigateBackEventArgs args,
        CancellationToken cancellationToken)
    {
        if (provider.Get(args.Scope ?? "Root")
            is ComponentScopeDescriptor descriptor)
        {
            if (descriptor?.Services?.GetService<INavigationScope>() is INavigationScope navigationScope)
            {
                await navigationScope.NavigateBackAsync(args.Context, cancellationToken);
            }
        }
    }
}