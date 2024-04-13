using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Toolkit.UI.Controls.Avalonia;

public class ThemeResources :
    Styles
{
    public ThemeResources(IServiceProvider? serviceProvider = null)
    {
        AvaloniaXamlLoader.Load(serviceProvider, this);
    }
}
