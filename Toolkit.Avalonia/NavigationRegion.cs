using Avalonia.Controls;
using Avalonia.Interactivity;
using Toolkit.Foundation;

namespace Toolkit.Avalonia;

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