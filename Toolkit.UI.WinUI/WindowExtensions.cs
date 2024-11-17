using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Windows.Win32;
using Windows.Win32.Foundation;
using WinRT.Interop;
using Windows.Win32.Graphics.Gdi;
using System.Drawing;
using Windows.Win32.UI.Shell;
using Windows.Win32.UI.WindowsAndMessaging;
using Toolkit.Windows;
using Rect = Windows.Foundation.Rect;
using WinUIEx;

namespace Toolkit.UI.WinUI;

public static partial class WindowExtensions
{
    private static SUBCLASSPROC? SubClassDelegate;

    public static void Hide(this Window window)
    {
        nint handle = WindowNative.GetWindowHandle(window);
        if (handle == 0) return;

        WindowHelper.HideWindow(new HWND(handle));
    }

    public static void IsShownInSwitchers(this Window window,
        bool value)
    {
        if (window.AppWindow is AppWindow appWindow)
        {
            appWindow.IsShownInSwitchers = value;
        }
    }

    public static void MoveAndResize(this Window window,
        Rect rect)
    {
        nint handle = WindowNative.GetWindowHandle(window);
        if (handle == 0) return;

        WindowHelper.MoveAndResizeWindow(new HWND(handle),
            (int)rect.Left,
            (int)rect.Top,
            (int)rect.Width,
            (int)rect.Height);
    }

    public static void SetBorderless(this Window window,
                bool value)
    {
        WindowStyle windowStyle = window.GetWindowStyle();

        if (value)
        {
            windowStyle &= ~(WindowStyle.Caption |
                             WindowStyle.ThickFrame |
                             WindowStyle.Border |
                             WindowStyle.SysMenu);
        }
        else
        {
            windowStyle |= WindowStyle.Caption |
                           WindowStyle.ThickFrame |
                           WindowStyle.Border |
                           WindowStyle.SysMenu;
        }

        window.SetWindowStyle(windowStyle);
    }

    public static void SetForeground(this Window window)
    {
        nint handle = WindowNative.GetWindowHandle(window);
        if (handle == 0) return;

        WindowHelper.SetForegroundWindow(new HWND(handle));
    }

    public static void SetTopMost(this Window window,
            bool value)
    {
        if (window.AppWindow is AppWindow appWindow &&
            appWindow.Presenter is OverlappedPresenter presenter)
        {
            presenter.IsAlwaysOnTop = value;
        }
    }

    public static void SetTransparency(this Window window,
        bool value)
    {
        nint handle = WindowNative.GetWindowHandle(window);
        if (handle == 0) return;

        HWND hWnd = new(handle);
        if (value)
        {
            EnableTransparency(hWnd);
        }
        else
        {
            DisableTransparency(hWnd);
        }
    }

    public static void Show(this Window window)
    {
        nint handle = WindowNative.GetWindowHandle(window);
        if (handle == 0) return;

        WindowHelper.ShowWindow(new HWND(handle));
    }

    private static unsafe void DisableTransparency(HWND hWnd)
    {
        if (SubClassDelegate != null)
        {
            _ = PInvoke.RemoveWindowSubclass(hWnd, SubClassDelegate, 0);
            SubClassDelegate = null;
        }

        WINDOW_EX_STYLE exStyle = (WINDOW_EX_STYLE)PInvoke.GetWindowLong(hWnd, WINDOW_LONG_PTR_INDEX.GWL_EXSTYLE);
        _ = PInvoke.SetWindowLong(hWnd, WINDOW_LONG_PTR_INDEX.GWL_EXSTYLE, (int)(exStyle & ~WINDOW_EX_STYLE.WS_EX_LAYERED));
    }

    private static unsafe void EnableTransparency(HWND hWnd)
    {
        SubClassDelegate = new SUBCLASSPROC(WindowSubClass);
        _ = PInvoke.SetWindowSubclass(hWnd, SubClassDelegate, 0, 0);

        WINDOW_EX_STYLE exStyle = (WINDOW_EX_STYLE)PInvoke.GetWindowLong(hWnd, WINDOW_LONG_PTR_INDEX.GWL_EXSTYLE);
        _ = PInvoke.SetWindowLong(hWnd, WINDOW_LONG_PTR_INDEX.GWL_EXSTYLE,
            (int)(exStyle | WINDOW_EX_STYLE.WS_EX_LAYERED));

        COLORREF blackColor = new COLORREF((uint)ToWin32(Color.Black));
        _ = PInvoke.SetLayeredWindowAttributes(hWnd, blackColor, 0, LAYERED_WINDOW_ATTRIBUTES_FLAGS.LWA_COLORKEY | 
            LAYERED_WINDOW_ATTRIBUTES_FLAGS.LWA_ALPHA);
    }

    private static int ToWin32(Color c) => c.B << 16 | c.G << 8 | c.R;

    private static unsafe LRESULT WindowSubClass(HWND hWnd,
        uint uMsg, WPARAM wParam, LPARAM lParam, nuint uIdSubclass, nuint dwRefData)
    {
        switch (uMsg)
        {
            case PInvoke.WM_ERASEBKGND:
                {
                    RECT rect;
                    PInvoke.GetClientRect(hWnd, &rect);

                    HBRUSH hBrush = PInvoke.CreateSolidBrush(new COLORREF((uint)ToWin32(Color.Black)));
                    _ = PInvoke.FillRect(new HDC((nint)wParam.Value),
                        &rect, hBrush);
                    _ = PInvoke.DeleteObject(new HGDIOBJ(hBrush));

                    return new LRESULT(1);
                }
        }

        return PInvoke.DefSubclassProc(hWnd, uMsg, wParam, lParam);
    }
}