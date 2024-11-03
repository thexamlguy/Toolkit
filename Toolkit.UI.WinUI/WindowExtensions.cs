using Microsoft.UI.Xaml;
using WinUIEx;

namespace Toolkit.UI.WinUI;

public static partial class WindowExtensions
{
    public static void SetWindowStyle(this Window window,
        WindowStyle style)
    {
        WinUIEx.WindowStyle windowStyle = window.GetWindowStyle();

        switch (style)
        {
            case WindowStyle.None:
                windowStyle &= ~(WinUIEx.WindowStyle.Caption |
                WinUIEx.WindowStyle.ThickFrame |
                WinUIEx.WindowStyle.Border |
                WinUIEx.WindowStyle.SysMenu);
                break;
        }

        window.SetWindowStyle(windowStyle);
    }
}