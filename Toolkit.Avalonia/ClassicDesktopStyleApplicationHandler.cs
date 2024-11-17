using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Toolkit.Foundation;

namespace Toolkit.Avalonia;

public class ClassicDesktopStyleApplicationHandler :
    IHandler<NavigateTemplateEventArgs>
{
    public void Handle(NavigateTemplateEventArgs args)
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
    }
}