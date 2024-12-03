using CommunityToolkit.Mvvm.Messaging;
using Toolkit.Foundation;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;

namespace Toolkit.Windows;

public class WndProc(IMessenger messenger) : 
    IWndProc
{
    private WNDPROC? handler;
    private bool isDisposed;

    ~WndProc()
    {
        Dispose(false);
    }

    public IntPtr Handle { get; private set; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Initialize() => InitializeWndProc();

    protected virtual void Dispose(bool disposing)
    {
        if (isDisposed)
        {
            return;
        }

        isDisposed = true;

        PInvoke.DestroyWindow((HWND)Handle);
    }

    private LRESULT HandleWndProc(HWND hWnd, uint msg,
        WPARAM wParam, LPARAM lParam)
    {
        messenger.Send(new WndProcEventArgs(msg,
            (uint)wParam.Value,
            (uint)lParam.Value));

        return PInvoke.DefWindowProc(hWnd, msg, wParam, lParam);
    }

    private unsafe void InitializeWndProc()
    {
        string windowId = $"WndProc_Handler_{Guid.NewGuid()}";
        handler = HandleWndProc;

        fixed (char* className = windowId)
        {
            WNDCLASSW wndCLass = new()
            {
                lpfnWndProc = handler,
                lpszClassName = className,
            };

            _ = PInvoke.RegisterClass(wndCLass);
        }

        Handle = PInvoke.CreateWindowEx(0, windowId, windowId, 0, 0, 0, 0, 0,
            new HWND(IntPtr.Zero), null, null, null);
    }
}
