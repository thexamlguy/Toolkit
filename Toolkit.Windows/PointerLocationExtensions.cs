namespace Toolkit.Windows;

public static class PointerLocationExtensions
{
    public static bool IsWithinBounds(this PointerLocation args, Rect bounds) => args.X >= bounds.X
            && args.X <= bounds.X + bounds.Width
            && args.Y >= bounds.Y
            && args.Y <= bounds.Y + bounds.Height;
}
