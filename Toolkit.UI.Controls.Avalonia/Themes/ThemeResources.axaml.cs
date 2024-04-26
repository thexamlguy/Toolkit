using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Toolkit.UI.Controls.Avalonia;

public class ThemeResources :
    Styles
{
    public ThemeResources(IServiceProvider? provider = null)
    {
        AvaloniaXamlLoader.Load(provider, this);
    }
}