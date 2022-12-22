using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using System.Xml.Linq;
using Toolkit.Framework.Foundation;

namespace Toolkit.Framework.Avalonia;
public class NavigateExtension : TriggerExtension
{
    private static readonly AttachedProperty<IMediator> MediatorProperty =
        AvaloniaProperty.RegisterAttached<NavigateExtension, Control, IMediator>("Mediator");

    private static readonly AttachedProperty<object> ParameterProperty =
        AvaloniaProperty.RegisterAttached<NavigateExtension, Control, object>("Parameter");

    private static readonly AvaloniaProperty PathProperty =
        AvaloniaProperty.RegisterAttached<NavigateExtension, Control, string>("Path");

    private readonly Binding? mediatorBinding;
    private readonly List<object?> parameters = new();
    private readonly Binding? pathBinding;

    public NavigateExtension(object mediator,
        object path)
    {
        mediatorBinding = mediator.ToBinding();
        this.pathBinding = path is Binding pathBinding ? pathBinding : path.ToBinding();
    }

    public NavigateExtension(object mediator,
        object path,
        object args1)
    {
        mediatorBinding = mediator.ToBinding();
        this.pathBinding = path is Binding pathBinding ? pathBinding : path.ToBinding();

        parameters.Add(args1 is MarkupExtension ? args1 : args1.ToBinding());
    }

    public NavigateExtension(object mediator,
        object path,
        object args1,
        object args2)
    {
        mediatorBinding = mediator.ToBinding();
        this.pathBinding = path is Binding pathBinding ? pathBinding : path.ToBinding();

        parameters.Add(args1 is MarkupExtension ? args1 : args1.ToBinding());
        parameters.Add(args2 is MarkupExtension ? args2 : args2.ToBinding());
    }

    public NavigateExtension(object mediator,
        object path,
        object args1,
        object args2,
        object args3)
    {
        mediatorBinding = mediator.ToBinding();
        this.pathBinding = path is Binding pathBinding ? pathBinding : path.ToBinding();

        parameters.Add(args1 is MarkupExtension ? args1 : args1.ToBinding());
        parameters.Add(args2 is MarkupExtension ? args2 : args2.ToBinding());
        parameters.Add(args3 is MarkupExtension ? args3 : args3.ToBinding());
    }

    public NavigateExtension(object mediator,
        object path,
        object args1,
        object args2,
        object args3,
        object args4)
    {
        mediatorBinding = mediator.ToBinding();
        this.pathBinding = path is Binding pathBinding ? pathBinding : path.ToBinding();

        parameters.Add(args1 is MarkupExtension ? args1 : args1.ToBinding());
        parameters.Add(args2 is MarkupExtension ? args2 : args2.ToBinding());
        parameters.Add(args3 is MarkupExtension ? args3 : args3.ToBinding());
        parameters.Add(args4 is MarkupExtension ? args4 : args4.ToBinding());
    }

    public NavigateExtension(object mediator,
        object path,
        object args1,
        object args2,
        object args3,
        object args4,
        object args5)
    {
        mediatorBinding = mediator.ToBinding();
        this.pathBinding = path is Binding pathBinding ? pathBinding : path.ToBinding();

        parameters.Add(args1 is MarkupExtension ? args1 : args1.ToBinding());
        parameters.Add(args2 is MarkupExtension ? args2 : args2.ToBinding());
        parameters.Add(args3 is MarkupExtension ? args3 : args3.ToBinding());
        parameters.Add(args4 is MarkupExtension ? args4 : args4.ToBinding());
        parameters.Add(args5 is MarkupExtension ? args5 : args5.ToBinding());
    }

    public NavigateExtension(object mediator,
        object path,
        object args1,
        object args2,
        object args3,
        object args4,
        object args5,
        object args6)
    {
        mediatorBinding = mediator.ToBinding();
        this.pathBinding = path is Binding pathBinding ? pathBinding : path.ToBinding();

        parameters.Add(args1 is MarkupExtension ? args1 : args1.ToBinding());
        parameters.Add(args2 is MarkupExtension ? args2 : args2.ToBinding());
        parameters.Add(args3 is MarkupExtension ? args3 : args3.ToBinding());
        parameters.Add(args4 is MarkupExtension ? args4 : args4.ToBinding());
        parameters.Add(args5 is MarkupExtension ? args5 : args5.ToBinding());
        parameters.Add(args6 is MarkupExtension ? args6 : args6.ToBinding());
    }

    public NavigateExtension(object mediator,
        object path,
        object args1,
        object args2,
        object args3,
        object args4,
        object args5,
        object args6,
        object args7)
    {
        mediatorBinding = mediator.ToBinding();
        this.pathBinding = path is Binding pathBinding ? pathBinding : path.ToBinding();

        parameters.Add(args1 is MarkupExtension ? args1 : args1.ToBinding());
        parameters.Add(args2 is MarkupExtension ? args2 : args2.ToBinding());
        parameters.Add(args3 is MarkupExtension ? args3 : args3.ToBinding());
        parameters.Add(args4 is MarkupExtension ? args4 : args4.ToBinding());
        parameters.Add(args5 is MarkupExtension ? args5 : args5.ToBinding());
        parameters.Add(args6 is MarkupExtension ? args6 : args6.ToBinding());
        parameters.Add(args7 is MarkupExtension ? args7 : args7.ToBinding());
    }

    public NavigateExtension(object mediator,
        object path,
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
        this.pathBinding = path is Binding pathBinding ? pathBinding : path.ToBinding();

        parameters.Add(args1 is MarkupExtension ? args1 : args1.ToBinding());
        parameters.Add(args2 is MarkupExtension ? args2 : args2.ToBinding());
        parameters.Add(args3 is MarkupExtension ? args3 : args3.ToBinding());
        parameters.Add(args4 is MarkupExtension ? args4 : args4.ToBinding());
        parameters.Add(args5 is MarkupExtension ? args5 : args5.ToBinding());
        parameters.Add(args6 is MarkupExtension ? args6 : args6.ToBinding());
        parameters.Add(args7 is MarkupExtension ? args7 : args7.ToBinding());
        parameters.Add(args8 is MarkupExtension ? args8 : args8.ToBinding());
    }

    public NavigateExtension(object mediator,
        object path,
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
        this.pathBinding = path is Binding pathBinding ? pathBinding : path.ToBinding();

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

    public NavigateExtension(object mediator,
        object path,
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
        this.pathBinding = path is Binding pathBinding ? pathBinding : path.ToBinding();

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

    public NavigateExtension(object mediator,
        object path,
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
        this.pathBinding = path is Binding pathBinding ? pathBinding : path.ToBinding();

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

    public NavigateExtension(object mediator,
        object path,
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
        this.pathBinding = path is Binding pathBinding ? pathBinding : path.ToBinding();

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

    public NavigateExtension(object mediator,
        object path,
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
        this.pathBinding = path is Binding pathBinding ? pathBinding : path.ToBinding();

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

    public NavigateExtension(object mediator,
        object path,
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
        this.pathBinding = path is Binding pathBinding ? pathBinding : path.ToBinding();

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

    public NavigateExtension(object mediator,
        object path,
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
        this.pathBinding = path is Binding pathBinding ? pathBinding : path.ToBinding();

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

    protected override void OnInvoked(object sender, EventArgs args)
    {
        if (TargetObject is not null && mediatorBinding is not null)
        {
            TargetObject.Bind(MediatorProperty, mediatorBinding);
            if (TargetObject.GetValue(MediatorProperty) is IMediator mediator)
            {
                if (pathBinding is not null)
                {
                    TargetObject.Bind(PathProperty, pathBinding);
                    if (TargetObject.GetValue(PathProperty) is string path)
                    {
                        List<object>? parameters = new();
                        foreach (object? parameter in this.parameters)
                        {
                            if (parameter is not null)
                            {
                                switch (parameter)
                                {
                                    case IParameter keyedParameter:
                                        if (keyedParameter.GetValue(TargetObject) is KeyValuePair<string, object> keyValuePair)
                                        {
                                            parameters.Add(keyValuePair);
                                        }
                                        break;

                                    case IEventParameter eventParameter:
                                        parameters.AddRange(eventParameter.GetValues(args));
                                        break;

                                    default:
                                        if (parameter.ToBinding() is Binding defaultDinding)
                                        {
                                            TargetObject.Bind(ParameterProperty, defaultDinding);
                                            parameters.Add((dynamic)TargetObject.GetValue(ParameterProperty));
                                        }
                                        break;
                                }
                            }
                        }

                        if (pathBinding?.StringFormat is string format)
                        {
                            path = string.Format(format, path);
                        }

                        mediator.Send(new Navigate(path, parameters.ToArray()));
                    }
                }
            }

            base.OnInvoked(sender, args);
        }
    }
}