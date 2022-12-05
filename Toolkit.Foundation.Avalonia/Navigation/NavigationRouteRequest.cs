using Avalonia.Controls.Primitives;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Toolkit.Foundation.Avalonia
{
    public class NavigationRouteRequest<TTarget> : AsyncRequestMessage<bool> where TTarget : TemplatedControl
    {
        public NavigationRouteRequest(TTarget target, object? data, object? template, IDictionary<string, object>? parameters = null)
        {
            Target = target;
            Data = data;
            Template = template;
            Parameters = parameters;
        }

        public TTarget Target { get; }

        public object? Data { get; }

        public object? Template { get; }

        public IDictionary<string, object>? Parameters { get; }
    }
}
