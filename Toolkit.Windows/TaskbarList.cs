namespace Toolkit.Windows;

public class TaskbarList(ITaskbar taskbar) : 
    ITaskbarList
{
    public IntPtr GetHandle()
    {
        nint rebarHandle = WindowHelper.Find("ReBarWindow32", taskbar.GetHandle());
        nint taskHandle = WindowHelper.Find("MSTaskSwWClass", rebarHandle);
        return WindowHelper.Find("MSTaskListWClass", taskHandle);
    }
}
