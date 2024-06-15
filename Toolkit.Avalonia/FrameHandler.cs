using Avalonia.Controls;
using Avalonia.Interactivity;
using FluentAvalonia.UI.Media.Animation;
using FluentAvalonia.UI.Navigation;
using Toolkit.Foundation;
using Toolkit.UI.Controls.Avalonia;

namespace Toolkit.Avalonia;

public class FrameHandler :
    INotificationHandler<NavigateEventArgs<Frame>>,
    INotificationHandler<NavigateBackEventArgs<Frame>>
{
    public Task Handle(NavigateEventArgs<Frame> args)
    {
        if (args.Region is Frame frame)
        {
            frame.NavigationPageFactory ??= new NavigationPageFactory();
            if (args.Template is Control control)
            {
                void NavigatedTo(Control sender)
                {
                    async void HandleNavigatedTo(object? _,
                        NavigationEventArgs __)
                    {
                        async void HandleNavigatingFrom(object? _,
                            NavigatingCancelEventArgs args)
                        {
                            sender.RemoveHandler(Frame.NavigatingFromEvent, HandleNavigatingFrom);
                            control.Unloaded -= HandleUnloaded;

                            async void HandleNavigatedFrom(object? _,
                                NavigationEventArgs args)
                            {
                                sender.RemoveHandler(Frame.NavigatedFromEvent, HandleNavigatedFrom);
                                if (sender.DataContext is object content)
                                {
                                    if (content is IDeactivated deactivated)
                                    {
                                        await deactivated.OnDeactivated();
                                    }

                                    if (content is not INavigationBackStack)
                                    {
                                        if (content is IDisposable disposable)
                                        {
                                            disposable.Dispose();
                                        }
                                    }
                                }
                            }

                            sender.AddHandler(Frame.NavigatedFromEvent, HandleNavigatedFrom);
                            if (sender.DataContext is object content)
                            {
                                if (content is IConfirmation confirmation &&
                                    !await confirmation.Confirm())
                                {
                                    args.Cancel = true;
                                }

                                if (!args.Cancel)
                                {
                                    if (content is IDeactivating deactivating)
                                    {
                                        await deactivating.OnDeactivating();
                                    }
                                }
                            }
                        }

                        sender.AddHandler(Frame.NavigatingFromEvent, HandleNavigatingFrom);
                        if (sender.DataContext is object content)
                        {
                            if (content is IActivated activated)
                            {
                                await activated.OnActivated();
                            }
                        }

                        async void HandleUnloaded(object? _, RoutedEventArgs __)
                        {
                            sender.RemoveHandler(Frame.NavigatedToEvent, HandleNavigatedTo);
                            sender.RemoveHandler(Frame.NavigatingFromEvent, HandleNavigatingFrom);

                            control.Unloaded -= HandleUnloaded;

                            if (control.DataContext is object content)
                            {
                                if (content is IDeactivated deactivated)
                                {
                                    await deactivated.OnDeactivated();
                                }

                                if (content is IDisposable disposable)
                                {
                                    disposable.Dispose();
                                }
                            }
                        }

                        sender.Unloaded += HandleUnloaded;
                    }

                    sender.AddHandler(Frame.NavigatedToEvent, HandleNavigatedTo);
                }

                control.DataContext = args.Content;
                NavigatedTo(control);

                FrameNavigationOptions navigationOptions = new();
                List<Action> postNavigateActions = [];

                void CleanUp()
                {
                    foreach (PageStackEntry? entry in frame.BackStack)
                    {
                        if (entry.Context is Control control)
                        {
                            if (control.DataContext is object content)
                            {
                                if (content is IDisposable disposable)
                                {
                                    disposable.Dispose();
                                }
                            }
                        }
                    }

                    frame.BackStack.Clear();
                }

                if (args.Parameters is not null)
                {
                    if (args.Parameters.TryGetValue("Transition", out object? transition))
                    {
                        switch ($"{transition}")
                        {
                            case "FromLeft":
                            case "FromRight":
                            case "FromTop":
                            case "FromBottom":
                                navigationOptions.TransitionInfoOverride =
                                    new SlideNavigationTransitionInfo
                                    {
                                        Effect = Enum.Parse<SlideNavigationTransitionEffect>($"{transition}")
                                    };
                                break;
                        }

                        if (args.Parameters.TryGetValue("IsBackStackEnabled", out object? isBackStackEnabled))
                        {
                            if (isBackStackEnabled is bool value)
                            {
                                navigationOptions.IsNavigationStackEnabled = value;
                            }
                        }

                        if (args.Parameters.TryGetValue("ClearBackStack", out object? clearBackStack))
                        {
                            if (clearBackStack is bool value)
                            {
                                if (value)
                                {
                                    postNavigateActions.Add(() => CleanUp());
                                }
                            }
                        }
                    }
                }

                frame.NavigateFromObject(control, navigationOptions);
                foreach (Action postAction in postNavigateActions)
                {
                    postAction.Invoke();
                }
            }
        }

        return Task.CompletedTask;
    }

    public Task Handle(NavigateBackEventArgs<Frame> args)
    {
        if (args.Context is Frame frame)
        {
            frame.GoBack();
        }

        return Task.CompletedTask;
    }
}