using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace Toolkit.UI.Controls.Avalonia;

public class SettingsExpanderItem :
    FluentAvalonia.UI.Controls.SettingsExpanderItem
{
    public static readonly StyledProperty<object> ActionProperty =
        AvaloniaProperty.Register<SettingsExpanderItem, object>(nameof(Action));

    public static readonly StyledProperty<IDataTemplate> ActionTemplateProperty =
        AvaloniaProperty.Register<SettingsExpanderItem, IDataTemplate>(nameof(ActionTemplate));

    public static readonly StyledProperty<object> IconProperty =
        AvaloniaProperty.Register<SettingsExpanderItem, object>(nameof(Icon));

    public static readonly StyledProperty<IDataTemplate> IconTemplateProperty =
        AvaloniaProperty.Register<SettingsExpanderItem, IDataTemplate>(nameof(IconTemplate));

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == IconProperty || change.Property == IconTemplateProperty)
        {
            UpdateIcon();
        }
    }

    private void UpdateIcon()
    {
        PseudoClasses.Set(":icon", Icon is not null || IconTemplate is not null);
    }

    public object Action
    {
        get => GetValue(ActionProperty);
        set => SetValue(ActionProperty, value);
    }

    public IDataTemplate ActionTemplate
    {
        get => GetValue(ActionTemplateProperty);
        set => SetValue(ActionTemplateProperty, value);
    }

    public object Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public IDataTemplate IconTemplate
    {
        get => GetValue(IconTemplateProperty);
        set => SetValue(IconTemplateProperty, value);
    }

    protected override Type StyleKeyOverride =>
        typeof(SettingsExpanderItem);
}