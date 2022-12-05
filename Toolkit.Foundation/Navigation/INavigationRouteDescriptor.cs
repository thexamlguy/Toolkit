namespace Toolkit.Foundation
{
    public interface INavigationRouteDescriptor
    {
        object Route { get; }

        string? Name { get; }
    }
}