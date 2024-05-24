using Avalonia;
using Avalonia.Controls.Templates;
using Avalonia.Metadata;

namespace Toolkit.UI.Controls.Avalonia;

public class ContentIconSource : FluentAvalonia.UI.Controls.IconSource
{
    public static readonly StyledProperty<object?> ContentProperty =
        AvaloniaProperty.Register<ContentIconSource, object?>("Content");

    public static readonly StyledProperty<IDataTemplate?> ContentTemplateProperty =
        AvaloniaProperty.Register<ContentIconSource, IDataTemplate?>("ContentTemplate");

    [Content]
    public object? Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public IDataTemplate? IconTemplate
    {
        get => GetValue(ContentTemplateProperty);
        set => SetValue(ContentTemplateProperty, value);
    }
}