using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Mediator;

namespace Toolkit.Foundation.Avalonia
{
    public class NavigationRouteExtension : MarkupExtension
    {
        private static readonly AttachedProperty<IMediator> MediatorProperty =
            AvaloniaProperty.RegisterAttached<NavigationRouteExtension, Control, IMediator>("Mediator");

        private readonly string name;
        private readonly Binding? mediatorBinding;

        public NavigationRouteExtension(object mediator, string name)
        {
            mediatorBinding = mediator is Binding toBinding ? toBinding : mediator.ToBinding();
            this.name = name;
        }

        private bool TryGetBinding(AvaloniaObject sender, out object? binding)
        {
            binding = sender.GetValue(StyledElement.DataContextProperty);
            return binding is not null;
        }

        public override object? ProvideValue(IServiceProvider serviceProvider)
        {
            if (serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget target)
            {
                if (target.TargetObject is TemplatedControl control)
                {
                    if (!TryGetBinding(control, out object? binding))
                    {
                        void HandleDataContextChanged(object? sender, EventArgs args)
                        {
                            if (TryGetBinding(control, out binding))
                            {
                                control.Loaded -= HandleLoaded;
                                if (mediatorBinding is not null)
                                {
                                    control.Bind(MediatorProperty, mediatorBinding);
                                    if (control.GetValue(MediatorProperty) is IMediator mediator)
                                    {
                                        mediator.Send(new NavigationRoute(name, control));
                                        control.ClearValue(MediatorProperty);
                                    }
                                }
                            }
                        }

                        control.DataContextChanged += HandleDataContextChanged;

                        void HandleLoaded(object? sender, RoutedEventArgs args)
                        {
                            control.Loaded -= HandleLoaded;
                            if (TryGetBinding(control, out binding))
                            {
                                if (mediatorBinding is not null)
                                {
                                    control.Bind(MediatorProperty, mediatorBinding);
                                    if (control.GetValue(MediatorProperty) is IMediator mediator)
                                    {
                                        mediator.Send(new NavigationRoute(name, control));
                                        control.ClearValue(MediatorProperty);
                                    }
                                }
                            }
                        }

                        control.Loaded += HandleLoaded;
                    }
                    else
                    {
                        if (mediatorBinding is not null)
                        {
                            control.Bind(MediatorProperty, mediatorBinding);
                            if (control.GetValue(MediatorProperty) is IMediator mediator)
                            {
                                mediator.Send(new NavigationRoute(name, control));
                                control.ClearValue(MediatorProperty);
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}