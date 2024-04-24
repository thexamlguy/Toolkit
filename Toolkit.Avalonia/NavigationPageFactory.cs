using Avalonia.Controls;
using FluentAvalonia.UI.Controls;

namespace Toolkit.Avalonia;

public class NavigationPageFactory : 
    INavigationPageFactory
{
    public Control? GetPage(Type srcType) => default;

    public Control GetPageFromObject(object target) => (Control)target;
}
