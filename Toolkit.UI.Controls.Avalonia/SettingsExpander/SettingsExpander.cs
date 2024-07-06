﻿using Avalonia;
using Avalonia.Controls.Templates;

namespace Toolkit.UI.Controls.Avalonia;

public class SettingsExpander :
    FluentAvalonia.UI.Controls.SettingsExpander
{
    public static readonly StyledProperty<object> ActionProperty =
        AvaloniaProperty.Register<SettingsExpander, object>(nameof(Action));

    public static readonly StyledProperty<IDataTemplate> ActionTemplateProperty =
        AvaloniaProperty.Register<SettingsExpander, IDataTemplate>(nameof(ActionTemplate));

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

    protected override Type StyleKeyOverride =>
        typeof(SettingsExpander);
}
