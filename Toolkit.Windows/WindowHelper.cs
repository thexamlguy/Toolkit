using System.Diagnostics.CodeAnalysis;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;

namespace Toolkit.Windows;

public class WindowHelper
{
    public static IntPtr GetHandle(string windowName) => PInvoke.FindWindow(windowName, null);

    public static uint GetDpi(IntPtr handle) => PInvoke.GetDpiForWindow((HWND)handle);

    public static void BringToForeground(HWND handle)
    {
        if (TryGetBoundsUnsafe(handle, out RECT bounds))
        {
            PInvoke.SetWindowPos(handle, new HWND(), bounds.left, bounds.top, bounds.right - 
                bounds.left, bounds.bottom - bounds.top, SET_WINDOW_POS_FLAGS.SWP_SHOWWINDOW | SET_WINDOW_POS_FLAGS.SWP_NOACTIVATE);
        }
    }

    public static IntPtr Find(string windowName) => 
        PInvoke.FindWindow(windowName, null);

    public static IntPtr Find(string windowName, IntPtr parentHandle) => 
        PInvoke.FindWindowEx(new HWND(parentHandle), new HWND(), windowName, null);

    public static void MoveAndResize(HWND handle, int x, int y, int width, int height) => 
        PInvoke.SetWindowPos(handle, new HWND(), x, y, width, height, 0);

    public static bool TryGetBounds(IntPtr handle,
        [MaybeNullWhen(false)] out Rect rect)
    {
        if (TryGetBoundsUnsafe(handle, out RECT unsafeRect))
        {
            rect = new Rect(unsafeRect.left, unsafeRect.top, unsafeRect.right - unsafeRect.left, unsafeRect.bottom - unsafeRect.top);

            return true;
        }

        rect = null;
        return false;
    }

    private static unsafe bool TryGetBoundsUnsafe(IntPtr handle, out RECT rect)
    {
        fixed (RECT* lpRectLocal = &rect)
        {
            return PInvoke.GetWindowRect(new HWND(handle), lpRectLocal);
        }
    }
}
