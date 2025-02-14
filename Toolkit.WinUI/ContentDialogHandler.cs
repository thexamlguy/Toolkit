﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Toolkit.Foundation;

namespace Toolkit.WinUI;

public class ContentDialogHandler :
    IHandler<NavigateTemplateEventArgs>
{
    public async void Handle(NavigateTemplateEventArgs args)
    {
        if (args.Template is not ContentDialog dialog)
        {
            return;
        }

        if (args.Sender is not Control parent)
        {
            return;
        }

        dialog.XamlRoot = parent.XamlRoot;
        dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;

        dialog.DataContext = args.Content;

        async void HandlePrimaryButtonClick(ContentDialog sender,
            ContentDialogButtonClickEventArgs args)
        {
            dialog.PrimaryButtonClick -= HandlePrimaryButtonClick;
            if (dialog.DataContext is object content)
            {
                if (content is IAsyncPrimaryConfirmation primaryConfirmation)
                {
                    ContentDialogButtonClickDeferral deferral = args.GetDeferral();
                    if (!await primaryConfirmation.ConfirmPrimary())
                    {
                        args.Cancel = true;
                        dialog.PrimaryButtonClick += HandlePrimaryButtonClick;
                    }

                    deferral.Complete();
                }
            }
        }

        async void HandleSecondaryButtonClick(ContentDialog sender,
            ContentDialogButtonClickEventArgs args)
        {
            dialog.SecondaryButtonClick -= HandleSecondaryButtonClick;
            if (dialog.DataContext is object content)
            {
                if (content is IAsyncSecondaryConfirmation secondaryConfirmation)
                {
                    ContentDialogButtonClickDeferral deferral = args.GetDeferral();
                    if (!await secondaryConfirmation.ConfirmSecondary())
                    {
                        args.Cancel = true;
                        dialog.SecondaryButtonClick += HandleSecondaryButtonClick;
                    }

                    deferral.Complete();
                }
            }
        }

        async void HandleClosing(ContentDialog sender,
            ContentDialogClosingEventArgs args)
        {
            if (args.Result is ContentDialogResult.Primary ||
                args.Result is ContentDialogResult.Secondary)
            {
                dialog.Closing -= HandleClosing;
                if (dialog.DataContext is object content)
                {
                    if (content is IAsyncConfirmation confirmation)
                    {
                        ContentDialogClosingDeferral deferral = args.GetDeferral();
                        if (!await confirmation.Confirm())
                        {
                            args.Cancel = true;
                            dialog.Closing += HandleClosing;
                        }

                        deferral.Complete();
                    }
                }
            }
        }

        void HandleOpened(ContentDialog sender,
            ContentDialogOpenedEventArgs args)
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

        void HandleClosed(ContentDialog sender,
            ContentDialogClosedEventArgs args)
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
    }
}