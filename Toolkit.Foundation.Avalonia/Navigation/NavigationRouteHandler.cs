using Avalonia.Controls.Primitives;
using CommunityToolkit.Mvvm.Messaging;

namespace Toolkit.Foundation.Avalonia
{
    public abstract class NavigationRouteHandler<TTarget> : IRecipient<NavigationRouteRequest<TTarget>> where TTarget : TemplatedControl
    {
        public abstract void Receive(NavigationRouteRequest<TTarget> message);
    }
}
