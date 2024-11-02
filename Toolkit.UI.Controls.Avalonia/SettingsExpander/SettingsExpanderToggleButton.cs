using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Input;

namespace Toolkit.UI.Controls.Avalonia;

public class SettingsExpanderToggleButton :
    ToggleButton
{
    public static readonly StyledProperty<bool> IsToggleableProperty =
        AvaloniaProperty.Register<SettingsExpanderToggleButton, bool>(nameof(IsToggleable));

    public object IsToggleable
    {
        get => GetValue(IsToggleableProperty);
        set => SetValue(IsToggleableProperty, value);
    }

    protected override void OnKeyDown(KeyEventArgs args)
    {
        if (args.Key is not Key.Space)
        {
            base.OnKeyDown(args);
        }
    }
}