namespace Toolkit.Foundation;

public class ContentTemplateDescriptorProvider(IEnumerable<IContentTemplateDescriptor> contentTemplates) :
    IContentTemplateDescriptorProvider
{
    public IContentTemplateDescriptor? Get(object key)
    {
        if (contentTemplates.FirstOrDefault(x => x.Key.Equals(key) || x.Key.Equals($"{key}".Replace("ViewModel", "")))
            is IContentTemplateDescriptor viewModelTemplate)
        {
            return viewModelTemplate;
        }

        return default;
    }
}