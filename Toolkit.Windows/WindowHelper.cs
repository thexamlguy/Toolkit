using System.Diagnostics.CodeAnalysis;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;

namespace Toolkit.Windows;

public class WindowHelper
{
    public static IntPtr FindWindow(string name) =>
        PInvoke.FindWindow(name, null);

    public static IntPtr FindWindow(string name, IntPtr handle) =>
        PInvoke.FindWindowEx(new HWND(handle), new HWND(), name, null);

    public static uint GetDpi(IntPtr handle) =>
        PInvoke.GetDpiForWindow((HWND)handle);

    public static IntPtr GetWindowHandle(string name) =>
        PInvoke.FindWindow(name, null);

    public static bool HideWindow(IntPtr hWnd) =>
        PInvoke.ShowWindow(new HWND(hWnd), SHOW_WINDOW_CMD.SW_HIDE);

    public static void MoveAndResizeWindow(HWND handle, int x, int y, int width, int height) =>
        PInvoke.SetWindowPos(handle, new HWND(), x, y, width, height, 0);

    public static bool SetForegroundWindow(HWND handle)
    {
        if (TryGetWindowBoundsUnsafe(handle, out RECT bounds))
        {
            return PInvoke.SetWindowPos(handle, new HWND(), bounds.left, bounds.top, bounds.right -
                bounds.left, bounds.bottom - bounds.top, SET_WINDOW_POS_FLAGS.SWP_SHOWWINDOW | SET_WINDOW_POS_FLAGS.SWP_NOACTIVATE) == 1;
        }

        return false;
    }

    public static bool ShowWindow(IntPtr hWnd) =>
        PInvoke.ShowWindow(new HWND(hWnd), SHOW_WINDOW_CMD.SW_SHOW);

    public static bool TryGetWindowBounds(IntPtr handle,
        [MaybeNullWhen(false)] out Rect rect)
    {
        if (TryGetWindowBoundsUnsafe(handle, out RECT unsafeRect))
        {
            rect = new Rect(unsafeRect.left, unsafeRect.top, unsafeRect.right - unsafeRect.left, unsafeRect.bottom - unsafeRect.top);
            return true;
        }

        rect = null;
        return false;
    }

    private static unsafe bool TryGetWindowBoundsUnsafe(IntPtr handle, out RECT rect)
    {
        fixed (RECT* lpRectLocal = &rect)
        {
            return PInvoke.GetWindowRect(new HWND(handle), lpRectLocal);
        }
    }
}
