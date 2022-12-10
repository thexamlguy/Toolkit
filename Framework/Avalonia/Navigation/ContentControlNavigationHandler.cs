using Avalonia.Controls.Primitives;
using Mediator;

namespace Toolkit.Foundation.Avalonia;

public class ContentControlNavigationHandler : IRequestHandler<ContentControlNavigation, bool>
{
    public async ValueTask<bool> Handle(ContentControlNavigation request, CancellationToken cancellationToken)
    {
        if (request.Template is TemplatedControl control)
        {
            control.DataContext = request.Content;
            request.Route.Content = control;
        }

        return await Task.FromResult(true);
    }
}