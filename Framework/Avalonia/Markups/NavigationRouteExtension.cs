using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Mediator;
using Toolkit.Framework.Foundation;

namespace Toolkit.Framework.Avalonia;

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
                    void AddRoute(TemplatedControl control)
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

                    void HandleDataContextChanged(object? sender, EventArgs args)
                    {
                        control.Loaded -= HandleLoaded;
                        if (TryGetBinding(control, out binding))
                        {
                            AddRoute(control);
                        }
                    }

                    control.DataContextChanged += HandleDataContextChanged;
                    void HandleLoaded(object? sender, RoutedEventArgs args)
                    {
                        control.Loaded -= HandleLoaded;
                        if (TryGetBinding(control, out binding))
                        {
                            AddRoute(control);
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