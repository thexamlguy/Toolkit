using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Diagnostics;
using Toolkit.Framework.Foundation;

namespace Toolkit.Framework.Avalonia;

public class ContentExtension : MarkupExtension
{
    private static readonly AttachedProperty<IMediator> MediatorProperty =
        AvaloniaProperty.RegisterAttached<ContentExtension, AvaloniaObject, IMediator>("Mediator");

    private static readonly AttachedProperty<object> ParameterProperty =
        AvaloniaProperty.RegisterAttached<ContentExtension, AvaloniaObject, object>("Parameter");

    private static readonly AvaloniaProperty ToProperty =
        AvaloniaProperty.RegisterAttached<ContentExtension, AvaloniaObject, object>("To");

    private readonly Binding? mediatorBinding;
    private readonly List<object?> parameters = new();
    private readonly Binding? toBinding;

    public ContentExtension(object mediator,
        object to)
    {
        mediatorBinding = mediator.ToBinding();
        this.toBinding = to is Binding toBinding ? toBinding : to.ToBinding();
    }

    public ContentExtension(object mediator,
        object to,
        object args1)
    {
        mediatorBinding = mediator.ToBinding();
        this.toBinding = to is Binding toBinding ? toBinding : to.ToBinding();

        parameters.Add(args1 is MarkupExtension ? args1 : args1.ToBinding());
    }

    public ContentExtension(object mediator,
        object to,
        object args1,
        object args2)
    {
        mediatorBinding = mediator.ToBinding();
        this.toBinding = to is Binding toBinding ? toBinding : to.ToBinding();

        parameters.Add(args1 is MarkupExtension ? args1 : args1.ToBinding());
        parameters.Add(args2 is MarkupExtension ? args2 : args2.ToBinding());
    }

    public ContentExtension(object mediator,
        object to,
        object args1,
        object args2,
        object args3)
    {
        mediatorBinding = mediator.ToBinding();
        this.toBinding = to is Binding toBinding ? toBinding : to.ToBinding();

        parameters.Add(args1 is MarkupExtension ? args1 : args1.ToBinding());
        parameters.Add(args2 is MarkupExtension ? args2 : args2.ToBinding());
        parameters.Add(args3 is MarkupExtension ? args3 : args3.ToBinding());
    }

    public ContentExtension(object mediator,
        object to,
        object args1,
        object args2,
        object args3,
        object args4)
    {
        mediatorBinding = mediator.ToBinding();
        this.toBinding = to is Binding toBinding ? toBinding : to.ToBinding();

        parameters.Add(args1 is MarkupExtension ? args1 : args1.ToBinding());
        parameters.Add(args2 is MarkupExtension ? args2 : args2.ToBinding());
        parameters.Add(args3 is MarkupExtension ? args3 : args3.ToBinding());
        parameters.Add(args4 is MarkupExtension ? args4 : args4.ToBinding());
    }

    public ContentExtension(object mediator,
        object to,
        object args1,
        object args2,
        object args3,
        object args4,
        object args5)
    {
        mediatorBinding = mediator.ToBinding();
        this.toBinding = to is Binding toBinding ? toBinding : to.ToBinding();

        parameters.Add(args1 is MarkupExtension ? args1 : args1.ToBinding());
        parameters.Add(args2 is MarkupExtension ? args2 : args2.ToBinding());
        parameters.Add(args3 is MarkupExtension ? args3 : args3.ToBinding());
        parameters.Add(args4 is MarkupExtension ? args4 : args4.ToBinding());
        parameters.Add(args5 is MarkupExtension ? args5 : args5.ToBinding());
    }

    public ContentExtension(object mediator,
        object to,
        object args1,
        object args2,
        object args3,
        object args4,
        object args5,
        object args6)
    {
        mediatorBinding = mediator.ToBinding();
        this.toBinding = to is Binding toBinding ? toBinding : to.ToBinding();

        parameters.Add(args1 is MarkupExtension ? args1 : args1.ToBinding());
        parameters.Add(args2 is MarkupExtension ? args2 : args2.ToBinding());
        parameters.Add(args3 is MarkupExtension ? args3 : args3.ToBinding());
        parameters.Add(args4 is MarkupExtension ? args4 : args4.ToBinding());
        parameters.Add(args5 is MarkupExtension ? args5 : args5.ToBinding());
        parameters.Add(args6 is MarkupExtension ? args6 : args6.ToBinding());
    }

    public ContentExtension(object mediator,
        object to,
        object args1,
        object args2,
        object args3,
        object args4,
        object args5,
        object args6,
        object args7)
    {
        mediatorBinding = mediator.ToBinding();
        this.toBinding = to is Binding toBinding ? toBinding : to.ToBinding();

        parameters.Add(args1 is MarkupExtension ? args1 : args1.ToBinding());
        parameters.Add(args2 is MarkupExtension ? args2 : args2.ToBinding());
        parameters.Add(args3 is MarkupExtension ? args3 : args3.ToBinding());
        parameters.Add(args4 is MarkupExtension ? args4 : args4.ToBinding());
        parameters.Add(args5 is MarkupExtension ? args5 : args5.ToBinding());
        parameters.Add(args6 is MarkupExtension ? args6 : args6.ToBinding());
        parameters.Add(args7 is MarkupExtension ? args7 : args7.ToBinding());
    }

    public ContentExtension(object mediator,
        object to,
        object args1,
        object args2,
        object args3,
        object args4,
        object args5,
        object args6,
        object args7,
        object args8)
    {
        mediatorBinding = mediator.ToBinding();
        this.toBinding = to is Binding toBinding ? toBinding : to.ToBinding();

        parameters.Add(args1 is MarkupExtension ? args1 : args1.ToBinding());
        parameters.Add(args2 is MarkupExtension ? args2 : args2.ToBinding());
        parameters.Add(args3 is MarkupExtension ? args3 : args3.ToBinding());
        parameters.Add(args4 is MarkupExtension ? args4 : args4.ToBinding());
        parameters.Add(args5 is MarkupExtension ? args5 : args5.ToBinding());
        parameters.Add(args6 is MarkupExtension ? args6 : args6.ToBinding());
        parameters.Add(args7 is MarkupExtension ? args7 : args7.ToBinding());
        parameters.Add(args8 is MarkupExtension ? args8 : args8.ToBinding());
    }

    public ContentExtension(object mediator,
        object to,
        object args1,
        object args2,
        object args3,
        object args4,
        object args5,
        object args6,
        object args7,
        object args8,
        object args9)
    {
        mediatorBinding = mediator.ToBinding();
        this.toBinding = to is Binding toBinding ? toBinding : to.ToBinding();

        parameters.Add(args1 is MarkupExtension ? args1 : args1.ToBinding());
        parameters.Add(args2 is MarkupExtension ? args2 : args2.ToBinding());
        parameters.Add(args3 is MarkupExtension ? args3 : args3.ToBinding());
        parameters.Add(args4 is MarkupExtension ? args4 : args4.ToBinding());
        parameters.Add(args5 is MarkupExtension ? args5 : args5.ToBinding());
        parameters.Add(args6 is MarkupExtension ? args6 : args6.ToBinding());
        parameters.Add(args7 is MarkupExtension ? args7 : args7.ToBinding());
        parameters.Add(args8 is MarkupExtension ? args8 : args8.ToBinding());
        parameters.Add(args9 is MarkupExtension ? args9 : args9.ToBinding());
    }

    public ContentExtension(object mediator,
        object to,
        object args1,
        object args2,
        object args3,
        object args4,
        object args5,
        object args6,
        object args7,
        object args8,
        object args9,
        object args10)
    {
        mediatorBinding = mediator.ToBinding();
        this.toBinding = to is Binding toBinding ? toBinding : to.ToBinding();

        parameters.Add(args1 is MarkupExtension ? args1 : args1.ToBinding());
        parameters.Add(args2 is MarkupExtension ? args2 : args2.ToBinding());
        parameters.Add(args3 is MarkupExtension ? args3 : args3.ToBinding());
        parameters.Add(args4 is MarkupExtension ? args4 : args4.ToBinding());
        parameters.Add(args5 is MarkupExtension ? args5 : args5.ToBinding());
        parameters.Add(args6 is MarkupExtension ? args6 : args6.ToBinding());
        parameters.Add(args7 is MarkupExtension ? args7 : args7.ToBinding());
        parameters.Add(args8 is MarkupExtension ? args8 : args8.ToBinding());
        parameters.Add(args9 is MarkupExtension ? args9 : args9.ToBinding());
        parameters.Add(args10 is MarkupExtension ? args10 : args10.ToBinding());
    }

    public ContentExtension(object mediator,
        object to,
        object args1,
        object args2,
        object args3,
        object args4,
        object args5,
        object args6,
        object args7,
        object args8,
        object args9,
        object args10,
        object args11)
    {
        mediatorBinding = mediator.ToBinding();
        this.toBinding = to is Binding toBinding ? toBinding : to.ToBinding();

        parameters.Add(args1 is MarkupExtension ? args1 : args1.ToBinding());
        parameters.Add(args2 is MarkupExtension ? args2 : args2.ToBinding());
        parameters.Add(args3 is MarkupExtension ? args3 : args3.ToBinding());
        parameters.Add(args4 is MarkupExtension ? args4 : args4.ToBinding());
        parameters.Add(args5 is MarkupExtension ? args5 : args5.ToBinding());
        parameters.Add(args6 is MarkupExtension ? args6 : args6.ToBinding());
        parameters.Add(args7 is MarkupExtension ? args7 : args7.ToBinding());
        parameters.Add(args8 is MarkupExtension ? args8 : args8.ToBinding());
        parameters.Add(args9 is MarkupExtension ? args9 : args9.ToBinding());
        parameters.Add(args10 is MarkupExtension ? args10 : args10.ToBinding());
        parameters.Add(args11 is MarkupExtension ? args11 : args11.ToBinding());
    }

    public ContentExtension(object mediator,
        object to,
        object args1,
        object args2,
        object args3,
        object args4,
        object args5,
        object args6,
        object args7,
        object args8,
        object args9,
        object args10,
        object args11,
        object args12)
    {
        mediatorBinding = mediator.ToBinding();
        this.toBinding = to is Binding toBinding ? toBinding : to.ToBinding();

        parameters.Add(args1 is MarkupExtension ? args1 : args1.ToBinding());
        parameters.Add(args2 is MarkupExtension ? args2 : args2.ToBinding());
        parameters.Add(args3 is MarkupExtension ? args3 : args3.ToBinding());
        parameters.Add(args4 is MarkupExtension ? args4 : args4.ToBinding());
        parameters.Add(args5 is MarkupExtension ? args5 : args5.ToBinding());
        parameters.Add(args6 is MarkupExtension ? args6 : args6.ToBinding());
        parameters.Add(args7 is MarkupExtension ? args7 : args7.ToBinding());
        parameters.Add(args8 is MarkupExtension ? args8 : args8.ToBinding());
        parameters.Add(args9 is MarkupExtension ? args9 : args9.ToBinding());
        parameters.Add(args10 is MarkupExtension ? args10 : args10.ToBinding());
        parameters.Add(args11 is MarkupExtension ? args11 : args11.ToBinding());
        parameters.Add(args12 is MarkupExtension ? args12 : args12.ToBinding());
    }

    public ContentExtension(object mediator,
        object to,
        object args1,
        object args2,
        object args3,
        object args4,
        object args5,
        object args6,
        object args7,
        object args8,
        object args9,
        object args10,
        object args11,
        object args12,
        object args13)
    {
        mediatorBinding = mediator.ToBinding();
        this.toBinding = to is Binding toBinding ? toBinding : to.ToBinding();

        parameters.Add(args1 is MarkupExtension ? args1 : args1.ToBinding());
        parameters.Add(args2 is MarkupExtension ? args2 : args2.ToBinding());
        parameters.Add(args3 is MarkupExtension ? args3 : args3.ToBinding());
        parameters.Add(args4 is MarkupExtension ? args4 : args4.ToBinding());
        parameters.Add(args5 is MarkupExtension ? args5 : args5.ToBinding());
        parameters.Add(args6 is MarkupExtension ? args6 : args6.ToBinding());
        parameters.Add(args7 is MarkupExtension ? args7 : args7.ToBinding());
        parameters.Add(args8 is MarkupExtension ? args8 : args8.ToBinding());
        parameters.Add(args9 is MarkupExtension ? args9 : args9.ToBinding());
        parameters.Add(args10 is MarkupExtension ? args10 : args10.ToBinding());
        parameters.Add(args11 is MarkupExtension ? args11 : args11.ToBinding());
        parameters.Add(args12 is MarkupExtension ? args12 : args12.ToBinding());
        parameters.Add(args13 is MarkupExtension ? args13 : args13.ToBinding());
    }

    public ContentExtension(object mediator,
        object to,
        object args1,
        object args2,
        object args3,
        object args4,
        object args5,
        object args6,
        object args7,
        object args8,
        object args9,
        object args10,
        object args11,
        object args12,
        object args13,
        object args14)
    {
        mediatorBinding = mediator.ToBinding();
        this.toBinding = to is Binding toBinding ? toBinding : to.ToBinding();

        parameters.Add(args1 is MarkupExtension ? args1 : args1.ToBinding());
        parameters.Add(args2 is MarkupExtension ? args2 : args2.ToBinding());
        parameters.Add(args3 is MarkupExtension ? args3 : args3.ToBinding());
        parameters.Add(args4 is MarkupExtension ? args4 : args4.ToBinding());
        parameters.Add(args5 is MarkupExtension ? args5 : args5.ToBinding());
        parameters.Add(args6 is MarkupExtension ? args6 : args6.ToBinding());
        parameters.Add(args7 is MarkupExtension ? args7 : args7.ToBinding());
        parameters.Add(args8 is MarkupExtension ? args8 : args8.ToBinding());
        parameters.Add(args9 is MarkupExtension ? args9 : args9.ToBinding());
        parameters.Add(args10 is MarkupExtension ? args10 : args10.ToBinding());
        parameters.Add(args11 is MarkupExtension ? args11 : args11.ToBinding());
        parameters.Add(args12 is MarkupExtension ? args12 : args12.ToBinding());
        parameters.Add(args13 is MarkupExtension ? args13 : args13.ToBinding());
        parameters.Add(args14 is MarkupExtension ? args14 : args14.ToBinding());
    }

    public ContentExtension(object mediator,
        object to,
        object args1,
        object args2,
        object args3,
        object args4,
        object args5,
        object args6,
        object args7,
        object args8,
        object args9,
        object args10,
        object args11,
        object args12,
        object args13,
        object args14,
        object args15)
    {
        mediatorBinding = mediator.ToBinding();
        this.toBinding = to is Binding toBinding ? toBinding : to.ToBinding();

        parameters.Add(args1 is MarkupExtension ? args1 : args1.ToBinding());
        parameters.Add(args2 is MarkupExtension ? args2 : args2.ToBinding());
        parameters.Add(args3 is MarkupExtension ? args3 : args3.ToBinding());
        parameters.Add(args4 is MarkupExtension ? args4 : args4.ToBinding());
        parameters.Add(args5 is MarkupExtension ? args5 : args5.ToBinding());
        parameters.Add(args6 is MarkupExtension ? args6 : args6.ToBinding());
        parameters.Add(args7 is MarkupExtension ? args7 : args7.ToBinding());
        parameters.Add(args8 is MarkupExtension ? args8 : args8.ToBinding());
        parameters.Add(args9 is MarkupExtension ? args9 : args9.ToBinding());
        parameters.Add(args10 is MarkupExtension ? args10 : args10.ToBinding());
        parameters.Add(args11 is MarkupExtension ? args11 : args11.ToBinding());
        parameters.Add(args12 is MarkupExtension ? args12 : args12.ToBinding());
        parameters.Add(args13 is MarkupExtension ? args13 : args13.ToBinding());
        parameters.Add(args14 is MarkupExtension ? args14 : args14.ToBinding());
        parameters.Add(args15 is MarkupExtension ? args15 : args15.ToBinding());
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        if (serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget target)
        {
            AvaloniaObject? targetObject = null;
            if (target.TargetObject is AvaloniaObject avaloniaObject)
            {
                targetObject = avaloniaObject;
            }
            else if (serviceProvider.GetService(typeof(IRootObjectProvider)) is IRootObjectProvider root)
            {
                targetObject = (AvaloniaObject)root.RootObject;
            }

            if (targetObject is not null && toBinding is not null && mediatorBinding is not null)
            {
                if (target.TargetProperty is StyledProperty<object> targetProperty)
                {
                    if (targetObject is Control control)
                    {
                        void SetContent()
                        {
                            targetObject.Bind(MediatorProperty, mediatorBinding);
                            if (targetObject.GetValue(MediatorProperty) is IMediator mediator)
                            {
                                targetObject.Bind(ToProperty, toBinding);
                                if (targetObject.GetValue(ToProperty) is { } to)
                                {
                                    List<object>? parameters = new();
                                    foreach (object? parameter in this.parameters)
                                    {
                                        if (parameter is not null)
                                        {
                                            switch (parameter)
                                            {
                                                case IParameter keyedParameter:
                                                    if (keyedParameter.GetValue(targetObject) is KeyValuePair<string, object> keyValuePair)
                                                    {
                                                        parameters.Add(keyValuePair);
                                                    }
                                                    break;

                                                default:
                                                    if (parameter.ToBinding() is Binding defaultDinding)
                                                    {
                                                        targetObject.Bind(ParameterProperty, defaultDinding);
                                                        parameters.Add((dynamic)targetObject.GetValue(ParameterProperty));
                                                    }
                                                    break;
                                            }
                                        }
                                    }

                                    if (to is string name)
                                    {
                                        Trace.WriteLine(name);

                                        ValueTask<object?> namedTask = mediator.Send(new Content(name, parameters.ToArray()));
                                        if (namedTask is ValueTask<object?> { Result: object result })
                                        {
                                            control.SetValue(targetProperty, result);
                                        }
                                    }

                                    if (to is Type type)
                                    {
                                        ValueTask<object?> typedTask = mediator.Send(new Content(type, parameters.ToArray()));
                                        if (typedTask is ValueTask<object?> { Result: object result })
                                        {
                                            control.SetValue(targetProperty, result);
                                        }
                                    }
                                }
                            }
                        }

                        if (!control.IsLoaded)
                        {
                            void HandleLoaded(object? sender, RoutedEventArgs args)
                            {
                                control.Loaded -= HandleLoaded;
                                SetContent();
                            }

                            control.Loaded += HandleLoaded;
                        }
                        else
                        {
                            SetContent();
                        }
                    }
                }
            }
        }

        return default;
    }
}
