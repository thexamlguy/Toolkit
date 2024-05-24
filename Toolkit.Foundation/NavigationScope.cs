using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public class NavigationScope(IPublisher publisher,
    IServiceProvider provider,
    IServiceFactory factory,
    INavigationProvider navigationProvider,
    INavigationRegionProvider navigationRegionProvider,
    IContentTemplateDescriptorProvider contentTemplateDescriptorProvider) :
    INavigationScope
{
    public void Navigate(string route,
        object? sender = null,
        object? region = null,
        EventHandler? navigated = null,
        object[]? parameters = null)
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

            if (contentTemplateDescriptorProvider.Get(segment)
                is IContentTemplateDescriptor descriptor)
            {
                Dictionary<string, object>? arguments = parameters?.OfType<KeyValuePair<string, object>>()
                    .ToDictionary(x => x.Key, x => x.Value, StringComparer.InvariantCultureIgnoreCase) ?? [];

                IEnumerable<object?>? mappedParameters = descriptor.ContentType
                     .GetConstructors()
                     .FirstOrDefault()?
                     .GetParameters()
                     .Select(parameter => parameter?.Name is not null && arguments
                         .TryGetValue(parameter.Name, out object? argument) ? argument : default)
                     .Where(argument => argument is not null);

                parameters = [.. parameters?.Where(x => x.GetType() != typeof(KeyValuePair<string, object>)) ??
                    Enumerable.Empty<object?>(),
                    .. mappedParameters ?? Enumerable.Empty<object?>()];

                if (provider.GetRequiredKeyedService(descriptor.TemplateType, segment) is object view)
                {
                    if (region is not null)
                    {
                        switch (region)
                        {
                            case "self":
                                region = view;
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
                        if ((parameters is { Length: > 0 }
                        ? factory.Create(descriptor.ContentType, parameters)
                        : provider.GetRequiredKeyedService(descriptor.ContentType, segment)) is object viewModel)
                        {
                            if (navigationProvider.Get(region is Type type ? type : region.GetType()) is INavigation navigation)
                            {
                                Type navigateType = typeof(NavigateEventArgs<>).MakeGenericType(navigation.Type);
                                if (Activator.CreateInstance(navigateType, [region, view, viewModel, sender, parameters]) is object navigate)
                                {
                                    publisher.Publish(navigate);
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
        }
    }

    public void Back(object? context)
    {
        if (context is not null)
        {
            navigationRegionProvider.TryGet(context, out context);
        }

        if (context is not null)
        {
            if (navigationProvider.Get(context is Type type ? type : context.GetType())
                is INavigation navigation)
            {
                Type navigateType = typeof(NavigateBackEventArgs<>).MakeGenericType(navigation.Type);
                if (Activator.CreateInstance(navigateType, [context]) is object navigate)
                {
                    publisher.Publish(navigate);
                }
            }
        }
    }
}