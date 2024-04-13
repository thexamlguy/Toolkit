using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using System.Reflection;
using Toolkit.Foundation;

namespace Toolkit.Avalonia;

public class NavigationContext(INavigationContextCollection contexts) :
    INavigationContext
{
    public void Set(Control control)
    {
        if (control.GetType().GetCustomAttributes<NavigationTargetAttribute>()
            is IEnumerable<NavigationTargetAttribute> attributes)
        {
            foreach (NavigationTargetAttribute attribute in attributes)
            {
                if (!contexts.ContainsKey(attribute.Name))
                {
                    if (control.Find<TemplatedControl>(attribute.Name) is TemplatedControl content)
                    {
                        contexts.Add(attribute.Name, content);
                        void HandleUnloaded(object? sender, RoutedEventArgs args)
                        {
                            control.Unloaded -= HandleUnloaded;
                            contexts.Remove(attribute.Name);
                        }

                        control.Unloaded += HandleUnloaded;
                    }
                }
            }
        }
    }
}

