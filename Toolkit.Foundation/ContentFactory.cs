using Microsoft.Extensions.DependencyInjection;

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

        if (content is null)
        {
            if (parameters is { Length: > 0 })
            {
                content = factory.Create(descriptor.ContentType, args =>
                {
                    if (args is IPostInitialization initialization)
                    {
                        initialization.PostInitialize();
                    }
                }, parameters);
            }
            else
            {
                content = provider.GetRequiredKeyedService(descriptor.ContentType, args =>
                    {
                        if (args is IPostInitialization initialization)
                        {
                            initialization.PostInitialize();
                        }
                    }, descriptor.Key);
            }
        }

        return content;
    }
}
