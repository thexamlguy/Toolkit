using FluentAvalonia.UI.Controls;
using Kromek.Framework.Core.Extensions;
using Toolkit.Foundation.Avalonia;

namespace Kromek.Framework.Avalonia
{
    public class ContentDialogHandler : NavigationRouteHandler<ContentDialog>
    {
        public override async void Receive(NavigationRouteRequest<ContentDialog> message)
        {
            if (message.Template is ContentDialog contentDialog)
            {
                async void HandleButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
                {
                    ContentDialogButtonClickDeferral defferal = args.GetDeferral();

                    if (sender.DataContext is INavigationConfirmationAsync confirmationAsync)
                    {
                        if (!await confirmationAsync.CanConfirmAsync())
                        {
                            args.Cancel = true;
                        }
                    }

                    if (sender.DataContext is INavigationConfirmation confirmation)
                    {
                        if (!confirmation.CanConfirm())
                        {
                            args.Cancel = true;
                        }
                    }

                    if (!args.Cancel)
                    {
                        contentDialog.SecondaryButtonClick -= HandleButtonClick;
                        contentDialog.PrimaryButtonClick -= HandleButtonClick;
                        contentDialog.CloseButtonClick -= HandleButtonClick;
                    }

                    defferal.Complete();
                }

                contentDialog.SecondaryButtonClick += HandleButtonClick;
                contentDialog.PrimaryButtonClick += HandleButtonClick;
                contentDialog.CloseButtonClick += HandleButtonClick;

                contentDialog.DataContext = message.Data;
                await contentDialog.ShowAsync();

                message.Reply(true);
            }
        }
    }
}
