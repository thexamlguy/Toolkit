using System.Runtime.InteropServices;
using Windows.Foundation;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;

namespace Toolkit.Windows;

internal static class SystemInformationHelper
{
    private const int SPI_GETWORKAREA = 48;

    public static Rect VirtualScreen => GetVirtualScreen();
    public static Rect WorkingArea => GetWorkingArea();

    private static Rect GetVirtualScreen()
    {
        Size size = new(PInvoke.GetSystemMetrics(SYSTEM_METRICS_INDEX.SM_CXSCREEN), 
            PInvoke.GetSystemMetrics(SYSTEM_METRICS_INDEX.SM_CYSCREEN));
        return new Rect(0, 0, (int)size.Width, (int)size.Height);
    }

    private static Rect GetWorkingArea()
    {
        var rect = new RECT();

        SystemParametersInfo(SPI_GETWORKAREA, 0, ref rect, 0);
        return new Rect(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern bool SystemParametersInfo(int nAction, int nParam, ref RECT rc, int nUpdate);
}
