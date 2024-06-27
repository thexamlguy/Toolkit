using Avalonia.Controls.Primitives;
using Avalonia.Input;

namespace Toolkit.UI.Controls.Avalonia;

public class SettingsExpanderToggleButton : 
    ToggleButton
{
    protected override void OnKeyDown(KeyEventArgs args)
    {
        if (args.Key is not Key.Space)
        {
            base.OnKeyDown(args);
        }
    }
}