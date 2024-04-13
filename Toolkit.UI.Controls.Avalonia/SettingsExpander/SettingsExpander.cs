using Avalonia;

namespace Toolkit.UI.Controls.Avalonia;

public class SettingsExpander : FluentAvalonia.UI.Controls.SettingsExpander
{
    protected override Type StyleKeyOverride => 
        typeof(SettingsExpander);

    public new static readonly StyledProperty<object> DescriptionProperty =
        AvaloniaProperty.Register<SettingsExpander, object>(nameof(Description));

    public new object Description
    {
        get => GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }
}

public class SettingsExpanderItem : FluentAvalonia.UI.Controls.SettingsExpanderItem
{
    protected override Type StyleKeyOverride =>
        typeof(SettingsExpanderItem);

    public new static readonly StyledProperty<object> DescriptionProperty =
        AvaloniaProperty.Register<SettingsExpanderItem, object>(nameof(Description));

    public new object Description
    {
        get => GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }
}
