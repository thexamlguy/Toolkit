using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.Messaging;
using FluentAvalonia.UI.Controls;

namespace Toolkit.Foundation.Avalonia
{
    public class NavigationRouter : INavigationRouter
    {
        private readonly INavigationRouteDescriptorCollection descriptors;
        private readonly IMessenger messenger;
        private readonly INamedDataTemplateFactory namedDataTemplateFactory;
        private readonly INamedTemplateFactory namedTemplateFactory;
        private readonly ITemplateDescriptorProvider templateDescriptorProvider;
        private readonly ITemplateFactory templateFactory;
        private readonly ITypedDataTemplateFactory typedDataTemplateFactory;

        public NavigationRouter(ITemplateDescriptorProvider templateDescriptorProvider,
            ITemplateFactory templateFactory,
            INamedTemplateFactory namedTemplateFactory,
            INamedDataTemplateFactory namedDataTemplateFactory,
            ITypedDataTemplateFactory typedDataTemplateFactory,
            IMessenger messenger,
            INavigationRouteDescriptorCollection descriptors)
        {
            this.templateDescriptorProvider = templateDescriptorProvider;
            this.templateFactory = templateFactory;
            this.namedTemplateFactory = namedTemplateFactory;
            this.namedDataTemplateFactory = namedDataTemplateFactory;
            this.typedDataTemplateFactory = typedDataTemplateFactory;
            this.messenger = messenger;
            this.descriptors = descriptors;
        }

        public async void Navigate(Navigate args)
        {
            object? data = null;
            object? template = null;

            Dictionary<string, object> keyedParameters = new();
            List<object> parameters = new();

            foreach (object? parameter in args.Parameters)
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

            if (args.Name is { Length: > 0 } name)
            {
                data = namedDataTemplateFactory.Create(name, parameters.ToArray());
                template = namedTemplateFactory.Create(name);
            }

            if (args.Type is Type type)
            {
                data = typedDataTemplateFactory.Create(type, parameters.ToArray());
                template = templateFactory.Create(data);
            }

            if (template is not null)
            {
                bool navigated = false;
                if (template is ContentDialog contentDialog)
                {
                    navigated = await messenger.Send(new NavigationRouteRequest<ContentDialog>(contentDialog, data, template, keyedParameters));
                }
                else
                {
                    if (descriptors.FirstOrDefault(x => args.Route is string { } name && name == x.Name) is NavigationRouteDescriptor descriptor)
                    {
                        switch (descriptor.Route)
                        {
                            case Frame frame:
                                navigated = await messenger.Send(new NavigationRouteRequest<Frame>(frame, data, template, keyedParameters));
                                break;
                            case ContentControl contentControl:
                                navigated = await messenger.Send(new NavigationRouteRequest<ContentControl>(contentControl, data, template, keyedParameters));
                                break;
                        }
                    }
                }

                if (navigated)
                {
                    messenger.Send((Navigated)Navigated.Create((dynamic?)template, (dynamic?)data, keyedParameters));
                }
            }
            else
            {
                if (descriptors.FirstOrDefault(x => args.Route is string { } name && name == x.Name) is NavigationRouteDescriptor descriptor)
                {
                    if (descriptor.Route is ContentControl contentControl)
                    {
                        contentControl.Content = null;
                    }
                }
            }
        }

        public void Register(string name, object route)
        {
            if (route is TemplatedControl control)
            {
                void HandleUnloaded(object? sender, RoutedEventArgs args)
                {
                    if (descriptors.FirstOrDefault(x => x.Route == sender) is INavigationRouteDescriptor descriptor)
                    {
                        descriptors.Remove(descriptor);
                    }
                }

                control.Unloaded += HandleUnloaded;
            }

            if (descriptors.FirstOrDefault(x => x.Name == name) is INavigationRouteDescriptor descriptor)
            {
                descriptors.Remove(descriptor);
            }

            descriptors.Add(new NavigationRouteDescriptor(name, route));
        }

        public void GoBack(NavigateBack args)
        {
            if (descriptors.FirstOrDefault(x => args.Route is string { } name && name == x.Name) is NavigationRouteDescriptor descriptor)
            {
                if (descriptor.Route is ContentControl { Content: TemplatedControl content })
                {
                    if (content.DataContext is IDisposable disposable)
                    {
                        disposable.Dispose();
                    }
                }

                if (descriptor.Route is Frame frame)
                {
                    frame.GoBack();
                }
            }
        }
    }
}
