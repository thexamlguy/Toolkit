using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public class NavigateBackHandler(IComponentScopeProvider provider) :
    IHandler<NavigateBackEventArgs>
{
    public void Handle(NavigateBackEventArgs args)
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
    }
}