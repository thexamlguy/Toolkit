using Avalonia;
using Mediator;
using Toolkit.Framework.Foundation;

namespace Toolkit.Framework.Avalonia;

public class ContentHandler : IRequestHandler<Content, object?>
{
    private readonly INamedContentTemplateFactory namedContentTemplateFactory;
    private readonly INamedContentFactory namedContentFactory;
    private readonly IContentTemplateFactory contentTemplateFactory;
    private readonly ITypedContentFactory typedContentFactory;

    public ContentHandler(IContentTemplateFactory contentTemplateFactory,
        INamedContentFactory namedContentFactory,
        INamedContentTemplateFactory namedContentTemplateFactory,
        ITypedContentFactory typedContentFactory)
    {
        this.contentTemplateFactory = contentTemplateFactory;
        this.namedContentFactory = namedContentFactory;
        this.namedContentTemplateFactory = namedContentTemplateFactory;
        this.typedContentFactory = typedContentFactory;
    }
    
    public ValueTask<object?> Handle(Content request, CancellationToken cancellationToken)
    {
        object? content = null;
        object? template = null;

        if (request.Name is { Length: > 0 } name)
        {
            content = namedContentFactory.Create(name, request.Parameters);
            template = namedContentTemplateFactory.Create(name);
        }

        if (request.Type is Type type)
        {
            content = typedContentFactory.Create(type, request.Parameters);
            template = contentTemplateFactory.Create(content);
        }

        if (template is Visual visual)
        {
            visual.DataContext = content;
            return new ValueTask<object?>(visual);
        }

        return default;
    }
}