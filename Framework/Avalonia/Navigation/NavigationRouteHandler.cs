using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Toolkit.Framework.Foundation;

namespace Toolkit.Framework.Avalonia;

public class NavigationRouteHandler : IRequestHandler<NavigationRoute>
{
    private readonly INavigationRouteDescriptorCollection descriptors;

    public NavigationRouteHandler(INavigationRouteDescriptorCollection descriptors)
    {
        this.descriptors = descriptors;
    }

    public ValueTask<Unit> Handle(NavigationRoute request, CancellationToken cancellationToken)
    {
        if (request.Route is TemplatedControl control)
        {
            void HandleUnloaded(object? sender, RoutedEventArgs args)
            {
                if (descriptors.FirstOrDefault(x => x.Route == sender) is INavigationRouteDescriptor descriptor)
                {
                    descriptors.Remove(descriptor);
                }
            }
            control.Unloaded += HandleUnloaded;
        }

        if (descriptors.FirstOrDefault(x => x.Name == request.Name) is INavigationRouteDescriptor descriptor)
        {
            descriptors.Remove(descriptor);
        }

        descriptors.Add(new NavigationRouteDescriptor(request.Name, request.Route));
        return default;
    }
}