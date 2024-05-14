using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Toolkit.Foundation;

namespace Toolkit.Avalonia;

public class ClassicDesktopStyleApplicationHandler :
    INavigateHandler<IClassicDesktopStyleApplicationLifetime>
{
    public Task Handle(NavigateEventArgs<IClassicDesktopStyleApplicationLifetime> args)
    {
        if (Application.Current?.ApplicationLifetime is
            IClassicDesktopStyleApplicationLifetime lifeTime)
        {
            if (args.Template is Window window)
            {
                lifeTime.MainWindow = window;
                window.DataContext = args.Content;
            }
        }

        return Task.CompletedTask;
    }
}