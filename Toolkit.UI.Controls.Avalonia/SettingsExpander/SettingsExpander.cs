using Avalonia;

namespace Toolkit.UI.Controls.Avalonia;

public class SettingsExpander : FluentAvalonia.UI.Controls.SettingsExpander
{
    protected override Type StyleKeyOverride =>
        typeof(FluentAvalonia.UI.Controls.SettingsExpander);
}