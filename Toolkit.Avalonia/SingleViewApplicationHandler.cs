using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Toolkit.Foundation;

namespace Toolkit.Avalonia;

public class SingleViewApplicationHandler :
    IHandler<NavigateEventArgs<ISingleViewApplicationLifetime>>
{
    public void Handle(NavigateEventArgs<ISingleViewApplicationLifetime> args)
    {
        if (Application.Current?.ApplicationLifetime is
            ISingleViewApplicationLifetime lifeTime)
        {
            if (args.Template is Control control)
            {
                lifeTime.MainView = control;
                control.DataContext = args.Content;
            }
        }
    }
}