using Avalonia;
using Avalonia.Controls.Templates;

namespace Toolkit.UI.Controls.Avalonia;

public class SettingsExpander :
    FluentAvalonia.UI.Controls.SettingsExpander
{
    public static readonly StyledProperty<object> ActionProperty =
        AvaloniaProperty.Register<SettingsExpander, object>(nameof(Action));

    public static readonly StyledProperty<IDataTemplate> ActionTemplateProperty =
        AvaloniaProperty.Register<SettingsExpander, IDataTemplate>(nameof(ActionTemplate));

    public static readonly StyledProperty<object> IconProperty =
        AvaloniaProperty.Register<SettingsExpander, object>(nameof(Icon));

    public static readonly StyledProperty<IDataTemplate> IconTemplateProperty =
        AvaloniaProperty.Register<SettingsExpander, IDataTemplate>(nameof(IconTemplate));

    public static readonly StyledProperty<bool> IsExpandableProperty =
        AvaloniaProperty.Register<SettingsExpander, bool>(nameof(IsExpandable));

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

    public bool IsExpandable
    {
        get => GetValue(IsExpandableProperty);
        set => SetValue(IsExpandableProperty, value);
    }

    protected override Type StyleKeyOverride =>
        typeof(SettingsExpander);
}