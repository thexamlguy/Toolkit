namespace Toolkit.Foundation;

public class ContentFactory(IMediator mediator,
    IServiceProvider provider,
    IServiceFactory factory) : IContentFactory
{
    public async Task<object?> CreateAsync(IContentTemplateDescriptor descriptor, 
        object[] parameters)
    {
        Type createEventType = typeof(CreateEventArgs<>).MakeGenericType(descriptor.ContentType);

        object? content = null;
        if (Activator.CreateInstance(createEventType, [null, parameters]) is object createEvent)
        {
            content = await mediator.Handle(descriptor.ContentType, createEvent, descriptor.Key);
        }

        content ??= parameters is { Length: > 0 }
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
