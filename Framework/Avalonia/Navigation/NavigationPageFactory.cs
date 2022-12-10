using Avalonia.Controls;
using FluentAvalonia.UI.Controls;

namespace Toolkit.Framework.Avalonia;

internal class NavigationPageFactory : INavigationPageFactory
{
    public IControl? GetPage(Type srcType)
    {
        return default;
    }

    public IControl GetPageFromObject(object target)
    {
        return (IControl)target;
    }
}