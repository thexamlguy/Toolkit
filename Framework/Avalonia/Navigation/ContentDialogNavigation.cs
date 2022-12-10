using FluentAvalonia.UI.Controls;

namespace Toolkit.Foundation.Avalonia;

public record ContentDialogNavigation : Navigation<ContentDialog>
{
    public ContentDialogNavigation(ContentDialog route,
        object? content,
        object? template,
        IDictionary<string, object>? parameters) : base(route, content, template, parameters)
    {
    }
}