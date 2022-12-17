using Avalonia.Controls.Primitives;
using Toolkit.Framework.Foundation;

namespace Toolkit.Framework.Avalonia;

public class ContentControlNavigationHandler : IRequestHandler<ContentControlNavigation, bool>
{
    public ValueTask<bool> Handle(ContentControlNavigation request, CancellationToken cancellationToken)
    {
        if (request.Template is TemplatedControl control)
        {
            control.DataContext = request.Content;
            request.Route.Content = control;
        }

        return new ValueTask<bool>(true);
    }
}