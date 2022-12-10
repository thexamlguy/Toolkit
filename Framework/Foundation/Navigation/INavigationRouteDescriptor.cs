namespace Toolkit.Framework.Foundation;

public interface INavigationRouteDescriptor
{
    object Route { get; }

    string? Name { get; }
}