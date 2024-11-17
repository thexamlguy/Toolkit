using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;

namespace Toolkit.Windows;

public class Pointer(IMessenger messenger) : 
    IPointer
{
    private bool isDisposed;
    private bool isPointerDrag;
    private bool isPointerPressed;
    private HOOKPROC? mouseEventDelegate;
    private UnhookWindowsHookExSafeHandle? mouseHandle;

    ~Pointer()
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
            if (TryGetPointerLocation(out PointerLocation? location))
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

            messenger.Send(new PointerDragEventArgs(location));
        }

        messenger.Send(new PointerMovedEventArgs(location));
    }

    private void SendPointerPressed(PointerLocation location, PointerButton button)
    {
        isPointerPressed = true;
        messenger.Send(new PointerPressedEventArgs(location, button));
    }

    private void SendPointerReleased(PointerLocation location, PointerButton button)
    {
        if (isPointerPressed)
        {
            if (isPointerDrag)
            {
                isPointerDrag = false;
                messenger.Send(new PointerDragReleasedEventArgs(location, button));
            }

            isPointerPressed = false;
            messenger.Send(new PointerReleasedEventArgs(location, button));
        }
    }

    private unsafe bool TryGetPointer(out System.Drawing.Point point)
    {
        fixed (System.Drawing.Point* lpPointLocal = &point)
        {
            return PInvoke.GetPhysicalCursorPos(lpPointLocal);
        }
    }

    private bool TryGetPointerLocation([MaybeNullWhen(false)] out PointerLocation location)
    {
        if (TryGetPointer(out System.Drawing.Point point))
        {
            location = new PointerLocation(point.X, point.Y);
            return true;

        }

        location = null;
        return false;
    }
}