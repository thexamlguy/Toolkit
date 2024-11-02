using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace Toolkit.UI.Controls.Avalonia;

public class OverflowList :
    ListBox
{
    public static readonly StyledProperty<IItemContainerTemplateSelector?> ItemContainerTemplateSelectorProperty =
        AvaloniaProperty.Register<OverflowList, IItemContainerTemplateSelector?>(nameof(ItemContainerTemplateSelector));

    public IItemContainerTemplateSelector? ItemContainerTemplateSelector
    {
        get => GetValue(ItemContainerTemplateSelectorProperty);
        set => SetValue(ItemContainerTemplateSelectorProperty, value);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        if (ItemContainerTemplateSelector?.SelectTemplate(item, this) is IDataTemplate itemContainerTemplate)
        {
            if (itemContainerTemplate.Build(item) is OverflowItem container)
            {
                return container;
            }
        }

        return new OverflowItem();
    }

    protected override bool NeedsContainerOverride(object? item,
        int index,
        out object? recycleKey)
    {
        recycleKey = null;
        return item is not OverflowItem;
    }
}