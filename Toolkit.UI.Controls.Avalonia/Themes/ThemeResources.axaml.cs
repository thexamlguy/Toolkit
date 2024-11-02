using Avalonia.Markup.Xaml;
using FluentAvalonia.Styling;

namespace Toolkit.UI.Controls.Avalonia;

public class ThemeResources : FluentAvaloniaTheme
{
    public ThemeResources(IServiceProvider? provider = null)
    {
        AvaloniaXamlLoader.Load(provider, this);
    }
}