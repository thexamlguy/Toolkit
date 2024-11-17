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
    protected override DataTemplate? SelectTemplateCore(object item)
    {
        if (item is IObservableViewModel observableViewModel)
        {
            if (observableViewModel.Provider is IServiceProvider provider)
            {
                Type itemType = item.GetType();
                if (provider.GetRequiredKeyedService<IContentTemplateDescriptor>(itemType.Name.Replace("ViewModel", ""))
                    is IContentTemplateDescriptor descriptor)
                {
                    return CreateDataTemplate(descriptor);
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
                              xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
                              xmlns:local=""using:Toolkit.WinUI"">
                    <local:TemplateControl />
                </DataTemplate>";

        return (DataTemplate)XamlReader.Load(xamlString);
    }
}