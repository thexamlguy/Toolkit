using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.LogicalTree;

namespace Toolkit.Controls.Avalonia;

public class ContentIcon : FluentAvalonia.UI.Controls.FAIconElement
{
    public static readonly StyledProperty<IDataTemplate> IconTemplateProperty =
        AvaloniaProperty.Register<ContentIcon, IDataTemplate>("IconTemplate");

    public static readonly StyledProperty<object> IconProperty = 
        AvaloniaProperty.Register<ContentIcon, object>("Icon");

    public object Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
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

        content.Bind(ContentControl.ContentProperty, this.GetBindingObservable(IconProperty));
        content.Bind(ContentControl.ContentTemplateProperty, this.GetBindingObservable(IconTemplateProperty));

        LogicalChildren.Add(content);
        VisualChildren.Add(content);
    }
}