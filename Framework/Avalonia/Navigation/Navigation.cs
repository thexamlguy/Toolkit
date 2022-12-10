using Avalonia.Controls.Primitives;
using Mediator;

namespace Toolkit.Framework.Avalonia;

public record Navigation<TRoute> : IRequest<bool> where TRoute : TemplatedControl
{
    public TRoute Route { get; }

    public Navigation(TRoute route,
        object? content,
        object? template,
        IDictionary<string, object>? parameters)
    {
        Route = route;
        Content = content;
        Template = template;
        Parameters = parameters;
    }

    public object? Content { get; }

    public object? Template { get; }

    public IDictionary<string, object>? Parameters { get; }
}