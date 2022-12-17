using Avalonia.Styling;

namespace Toolkit.Controls.Avalonia;

public class SettingsExpander : FluentAvalonia.UI.Controls.SettingsExpander, IStyleable
{
    Type IStyleable.StyleKey => typeof(FluentAvalonia.UI.Controls.SettingsExpander);
}