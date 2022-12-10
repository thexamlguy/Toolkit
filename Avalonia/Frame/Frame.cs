using Avalonia.Styling;

namespace Toolkit.Controls.Avalonia;

public class Frame : FluentAvalonia.UI.Controls.Frame, IStyleable
{
    Type IStyleable.StyleKey => typeof(FluentAvalonia.UI.Controls.Frame);
}