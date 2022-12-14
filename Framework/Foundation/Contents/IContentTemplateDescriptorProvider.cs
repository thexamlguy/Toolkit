namespace Toolkit.Framework.Foundation;

public interface IContentTemplateDescriptorProvider
{
    IContentTemplateDescriptor? Get(string name);

    IContentTemplateDescriptor? Get(Type type);

    IContentTemplateDescriptor? Get<T>();
}