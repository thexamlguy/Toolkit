using Avalonia.Controls;
using FluentAvalonia.Core;
using Toolkit.Foundation;
using Toolkit.UI.Controls.Avalonia;

namespace Toolkit.Avalonia;

public class TaskDialogHandler(ITopLevelProvider topLevelProvider) :
    IHandler<NavigateEventArgs<TaskDialog>>
{
    public async void Handle(NavigateEventArgs<TaskDialog> args)
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
                        bool cancelled = false;

                        if (args.Result is TaskDialogResult result)
                        {
                            if (result is TaskDialogResult.OK && content is 
                                IPrimaryConfirmation primaryConfirmation)
                            {
                                Deferral deferral = args.GetDeferral();
                                if (!await primaryConfirmation.ConfirmPrimary())
                                {
                                    args.Cancel = true;
                                    cancelled = true;

                                    dialog.Closing += HandleClosing;
                                }

                                deferral.Complete();
                            }
                        }

                        if (!cancelled)
                        {
                            //if (content is IDeactivating deactivating)
                            //{
                            //    await deactivating.OnDeactivating();
                            //}
                        }
                    }
                }

                dialog.Closing += HandleClosing;
                await dialog.ShowAsync();
            }
        }
    }
}
