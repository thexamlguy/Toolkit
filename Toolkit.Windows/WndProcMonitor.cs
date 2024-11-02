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
