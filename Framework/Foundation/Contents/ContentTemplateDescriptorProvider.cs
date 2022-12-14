namespace Toolkit.Framework.Foundation;

public class ContentTemplateDescriptorProvider : IContentTemplateDescriptorProvider
{
    private readonly IReadOnlyCollection<IContentTemplateDescriptor> descriptors;

    public ContentTemplateDescriptorProvider(IReadOnlyCollection<IContentTemplateDescriptor> descriptors)
    {
        this.descriptors = descriptors;
    }

    public IContentTemplateDescriptor? Get(string name)
    {
        if (descriptors.FirstOrDefault(x => x.Name == name) is IContentTemplateDescriptor descriptor)
        {
            return descriptor;
        }

        return null;
    }

    public IContentTemplateDescriptor? Get(Type type)
    {
        if (descriptors.FirstOrDefault(x => x.ContentType == type) is IContentTemplateDescriptor descriptor)
        {
            return descriptor;
        }

        return null;
    }

    public IContentTemplateDescriptor? Get<T>()
    {
        if (descriptors.FirstOrDefault(x => x.ContentType == typeof(T)) is IContentTemplateDescriptor descriptor)
        {
            return descriptor;
        }

        return null;
    }
}