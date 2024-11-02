using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using Toolkit.Foundation;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.UI.WindowsAndMessaging;

namespace Toolkit.Windows;

public class WndProcMonitor(IPublisher publisher) : 
    IWndProcMonitor
{
    private WNDPROC? handler;
    private readonly IPublisher publisher = publisher;

    public IntPtr Handle { get; private set; }

    public void Dispose()
    {
        PInvoke.DestroyWindow((HWND)Handle);
    }

    private unsafe void InitializeWndProc()
    {
        var windowName = Guid.NewGuid().ToString();
        handler = Wndproc;

        WNDCLASSW wndProcWindow;

        wndProcWindow.style = 0;
        wndProcWindow.lpfnWndProc = handler;
        wndProcWindow.cbClsExtra = 0;
        wndProcWindow.cbWndExtra = 0;
        wndProcWindow.hInstance = new HINSTANCE();
        wndProcWindow.hIcon = new HICON();
        wndProcWindow.hCursor = new HCURSOR();
        wndProcWindow.hbrBackground = new HBRUSH();

        fixed (char* menuName = "")
        {
            wndProcWindow.lpszMenuName = new PCWSTR(menuName);
        }

        fixed (char* className = windowName)
        {
            wndProcWindow.lpszClassName = new PCWSTR(className);
        }

        PInvoke.RegisterClass(wndProcWindow);
        Handle = PInvoke.CreateWindowEx(0, wndProcWindow.lpszClassName, 
            new PCWSTR(), 0, 0, 0, 0, 0, new HWND(),
            new HMENU(),
            new HINSTANCE());
    }

    private LRESULT Wndproc(HWND param0, uint param1, WPARAM param2, LPARAM param3)
    {
        publisher.Publish(new WndProcEventArgs(param1, (uint)param2.Value, (uint)param3.Value));
        return PInvoke.DefWindowProc(param0, param1, param2, param3);
    }

    public void Initialize()
    {
        InitializeWndProc();
    }
}


public class PointerMonitor(IPublisher publisher) : 
    IPointerMonitor
{
    private bool isDisposed;
    private bool isPointerDrag;
    private bool isPointerPressed;
    private HOOKPROC? mouseEventDelegate;
    private UnhookWindowsHookExSafeHandle? mouseHandle;
    ~PointerMonitor()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public unsafe void Initialize() => 
        InitializeHook();

    protected virtual void Dispose(bool disposing)
    {
        if (!isDisposed)
        {
            RemoveHook();
            isDisposed = true;
        }
    }

    private unsafe void InitializeHook()
    {
        mouseEventDelegate = new HOOKPROC(MouseProc);
        mouseHandle = PInvoke.SetWindowsHookEx(WINDOWS_HOOK_ID.WH_MOUSE_LL, mouseEventDelegate,
            PInvoke.GetModuleHandle("user32.dll"), 0);
    }

    private LRESULT MouseProc(int nCode, WPARAM wParam, LPARAM lParam)
    {
        if (nCode >= 0)
        {

            if (TryGetPointerLocation(out var location))
            {
                switch ((uint)wParam.Value)
                {
                    case (uint)WndProcMessages.WM_MOUSEMOVE:
                        SendPointerMoved(location);
                        break;
                    case (uint)WndProcMessages.WM_LBUTTONUP:
                        SendPointerReleased(location, PointerButton.Left);
                        break;
                    case (uint)WndProcMessages.WM_MBUTTONUP:
                        SendPointerReleased(location, PointerButton.Middle);
                        break;
                    case (uint)WndProcMessages.WM_RBUTTONUP:
                        SendPointerReleased(location, PointerButton.Right);
                        break;
                    case (uint)WndProcMessages.WM_LBUTTONDOWN:
                        SendPointerPressed(location, PointerButton.Left);
                        break;
                    case (uint)WndProcMessages.WM_MBUTTONDOWN:
                        SendPointerPressed(location, PointerButton.Middle);
                        break;
                    case (uint)WndProcMessages.WM_RBUTTONDOWN:
                        SendPointerPressed(location, PointerButton.Right);
                        break;
                }
            }
        }

        return PInvoke.CallNextHookEx(mouseHandle, nCode, wParam, lParam);
    }

    private unsafe void RemoveHook()
    {
        if (mouseHandle is not null && mouseHandle.DangerousGetHandle() != nint.Zero)
        {
            PInvoke.UnhookWindowsHookEx((HHOOK)mouseHandle.DangerousGetHandle());
        }
    }

    private void SendPointerMoved(PointerLocation location)
    {
        if (isPointerPressed)
        {
            if (!isPointerDrag)
            {
                isPointerDrag = true;
            }

            publisher.Publish(new PointerDragEventArgs(location));
        }

        publisher.Publish(new PointerMovedEventArgs(location));
    }

    private void SendPointerPressed(PointerLocation location, PointerButton button)
    {
        isPointerPressed = true;
        publisher.Publish(new PointerPressedEventArgs(location, button));
    }

    private void SendPointerReleased(PointerLocation location, PointerButton button)
    {
        if (isPointerPressed)
        {
            if (isPointerDrag)
            {
                isPointerDrag = false;
                publisher.Publish(new PointerDragReleasedEventArgs(location, button));
            }

            isPointerPressed = false;
            publisher.Publish(new PointerReleasedEventArgs(location, button));
        }
    }

    private unsafe bool TryGetPointer(out Point point)
    {
        fixed (Point* lpPointLocal = &point)
        {
            return PInvoke.GetPhysicalCursorPos(lpPointLocal);
        }
    }

    private bool TryGetPointerLocation([MaybeNullWhen(false)] out PointerLocation location)
    {
        if (TryGetPointer(out Point point))
        {
            location = new PointerLocation(point.X, point.Y);
            return true;

        }

        location = null;
        return false;
    }
}