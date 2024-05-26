using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using FluentAvalonia.UI.Media.Animation;
using FluentAvalonia.UI.Navigation;
using Toolkit.Foundation;
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
                void NavigatingFrom(Control sender)
                {
                    async void HandleNavigatingFrom(object? _,
                        NavigatingCancelEventArgs args)
                    {
                        sender.RemoveHandler(Frame.NavigatingFromEvent, HandleNavigatingFrom);
                        NavigatedFrom(sender);

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
                }

                void NavigatedFrom(Control sender)
                {
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
                }

                void NavigatedTo(Control sender)
                {
                    async void HandleNavigatedTo(object? _,
                        NavigationEventArgs __)
                    {
                        sender.RemoveHandler(Frame.NavigatedToEvent, HandleNavigatedTo);
                        NavigatingFrom(sender);

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
                    }

                    sender.AddHandler(Frame.NavigatedToEvent, HandleNavigatedTo);
                }

                control.DataContext = args.Content;
                NavigatedTo(control);
                frame.NavigateFromObject(control, new FrameNavigationOptions { TransitionInfoOverride = new SuppressNavigationTransitionInfo() });
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