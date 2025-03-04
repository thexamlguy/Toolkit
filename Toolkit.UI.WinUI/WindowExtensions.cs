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
using WinUIEx;
using Windows.Graphics;
using Rect = Windows.Foundation.Rect;
using System;

namespace Toolkit.UI.WinUI;

public static partial class WindowExtensions
{
    private static SUBCLASSPROC? SubClassDelegate;

    public static uint GetDpi(this Window window)
    {
        nint handle = WindowNative.GetWindowHandle(window);
        if (handle == 0) return 0;

        return PInvoke.GetDpiForWindow(new HWND(handle));
    }

    public static void Hide(this Window window)
    {
        nint handle = WindowNative.GetWindowHandle(window);
        if (handle == 0) return;

        WindowHelper.HideWindow(new HWND(handle));
    }

    public static void SetIsShownInSwitchers(this Window window,
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

        PInvoke.SetForegroundWindow(new HWND(handle));
    }

    public static void SetIsMaximizable(this Window window, bool value) =>
        window.UpdateOverlappedPresenter(presenter => presenter.IsMaximizable = value);

    public static void SetIsMinimizable(this Window window, bool value) =>
        window.UpdateOverlappedPresenter(presenter => presenter.IsMinimizable = value);

    public static void SetIsResizable(this Window window, bool value) =>
        window.UpdateOverlappedPresenter(presenter => presenter.IsResizable = value);

    public static void SetSize(this Window window,
        int width, 
        int height)
    {
        nint handle = WindowNative.GetWindowHandle(window);
        if (handle == 0) return;

        float value = PInvoke.GetDpiForWindow(new HWND(handle)) / 96f;
        window.AppWindow.Resize(new SizeInt32((int)(width * (double)value), (int)(height * (double)value)));
    }

    public static void SetIsTopMost(this Window window, bool value) =>
        window.UpdateOverlappedPresenter(presenter => presenter.IsAlwaysOnTop = value);

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

    private static void UpdateOverlappedPresenter(this Window window,
        Action<OverlappedPresenter> action)
    {
        if (window.AppWindow.Presenter is OverlappedPresenter presenter)
        {
            action(presenter);
        }
    }

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
                    _ = PInvoke.DeleteObject(new HGDIOBJ((void*)hBrush));

                    return new LRESULT(1);
                }
        }

        return PInvoke.DefSubclassProc(hWnd, uMsg, wParam, lParam);
    }
}