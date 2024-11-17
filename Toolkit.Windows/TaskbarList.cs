namespace Toolkit.Windows;

public class TaskbarList(ITaskbar taskbar) : 
    ITaskbarList
{
    public IntPtr GetHandle()
    {
        nint rebarHandle = WindowHelper.FindWindow("ReBarWindow32", taskbar.GetHandle());
        nint taskHandle = WindowHelper.FindWindow("MSTaskSwWClass", rebarHandle);
        return WindowHelper.FindWindow("MSTaskListWClass", taskHandle);
    }
}
