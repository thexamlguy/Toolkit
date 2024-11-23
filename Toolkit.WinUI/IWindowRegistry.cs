using Microsoft.UI.Xaml;
using System.Diagnostics.CodeAnalysis;

namespace Toolkit.WinUI;

public interface IWindowRegistry :
    IDisposable
{
    void Add(Window window);

    bool TryGet<TWindow>([DisallowNull] out TWindow? window)
        where TWindow : Window;
}
