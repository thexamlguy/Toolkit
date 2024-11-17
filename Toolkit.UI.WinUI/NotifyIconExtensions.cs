using System.Drawing;
using System.IO;
using Toolkit.Windows;
using Toolkit.WinUI;

namespace Toolkit.UI.WinUI;

public static class NotifyIconExtensions
{
    public static void SetIcon(this INotifyIcon notifyIcon,
        Stream? stream)
    {
        nint shellTrayHandle = WindowHelper.GetWindowHandle("Shell_TrayWnd");
        uint dpi = WindowHelper.GetDpi(shellTrayHandle);

        if (stream?.ConvertToIcon(dpi) is Icon icon)
        {
            notifyIcon.SetIcon(icon.Handle);
        }
    }
}
