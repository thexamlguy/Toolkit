using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.LogicalTree;
using Avalonia.Metadata;

namespace Toolkit.UI.Controls.Avalonia;

public class ContentIcon : FluentAvalonia.UI.Controls.FAIconElement
{
    public static readonly StyledProperty<object?> ContentProperty =
        AvaloniaProperty.Register<ContentIcon, object?>("Content");

    public static readonly StyledProperty<IDataTemplate?> ContentTemplateProperty =
        AvaloniaProperty.Register<ContentIcon, IDataTemplate?>("ContentTemplate");

    private ContentControl? content;

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

    protected override Size MeasureOverride(Size availableSize)
    {
        if (content == null)
        {
            CreateContent();
        }

        return base.MeasureOverride(availableSize);

    }

    protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs args)
    {
        if (VisualChildren.Count > 0)
        {
            ((ILogical)VisualChildren[0]).NotifyAttachedToLogicalTree(args);
        }

        base.OnAttachedToLogicalTree(args);
    }

    protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs args)
    {
        if (VisualChildren.Count > 0)
        {
            ((ILogical)VisualChildren[0]).NotifyDetachedFromLogicalTree(args);
        }

        base.OnDetachedFromLogicalTree(args);
    }
    private void CreateContent()
    {
        content = new ContentControl();

        content.Bind(ContentControl.ContentProperty, this.GetBindingObservable(ContentProperty));
        content.Bind(ContentControl.ContentTemplateProperty, this.GetBindingObservable(ContentTemplateProperty));

        LogicalChildren.Add(content);
        VisualChildren.Add(content);
    }
}
