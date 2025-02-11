using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public class NavigateHandler(NamedComponent named,
    IComponentScopeProvider componentScopeProvider) :
    IHandler<NavigateEventArgs>
{
    public void Handle(NavigateEventArgs args)
    {
        INavigation? navigation = null;
        if (args.Scope is "self" || args.Scope is "new")
        {
            if (args.Sender is IServiceProviderRequired requireServiceProvider)
            {
                if (args.Scope is "self")
                {
                    navigation = requireServiceProvider.Provider.GetRequiredService<INavigation>();
                }

                if (args.Scope is "new")
                {
                    IServiceScope serviceScope = requireServiceProvider.Provider.CreateScope();
                    navigation = serviceScope.ServiceProvider.GetRequiredService<INavigation>();
                }
            }
        }

        if (navigation is null)
        {
            ComponentScopeDescriptor? descriptor = componentScopeProvider.Get(args.Scope ?? named.Key);
            navigation = descriptor?.Services?.GetRequiredService<INavigation>();
        }

        navigation?.Navigate(args.Route, args.Sender,
                args.Region, args.Navigated, args.Parameters);
    }
}