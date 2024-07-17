namespace Toolkit.Foundation
{
    public interface IContentFactory
    {
        Task<object?> CreateAsync(IContentTemplateDescriptor descriptor,
            object[] resolvedArguments);
    }
}