using System.Runtime.InteropServices;
using Windows.Win32.Foundation;

namespace Windows.Win32;

public static partial class PInvoke
{
    public static readonly int SPI_SETWORKAREA = 0x002F;
    public static readonly uint WM_TASKBARCREATED = RegisterWindowMessage("TaskbarCreated");

    [StructLayout(LayoutKind.Sequential)]
    public struct AppBarData
    {
        public uint cbSize;
        public IntPtr hWnd;
        public uint uCallbackMessage;
        public AppBarEdge uEdge;
        public RECT rect;
        public int lParam;
    }

    public enum AppBarMessage : uint
    {
        New = 0x00000000,
        Remove = 0x00000001,
        QueryPos = 0x00000002,
        SetPos = 0x00000003,
        GetState = 0x00000004,
        GetTaskbarPos = 0x00000005,
        Activate = 0x00000006,
        GetAutoHideBar = 0x00000007,
        SetAutoHideBar = 0x00000008,
        WindowPosChanged = 0x00000009,
        SetState = 0x0000000A,
    }

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr DefWindowProcW(IntPtr handle, uint msg, IntPtr wParam, IntPtr lParam);

    [DllImport("shell32.dll", SetLastError = true)]
    public static extern IntPtr SHAppBarMessage(AppBarMessage dwMessage, ref AppBarData pData);

    public static AppBarData GetAppBarData(IntPtr handle)
    {
        return new AppBarData
        {
            cbSize = (uint)Marshal.SizeOf(typeof(AppBarData)),
            hWnd = handle
        };
    }

    public enum AppBarEdge : uint
    {
        Left = 0,
        Top = 1,
        Right = 2,
        Bottom = 3
    }

    public static void GetAppBarPosition(ref AppBarData appBarData) => 
        SHAppBarMessage(AppBarMessage.GetTaskbarPos, ref appBarData);

    internal static bool SetWindowSubclass(HWND value1, Func<HWND, uint, WPARAM, LPARAM, nuint, nuint, LRESULT> value2, int v1, nuint v2)
    {
        throw new NotImplementedException();
    }
}
