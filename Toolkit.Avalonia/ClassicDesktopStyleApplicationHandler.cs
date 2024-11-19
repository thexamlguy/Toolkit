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
        if (Application.Current?.ApplicationLifetime 
            is not IClassicDesktopStyleApplicationLifetime lifeTime)
        {
            return;
        }

        if (args.Template is not Window window)
        {
            return;
        }

        lifeTime.MainWindow = window;
        window.DataContext = args.Content;
    }
}