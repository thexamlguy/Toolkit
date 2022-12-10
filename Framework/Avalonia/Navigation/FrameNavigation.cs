using FluentAvalonia.UI.Controls;

namespace Toolkit.Framework.Avalonia;

public record FrameNavigation : Navigation<Frame>
{
    public FrameNavigation(Frame route,
        object? content,
        object? template,
        IDictionary<string, object>?
        parameters) : base(route, content, template, parameters)
    {
    }
}