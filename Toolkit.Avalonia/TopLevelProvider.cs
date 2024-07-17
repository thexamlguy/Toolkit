using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace Toolkit.Avalonia;

public class TopLevelProvider :
    ITopLevelProvider
{
    public TopLevel? Get()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime classicDesktopStyleApplication)
        {
            if (TopLevel.GetTopLevel(classicDesktopStyleApplication.MainWindow) is TopLevel topLevel)
            {
                return topLevel;
            }
        }

        if (Application.Current?.ApplicationLifetime is ISingleViewApplicationLifetime singleViewApplication)
        {
            if (TopLevel.GetTopLevel(singleViewApplication.MainView) is TopLevel topLevel)
            {
                return topLevel;
            }
        }

        return default;
    }
}