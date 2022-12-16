using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.LogicalTree;
using Avalonia.Metadata;

namespace Toolkit.Controls.Avalonia;

public class ContentIcon : FluentAvalonia.UI.Controls.FAIconElement
{
    public static readonly StyledProperty<IDataTemplate> IconTemplateProperty =
        AvaloniaProperty.Register<ContentIcon, IDataTemplate>("IconTemplate");

    public static readonly StyledProperty<object> ContentProperty = 
        AvaloniaProperty.Register<ContentIcon, object>("Content");

    [Content]
    public object Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    private ContentControl? content;

    protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        if (VisualChildren.Count > 0)
        {
            ((ILogical)VisualChildren[0]).NotifyAttachedToLogicalTree(e);
        }

        base.OnAttachedToLogicalTree(e);
    }

    /// <inheritdoc/>
    protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        if (VisualChildren.Count > 0)
        {
            ((ILogical)VisualChildren[0]).NotifyDetachedFromLogicalTree(e);
        }

        base.OnDetachedFromLogicalTree(e);
    }


    public IDataTemplate IconTemplate
    {
        get => GetValue(IconTemplateProperty);
        set => SetValue(IconTemplateProperty, value);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        if (content == null)
        {
            CreateContent();
        }

        return base.MeasureOverride(availableSize);

    }

    private void CreateContent()
    {
        content = new ContentControl();

        content.Bind(ContentControl.ContentProperty, this.GetBindingObservable(ContentProperty));
        content.Bind(ContentControl.ContentTemplateProperty, this.GetBindingObservable(IconTemplateProperty));

        LogicalChildren.Add(content);
        VisualChildren.Add(content);
    }
}