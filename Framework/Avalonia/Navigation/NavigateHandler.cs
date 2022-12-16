﻿using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using Mediator;
using System.Diagnostics;
using Toolkit.Framework.Foundation;

namespace Toolkit.Framework.Avalonia;

public class NavigateHandler : IRequestHandler<Navigate>
{
    private readonly INavigationRouteDescriptorCollection descriptors;
    private readonly IMediator mediator;
    private readonly INamedContentTemplateFactory namedContentTemplateFactory;
    private readonly INamedContentFactory namedContentFactory;
    private readonly IContentTemplateFactory contentTemplateFactory;
    private readonly ITypedContentFactory typedContentFactory;

    public NavigateHandler(IMediator mediator,
        IContentTemplateFactory contentTemplateFactory,
        INamedContentFactory namedContentFactory,
        INamedContentTemplateFactory namedContentTemplateFactory,
        ITypedContentFactory typedContentFactory,
        INavigationRouteDescriptorCollection descriptors)
    {
        this.mediator = mediator;
        this.contentTemplateFactory = contentTemplateFactory;
        this.namedContentFactory = namedContentFactory;
        this.namedContentTemplateFactory = namedContentTemplateFactory;
        this.typedContentFactory = typedContentFactory;
        this.descriptors = descriptors;
    }

    public async ValueTask<Unit> Handle(Navigate request, CancellationToken cancellationToken)
    {
        object? content = null;
        object? template = null;

        Dictionary<string, object> keyedParameters = new();
        List<object> parameters = new();

        foreach (object? parameter in request.Parameters)
        {
            if (parameter is not null)
            {
                if (parameter is KeyValuePair<string, object> keyed)
                {
                    keyedParameters.Add(keyed.Key, keyed.Value);
                }
                else
                {
                    parameters.Add(parameter);
                }
            }
        }

        if (request.Name is { Length: > 0 } name)
        {
            content = namedContentFactory.Create(name, parameters.ToArray());
            template = namedContentTemplateFactory.Create(name);
        }

        if (request.Type is Type type)
        {
            content = typedContentFactory.Create(type, parameters.ToArray());
            template = contentTemplateFactory.Create(content);
        }

        if (template is not null)
        {
            object? target = null;
            if (descriptors.FirstOrDefault(x => request.Route is string { } name && name == x.Name) is NavigationRouteDescriptor descriptor)
            {
                target = descriptor.Route;
            }
            else
            {
                target = template;
            }

            bool hasNavigated = false;
            if (target is Frame frame)
            {
                hasNavigated = await mediator.Send(new FrameNavigation(frame, content, template, keyedParameters));
            }

            if (target is ContentDialog dialog)
            {
                hasNavigated = await mediator.Send(new ContentDialogNavigation(dialog, content, template, keyedParameters));
            }

            if (target is ContentControl contentControl)
            {
                hasNavigated = await mediator.Send(new ContentControlNavigation(contentControl, content, template, keyedParameters));
            }

            if (hasNavigated)
            {
                if (content is INavigated navigated)
                {
                    await navigated.Navigated();
                }
            }
        }
        else
        {
            if (descriptors.FirstOrDefault(x => request.Route is string { } name && name == x.Name) is NavigationRouteDescriptor descriptor)
            {
                if (descriptor.Route is ContentControl contentControl)
                {
                    contentControl.Content = null;
                }
            }
        }

        return default;
    }
}