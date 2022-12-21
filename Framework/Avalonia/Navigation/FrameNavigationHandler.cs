using Avalonia.Controls.Primitives;
using FluentAvalonia.UI.Navigation;
using Toolkit.Framework.Foundation;

namespace Toolkit.Framework.Avalonia;

public class FrameNavigationHandler : IRequestHandler<FrameNavigation>
{
    public ValueTask<Unit> Handle(FrameNavigation request, CancellationToken cancellationToken)
    {
        request.Route.NavigationPageFactory = new NavigationPageFactory();

        TaskCompletionSource<bool> completionSource = new();
        if (request.Template is TemplatedControl content)
        {
            void HandleNavigated(object sender, NavigationEventArgs args)
            {
                request.Route.Navigated -= HandleNavigated;
                if (request.Route.Content is TemplatedControl control)
                {
                    control.DataContext = request.Content;
                    completionSource.SetResult(true);
                }
            }

            request.Route.Navigated += HandleNavigated;
            request.Route.NavigateFromObject(content);
        }

        return default;
    }
}