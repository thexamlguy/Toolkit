using FluentAvalonia.Core;
using Toolkit.Foundation;
using Toolkit.UI.Controls.Avalonia;

namespace Toolkit.Avalonia;

public class ContentDialogHandler :
    IHandler<NavigateTemplateEventArgs>
{
    public async void Handle(NavigateTemplateEventArgs args)
    {
        if (args.Template is ContentDialog dialog)
        {
            dialog.DataContext = args.Content;

            async void HandlePrimaryButtonClick(FluentAvalonia.UI.Controls.ContentDialog sender,
                FluentAvalonia.UI.Controls.ContentDialogButtonClickEventArgs args)
            {
                dialog.PrimaryButtonClick -= HandlePrimaryButtonClick;
                if (dialog.DataContext is object content)
                {
                    if (content is IPrimaryConfirmation primaryConfirmation)
                    {
                        Deferral deferral = args.GetDeferral();
                        if (!await primaryConfirmation.ConfirmPrimary())
                        {
                            args.Cancel = true;
                            dialog.PrimaryButtonClick += HandlePrimaryButtonClick;
                        }

                        deferral.Complete();
                    }
                }
            }

            async void HandleSecondaryButtonClick(FluentAvalonia.UI.Controls.ContentDialog sender,
                FluentAvalonia.UI.Controls.ContentDialogButtonClickEventArgs args)
            {
                dialog.SecondaryButtonClick -= HandleSecondaryButtonClick;
                if (dialog.DataContext is object content)
                {
                    if (content is ISecondaryConfirmation secondaryConfirmation)
                    {
                        Deferral deferral = args.GetDeferral();
                        if (!await secondaryConfirmation.ConfirmSecondary())
                        {
                            args.Cancel = true;
                            dialog.SecondaryButtonClick += HandleSecondaryButtonClick;
                        }

                        deferral.Complete();
                    }
                }
            }

            async void HandleClosing(FluentAvalonia.UI.Controls.ContentDialog sender,
                 FluentAvalonia.UI.Controls.ContentDialogClosingEventArgs args)
            {
                if (args.Result is FluentAvalonia.UI.Controls.ContentDialogResult.Primary ||
                    args.Result is FluentAvalonia.UI.Controls.ContentDialogResult.Secondary)
                {
                    dialog.Closing -= HandleClosing;
                    if (dialog.DataContext is object content)
                    {
                        bool cancelled = false;
                        if (content is IConfirmation confirmation)
                        {
                            Deferral deferral = args.GetDeferral();
                            if (!await confirmation.Confirm())
                            {
                                args.Cancel = true;
                                cancelled = true;

                                dialog.Closing += HandleClosing;
                            }

                            deferral.Complete();
                        }
                    }
                }
            }

            void HandleOpened(FluentAvalonia.UI.Controls.ContentDialog sender,
                EventArgs args)
            {
                dialog.Opened -= HandleOpened;
                if (dialog.DataContext is object content)
                {
                    if (content is IActivation activation)
                    {
                        activation.IsActive = true;
                    }
                }
            }

            void HandleClosed(FluentAvalonia.UI.Controls.ContentDialog sender,
                FluentAvalonia.UI.Controls.ContentDialogClosedEventArgs args)
            {
                dialog.Closed -= HandleClosed;
                if (dialog.DataContext is object content)
                {
                    if (content is IActivation activation)
                    {
                        activation.IsActive = false;
                    }
                }
            }

            dialog.Opened += HandleOpened;
            dialog.Closing += HandleClosing;
            dialog.Closed += HandleClosed;

            dialog.PrimaryButtonClick += HandlePrimaryButtonClick;
            dialog.SecondaryButtonClick += HandleSecondaryButtonClick;

            await dialog.ShowAsync();

            dialog.PrimaryButtonClick += HandlePrimaryButtonClick;
            dialog.SecondaryButtonClick += HandleSecondaryButtonClick;
        }
    }
}