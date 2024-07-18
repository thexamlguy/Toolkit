using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace Toolkit.UI.Controls.Avalonia;
public class ListView :
    ListBox
{
    public static readonly StyledProperty<IItemContainerTemplateSelector?> ItemContainerTemplateSelectorProperty =
        AvaloniaProperty.Register<ListView, IItemContainerTemplateSelector?>(nameof(ItemContainerTemplateSelector));

    public IItemContainerTemplateSelector? ItemContainerTemplateSelector
    {
        get => GetValue(ItemContainerTemplateSelectorProperty);
        set => SetValue(ItemContainerTemplateSelectorProperty, value);
    }

    protected override Type StyleKeyOverride =>
        typeof(ListBox);

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        if (ItemContainerTemplateSelector?.SelectTemplate(item, this) is IDataTemplate itemContainerTemplate)
        {
            if (itemContainerTemplate.Build(item) is ListViewItem container)
            {
                return container;
            }
        }

        return new ListViewItem();
    }

    protected override bool NeedsContainerOverride(object? item, 
        int index,
        out object? recycleKey)
    {
        recycleKey = null;
        return item is not ListViewItem;
    }
}