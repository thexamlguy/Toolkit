using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Markup;
using Toolkit.Foundation;

namespace Toolkit.WinUI;

public class ContentTemplate :
    DataTemplateSelector,
    IContentTemplate
{
    private readonly Dictionary<string, DataTemplate> _cache = [];

    protected override DataTemplate? SelectTemplateCore(object item)
    {
        if (item is IObservableViewModel observableViewModel)
        {
            if (observableViewModel.Provider is IServiceProvider provider)
            {
                Type itemType = item.GetType();
                string key = itemType.Name.Replace("ViewModel", "");

                if (_cache.TryGetValue(key, out DataTemplate? cachedTemplate))
                {
                    return cachedTemplate;
                }

                if (provider.GetRequiredKeyedService<IContentTemplateDescriptor>(key)
                    is IContentTemplateDescriptor descriptor)
                {
                    var newTemplate = CreateDataTemplate(descriptor);
                    _cache[key] = newTemplate;
                    return newTemplate;
                }
            }
        }

        return default;
    }

    protected override DataTemplate? SelectTemplateCore(object item,
        DependencyObject container) => SelectTemplateCore(item);

    private static DataTemplate CreateDataTemplate(IContentTemplateDescriptor descriptor)
    {
        string xamlString = @$"
                <DataTemplate xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                              xmlns:Template=""using:{descriptor.TemplateType.Namespace}"">
                      <Template:{descriptor.TemplateType.Name}/>
                </DataTemplate>";

        return (DataTemplate)XamlReader.Load(xamlString);
    }
}
