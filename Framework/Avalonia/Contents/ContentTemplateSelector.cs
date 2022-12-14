using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Toolkit.Framework.Foundation;

namespace Toolkit.Framework.Avalonia;

public class ContentTemplateSelector : IDataTemplate, IContentTemplateSelector
{
    private readonly Dictionary<object, IControl> dataTracking = new();

    private readonly IContentTemplateFactory templateFactory;

    public ContentTemplateSelector(IContentTemplateFactory templateFactory)
    {
        this.templateFactory = templateFactory;
    }

    public IControl? Build(object? item)
    {
        if (item is not null)
        {
            if (dataTracking.TryGetValue(item, out IControl? control))
            {
                return control;
            }

            return (IControl?)templateFactory.Create(item);
        }

        return null;
    }

    public bool Match(object? data)
    {
        return true;
    }
}