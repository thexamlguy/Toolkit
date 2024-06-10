using FluentAvalonia.Core;
using Toolkit.Foundation;
using Toolkit.UI.Controls.Avalonia;

namespace Toolkit.Avalonia;

public class ContentDialogHandler(IDispatcher dispatcher) :
    INavigateHandler<ContentDialog>
{
    public async Task Handle(NavigateEventArgs<ContentDialog> args)
    {
        if (args.Region is ContentDialog contentDialog)
        {
            contentDialog.DataContext = args.Content;

            async void HandlePrimaryButtonClick(FluentAvalonia.UI.Controls.ContentDialog sender,
                FluentAvalonia.UI.Controls.ContentDialogButtonClickEventArgs args)
            {
                contentDialog.PrimaryButtonClick -= HandlePrimaryButtonClick;
                if (contentDialog.DataContext is object content)
                {
                    if (content is IPrimaryConfirmation primaryConfirmation)
                    {
                        List<Action> postActions = [];
                        if (content is IActivityIndicator activityIndicator)
                        {
                            activityIndicator.Active = true;
                            postActions.Add(() => activityIndicator.Active = false);
                        }

                        Deferral deferral = args.GetDeferral();
                        if (!await primaryConfirmation.Confirm())
                        {
                            args.Cancel = true;
                            contentDialog.PrimaryButtonClick += HandlePrimaryButtonClick;
                        }

                        deferral.Complete();
                        foreach (Action action in postActions)
                        {
                            action.Invoke();
                        }
                    }
                }
            }

            async void HandleSecondaryButtonClick(FluentAvalonia.UI.Controls.ContentDialog sender,
                FluentAvalonia.UI.Controls.ContentDialogButtonClickEventArgs args)
            {
                contentDialog.SecondaryButtonClick -= HandleSecondaryButtonClick;
                if (contentDialog.DataContext is object content)
                {
                    if (content is ISecondaryConfirmation secondaryConfirmation)
                    {
                        List<Action> postActions = [];
                        if (content is IActivityIndicator activityIndicator)
                        {
                            activityIndicator.Active = true;
                            postActions.Add(() => activityIndicator.Active = false);
                        }

                        Deferral deferral = args.GetDeferral();
                        if (!await secondaryConfirmation.Confirm())
                        {
                            args.Cancel = true;
                            contentDialog.SecondaryButtonClick += HandleSecondaryButtonClick;
                        }

                        deferral.Complete();
                        foreach (Action action in postActions)
                        {
                            action.Invoke();
                        }
                    }
                }
            }

            async void HandleClosing(FluentAvalonia.UI.Controls.ContentDialog sender,
                 FluentAvalonia.UI.Controls.ContentDialogClosingEventArgs args)
            {
                if (args.Result == FluentAvalonia.UI.Controls.ContentDialogResult.Primary ||
                    args.Result == FluentAvalonia.UI.Controls.ContentDialogResult.Secondary)
                {
                    contentDialog.Closing -= HandleClosing;
                    if (contentDialog.DataContext is object content)
                    {
                        if (content is IConfirmation confirmation)
                        {
                            List<Action> postActions = [];
                            if (content is IActivityIndicator activityIndicator)
                            {
                                activityIndicator.Active = true;
                                postActions.Add(() => activityIndicator.Active = false);
                            }

                            Deferral deferral = args.GetDeferral();
                            if (!await confirmation.Confirm())
                            {
                                args.Cancel = true;
                                contentDialog.Closing += HandleClosing;
                            }

                            deferral.Complete();
                            foreach (Action action in postActions)
                            {
                                action.Invoke();
                            }
                        }
                    }
                }
            }

            async void HandleOpened(FluentAvalonia.UI.Controls.ContentDialog sender,
                EventArgs args)
            {
                contentDialog.Opened -= HandleOpened;
                if (contentDialog.DataContext is object content)
                {
                    if (content is IDeactivatable deactivatable)
                    {
                        async void DeactivateHandler(object? sender, EventArgs args)
                        {
                            deactivatable.DeactivateHandler -= DeactivateHandler;
                            await dispatcher.Invoke(contentDialog.Hide);
                        }

                        deactivatable.DeactivateHandler += DeactivateHandler;
                    }

                    // A hack to wait for the dialog to finish loading up to make it appear more responsive
                    await Task.Delay(250);
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

            contentDialog.Opened += HandleOpened;
            contentDialog.Closing += HandleClosing;
            contentDialog.PrimaryButtonClick += HandlePrimaryButtonClick;
            contentDialog.SecondaryButtonClick += HandleSecondaryButtonClick;

            await contentDialog.ShowAsync();

            contentDialog.PrimaryButtonClick += HandlePrimaryButtonClick;
            contentDialog.SecondaryButtonClick += HandleSecondaryButtonClick;
        }
    }
}