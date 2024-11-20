using Avalonia.Controls;
using FluentAvalonia.Core;
using Toolkit.Foundation;
using Toolkit.UI.Controls.Avalonia;

namespace Toolkit.Avalonia;

public class TaskDialogHandler(ITopLevelProvider topLevelProvider) :
    IHandler<NavigateTemplateEventArgs>
{
    public async void Handle(NavigateTemplateEventArgs args)
    {
        if (args.Template is TaskDialog dialog)
        {
            if (topLevelProvider.Get() is TopLevel topLevel)
            {
                dialog.XamlRoot = topLevel;
                dialog.DataContext = args.Content;

                async void HandleClosing(FluentAvalonia.UI.Controls.TaskDialog sender, 
                    FluentAvalonia.UI.Controls.TaskDialogClosingEventArgs args)
                {
                    dialog.Closing -= HandleClosing;
                    if (dialog.DataContext is object content)
                    {
                        if (args.Result is TaskDialogResult result)
                        {
                            if (result is TaskDialogResult.OK && content is 
                                IAsyncPrimaryConfirmation primaryConfirmation)
                            {
                                Deferral deferral = args.GetDeferral();
                                if (!await primaryConfirmation.ConfirmPrimary())
                                {
                                    args.Cancel = true;
                                    dialog.Closing += HandleClosing;
                                }

                                deferral.Complete();
                            }
                        }
                    }
                }

                dialog.Closing += HandleClosing;
                await dialog.ShowAsync();
            }
        }
    }
}
