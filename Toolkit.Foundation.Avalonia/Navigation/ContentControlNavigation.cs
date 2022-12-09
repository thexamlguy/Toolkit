using Avalonia.Controls;

namespace Toolkit.Foundation.Avalonia
{
    public record ContentControlNavigation : Navigation<ContentControl>
    {
        public ContentControlNavigation(ContentControl route,
            object? content,
            object? template,
            IDictionary<string, object>? parameters) : base(route, content, template, parameters)
        {
        }
    }
}
