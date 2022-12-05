using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;

namespace Toolkit.Foundation.Avalonia
{
    public class NavigationRoute : INavigationRoute
    {
        private readonly INavigationRouteDescriptorCollection routes;

        public NavigationRoute(INavigationRouteDescriptorCollection routes)
        {
            this.routes = routes;
        }

        public void Add(string name, object route)
        {
            if (route is TemplatedControl control)
            {
                void HandleUnloaded(object? sender, RoutedEventArgs args)
                {
                    if (routes.FirstOrDefault(x => x.Route == sender) is INavigationRouteDescriptor descriptor)
                    {
                        routes.Remove(descriptor);
                    }
                }

                control.Unloaded += HandleUnloaded;
            }

            if (routes.FirstOrDefault(x => x.Name == name) is INavigationRouteDescriptor descriptor)
            {
                routes.Remove(descriptor);
            }

            routes.Add(new NavigationRouteDescriptor(name, route));
        }
    }
}
