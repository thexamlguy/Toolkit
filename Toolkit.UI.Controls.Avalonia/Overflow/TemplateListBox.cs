using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace Toolkit.UI.Controls.Avalonia;

public class TemplateListBox :
    ListBox
{
    protected override Type StyleKeyOverride =>
        typeof(ListBox);

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        if (recycleKey is IDataTemplate itemContainerTemplate)
        {
            if (itemContainerTemplate.Build(item) is ListBoxItem container)
            {
                return container;
            }
        }

        return new ListBoxItem();
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        if (item is ListBoxItem)
        {
            recycleKey = null;
            return false;
        }

        if (this.FindDataTemplate(item, ItemTemplate) is IDataTemplate itemContainerTemplate)
        {
            recycleKey = itemContainerTemplate;
            return true;
        }

        recycleKey = DefaultRecycleKey;
        return true;
    }
}

