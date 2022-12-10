using FluentAvalonia.UI.Controls;
using Avalonia.Controls;

namespace Toolkit.Foundation.Avalonia;

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
