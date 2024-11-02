using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using Windows.Win32.Graphics.Gdi;

namespace Toolkit.Windows;

public class Screen
{
    private const int CCHDEVICENAME = 32;
    private const int PRIMARY_MONITOR = unchecked((int)0xBAADF00D);
    private static readonly bool _multiMonitorSupport;

    private readonly IntPtr _monitorHandle;

    static Screen()
    {
        _multiMonitorSupport = PInvoke.GetSystemMetrics(SYSTEM_METRICS_INDEX.SM_CMONITORS) != 0;
    }

    internal Screen(IntPtr monitorHandle)
    {
        if (!_multiMonitorSupport || monitorHandle == (IntPtr)PRIMARY_MONITOR)
        {
            Bounds = SystemInformationHelper.VirtualScreen;
            Primary = true;
            DeviceName = "DISPLAY";
        }
        else
        {
            var monitorData = GetMonitorData(monitorHandle);

            Bounds = new Rect(monitorData.MonitorRect.left, monitorData.MonitorRect.top,
                monitorData.MonitorRect.right - monitorData.MonitorRect.left,
                monitorData.MonitorRect.bottom - monitorData.MonitorRect.top);

            Primary = (monitorData.Flags & (int)MonitorFlag.MONITOR_DEFAULTTOPRIMARY) != 0;
            DeviceName = monitorData.DeviceName;
        }

        _monitorHandle = monitorHandle;
    }

    private enum MonitorFlag : uint
    {
        MONITOR_DEFAULTTONULL = 0,
        MONITOR_DEFAULTTOPRIMARY = 1,
        MONITOR_DEFAULTTONEAREST = 2
    }

    public Rect Bounds { get; }

    public string DeviceName { get; }

    public bool Primary { get; }

    public Rect WorkingArea => GetWorkingArea();

    public static Screen FromHandle(IntPtr handle) => _multiMonitorSupport ? 
        new Screen(PInvoke.MonitorFromWindow((HWND)handle,
            MONITOR_FROM_FLAGS.MONITOR_DEFAULTTONEAREST)) : 
                new Screen(PRIMARY_MONITOR);

    public override bool Equals(object? obj)
    {
        if (obj is not Screen monitor) return false;
        return _monitorHandle == monitor._monitorHandle;
    }

    public override  int GetHashCode()
    {
        checked
        {
            return (int)_monitorHandle;
        }
    }

    [DllImport("user32.dll", EntryPoint = "GetMonitorInfo", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool GetMonitorInfoEx(IntPtr hMonitor, ref MonitorData lpmi);

    private MonitorData GetMonitorData(IntPtr monitorHandle)
    {
        MonitorData monitorData = new();
        monitorData.Size = Marshal.SizeOf(monitorData);
        GetMonitorInfoEx(monitorHandle, ref monitorData);

        return monitorData;
    }

    private Rect GetWorkingArea()
    {
        if (!_multiMonitorSupport || _monitorHandle == PRIMARY_MONITOR)
        {
            return SystemInformationHelper.WorkingArea;
        }

        var monitorData = GetMonitorData(_monitorHandle);
        return new Rect(monitorData.WorkAreaRect.left, monitorData.WorkAreaRect.top, monitorData.WorkAreaRect.right - monitorData.WorkAreaRect.left, monitorData.WorkAreaRect.bottom - monitorData.WorkAreaRect.top);
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private struct MonitorData
    {
        public int Size;
        public RECT MonitorRect;
        public RECT WorkAreaRect;
        public uint Flags;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)]
        public string DeviceName;
    }
}
