using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using FluentAvalonia.UI.Controls;
using Mediator;

namespace Toolkit.Foundation.Avalonia
{
    public class NavigationRouter : INavigationRouter
    {
        private readonly INavigationRouteDescriptorCollection descriptors;
        private readonly IMediator mediator;
        private readonly INamedDataTemplateFactory namedDataTemplateFactory;
        private readonly INamedTemplateFactory namedTemplateFactory;
        private readonly ITemplateFactory templateFactory;
        private readonly ITypedDataTemplateFactory typedDataTemplateFactory;

        public NavigationRouter(IMediator mediator,
            ITemplateFactory templateFactory,
            INamedTemplateFactory namedTemplateFactory,
            INamedDataTemplateFactory namedDataTemplateFactory,
            ITypedDataTemplateFactory typedDataTemplateFactory,
            INavigationRouteDescriptorCollection descriptors)
        {
            this.mediator = mediator;
            this.templateFactory = templateFactory;
            this.namedTemplateFactory = namedTemplateFactory;
            this.namedDataTemplateFactory = namedDataTemplateFactory;
            this.typedDataTemplateFactory = typedDataTemplateFactory;
            this.descriptors = descriptors;
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

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public async void Navigate(Navigate args)
        {
            object? content = null;
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
                content = namedDataTemplateFactory.Create(name, parameters.ToArray());
                template = namedTemplateFactory.Create(name);
            }

            if (args.Type is Type type)
            {
                content = typedDataTemplateFactory.Create(type, parameters.ToArray());
                template = templateFactory.Create(content);
            }

            if (template is not null)
            {
                object? target  = null;
                if (descriptors.FirstOrDefault(x => args.Route is string { } name && name == x.Name) is NavigationRouteDescriptor descriptor)
                {
                    target = descriptor.Route;
                }
                else
                {
                    target = template;
                }

                if (target is TemplatedControl control)
                {
                    //if (await messenger.Send(NavigationRouteRequest.Create(control, content, template, keyedParameters)))
                    //{
                    //    messenger.Send(Navigated.Create(template, content, keyedParameters));
                    //}
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
    }
}
