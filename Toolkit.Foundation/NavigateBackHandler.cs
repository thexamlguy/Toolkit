using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public class NavigateBackHandler(IComponentScopeProvider provider) :
    INotificationHandler<NavigateBackEventArgs>
{
    public Task Handle(NavigateBackEventArgs args)
    {
        if (provider.Get(args.Scope ?? "Root")
            is ComponentScopeDescriptor descriptor)
        {
            if (descriptor?.Services?.GetService<INavigation>() is
                INavigation navigationScope)
            {
                navigationScope.Back(args.Context);
            }
        }

        return Task.CompletedTask;
    }
}