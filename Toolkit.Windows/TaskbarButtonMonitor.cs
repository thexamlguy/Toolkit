using Windows.Win32;
using Windows.Win32.UI.WindowsAndMessaging;
using Windows.Win32.Foundation;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace Toolkit.Windows;

public record PointerPressed(PointerLocation Location, PointerButton Button = PointerButton.Left);

public record PointerDragReleased(PointerLocation Location, PointerButton Button = PointerButton.Left);

public record PointerReleased(PointerLocation Location, PointerButton Button = PointerButton.Left);

public record PointerLocation(int X, int Y);

public record PointerMoved(PointerLocation Location);

internal enum WndProcMessages
{
    WM_LBUTTONUP = 0x0202,
    WM_MBUTTONUP = 0x0208,
    WM_RBUTTONUP = 0x0205,
    WM_MOUSEMOVE = 0x0200,
    WM_SETTINGCHANGE = 0x001A,
    WM_MBUTTONDOWN = 0x0207,
    WM_LBUTTONDOWN = 0x0201,
    WM_RBUTTONDOWN = 0x0204
}

public enum PointerButton
{
    Left,
    Middle,
    Right
}

public record PointerDrag(PointerLocation Location);


public class PointerMonitor : IPointerMonitor
{
    private readonly IMessenger messenger;
    private bool isDisposed;
    private bool isPointerPressed;
    private HOOKPROC? mouseEventDelegate;
    private UnhookWindowsHookExSafeHandle? mouseHandle;
    private bool isPointerDrag;

    public PointerMonitor(IMessenger messenger)
    {
        this.messenger = messenger;
    }

    ~PointerMonitor()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public unsafe Task Initialize()
    {
        InitializeHook();
        return Task.CompletedTask;
    }

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

            messenger.Send(new PointerDrag(location));
        }

        messenger.Send(new PointerMoved(location));
    }

    private void SendPointerPressed(PointerLocation location, PointerButton button)
    {
        isPointerPressed = true;
        messenger.Send(new PointerPressed(location, button));
    }

    private void SendPointerReleased(PointerLocation location, PointerButton button)
    {
        if (isPointerPressed)
        {
            if (isPointerDrag)
            {
                isPointerDrag = false;
                messenger.Send(new PointerDragReleased(location, button));
            }

            isPointerPressed = false;
            messenger.Send(new PointerReleased(location, button));
        }
    }
}