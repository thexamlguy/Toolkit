using Avalonia.Controls;
using Avalonia.Interactivity;
using FluentAvalonia.UI.Media.Animation;
using FluentAvalonia.UI.Navigation;
using Toolkit.Foundation;
using Toolkit.UI.Controls.Avalonia;

namespace Toolkit.Avalonia;

public class FrameHandler(ITransientNavigationStore<Frame> navigationStore) :
    IHandler<NavigateTemplateEventArgs>,
    IHandler<NavigateBackEventArgs<Frame>>
{
    public void Handle(NavigateTemplateEventArgs args)
    {
        if (args.Region is not Frame frame)
        {
            return;
        }

        if (args.Template is not Control control)
        {
            return;
        }

        frame.NavigationPageFactory ??= new NavigationPageFactory();

        void Navigated(Control sender)
        {
            void HandleNavigatedTo(object? _, NavigationEventArgs __)
            {
                async void HandleNavigatingFrom(object? _, NavigatingCancelEventArgs args)
                {
                    sender.RemoveHandler(Frame.NavigatingFromEvent, HandleNavigatingFrom);
                    control.Unloaded -= HandleUnloaded;

                    void HandleNavigatedFrom(object? _, NavigationEventArgs args)
                    {
                        sender.RemoveHandler(Frame.NavigatedFromEvent, HandleNavigatedFrom);
                        if (sender.DataContext is object content)
                        {
                            if (content is IActivation activation)
                            {
                                activation.IsActive = false;
                            }

                            if (content is IDisposable disposable)
                            {
                                FrameNavigationOptions? options = navigationStore.Get<FrameNavigationOptions>(frame);
                                if (options is not FrameNavigationOptions frameOptions ||
                                    !frameOptions.IsNavigationStackEnabled)
                                {
                                    disposable.Dispose();
                                }
                            }
                        }
                    }

                    sender.AddHandler(Frame.NavigatedFromEvent, HandleNavigatedFrom);

                    if (sender.DataContext is object content)
                    {
                        if (content is IAsyncConfirmation confirmation &&
                            !await confirmation.Confirm())
                        {
                            args.Cancel = true;
                        }
                    }
                }

                sender.AddHandler(Frame.NavigatingFromEvent, HandleNavigatingFrom);
                if (sender.DataContext is object content)
                {
                    if (content is IActivation activation)
                    {
                        activation.IsActive = true;
                    }
                }

                void HandleUnloaded(object? _, RoutedEventArgs __)
                {
                    sender.RemoveHandler(Frame.NavigatedToEvent, HandleNavigatedTo);
                    sender.RemoveHandler(Frame.NavigatingFromEvent, HandleNavigatingFrom);

                    control.Unloaded -= HandleUnloaded;

                    if (control.DataContext is object content)
                    {
                        if (content is IActivation activation)
                        {
                            activation.IsActive = true;
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

        FrameNavigationOptions navigationOptions = new();
        List<Action> postNavigateActions = [];

        void CleanUp(int start, int end)
        {
            int startIndex = Math.Max(0, start - 1);
            int endIndex = Math.Min(frame.BackStack.Count - 1, end - 1);

            for (int i = endIndex; i >= startIndex; i--)
            {
                PageStackEntry? entry = frame.BackStack[i];
                if (entry.Context is Control control)
                {
                    if (control.DataContext is IDisposable disposable)
                    {
                        disposable.Dispose();
                    }
                }

                frame.BackStack.RemoveAt(i);
            }
        }

        if (args.Parameters is not null)
        {
            if (args.Parameters.TryGetValue("Transition", out object? transition))
            {
                switch ($"{transition}")
                {
                    case "Suppress":
                        navigationOptions.TransitionInfoOverride = new SuppressNavigationTransitionInfo();
                        break;

                    case "FromLeft":
                    case "FromRight":
                    case "FromTop":
                    case "FromBottom":
                        navigationOptions.TransitionInfoOverride = new SlideNavigationTransitionInfo
                        {
                            Effect = Enum.Parse<SlideNavigationTransitionEffect>($"{transition}")
                        };
                        break;
                }
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
                if (clearBackStack is bool clearBool)
                {
                    if (clearBool)
                    {
                        postNavigateActions.Add(() => CleanUp(1, frame.BackStack.Count));
                    }
                }

                if (clearBackStack is string clearString)
                {
                    if (clearString.StartsWith('[') && clearString.EndsWith(']') && clearString.Contains('-'))
                    {
                        string range = clearString.Trim('[', ']');
                        string[] parts = range.Split('-');

                        if (parts.Length == 2 && int.TryParse(parts[0], out int start) &&
                            int.TryParse(parts[1], out int end))
                        {
                            postNavigateActions.Add(() => CleanUp(start, end));
                        }
                        else
                        {
                            postNavigateActions.Add(() => CleanUp(1, frame.BackStack.Count));
                        }
                    }
                    else
                    {
                        postNavigateActions.Add(() => CleanUp(1, frame.BackStack.Count));
                    }
                }
            }
        }

        Navigated(control);

        navigationStore.Set(frame, navigationOptions);
        frame.NavigateFromObject(control, navigationOptions);

        foreach (Action postAction in postNavigateActions)
        {
            postAction.Invoke();
        }
    }

    public void Handle(NavigateBackEventArgs<Frame> args)
    {
        if (args.Context is Frame frame)
        {
            frame.GoBack();
        }
    }
}