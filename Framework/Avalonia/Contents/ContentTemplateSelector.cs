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

    public IControl? Build(object? content)
    {
        if (content is not null)
        {
            if (dataTracking.TryGetValue(content, out IControl? control))
            {
                return control;
            }
            else
            {
                control = (IControl?)templateFactory.Create(content);
            }

            return control;
        }

        return null;
    }

    public bool Match(object? data)
    {
        return true;
    }
}