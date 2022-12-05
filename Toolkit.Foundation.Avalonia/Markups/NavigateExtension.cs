using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Kromek.Framework.Core.Extensions;
using Kromek.Framework.Core;
using System;
using System.Collections.Generic;

namespace Toolkit.Foundation.Avalonia
{
    public class NavigateExtension : TriggerExtension
    {
        private static readonly AttachedProperty<INavigationRouter> NavigationRouterProperty =
            AvaloniaProperty.RegisterAttached<NavigateExtension, Control, INavigationRouter>("NavigationRouter");

        private static readonly AttachedProperty<object> ParameterProperty =
            AvaloniaProperty.RegisterAttached<NavigateExtension, Control, object>("Parameter");

        private static readonly AvaloniaProperty RouteProperty =
            AvaloniaProperty.RegisterAttached<NavigateExtension, Control, object>("Route");

        private static readonly AvaloniaProperty ToProperty =
            AvaloniaProperty.RegisterAttached<NavigateExtension, Control, object>("To");

        private readonly Binding? navigationRouterBinding;
        private readonly List<object?> parameters = new();
        private readonly Binding? toBinding;
        private object? route;
        private Binding? routeBinding;

        public NavigateExtension(object navigationRouter,
            object to)
        {
            navigationRouterBinding = navigationRouter.ToBinding();
            this.toBinding = to is Binding toBinding ? toBinding : to.ToBinding();
        }

        public NavigateExtension(object navigationRouter,
            object to,
            object args1)
        {
            navigationRouterBinding = navigationRouter.ToBinding();
            this.toBinding = to is Binding toBinding ? toBinding : to.ToBinding();

            parameters.Add(args1 is MarkupExtension ? args1 : args1.ToBinding());
        }

        public NavigateExtension(object navigationRouter,
            object to,
            object args1,
            object args2)
        {
            navigationRouterBinding = navigationRouter.ToBinding();
            this.toBinding = to is Binding toBinding ? toBinding : to.ToBinding();

            parameters.Add(args1 is MarkupExtension ? args1 : args1.ToBinding());
            parameters.Add(args2 is MarkupExtension ? args2 : args2.ToBinding());
        }

        public NavigateExtension(object navigationRouter,
            object to,
            object args1,
            object args2,
            object args3)
        {
            navigationRouterBinding = navigationRouter.ToBinding();
            this.toBinding = to is Binding toBinding ? toBinding : to.ToBinding();

            parameters.Add(args1 is MarkupExtension ? args1 : args1.ToBinding());
            parameters.Add(args2 is MarkupExtension ? args2 : args2.ToBinding());
            parameters.Add(args3 is MarkupExtension ? args3 : args3.ToBinding());
        }

        public NavigateExtension(object navigationRouter,
            object to,
            object args1,
            object args2,
            object args3,
            object args4)
        {
            navigationRouterBinding = navigationRouter.ToBinding();
            this.toBinding = to is Binding toBinding ? toBinding : to.ToBinding();

            parameters.Add(args1 is MarkupExtension ? args1 : args1.ToBinding());
            parameters.Add(args2 is MarkupExtension ? args2 : args2.ToBinding());
            parameters.Add(args3 is MarkupExtension ? args3 : args3.ToBinding());
            parameters.Add(args4 is MarkupExtension ? args4 : args4.ToBinding());
        }

        public NavigateExtension(object navigationRouter,
            object to,
            object args1,
            object args2,
            object args3,
            object args4,
            object args5)
        {
            navigationRouterBinding = navigationRouter.ToBinding();
            this.toBinding = to is Binding toBinding ? toBinding : to.ToBinding();

            parameters.Add(args1 is MarkupExtension ? args1 : args1.ToBinding());
            parameters.Add(args2 is MarkupExtension ? args2 : args2.ToBinding());
            parameters.Add(args3 is MarkupExtension ? args3 : args3.ToBinding());
            parameters.Add(args4 is MarkupExtension ? args4 : args4.ToBinding());
            parameters.Add(args5 is MarkupExtension ? args5 : args5.ToBinding());
        }

        public NavigateExtension(object navigationRouter,
            object to,
            object args1,
            object args2,
            object args3,
            object args4,
            object args5,
            object args6)
        {
            navigationRouterBinding = navigationRouter.ToBinding();
            this.toBinding = to is Binding toBinding ? toBinding : to.ToBinding();

            parameters.Add(args1 is MarkupExtension ? args1 : args1.ToBinding());
            parameters.Add(args2 is MarkupExtension ? args2 : args2.ToBinding());
            parameters.Add(args3 is MarkupExtension ? args3 : args3.ToBinding());
            parameters.Add(args4 is MarkupExtension ? args4 : args4.ToBinding());
            parameters.Add(args5 is MarkupExtension ? args5 : args5.ToBinding());
            parameters.Add(args6 is MarkupExtension ? args6 : args6.ToBinding());
        }

        public NavigateExtension(object navigationRouter,
            object to,
            object args1,
            object args2,
            object args3,
            object args4,
            object args5,
            object args6,
            object args7)
        {
            navigationRouterBinding = navigationRouter.ToBinding();
            this.toBinding = to is Binding toBinding ? toBinding : to.ToBinding();

            parameters.Add(args1 is MarkupExtension ? args1 : args1.ToBinding());
            parameters.Add(args2 is MarkupExtension ? args2 : args2.ToBinding());
            parameters.Add(args3 is MarkupExtension ? args3 : args3.ToBinding());
            parameters.Add(args4 is MarkupExtension ? args4 : args4.ToBinding());
            parameters.Add(args5 is MarkupExtension ? args5 : args5.ToBinding());
            parameters.Add(args6 is MarkupExtension ? args6 : args6.ToBinding());
            parameters.Add(args7 is MarkupExtension ? args7 : args7.ToBinding());
        }

        public NavigateExtension(object navigationRouter,
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
            navigationRouterBinding = navigationRouter.ToBinding();
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

        public NavigateExtension(object navigationRouter,
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
            navigationRouterBinding = navigationRouter.ToBinding();
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

        public NavigateExtension(object navigationRouter,
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
            navigationRouterBinding = navigationRouter.ToBinding();
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

        public NavigateExtension(object navigationRouter,
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
            navigationRouterBinding = navigationRouter.ToBinding();
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

        public NavigateExtension(object navigationRouter,
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
            navigationRouterBinding = navigationRouter.ToBinding();
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

        public NavigateExtension(object navigationRouter,
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
            navigationRouterBinding = navigationRouter.ToBinding();
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

        public NavigateExtension(object navigationRouter,
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
            navigationRouterBinding = navigationRouter.ToBinding();
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

        public NavigateExtension(object navigationRouter,
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
            navigationRouterBinding = navigationRouter.ToBinding();
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

        public object? Route
        {
            get
            {
                return route;
            }
            set
            {
                route = value;
                if (route is not null)
                {
                    routeBinding = route.ToBinding();
                }
            }

        }

        protected override void OnInvoked(object sender, EventArgs args)
        {
            if (TargetObject is not null)
            {
                if (navigationRouterBinding is not null)
                {
                    TargetObject.Bind(NavigationRouterProperty, navigationRouterBinding);
                    if (TargetObject.GetValue(NavigationRouterProperty) is INavigationRouter router)
                    {
                        if (toBinding is not null)
                        {
                            TargetObject.Bind(ToProperty, toBinding);
                            if (TargetObject.GetValue(ToProperty) is { } to)
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

                                object? route = null;
                                if (routeBinding is not null)
                                {
                                    TargetObject.Bind(RouteProperty, routeBinding);
                                    route = TargetObject.GetValue(RouteProperty);
                                }

                                if (to is string name)
                                {
                                    if (toBinding?.StringFormat is string format)
                                    {
                                        name = string.Format(format, name);
                                    }

                                    router.Navigate(new Navigate(name, parameters.ToArray()) { Route = route });
                                }

                                if (to is Type type)
                                {
                                    router.Navigate(new Navigate(type, parameters.ToArray()) { Route = route });
                                }
                            }
                        }

                    }

                    base.OnInvoked(sender, args);
                }
            }
        }
    }
}