namespace Toolkit.Foundation
{
    public class NavigationRouteDescriptorCollection : List<INavigationRouteDescriptor>, INavigationRouteDescriptorCollection
    {
        public NavigationRouteDescriptorCollection(IEnumerable<INavigationRouteDescriptor> collection) : base(collection)
        {
        }
    }
}