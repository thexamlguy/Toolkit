using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public class Navigation(IServiceProvider provider,
    INavigationRegionProvider navigationRegionProvider,
    IContentFactory contentFactory,
    IMessenger messenger) :
    INavigation
{
    public void Navigate(string route,
        object? sender = null,
        object? region = null,
        EventHandler? navigated = null,
        IDictionary<string, object>? parameters = null)
    {
        if (region is null)
        {
            return;
        }

        string[] segments = route.Split('/');
        int segmentCount = segments.Length;
        int currentSegmentIndex = 0;

        foreach (object segment in segments)
        {
            currentSegmentIndex++;

            if (provider.GetRequiredKeyedService<IContentTemplateDescriptor>(segment)
                is IContentTemplateDescriptor descriptor)
            {
                Dictionary<string, object>? arguments = parameters?.ToDictionary(x => x.Key, x => x.Value, StringComparer.InvariantCultureIgnoreCase) ?? [];
                object?[] resolvedArguments = parameters is not null
                    ? descriptor.ContentType
                        .GetConstructors()
                        .FirstOrDefault()?
                        .GetParameters()
                        .Select(x => x?.Name is not null && arguments is not null && arguments.TryGetValue(x.Name, out object? argument)
                                ? argument
                                : null)
                        .Where(argument => argument is not null)
                        .ToArray() ?? [] : [];

                if (provider.GetRequiredKeyedService(descriptor.TemplateType, descriptor.Key)
                    is object template)
                {
                    if (region is not null)
                    {
                        switch (region)
                        {
                            case "self":
                                region = template;
                                break;

                            default:
                                if (navigationRegionProvider.TryGet(region, out object? value))
                                {
                                    region = value;
                                }

                                break;
                        }
                    }

                    if (region is not null)
                    {
                        object? content = contentFactory.Create(descriptor, resolvedArguments);
                        if (content is not null)
                        {
                            messenger.Send(new NavigateTemplateEventArgs(region, template, content, sender, parameters), region is string ? $"{region}" : region.GetType().Name);
                            if (currentSegmentIndex == segmentCount)
                            {
                                navigated?.Invoke(this, EventArgs.Empty);
                            }
                        }
                    }
                }
            }
        }
    }

    public void Back(object? region)
    {
        if (region is not null)
        {
            navigationRegionProvider.TryGet(region, out region);
        }

        if (region is not null)
        {
            Type navigationType = region is Type type ? type : region.GetType();
            Type navigateType = typeof(NavigateBackEventArgs<>).MakeGenericType(navigationType);
            if (Activator.CreateInstance(navigateType, [region]) is object navigate)
            {
                messenger.Send(navigate, navigationType.Name);
            }
        }
    }
}