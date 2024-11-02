using Windows.Win32.Foundation;

namespace Toolkit.Windows;

internal static class RectExtensions
{
    internal static Rect ToRect(this RECT rect) =>
        rect.right - rect.left < 0 || rect.bottom - rect.top < 0
            ? new Rect(rect.left, rect.top, 0, 0)
            : new Rect(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
}
