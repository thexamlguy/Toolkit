namespace Toolkit.Foundation;

public class ContentFactory(IServiceProvider provider,
    IServiceFactory factory) : 
    IContentFactory
{
    public object? Create(IContentTemplateDescriptor descriptor,
        object?[] parameters)
    {
        object? content = parameters is { Length: > 0 }
                ? factory.Create(descriptor.ContentType, args =>
                {
                    if (args is IInitialization initialization)
                    {
                        initialization.Initialize();
                    }
                }, parameters)
                : provider.GetRequiredKeyedService(descriptor.ContentType, args =>
                {
                    if (args is IInitialization initialization)
                    {
                        initialization.Initialize();
                    }
                }, descriptor.Key);

        return content;
    }
}