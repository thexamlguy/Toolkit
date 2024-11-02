using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace Toolkit.UI.Controls.Avalonia;

public interface IItemContainerTemplateSelector
{
    IDataTemplate? SelectTemplate(object? item, ItemsControl itemsControl);
}
