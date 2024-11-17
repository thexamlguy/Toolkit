using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Toolkit.Foundation;

namespace Toolkit.WinUI;

public class NavigationRegion(INavigationRegionCollection collection) :
    INavigationRegion
{
    public void Register(string name,
        object target)
    {
        if (target is Control control)
        {
            if (!collection.ContainsKey(name))
            {
                collection.Add(name, control);
                void HandleUnloaded(object? sender, RoutedEventArgs args)
                {
                    control.Unloaded -= HandleUnloaded;
                    collection.Remove(name);
                }

                control.Unloaded += HandleUnloaded;
            }
        }
    }
}
