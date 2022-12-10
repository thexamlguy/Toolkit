using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using FluentAvalonia.UI.Controls;
using Mediator;
using Toolkit.Framework.Foundation;

namespace Toolkit.Framework.Avalonia;

public class NavigateBackHandler : IRequestHandler<NavigateBack>
{
    private readonly INavigationRouteDescriptorCollection descriptors;

    public NavigateBackHandler(INavigationRouteDescriptorCollection descriptors)
    {
        this.descriptors = descriptors;
    }

    public ValueTask<Unit> Handle(NavigateBack request, CancellationToken cancellationToken)
    {
        if (descriptors.FirstOrDefault(x => request.Route is string { } name && name == x.Name) is NavigationRouteDescriptor descriptor)
        {
            if (descriptor.Route is ContentControl { Content: TemplatedControl content })
            {
                if (content.DataContext is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }

            if (descriptor.Route is Frame frame)
            {
                frame.GoBack();
            }
        }

        return default;
    }
}