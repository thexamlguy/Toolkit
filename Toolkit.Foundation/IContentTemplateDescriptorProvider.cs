namespace Toolkit.Foundation;

public interface IContentTemplateDescriptorProvider
{
    IContentTemplateDescriptor? Get(object key);
}