namespace Toolkit.Foundation
{
    public record NavigationRouteDescriptor : INavigationRouteDescriptor
    {
        public NavigationRouteDescriptor(string name, object route)
        {
            Name = name;
            Route = route;
        }

        public string Name { get; }

        public object Route { get; }
    }
}