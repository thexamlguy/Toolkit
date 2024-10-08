namespace Toolkit.Foundation
{
    public interface IContentFactory
    {
        object? Create(IContentTemplateDescriptor descriptor,
            object[] parameters);
    }
}