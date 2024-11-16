using Microsoft.UI.Xaml;
using System.Diagnostics.CodeAnalysis;

namespace Toolkit.WinUI;

public class WindowRegistry : 
    IWindowRegistry
{
    private readonly List<Window> windows = [];

    public void Add(Window window)
    {
        if (!windows.Contains(window))
        {
            void OnWindowClosed(object sender, WindowEventArgs args)
            {
                window.Closed -= OnWindowClosed;
                windows.Remove(window);
            }

            windows.Add(window);
            window.Closed += OnWindowClosed;
        }
    }

    public bool TryGet<TWindow>([DisallowNull] out TWindow? window)
        where TWindow : Window
    {
        window = windows.OfType<TWindow>().FirstOrDefault() ?? null;
        return window is not null;
    }
}
