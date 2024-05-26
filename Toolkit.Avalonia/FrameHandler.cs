﻿using Avalonia.Controls;
using Avalonia.Interactivity;
using FluentAvalonia.UI.Media.Animation;
using FluentAvalonia.UI.Navigation;
using Toolkit.Foundation;
using Toolkit.UI.Avalonia;
using Toolkit.UI.Controls.Avalonia;

namespace Toolkit.Avalonia;

public class FrameHandler :
    INavigateHandler<Frame>,
    INavigateBackHandler<Frame>
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
                        sender.RemoveHandler(Frame.NavigatedToEvent, HandleNavigatedTo);

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

                                    if (content is IDisposable disposable)
                                    {
                                        disposable.Dispose();
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
                            if (content is IInitializer initializer)
                            {
                                await initializer.Initialize();
                            }

                            if (content is IActivated activated)
                            {
                                await activated.OnActivated();
                            }
                        }

                        async void HandleUnloaded(object? _, RoutedEventArgs __)
                        {
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
                if (args.Parameters is not null)
                {
                    if (args.Parameters.TryGetValue("Transition", out object? transition))
                    {
                        switch($"{transition}")
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
                    }
                }


                frame.NavigateFromObject(control, navigationOptions);
            }
        }

        return Task.CompletedTask;
    }

    public Task Handle(NavigateBackEventArgs<Frame> args)
    {
        if (args.Context is Frame frame)
        {
            //NavigationTransitionInfo? navigationTransitionInfo = null;
            //if (frame.Content is IBackwardNavigation navigation)
            //{
            //    navigationTransitionInfo = navigation.Transition;
            //}

            frame.GoBack();
        }

        return Task.CompletedTask;
    }
}