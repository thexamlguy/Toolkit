using Microsoft.UI.Xaml;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Disposables;
using Toolkit.Foundation;

namespace Toolkit.WinUI;

public class WindowRegistry(IDisposer disposer) : 
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

            disposer.Add(this, Disposable.Create(() =>
            {
                windows.Remove(window);
                window.Close();
            }));

            window.Closed += OnWindowClosed;
        }
    }

    public void Dispose()
    {
        disposer.Dispose(this);
        GC.SuppressFinalize(this);
    }

    public bool TryGet<TWindow>([DisallowNull] out TWindow? window)
        where TWindow : Window
    {

        window = windows.OfType<TWindow>().FirstOrDefault() ?? null;
        return window is not null;
    }
}
