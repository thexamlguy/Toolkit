namespace Toolkit.Framework.Foundation;

public interface ITemplateDescriptorProvider
{
    ITemplateDescriptor? Get(string name);

    ITemplateDescriptor? Get(Type type);

    ITemplateDescriptor? Get<T>();
}