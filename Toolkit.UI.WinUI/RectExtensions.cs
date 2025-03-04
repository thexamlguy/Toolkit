using Rect = Windows.Foundation.Rect;

namespace Toolkit.UI.WinUI;

public static class RectExtensions
{
    public static Rect ToWindowsRect(this Windows.Rect rect) => new(rect.X, rect.Y, rect.Width, rect.Height);
}
